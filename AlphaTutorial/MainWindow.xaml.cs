using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using AlphaTab.Importer;
using AlphaTab.Model;
using AlphaTab.Core;
using Microsoft.Win32;
using Notation_to_Tab;

namespace AlphaTabTutorial
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        

        private Visibility _loadingIndicatorVisibility = Visibility.Collapsed;
        public Visibility LoadingIndicatorVisibility
        {
            get => _loadingIndicatorVisibility;
            set
            {
                if (value == _loadingIndicatorVisibility) return;
                _loadingIndicatorVisibility = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void loadTex() 
        {

            AlphaTab.Api.Tex(Program.MidiToTabTex());
        }

        private void OnAlphaTabLoaded(object sender, RoutedEventArgs e)
        {
            AlphaTab.Api.RenderStarted.On(e =>
            {
                LoadingIndicatorVisibility = Visibility.Visible;
            });

            AlphaTab.Api.RenderFinished.On(e =>
            {
                LoadingIndicatorVisibility = Visibility.Collapsed;
            });
        }

        private void OnOpenClick(object sender, RoutedEventArgs e)
        {
            loadTex();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
