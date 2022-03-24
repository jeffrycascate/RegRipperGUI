using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace RegRipperAndAddIn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            // Start a new process for the cmd
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = @"C:\Users\jeffr\Documents\Security\RegRipper3.0-master\rip.exe";
            process.StartInfo.Arguments = "-l";
            process.StartInfo.WorkingDirectory = "";
            process.Start();              
            var line = process.StandardOutput.ReadToEnd().Split(new String[] { "\r\n\r\n" }, System.StringSplitOptions.RemoveEmptyEntries) ;            

        }

        public int SleepTime { get; } = 400;
        public bool CancelRequested { get; } = false;
    }
}
