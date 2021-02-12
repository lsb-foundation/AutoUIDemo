using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace AutoUIDemo.UIAuto
{
    public class TabElement : ConfigurationElement, IBuildControl
    {
        [ConfigurationProperty("Header", IsRequired = true)]
        public string Header
        {
            get => base["Header"] as string;
            set => base["Header"] = value;
        }

        [ConfigurationProperty("Groups", IsRequired = true)]
        [ConfigurationCollection(typeof(GroupCollection), AddItemName = "Group")]
        public GroupCollection Groups
        {
            get => this["Groups"] as GroupCollection;
        }

        public IEnumerable<DependencyObject> Build()
        {
            TabItem tabItem = new TabItem
            {
                Header = this.Header,
                Content = new StackPanel()
            };
            return new DependencyObject[] { tabItem };
        }
    }
}
