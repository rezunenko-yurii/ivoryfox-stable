namespace WebSdk.Core.Runtime.Global
{
    public interface IMediatorComponent
    {
        IMediator Mediator { get; }
        void SetMediator(IMediator mediator);
    }
}