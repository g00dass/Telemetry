namespace DataLayer
{
    public interface IMongoDbSettings
    {
        public string ConnectionString { get; }
        public string DatabaseName { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string ReplicaSet { get; set; }
    }

    public class MongoDbSettings : IMongoDbSettings
    {
        public string ConnectionString =>
            (string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Password))
                ? $@"mongodb://{Host}:{Port}connect=replicaSet&replicaSet={ReplicaSet}&authSource=admin"
                : $@"mongodb://{User}:{Password}@{Host}:{Port}?connect=replicaSet&replicaSet={ReplicaSet}&authSource=admin";
        
        public string DatabaseName { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string ReplicaSet { get; set; }
    }
}
