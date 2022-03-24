using Newtonsoft.Json;
using RegRipperGUI.DTOs;
using RegRipperGUI.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

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
        }

        public int SleepTime { get; } = 400;
        public bool CancelRequested { get; } = false;

        private void btnPathRegRipper_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog1.Description = "Select path base of the RegRippe";
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtPathRegRipper.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.PathRegRipper = txtPathRegRipper.Text;
            settings.PathOuputFileDumpMemory = txtPathOuputFileDumpMemory.Text;
            System.IO.File.WriteAllText("settings.json", JsonConvert.SerializeObject(settings));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists("settings.json"))
            {
                Settings settings = JsonConvert.DeserializeObject<Settings>(System.IO.File.ReadAllText("settings.json"));
                txtPathRegRipper.Text = settings.PathRegRipper;
                txtPathOuputFileDumpMemory.Text = settings.PathOuputFileDumpMemory;
                if (settings.PathOuputFileDumpMemory != String.Empty)
                {
                    trwOutPutFiles.ItemsSource = new List<RegRipperGUI.DTOs.File>() { FileHandler.Process(settings.PathOuputFileDumpMemory, RegRipperGUI.Enumerations.ProcessMode.Parallel) };
                }
            }
        }

        //private void btnLoadAddIns_Click(object sender, RoutedEventArgs e)
        //{
        //    var items = RegRipperGUI.Handlers.RegRipperHandler.AddIns(txtPathRegRipper.Text);
        //    trwAddIns.ItemsSource = items;
        //}

        private void btnPathOuputFileDumpMemory_Click(object sender, RoutedEventArgs e)
        {
        }

        private void trwSource_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
        }

        private void trwOutPutFiles_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                var selectedNode = trwOutPutFiles.SelectedItem as RegRipperGUI.DTOs.File;
                if (selectedNode != null && !selectedNode.IsFolder)
                {
                    var items = RegRipperGUI.Handlers.RegRipperHandler.AddIns(txtPathRegRipper.Text);
                    items = items.Where(c => c.Filters.Contains("All") || c.Filters.Contains(selectedNode.Name, StringComparer.CurrentCultureIgnoreCase)).ToList();
                    trwAddIns.ItemsSource = items;
                }
                else
                    trwAddIns.ItemsSource = null;
            }
            catch (Exception)
            {
            }
        }

        private void trwOutPutFiles_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }

        private void trwAddIns_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }

        private void trwAddIns_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedNode = trwOutPutFiles.SelectedItem as RegRipperGUI.DTOs.File;
                if (selectedNode != null && !selectedNode.IsFolder)
                {
                    StringBuilder body = new StringBuilder();
                    var items = trwAddIns.ItemsSource as List<RegRipperGUI.DTOs.AddIn>;
                    foreach (RegRipperGUI.DTOs.AddIn item in items)
                    {
                        if(item.IsSelected)
                        {
                            var command = $"{txtPathRegRipper.Text}\\rip.exe";
                            var parametros = $"-r {selectedNode.PathFullName} -p {item.Name}";
                            body.Append( RegRipperHandler.RunAddIn(selectedNode, item, command, parametros));
                        }
                    }
                    if(body.ToString() != string.Empty)
                        System.Windows.Clipboard.SetText(body.ToString());
                    //var items = RegRipperGUI.Handlers.RegRipperHandler.AddIns(txtPathRegRipper.Text);
                    //items = items.Where(c => c.Filters.Contains("All") || c.Filters.Contains(selectedNode.Name, StringComparer.CurrentCultureIgnoreCase)).ToList();
                    //trwAddIns.ItemsSource = items;
                }
                else
                    trwAddIns.ItemsSource = null;
            }
            catch (Exception)
            {
            }
        }
    }
}