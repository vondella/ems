namespace Evently.Modules.Events.Infrastracture.Database;

public sealed   class MongoConfig
{
    public string ConnectionString { get; init; }
    public string Database { get; init; }
    public string Collection { get; init; }
}