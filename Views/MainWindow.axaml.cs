using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Platform;
using Avalonia.Threading;
using Avalonia.Win32.Interop;
using LLDock.ViewModels;
using ReactiveUI;

namespace LLDock.Views;
public partial class MainWindow : Window
{
    #region Variables

    //private Point _lockedPosition;

    public static int DisplayHeight;
    
    public DispatcherTimer PanelHidingTimer;
    public static event Action PanelShowsEvent;

    public static void RaiseStaticEvent()
    {
        PanelShowsEvent?.Invoke();

    }

    #endregion
    
    
    
    public MainWindow()
    {
        InitializeComponent();
        this.Position = new PixelPoint(0, MainWindowViewModel.TopMargin);
        
        PanelHidingTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        PanelHidingTimer.Tick += HidePanel;
        //_lockedPosition = new Point(0, 300); // set the desired position
        //PositionChanged += Window_PositionChanged;
        DisplayHeight = Screens.Primary.Bounds.Height;
        
    }

    #region Button commands

    public ICommand StartApplicationCommand { get; set; }

    #endregion

    /*private void WindowBase_OnPositionChanged(object? sender, PixelPointEventArgs e)
    {
        this.Position = new PixelPoint((int)_lockedPosition.X, (int)_lockedPosition.Y);
        //PositionChanged="WindowBase_OnPositionChanged"
    }*/

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
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error while starting: {ex.Message}");
        }
    }

    private void InputElement_OnPointerExited(object? sender, PointerEventArgs e)
    {
        PanelHidingTimer.Start();
        // panel hiding
    }

    private void HidePanel(object sender, EventArgs e)
    {
        PanelHidingTimer.Stop();
        //RaiseStaticEvent();
        this.Position = new PixelPoint(-87, MainWindowViewModel.TopMargin);
    }

    private void InputElement_OnPointerEntered(object? sender, PointerEventArgs e)
    {
        this.Position = new PixelPoint(0, MainWindowViewModel.TopMargin);
        RaiseStaticEvent();
        var newButton = new Button
        {
            Height = 70,
            Width = 70,
            CornerRadius = new CornerRadius(15),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0,5,0,0)
        };
        WrapPanel.Children.Add(newButton);
    }

    #endregion
    
}