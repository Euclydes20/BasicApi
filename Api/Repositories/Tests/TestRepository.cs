using Api.Domain.Tests;
using Api.Infra.Database;
using LinqToDB;

namespace Api.Repositories.Tests
{
    public class TestRepository : ITestRepository
    {
        private readonly DataContextEF _dataContextEF;
        private readonly DataContextLQ _dataContextLQ;

        public TestRepository(DataContextEF dataContext, DataContextLQ dataContextLQ)
        {
            _dataContextEF = dataContext;
            _dataContextLQ = dataContextLQ;
        }

        public async Task<int> DeleteAllWithEFAsync()
        {
            int quantity = _dataContextEF.Test
                .Count();

            _dataContextEF.Test
                .RemoveRange(_dataContextEF.Test);

            await _dataContextEF.SaveChangesAsync();

            return quantity;
        }

        public async Task<int> DeleteAllWithLQAsync()
        {
            int quantity = await _dataContextLQ.GetTable<Test>()
                .DeleteAsync();

            return quantity;
        }

        public async Task<Test> AddWithEFAsync(Test test)
        {
            await _dataContextEF.Test
                .AddAsync(test);

            await _dataContextEF.SaveChangesAsync();

            return test;
        }

        public async Task<Test> AddWithLQAsync(Test test)
        {
            test.Id = await _dataContextLQ.InsertWithInt32IdentityAsync(test);

            return test;
        }

        public async Task<IList<Test>> AddWithEFAsync(IList<Test> tests)
        {
            await _dataContextEF.Test
                .AddRangeAsync(tests);

            await _dataContextEF.SaveChangesAsync();

            return tests;
        }

        public async Task<IList<Test>> AddWithLQAsync(IList<Test> tests)
        {
            // LINQ2DB NÃO SUPORTA INSERT MÚLTIPLO COM RETORNO DE IDENTITY
            foreach (var test in tests)
            {
                test.Id = await _dataContextLQ.InsertWithInt32IdentityAsync(test);
            }

            return tests;
        }

        public async Task DeleteWithEFAsync(int testId)
        {
            await _dataContextEF.Test
                .DeleteAsync(t => t.Id == testId);

            await _dataContextEF.SaveChangesAsync();
        }

        public async Task DeleteWithLQAsync(int testId)
        {
            await _dataContextLQ.GetTable<Test>()
                .DeleteAsync(t => t.Id == testId);
        }
    }
}
