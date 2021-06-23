namespace WebSdk.Core.Runtime.GlobalPart
{
    public interface IMediator
    {
        void Notify(object sender, string ev);
        void DoWork();
    }
}