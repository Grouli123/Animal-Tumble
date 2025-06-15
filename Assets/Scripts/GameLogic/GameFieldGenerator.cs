using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Animal.Data;
using Animal.Infrastructure;
using Animal.Services;
using Animal.Domain;

namespace Animal.GameLogic
{
    public class GameFieldGenerator : MonoBehaviour
    {
        [Header("Зависимости")]
        [SerializeField] private AnimalFactory animalFactory;
        private IActionBarModel barModel;

        [Header("Настройки генерации")]
        [SerializeField] private Vector2 spawnXRange = new Vector2(-3.5f, 3.5f);
        [SerializeField] private float spawnHeight = 6f;
        [SerializeField] private float delayBetweenDrops = 0.05f;
        [SerializeField] private int numberOfTriplets = 10;

        private readonly List<GameObject> spawnedAnimals = new();
        private readonly List<AnimalData> extraSpawnData = new();

        private IGameStateService gameState;
        private IFigureCounterService figureCounter;
        
        private bool victoryChecked = false;
        private bool suppressVictoryCheck = false;

        public void SetActionBarModel(IActionBarModel model)
        {
            barModel = model;
        }

        public void Init(IGameStateService gameStateService, IFigureCounterService figureCounterService)
        {
            gameState = gameStateService;
            figureCounter = figureCounterService;
        }

        public void GenerateField()
        {
            GenerateField(numberOfTriplets);
        }

        public void GenerateField(int tripletCount)
        {
            Debug.Log($"GenerateField: {tripletCount} троек ({tripletCount * 3} фигур)");
            figureCounter.ResetFigureCount();

            List<AnimalData> animalsToSpawn = GenerateAnimalDataList(tripletCount);
            StartCoroutine(SpawnWithSandEffect(animalsToSpawn));
        }

        public void AddExtraFigure(AnimalData data)
        {
            extraSpawnData.Add(data);
        }

        private List<AnimalData> GenerateAnimalDataList(int tripletCount)
        {
            List<AnimalConfig> configs = animalFactory.GetAvailableConfigs();
            if (configs.Count == 0)
            {
                Debug.LogError("Нет доступных конфигураций животных.");
                return new List<AnimalData>();
            }

            List<AnimalData> result = new();
            for (int i = 0; i < tripletCount; i++)
            {
                AnimalConfig cfg = configs[i % configs.Count];
                AnimalData data = new(cfg.animalShape, cfg.animalFrameColor, cfg.animalType);
                result.AddRange(new[] { data, data, data });
            }

            result.AddRange(extraSpawnData);
            extraSpawnData.Clear();

            return result.OrderBy(_ => Random.value).ToList();
        }

        private IEnumerator SpawnWithSandEffect(IEnumerable<AnimalData> animals, System.Action onComplete = null)
        {
            spawnedAnimals.Clear();

            foreach (AnimalData animal in animals)
            {
                float x = Random.Range(spawnXRange.x, spawnXRange.y);
                Vector2 pos = new Vector2(x, spawnHeight);

                GameObject go = animalFactory.CreateAnimal(animal, pos);

                if (go != null)
                {
                    spawnedAnimals.Add(go);
                    figureCounter.Increment();
                }

                yield return new WaitForSeconds(delayBetweenDrops);
            }

            onComplete?.Invoke();
        }



        public void RefillField()
        {
            suppressVictoryCheck = true;

            List<AnimalData> current = spawnedAnimals
                .Where(go => go != null && go.TryGetComponent<AnimalView>(out var view))
                .Select(go => go.GetComponent<AnimalView>().Data)
                .ToList();

            AnimalData? lastFromBar = barModel.PopLast(); // если PopLast() реализован
            if (lastFromBar.HasValue)
                current.Add(lastFromBar.Value);


            if (current.Count < 3)
            {
                Debug.Log("Недостаточно фигурок для новой генерации");
                return;
            }

            ClearScene();
            List<AnimalData> newData = current.OrderBy(_ => Random.value).ToList();
            StartCoroutine(SpawnWithSandEffect(newData, () =>
            {
                Debug.Log("🟢 Refill завершён, проверяем победу");

                if (GameService.Instance.CurrentBarIsEmpty() && GameService.Instance.NoFiguresLeft())
                {
                    Debug.Log("🎉 Победа после Refill");
                    GameService.Instance.TriggerWin();
                }
            }));
        }

        private void CheckForVictory()
        {
            if (victoryChecked || suppressVictoryCheck) return;

            if (spawnedAnimals.All(go => go == null) && GameService.Instance.CurrentBarIsEmpty())
            {
                victoryChecked = true;
                GameService.Instance.TriggerWin();
            }
        }

        public void RestartField()
        {
            ClearScene();
            barModel.Clear();
            figureCounter.ResetFigureCount();
            victoryChecked = false; 
            GenerateField();
        }


        public void ClearScene()
        {
            foreach (GameObject go in spawnedAnimals)
            {
                if (go != null)
                    Destroy(go); 
            }
        }
        
        public void SetRefillService(IRefillService service)
        {
            
        }

        public int GetLiveFigureCount() => spawnedAnimals.Count(go => go != null);
    }
}