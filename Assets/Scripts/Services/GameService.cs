using UnityEngine;
using Animal.GameLogic;
using Animal.Domain;
using Animal.Presentation;
using Animal.Infrastructure;

namespace Animal.Services
{
    public class GameService : IRefillService, IGameEndHandler, IGameStateService, IFigureCounterService
    {
        public static GameService Instance { get; private set; }

        private readonly GameFieldGenerator generator;
        private readonly IActionBarModel barModel;
        private readonly RefillButtonPresenter refillButton;
        private readonly GameObject winScreen;
        private readonly GameObject loseScreen;

        private int currentFigureCount = 0;
        private bool isGameOver = false;
        private bool victoryChecked = false;
        private bool isRefilling = false;
        
        public GameService(GameFieldGenerator generator,
            IActionBarModel barModel,
            RefillButtonPresenter refillButton,
            GameObject winScreen,
            GameObject loseScreen)
        {
            this.generator = generator;
            this.barModel = barModel;
            this.refillButton = refillButton;
            this.winScreen = winScreen;
            this.loseScreen = loseScreen;

            Instance = this; 
        }

        public void TriggerWin()
        {
            isGameOver = true;
            winScreen.SetActive(true);
            refillButton.SetActive(false);
            generator.ClearScene();
        }

        public void TriggerLose()
        {
            isGameOver = true;
            generator.ClearScene(); 
            loseScreen.SetActive(true);
            refillButton.SetActive(false);
        }

        public void OnCloseScreen()
        {
            isGameOver = false;
            winScreen.SetActive(false);
            loseScreen.SetActive(false);
            ResetGameState();
            barModel.Clear();
            generator.ClearScene(); 
            refillButton.SetActive(true);
            currentFigureCount = 0; 
            generator.RestartField();
        }

        public void Refill()
        {
            isRefilling = true;
            generator.RefillField();
            isRefilling = false;
        }
        
        public bool IsRefilling => isRefilling;

        public bool IsGameOver => isGameOver;

        public void SetWinState()
        {
            isGameOver = true;
        }

        public void SetLoseState()
        {
            isGameOver = true;
        }

        public void ResetGameState()
        {
            isGameOver = false;
            victoryChecked = false;
        }

        public void ResetFigureCount()
        {
            currentFigureCount = 0;
        }

        public void Increment()
        {
            currentFigureCount++;
        }
        
        public bool CurrentBarIsEmpty()
        {
            return barModel.Data.Count == 0;
        }

        public void Decrement()
        {
            currentFigureCount = Mathf.Max(0, currentFigureCount - 1);
            Debug.Log($"[GameService] Текущих фигур: {currentFigureCount}");
        }
        
        public int CurrentCount => currentFigureCount;

        public bool NoFiguresLeft() => currentFigureCount <= 0;
    }
}