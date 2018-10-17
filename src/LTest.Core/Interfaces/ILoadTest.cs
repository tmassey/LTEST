namespace LTest.Core.Interfaces
{
    public interface ILoadTest
    {
        string TestName { get; }
        void BeforeTest();
        void AfterTest();
        void Execute();
    }
}