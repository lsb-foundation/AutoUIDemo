using AutoUIDemo.UIAuto;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace AutoUIDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UIAutoSection.GetUISection() is UIAutoSection section)
                {
                    section.UIAutoActionInvoked += Section_UIAutoActionInvoked;
                    if(section.Build() is TabControl tab)
                    {
                        this.Content = tab;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Section_UIAutoActionInvoked(UIAutoActionEventArgs e)
        {
            try
            {
                string convertedFormat = e.Action.Format;
                MatchCollection matches = Regex.Matches(e.Action.Format, @"{\w+}");
                foreach (Match match in matches)
                {
                    string parameterName = match.Value.Trim('{', '}');
                    if (!(e.Parameters.FirstOrDefault(p => p.Name == parameterName) is ParameterElement parameter))
                    {
                        throw new Exception($"参数{parameterName}未找到。");
                    }
                    
                    if (string.IsNullOrWhiteSpace(parameter.Value))
                    {
                        throw new Exception($"参数{parameter.Description}为空。");
                    }
                    convertedFormat = convertedFormat.Replace(match.Value, parameter.Value);
                }
                MessageBox.Show(convertedFormat);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
