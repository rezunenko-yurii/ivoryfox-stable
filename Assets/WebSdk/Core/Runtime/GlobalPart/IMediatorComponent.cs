namespace WebSdk.Core.Runtime.GlobalPart
{
    public interface IMediatorComponent
    {
        IMediator Mediator { get; }
        void SetMediator(IMediator mediator);
    }
}