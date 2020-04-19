using System;
using System.Threading;
using System.Windows;
using Pomodoro.Src;

namespace Pomodoro
{
    /// <summary>
    /// Interaction logic for Greetings.xaml
    /// </summary>
    public partial class Greetings : Window
    {
        public Greetings()
        {
            InitializeComponent();
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("button clicked");
            var pomodoro = Pomodoro.Src.Pomodoro._pomodoro;
            var interval = new TimeSpan(0, 0, 1);
            var nextTick = pomodoro + interval;
            while (true)
            {
                while (pomodoro < nextTick)
                {
                    Thread.Sleep(nextTick - pomodoro);
                }

                nextTick += interval;
                // Notice we're adding onto when the last tick was supposed to be, not when it is now
                // Insert tick() code here
                Console.Write("{0} interval:{1}\n", nextTick, interval);

            }
        }
    }
}