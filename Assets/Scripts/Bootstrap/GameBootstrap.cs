using UnityEngine;
using Animal.Presentation;
using Animal.GameLogic;
using Animal.Domain;
using Animal.Services; 
using Animal.GameCore;

namespace Animal.Bootstrap
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        [Header("Scene refs")]
        [SerializeField] private ActionBarView actionBarView;
        [SerializeField] private RefillButtonPresenter refillButton;
        [SerializeField] private GameFieldGenerator generator;
        [SerializeField] private GameObject winScreen;
        [SerializeField] private GameObject loseScreen;
        [SerializeField] private GameManager gameManager;

        private GameService gameService;

        private void Awake()
        {
            var barModel = new ActionBarModel(actionBarView.SlotCount);

            gameService = new GameService(generator, barModel, refillButton, winScreen, loseScreen);

            actionBarView.Init(barModel);
            refillButton.Init(gameService);
            generator.Init(gameService, gameService);
            generator.SetActionBarModel(barModel);
            gameManager.SetService(gameService);
            actionBarView.Init(barModel);
        }

        private void Start()
        {
            generator.GenerateField(); 
        }
    }
}