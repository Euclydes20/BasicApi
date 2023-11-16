using Api.Domain.Tests;

namespace Api.Services.Tests
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _testRepository;

        public TestService(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        public async Task<int> DeleteAllWithEFAsync()
        {
            return await _testRepository.DeleteAllWithEFAsync();
        }

        public async Task<int> DeleteAllWithLQAsync()
        {
            return await _testRepository.DeleteAllWithLQAsync();
        }

        public async Task<Test> AddWithEFAsync(Test test)
        {
            return await _testRepository.AddWithEFAsync(test);
        }

        public async Task DeleteWithEFAsync(int testId)
        {
            await _testRepository.DeleteWithEFAsync(testId);
        }

        public async Task<Test> AddWithLQAsync(Test test)
        {
            return await _testRepository.AddWithLQAsync(test);
        }

        public async Task DeleteWithLQAsync(int testId)
        {
            await _testRepository.DeleteWithLQAsync(testId);
        }
    }
}
