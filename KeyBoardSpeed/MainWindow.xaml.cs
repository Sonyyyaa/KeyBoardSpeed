using System;
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
using System.Windows.Threading;

namespace KeyBoardSpeed
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;
        }

        int currentLetter = 0;
        private void Timer_Tick(object? sender, EventArgs e)
        {
            letter.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            TextRange textRange = new TextRange(texttocopy.Document.ContentStart, texttocopy.Document.ContentEnd);
            letter.Text = textRange.Text[currentLetter].ToString();
            currentLetter++;
        }

        string text;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            text = File.ReadAllText("text.txt");
            texttocopy.AppendText(text);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
            }
            else
            {
                timer.Start();
            }
        }
        int score = 0;
        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextRange textRange = new TextRange(typeText.Document.ContentStart, typeText.Document.ContentEnd);
            if(textRange.Text.Trim('\n', '\r').Last() == letter.Text.Last())
            {
                score++;
                scoreChange.Content = score.ToString();
                letter.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            else if (textRange.Text.Trim('\n', '\r').Last() != letter.Text.Last())
            {
                score--;
                scoreChange.Content = score.ToString();
                letter.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));

            }
        }
    }
}
