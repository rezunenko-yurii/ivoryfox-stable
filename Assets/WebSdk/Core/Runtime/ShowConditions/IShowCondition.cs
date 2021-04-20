namespace WebSdk.Runtime.ShowConditions
{
    public interface IShowCondition
    {
        string ParamName { get; }

        public bool Compare();
    }
}