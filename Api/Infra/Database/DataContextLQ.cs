using LinqToDB;
using LinqToDB.Data;

namespace Api.Infra.Database
{
    public class DataContextLQ : DataConnection
    {
        public DataContextLQ(string providerName, string connectionString)
            : base(providerName, connectionString)
        {
            (this as IDataContext).CloseAfterUse = true;
        }
    }
}
