using System;

namespace WebSdk.Core.Runtime.WebCore
{
    public class IdAttribute: Attribute
    {
        private string id;
        
        public IdAttribute(string id)
        {
            this.id = id;
        }
        public bool IsEquals(string value)
        {
            return id.Equals(value);
        }
    }
}