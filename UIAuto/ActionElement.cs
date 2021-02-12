using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace AutoUIDemo.UIAuto
{
    public class ActionElement : ConfigurationElement, IBuildControl
    {
        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name
        {
            get => base["Name"] as string;
            set => base["Name"] = value;
        }

        [ConfigurationProperty("Description", IsRequired = true)]
        public string Description
        {
            get => base["Description"] as string;
            set => base["Description"] = value;
        }

        [ConfigurationProperty("Format", IsRequired = true)]
        public string Format
        {
            get => base["Format"] as string;
            set => base["Format"] = value;
        }

        private CommandElement _command;
        public CommandElement Command { get => _command; }

        public IEnumerable<DependencyObject> Build()
        {
            Button commandButton = new Button
            {
                Content = Description,
                Tag = this
            };
            return new DependencyObject[] { commandButton };
        }

        public void SetCommand(CommandElement command) => _command = command;
    }
}
