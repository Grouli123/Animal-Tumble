using UnityEngine;
using System.Collections.Generic;
using Animal.Data;

namespace Animal.GameLogic
{
    public class AnimalFactory : MonoBehaviour
    {
        public static AnimalFactory Instance { get; private set; }

        [SerializeField] private AnimalConfigDatabase configDatabase;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public AnimalConfig GetConfig(AnimalData data)
        {
            return configDatabase.GetConfig(data);
        }

        public List<AnimalConfig> GetAvailableConfigs()
        {
            return configDatabase.GetAllConfigs();
        }

        public GameObject CreateAnimal(AnimalData data, Vector2 spawnPosition)
        {
            var config = configDatabase.GetConfig(data);
            if (config == null)
            {
                Debug.LogError("Config not found for: " + data);
                return null;
            }

            if (config.prefab == null)
            {
                Debug.LogError("В AnimalConfig для " + data.animalType + " не указан prefab");
                return null;
            }

            GameObject go = Instantiate(config.prefab, spawnPosition, Quaternion.identity);
            if (go == null)
            {
                Debug.LogError("Префаб не создан");
                return null;
            }

            var view = go.GetComponent<AnimalView>();
            if (view == null)
            {
                Debug.LogError("На префабе отсутствует компонент AnimalView");
                return go;
            }

            view.Init(data, config.sprite);
            return go;
        }
    }
}