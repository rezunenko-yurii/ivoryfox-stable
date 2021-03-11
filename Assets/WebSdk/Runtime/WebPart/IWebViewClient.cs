namespace GlobalBlock.Interfaces.WebPart
{
    public interface IWebViewClient: IModule, IMediatorComponent
    {
        void Open(string url);
        void SetSettings();
    }
}