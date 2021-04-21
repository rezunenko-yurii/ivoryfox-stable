namespace WebSdk.Core.Runtime.ShowConditions
{
    public interface IShowCondition
    {
        string ParamName { get; }

        public bool Compare();
    }
}