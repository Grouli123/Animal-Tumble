namespace Animal.Infrastructure
{
    public interface IFigureCounterService
    {
        void ResetFigureCount();
        void Increment();
        int CurrentCount { get; }
        bool NoFiguresLeft();
    }
}