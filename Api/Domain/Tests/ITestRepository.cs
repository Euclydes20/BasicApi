namespace Api.Domain.Tests
{
    public interface ITestRepository
    {
        Task<Test> AddWithEFAsync(Test test);
        Task DeleteWithEFAsync(int testId);
        Task<Test> AddWithLQAsync(Test test);
        Task DeleteWithLQAsync(int testId);
    }
}
