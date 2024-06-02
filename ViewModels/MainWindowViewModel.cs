using System;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using LLDock.Views;

namespace LLDock.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    
#pragma warning restore CA1822 // Mark members as static

    #region Variables

    private int _panelLength;
    private static int _topMargin;
    public int DisplayHeight;
    private ObservableCollection<string> _runningApplications;

    public int PanelLength
    {
        get => _panelLength;
        set => this.RaiseAndSetIfChanged(ref _panelLength, value);
    }
    public static int TopMargin
    {
        get => _topMargin;
        set => _topMargin = value;
    }
    public ObservableCollection<string> RunningApplications
    {
        get => _runningApplications;
        set => this.RaiseAndSetIfChanged(ref _runningApplications, value);
    }

    #endregion

    public MainWindowViewModel()
    {
        StartApplicationCommand = ReactiveCommand
            .Create(() => StartApp("C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"));
        OpenSettingsWindowCommand = ReactiveCommand
            .Create(OpenSettingsWindow);
        MainWindow.PanelShowsEvent += CheckAppsRunning;
        
        ButtonBackColor = "#424242";
        PanelLength = 400;
        TopMargin = (MainWindow.DisplayHeight - PanelLength) / 2;
        
        RunningApplications = new ObservableCollection<string>();

    }
    #region Button commands

    public ICommand StartApplicationCommand { get; set; }
    public ICommand OpenSettingsWindowCommand { get; set; }
    

    #endregion
    #region Properties

    private string _buttonBackColor;
    public string ButtonBackColor
    {
        get => _buttonBackColor;
        set => this.RaiseAndSetIfChanged(ref _buttonBackColor, value);
    }
    
    #endregion
    #region Methods
    
    private void StartApp(string path)
    {
        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true // Це важливо для запуску GUI додатків
            };

            Process process = new Process
            {
                StartInfo = startInfo
            };
            process.Start();
            Views.MainWindow.RaiseStaticEvent();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error while starting: {ex.Message}");
        }
        GetRunningApps();
    }

    public void CheckAppsRunning()
    {
        var processes = Process.GetProcessesByName("chrome");
        if (processes.Any())
        {
            ButtonBackColor = "#888888";
        }
        else
        {
            ButtonBackColor = "#424242";
        }
    }

    private void GetRunningApps()
    {
        RunningApplications.Clear();

        // Get all running processes
        var processes = Process.GetProcesses();

        // Add process names to the collection
        foreach (var process in processes.OrderBy(p => p.ProcessName))
        {
            try
            {
                if (!string.IsNullOrEmpty(process.MainWindowTitle))
                {
                    RunningApplications.Add(process.MainWindowTitle);
                    Debug.WriteLine(process.ToString());
                    //Debug.WriteLine(process.ProcessName);
                }
            }
            catch
            {
                // Handle any exceptions thrown by accessing process properties
            }
        }
    }
    public void OpenSettingsWindow()
    {
        var settingWindow = new SettingsWindow();
        settingWindow.Show();
    }
    
    #endregion
    
    
}