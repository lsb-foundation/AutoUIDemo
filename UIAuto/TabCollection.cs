﻿using System.Configuration;

namespace AutoUIDemo.UIAuto
{
    public class TabCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TabElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as TabElement).Header;
        }
    }
}
