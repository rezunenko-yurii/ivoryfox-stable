using System.Collections.Generic;

namespace GlobalBlock.Interfaces
{
    public interface IGlobalBlock
    {
        void InitModules(IGlobalFactory factory);
        void TryLoadConfigs(bool hasConnection);
        void InitConfigs(Dictionary<string, string> configs);

        void ConnectToFacade();
    }
}