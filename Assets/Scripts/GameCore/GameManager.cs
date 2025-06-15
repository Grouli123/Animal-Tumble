using UnityEngine;
using Animal.GameLogic;
using Animal.Services;

namespace Animal.GameCore
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private int remainingFigures = 0;

        [SerializeField] private GameObject winScreen;
        [SerializeField] private GameObject loseScreen;
        [SerializeField] private GameService gameService;

        [SerializeField] private GameObject refillButton;
        [SerializeField] private GameFieldGenerator fieldGenerator; 

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void RegisterFigure()
        {
            remainingFigures++;
            Debug.Log($"[Register] Осталось фигурок: {remainingFigures}");
        }

        public void UnregisterFigure()
        {
            remainingFigures--;
            Debug.Log($"[Unregister] Осталось фигурок: {remainingFigures}");
        }

        public bool NoFiguresLeft()
        {
            return remainingFigures <= 0;
        }

        public void ResetFigureCount()
        {
            remainingFigures = 0;
            Debug.Log("[Reset] Количество фигур сброшено");
        }

        public void TriggerWin()
        {
            Debug.Log("🎉 Победа!");
            winScreen.SetActive(true);
            refillButton.SetActive(false); 
            fieldGenerator.ClearScene();
        }

        public void TriggerLose()
        {
            Debug.Log("💥 Поражение!");
            loseScreen.SetActive(true);
            refillButton.SetActive(false); 
            fieldGenerator.ClearScene();
        }

        public void OnCloseEndScreen()
        {
            winScreen.SetActive(false);
            loseScreen.SetActive(false);

            refillButton.SetActive(true);
            gameService.OnCloseScreen();
        }

        public void ShowRefillButton()
        {
            refillButton.SetActive(true);
        }
        
        public void SetService(GameService service)
        {
            gameService = service;
        }
    }
}