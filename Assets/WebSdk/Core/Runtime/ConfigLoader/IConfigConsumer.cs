namespace WebSdk.Core.Runtime.ConfigLoader
{
    public interface IConfigConsumer
    {
        string ConfigName { get; }
        void SetConfig(string json);
    }
}