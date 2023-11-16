using Api.Domain.Tests;
using Api.Infra.Database;

namespace Api.Services.Tests
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _testRepository;

        public TestService(ITestRepository testRepository)
        {
            _testRepository = testRepository;
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
