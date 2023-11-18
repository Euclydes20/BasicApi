using Api.Repositories.Tests;

namespace Api.Domain.Tests
{
    public interface ITestRepository
    {
        Task<int> DeleteAllWithEFAsync();
        Task<int> DeleteAllWithLQAsync();
        Task<Test> AddWithEFAsync(Test test);
        Task<Test> AddWithLQAsync(Test test);
        Task<IList<Test>> AddWithEFAsync(IList<Test> tests);
        Task<IList<Test>> AddWithLQAsync(IList<Test> tests);
        Task DeleteWithEFAsync(int testId);
        Task DeleteWithLQAsync(int testId);
    }
}
