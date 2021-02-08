using System.Configuration;

namespace AutoUIDemo.UIAuto
{
    public class UIAutoSection : ConfigurationSection
    {
        [ConfigurationProperty("Tabs")]
        [ConfigurationCollection(typeof(TabCollection), AddItemName = "Tab")]
        public TabCollection Tabs
        {
            get => base["Tabs"] as TabCollection;
        }

        public static UIAutoSection GetUISection()
        {
            return ConfigurationManager.GetSection("UIAuto") as UIAutoSection;
        }
    }
}
