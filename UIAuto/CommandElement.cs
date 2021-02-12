using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AutoUIDemo.UIAuto
{
    public class CommandElement : ConfigurationElement, IBuildControl
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

        [ConfigurationProperty("CommandType")]
        public string CommandType
        {
            get => base["CommandType"] as string;
            set => base["CommandType"] = value;
        }

        [ConfigurationProperty("Parameters")]
        [ConfigurationCollection(typeof(ParameterCollection), AddItemName = "Parameter")]
        public ParameterCollection Parameters
        {
            get => this["Parameters"] as ParameterCollection;
        }

        [ConfigurationProperty("Actions")]
        [ConfigurationCollection(typeof(ActionCollection), AddItemName = "Action")]
        public ActionCollection Actions
        {
            get => this["Actions"] as ActionCollection;
        }

        internal event Action<ActionElement> CommandButtonClicked;
        private readonly List<TextBox> _textBoxList = new List<TextBox>();

        public IEnumerable<DependencyObject> Build()
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int index = 0; index < Parameters.Count; index++)
            {
                ParameterElement parameter = Parameters[index];
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                IEnumerable<DependencyObject> parameterControls = parameter.Build();
                Label label = parameterControls.FirstOrDefault(c => c is Label) as Label;
                TextBox textBox = parameterControls.FirstOrDefault(c => c is TextBox) as TextBox;
                grid.Children.Add(label);
                grid.Children.Add(textBox);
                Grid.SetRow(label, index);
                Grid.SetRow(textBox, index);
                Grid.SetColumn(label, 0);
                Grid.SetColumn(textBox, 1);
                _textBoxList.Add(textBox);
            }

            for (int index = 0; index < Actions.Count; index++)
            {
                ActionElement action = Actions[index];
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                action.SetCommand(this);
                Button button = action.Build().FirstOrDefault() as Button;
                button.Click += Button_Click;
                grid.Children.Add(button);
                Grid.SetRow(button, Parameters.Count + index);
                Grid.SetColumn(button, 0);
                Grid.SetColumnSpan(button, 2);
            }

            return new DependencyObject[] { grid };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is ActionElement action)
            {
                foreach (ParameterElement parameter in Parameters)
                {
                    if (_textBoxList.FirstOrDefault(box => box.Name == parameter.Name + "TextBox") is TextBox textBox)
                    {
                        parameter.Value = textBox.Text;
                    }
                }
                CommandButtonClicked?.Invoke(action);
            }
        }
    }
}
