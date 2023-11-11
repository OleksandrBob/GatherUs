namespace GatherUs.DAL.Context;

public class ConnectionStrings : IConnectionStrings
{
    public string ConnectionString => MainDB;

    public string MainDB { get; set; }
}