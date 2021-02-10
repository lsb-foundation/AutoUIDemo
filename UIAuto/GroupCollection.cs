using System.Configuration;

namespace AutoUIDemo.UIAuto
{
    public class GroupCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new GroupElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as GroupElement).Header;
        }
    }
}
