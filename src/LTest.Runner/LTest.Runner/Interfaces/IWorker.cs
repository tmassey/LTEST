namespace LTest.Runner.Interfaces
{
    public interface IWorker
    {
        event TestEndHandler OnTestEnd;
        string Name { get; set; }
        void RunTest();
    }
}