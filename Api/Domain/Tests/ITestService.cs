﻿namespace Api.Domain.Tests
{
    public interface ITestService
    {
        Task<int> DeleteAllWithEFAsync();
        Task<int> DeleteAllWithLQAsync();
        Task<Test> AddWithEFAsync(Test test);
        Task DeleteWithEFAsync(int testId);
        Task<Test> AddWithLQAsync(Test test);
        Task DeleteWithLQAsync(int testId);
    }
}
