using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace Nano_Avionics_Homework
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
        string filePath;
        string folderPath;
        
        OpenFileDialog openFileDialog = new OpenFileDialog()
        {
            Filter = "Text files (*.txt)|*.txt| Log file (*.log)|*.log|All files (*.*)|*.*",
            Title = "Export error lines"
        };

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = Path.GetFullPath(openFileDialog.FileName);
                folderPath = Path.GetDirectoryName(openFileDialog.FileName);
                SaveFile();
               
            }

        }

        private void SaveFile()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            try
            {

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(folderPath, "Error_msgs.txt")))
                using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (BufferedStream bs = new BufferedStream(fs))
                using (StreamReader sr = new StreamReader(bs))
                {
                    
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("error"))
                            outputFile.WriteLine(line);

                    }
                    outputFile.Close();
                    watch.Stop();
                    lblStatus.Content = "Done, task took "+watch.ElapsedMilliseconds/1000+" seconds";
                    Console.WriteLine("done");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                $"Details:\n\n{ex.StackTrace}");
            }
        }

    }
}
