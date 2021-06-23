namespace WebSdk.Core.Runtime.GlobalPart
{
    public interface IModule
    {
        IModulesHost Parent { get; set; }
    }
}