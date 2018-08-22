namespace LTest.Core.Interfaces
{
    public interface ILoadTest
    {
        void BeforeTest();
        void AfterTest();
        void Execute();
    }
}