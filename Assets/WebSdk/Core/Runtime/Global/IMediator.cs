namespace WebSdk.Core.Runtime.Global
{
    public interface IMediator
    {
        void Notify(object sender, string ev);
        void DoWork();
    }
}