using System;

namespace CloackaV4.Scripts.Commands
{
    public interface ICommand
    {
        event Action<ResponseModel> OnResult;
        void DoCommand();
    }
}