using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AutoUIDemo.UIAuto
{
    public class UIAutoSection : ConfigurationSection, IBuildControl
    {
        [ConfigurationProperty("Tabs")]
        [ConfigurationCollection(typeof(TabCollection), AddItemName = "Tab")]
        public TabCollection Tabs
        {
            get => base["Tabs"] as TabCollection;
        }

        public event Action<UIAutoActionEventArgs> UIAutoActionInvoked;

        public static UIAutoSection GetUISection()
        {
            return ConfigurationManager.GetSection("UIAuto") as UIAutoSection;
        }

        public IEnumerable<DependencyObject> Build()
        {
            TabControl tabControl = new TabControl();
            foreach (TabElement tabElement in Tabs)
            {
                TabItem tab = tabElement.Build().FirstOrDefault() as TabItem;
                foreach (GroupElement groupElement in tabElement.Groups)
                {
                    GroupBox group = groupElement.Build().FirstOrDefault() as GroupBox;
                    foreach (CommandElement commandElement in groupElement.Commands)
                    {
                        commandElement.CommandButtonClicked += act => UIAutoActionInvoked?.Invoke(new UIAutoActionEventArgs(act));
                        Grid grid = commandElement.Build().FirstOrDefault() as Grid;
                        (group.Content as StackPanel).Children.Add(grid);
                    }
                    (tab.Content as StackPanel).Children.Add(group);
                }
                tabControl.Items.Add(tab);
            }
            return new DependencyObject[] { tabControl };
        }
    }
}
