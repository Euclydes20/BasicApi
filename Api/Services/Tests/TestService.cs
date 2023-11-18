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

        private Test GenerateRandomTest()
        {
            return new Test()
            {
                Text = Guid.NewGuid().ToString("N"),
            };
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

        public async Task<Test> AddWithLQAsync(Test test)
        {
            return await _testRepository.AddWithLQAsync(test);
        }

        public async Task<IList<Test>> AddWithEFAsync(IList<Test> tests)
        {
            return await _testRepository.AddWithEFAsync(tests);
        }

        public async Task<IList<Test>> AddWithLQAsync(IList<Test> tests)
        {
            return await _testRepository.AddWithLQAsync(tests);
        }

        public async Task DeleteWithEFAsync(int testId)
        {
            await _testRepository.DeleteWithEFAsync(testId);
        }

        public async Task DeleteWithLQAsync(int testId)
        {
            await _testRepository.DeleteWithLQAsync(testId);
        }

        public async Task<IList<Test>> AddRandomWithEFAsync(int quantity, bool multipleAdd = false)
        {
            IList<Test> items = new List<Test>();

            for (int i = 0; i < quantity; i++)
            {
                //yield return await _testRepository.AddWithEFAsync(GenerateRandomTest()); 
                if (multipleAdd)
                    items.Add(GenerateRandomTest());
                else
                    items.Add(await _testRepository.AddWithEFAsync(GenerateRandomTest()));
            }

            if (multipleAdd)
                await _testRepository.AddWithEFAsync(items);

            return items;
        }

        public async Task<IList<Test>> AddRandomWithLQAsync(int quantity, bool multipleAdd = false)
        {
            IList<Test> items = new List<Test>();

            for (int i = 0; i < quantity; i++)
            {
                //yield return await _testRepository.AddWithLQAsync(GenerateRandomTest()); 
                if (multipleAdd)
                    items.Add(GenerateRandomTest());
                else
                    items.Add(await _testRepository.AddWithLQAsync(GenerateRandomTest()));
            }

            if (multipleAdd)
                await _testRepository.AddWithLQAsync(items);

            return items;
        }
    }
}
