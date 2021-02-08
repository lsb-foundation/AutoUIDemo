using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
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

        [ConfigurationProperty("Description")]
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

        public event Action<object> CommandButtonClicked;
        private readonly List<TextBox> _textBoxList = new List<TextBox>();
        private readonly List<KeyValuePair<string, string>> _parameterDescriptions = new List<KeyValuePair<string, string>>();

        public DependencyObject Build()
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int index = 0; index < Parameters.Count; index++)
            {
                ParameterElement parameter = Parameters[index];
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                Label label = new Label { Content = string.IsNullOrWhiteSpace(parameter.Description) ? parameter.Name : parameter.Description };
                TextBox textBox = new TextBox() { Name = parameter.Name + "TextBox" };
                grid.Children.Add(label);
                grid.Children.Add(textBox);
                Grid.SetRow(label, index);
                Grid.SetRow(textBox, index);
                Grid.SetColumn(label, 0);
                Grid.SetColumn(textBox, 1);
                _textBoxList.Add(textBox);
                _parameterDescriptions.Add(new KeyValuePair<string, string>(parameter.Name, parameter.Description));
            }

            for (int index = 0; index < Actions.Count; index++)
            {
                ActionElement action = Actions[index];
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                Button button = new Button
                {
                    Content = string.IsNullOrWhiteSpace(action.Description) ? action.Name : action.Description,
                    Tag = action.Format
                };
                button.Click += Button_Click;
                grid.Children.Add(button);
                Grid.SetRow(button, Parameters.Count + index);
                Grid.SetColumn(button, 0);
                Grid.SetColumnSpan(button, 2);
            }

            return grid;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string format = (sender as Button).Tag as string;
                string convertedFormat = format;
                MatchCollection matches = Regex.Matches(format, @"{\w+}");
                foreach (Match match in matches)
                {
                    string parameter = match.Value.Trim('{', '}');
                    TextBox textBox = _textBoxList.FirstOrDefault(box => box.Name == parameter + "TextBox");
                    if (textBox == null)
                    {
                        throw new Exception($"参数{parameter}未找到。");
                    }
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        var parameterKeyValuePair = _parameterDescriptions.FirstOrDefault(kv => kv.Key == parameter);
                        string parameterDescription = parameterKeyValuePair.Equals(default(KeyValuePair<string, string>)) ? parameter : parameterKeyValuePair.Value;
                        throw new Exception($"参数{parameterDescription}为空。");
                    }
                    convertedFormat = convertedFormat.Replace(match.Value, match.Value.Replace(parameter, textBox.Text.Trim()));
                }
                CommandButtonClicked?.Invoke(convertedFormat);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
