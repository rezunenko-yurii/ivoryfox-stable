namespace GlobalBlock.Interfaces
{
    public interface IMediatorComponent
    {
        IMediator mediator { get; }
        void SetMediator(IMediator mediator);
    }
}