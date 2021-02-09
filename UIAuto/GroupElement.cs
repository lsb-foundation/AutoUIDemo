﻿using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace AutoUIDemo.UIAuto
{
    public class GroupElement : ConfigurationElement, IBuildControl
    {
        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name
        {
            get => base["Name"] as string;
            set => base["Name"] = value;
        }

        [ConfigurationProperty("Header", IsRequired = true)]
        public string Header
        {
            get => base["Header"] as string;
            set => base["Header"] = value;
        }


        [ConfigurationProperty("Commands")]
        [ConfigurationCollection(typeof(CommandCollection), AddItemName = "Command")]
        public CommandCollection Commands
        {
            get => this["Commands"] as CommandCollection;
        }

        public DependencyObject Build()
        {
            StackPanel groupStackPanel = new StackPanel { Orientation = Orientation.Vertical };
            GroupBox groupBox = new GroupBox
            {
                Header = this.Header,
                Content = groupStackPanel
            };
            return groupBox;
        }
    }
}
