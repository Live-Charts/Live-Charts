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
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Assets.Commands;
using Samples.Wpf.Views;

#endregion

namespace Samples.Wpf
{
    public class NavigationHandler : INotifyPropertyChanged
    {
        private static readonly Type[] Types = typeof(NavigationHandler).Assembly.GetTypes();
        private bool _isMenuHidden;
        private FrameworkElement _currentView;

        public NavigationHandler()
        {
            CurrentView = new Menu();
            NavigateTo = new DelegateCommand(o =>
            {
                var path = (string) o;
                _navigateTo(path);
            });
            ShowMenu = new DelegateCommand(o =>
            {
                _showMenu();
            });
        }

        public FrameworkElement CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public bool IsMenuHidden
        {
            get => _isMenuHidden;
            set
            {
                var changed = _isMenuHidden != value;
                if (!changed) return;
                _isMenuHidden = value;
                OnPropertyChanged(nameof(IsMenuHidden));
            }
        }

        public ICommand NavigateTo { get; }
        public ICommand ShowMenu { get; }

        private void _navigateTo(string view)
        {
            var targetType = Types.First(x => x.Name == view);
            var newView = (FrameworkElement) Activator.CreateInstance(targetType);
            CurrentView = newView;
            IsMenuHidden = view != "Menu";
        }

        private void _showMenu()
        {
            _navigateTo("Menu");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}