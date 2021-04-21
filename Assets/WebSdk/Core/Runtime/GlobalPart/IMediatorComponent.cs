namespace WebSdk.Core.Runtime.GlobalPart
{
    public interface IMediatorComponent
    {
        IMediator mediator { get; }
        void SetMediator(IMediator mediator);
    }
}