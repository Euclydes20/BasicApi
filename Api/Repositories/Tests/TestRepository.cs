using Api.Domain.Tests;
using Api.Infra.Database;
using LinqToDB;

namespace Api.Repositories.Tests
{
    public class TestRepository : ITestRepository
    {
        private readonly Infra.Database.DataContext _dataContext;
        private readonly DataContextLQ _dataContextLQ;

        public TestRepository(Infra.Database.DataContext dataContext, DataContextLQ dataContextLQ)
        {
            _dataContext = dataContext;
            _dataContextLQ = dataContextLQ;
        }

        public async Task<Test> AddWithEFAsync(Test test)
        {
            await _dataContext.Test
                .AddAsync(test);

            await _dataContext.SaveChangesAsync();

            return test;
        }

        public async Task DeleteWithEFAsync(int testId)
        {
            await _dataContext.Test
                .DeleteAsync(t => t.Id == testId);
        }

        public async Task<Test> AddWithLQAsync(Test test)
        {
            test.Id = await _dataContextLQ.InsertWithInt32IdentityAsync(test);

            await _dataContext.SaveChangesAsync();

            return test;
        }

        public async Task DeleteWithLQAsync(int testId)
        {
            await _dataContextLQ.GetTable<Test>()
                .DeleteAsync(t => t.Id == testId);
        }
    }
}
