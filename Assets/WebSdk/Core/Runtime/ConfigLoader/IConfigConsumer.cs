using System.Collections.Generic;

namespace GlobalBlock.Interfaces
{
    public interface IConfigConsumer
    {
        string ConfigName { get; }
        void SetConfig(string json);
    }
}