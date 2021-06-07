using System.Collections.Generic;

namespace WebSdk.Core.Runtime.GlobalPart
{
    public interface IGlobalBlock
    {
        void TryLoadConfigs(bool hasConnection);
        void InitConfigs(Dictionary<string, string> configs);
    }
}