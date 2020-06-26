﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text.Json;

namespace JobFilterGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            startButton.IsEnabled = false;
            statusLabel.Content = "Load config ....";

            const string settingFileName = "setting.json";
            ConfigLoader configLoader = new ConfigLoader
            {
                SettingFileName = settingFileName
            };
            if (!configLoader.Load())
            {
                startButton.IsEnabled = true;
                statusLabel.Content = "Load config fail !";
                return;
            }
            statusLabel.Content = "Load config done !";
        }
    }
}