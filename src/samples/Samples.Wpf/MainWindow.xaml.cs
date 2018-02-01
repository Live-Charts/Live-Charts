using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Assets.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Samples.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
             InitializeComponent();

            MouseDown += OnMouseDown;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
