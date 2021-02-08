using AutoUIDemo.UIAuto;
using System;
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
                UIAutoSection section = UIAutoSection.GetUISection();
                if (section == null) return;

                Grid mainGrid = new Grid { Margin = new Thickness(8) };
                TabControl mainTab = new TabControl();
                foreach (TabElement tabElement in section.Tabs)
                {
                    TabItem tab = tabElement.Build() as TabItem;
                    foreach (GroupElement groupElement in tabElement.Groups)
                    {
                        GroupBox group = groupElement.Build() as GroupBox;
                        foreach (CommandElement commandElement in groupElement.Commands)
                        {
                            commandElement.CommandButtonClicked += o => MessageBox.Show(o as string);
                            Grid grid = commandElement.Build() as Grid;
                            (group.Content as StackPanel).Children.Add(grid);
                        }
                        (tab.Content as StackPanel).Children.Add(group);
                    }
                    mainTab.Items.Add(tab);
                }
                mainGrid.Children.Add(mainTab);
                this.Content = mainGrid;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
