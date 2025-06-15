namespace Animal.Services
{
    public interface IGameEndHandler
    {
        void TriggerWin();
        void TriggerLose();
        void OnCloseScreen();
    }
}