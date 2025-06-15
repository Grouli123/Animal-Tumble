namespace Animal.Infrastructure
{
    public interface IGameStateService
    {
        bool IsGameOver { get; }
        void SetWinState();
        void SetLoseState();
        void ResetGameState();
    }
}