namespace WebSdk.Core.Runtime.Global
{
    public interface IModule
    {
        IModulesHost Parent { get; set; }
    }
}