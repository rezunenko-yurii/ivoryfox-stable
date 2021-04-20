namespace GlobalBlock.Interfaces
{
    public interface IMediator
    {
        void Notify(object sender, string ev);
        void Start();
    }
}