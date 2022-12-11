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
using Microsoft.Win32;
using System.Diagnostics;

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
        string folderText = "filesOfText";
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //text = File.ReadAllText("text.txt");
            //texttocopy.AppendText(text);
            if (!Directory.Exists(folderText))
            {
                Directory.CreateDirectory(folderText);
            }
            string [] allfiles = Directory.GetFiles(folderText);
           foreach (string file in allfiles)
            {
                string f = file.Substring(file.LastIndexOf('\\')+1);
                listbox.Items.Add(f);
            }
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

            if(string.IsNullOrEmpty(textRange.Text.Trim('\n', '\r')))
            {
                letter.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                return;
            }

            if(textRange.Text.Trim('\n', '\r').Last() == letter.Text.Last())
            {
                score++;
                scoreChange.Content = score.ToString();
                letter.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (speedValue != null)
            {
                speedValue.Text = ((int)speed.Value).ToString();
                timer.Interval = TimeSpan.FromSeconds((int)speed.Value);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "text file|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                //D:/Docs/Myfiles/data.txt
                string mainPath = openFileDialog.FileName;
                string file = mainPath.Substring(mainPath.LastIndexOf('\\')+1);

                File.Copy(mainPath, folderText+"/"+ file);

                listbox.Items.Add(file);
            }
            
            //MessageBox.Show(openFileDialog.FileName);
        }

        private void MenuItem_Delete(object sender, RoutedEventArgs e)
        {
            if(listbox.SelectedIndex != -1)
            {
                listbox.Items.RemoveAt(listbox.SelectedIndex);
            }
        }

        private void listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string path = folderText+"/"+ listbox.SelectedItem.ToString();
                texttocopy.AppendText(File.ReadAllText(path));
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
    
           
        }
    }
}
