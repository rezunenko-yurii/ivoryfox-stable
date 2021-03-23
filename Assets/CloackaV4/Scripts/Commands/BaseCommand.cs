namespace CloackaV4.Scripts.Commands
{
    public abstract class BaseCommand
    {
        protected abstract void OnCommandSuccess(string obj);
        protected abstract void OnCommandFailed(string obj);
        protected abstract void Subscribe();
        protected abstract void UnSubscribe();
    }
}