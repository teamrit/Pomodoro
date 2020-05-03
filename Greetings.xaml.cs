using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Pomodoro
{
    /// <summary>
    /// Interaction logic for Greetings.xaml
    /// </summary>
    public partial class Greetings : Window
    {
        public Greetings()
        {
            _minuteTextBlock = (TextBlock) FindName("minuteText");
            _secondTextBlock = (TextBlock) FindName("secondText");
            InitializeComponent();
            this.DataContext = this;
        }

        private TextBlock _minuteTextBlock;
        private TextBlock _secondTextBlock;
        public DateTime NextTick { get; set; }
        public DateTime Countdown { get; set; }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Window window = (Window) sender;
            window.Topmost = true;
        }

        private String AddZeroIfSingleDigit(int number)
        {
            if (number < 10)
            {
                return $"0{number}";
            }

            return $"{number}";
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            if (NextTick.Year != 1) return;
            var bc = new BrushConverter();
            var startButton = (Button) FindName("startButton");
            var startButtonText = (TextBlock) FindName("startButtonText");

            if (startButton != null) this.startButton.Background = (Brush) bc.ConvertFrom("#F0F66E");
            if (startButtonText != null) this.startButtonText.Text = "Pause";

            _minuteTextBlock = (TextBlock) FindName("minuteText");
            _secondTextBlock = (TextBlock) FindName("secondText");

            ThreadPool.QueueUserWorkItem(ignored =>
            {
                var pomodoro = Src.Pomodoro._pomodoro;
                var startTime = DateTime.Now;
                var interval = new TimeSpan(0, 0, 1);
                NextTick = DateTime.Now + interval;

                // infinite loop for now
                while (true)
                {
                    while (DateTime.Now < NextTick)
                    {
                        Thread.Sleep(NextTick - DateTime.Now);
                    }

                    NextTick += interval;
                    Countdown = pomodoro - (NextTick - startTime);
                    Thread.Sleep(100);
                    _minuteTextBlock.Dispatcher.BeginInvoke(
                        new Action(() => _minuteTextBlock.Text = AddZeroIfSingleDigit(Countdown.Minute)));
                    _secondTextBlock.Dispatcher.BeginInvoke(
                        new Action(() => _secondTextBlock.Text = AddZeroIfSingleDigit(Countdown.Second)));

                    Console.Write("{0}:{1}\n", Countdown.Minute, Countdown.Second);
                }
            });
        }

        // Minimize to system tray when application is minimized.
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized) this.Hide();

            base.OnStateChanged(e);
        }

        // Minimize to system tray when application is closed.
        protected override void OnClosing(CancelEventArgs e)
        {
            // setting cancel to true will cancel the close request
            // so the application is not closed
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
        }
    }
}