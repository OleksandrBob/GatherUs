using GatherUs.DAL.Constants;

namespace GatherUs.DAL.Context;

public class ConnectionStrings : IConnectionStrings
{
    public string MainDbEnvironment => Environment.GetEnvironmentVariable(DbConstants.PostgresConnectionString);
    
    public string MainDB { get; set; }
    
    public string ConnectionString => MainDbEnvironment ?? MainDB;
}