﻿using System;

namespace WebBlock.ivoryfox.websdk.parameters.Runtime
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