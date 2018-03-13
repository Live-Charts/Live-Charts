#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Assets.Commands;
using Samples.Wpf.Views;

#endregion

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
            NavigationHandler = new NavigationHandler();
            DataContext = NavigationHandler;
        }

        public NavigationHandler NavigationHandler { get; set; }
    }

    public class NavigationHandler : INotifyPropertyChanged
    {
        private static readonly Type[] Types = typeof(NavigationHandler).Assembly.GetTypes();

        public NavigationHandler()
        {
            History = new List<FrameworkElement>
            {
                new Menu()
            };
            NavigateTo = new DelegateCommand(o =>
            {
                var path = (string) o;
                _navigateTo(path);
            });
            GoBack = new DelegateCommand(o => _goBack());
        }

        public FrameworkElement CurrentView => History[History.Count - 1];

        public List<FrameworkElement> History { get; }

        public ICommand NavigateTo { get; private set; }
        public ICommand GoBack { get; private set; }

        private void _navigateTo(string view)
        {
            var targetType = Types.First(x => x.Name == view);
            if (History.Last().GetType() == targetType) return;
            History.Add((FrameworkElement) Activator.CreateInstance(targetType));
            OnPropertyChanged(nameof(CurrentView));
        }

        private void _goBack()
        {
            var previousIndex = History.Count - 1;
            if (previousIndex <= 0) return;
            History.RemoveAt(History.Count - 1);
            OnPropertyChanged(nameof(CurrentView));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
