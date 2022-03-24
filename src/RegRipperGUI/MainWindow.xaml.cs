using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RegRipperGUI.DTOs;
using RegRipperGUI.Extensions;
using RegRipperGUI.Handlers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace RegRipperAndAddIn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int SleepTime { get; } = 400;
        public bool CancelRequested { get; } = false;

        private string pathRegRipper = "";

        private Settings settings;

        private readonly BackgroundWorker workerTerminal = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            settings = new Settings();
            settings.PathRegRipper = pathRegRipper;
            settings.PathOuputFileDumpMemory = txtPathOuputFileDumpMemory.Text;
            System.IO.File.WriteAllText("settings.json", JsonConvert.SerializeObject(settings));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (System.IO.File.Exists("settings.json"))
            {
                settings = JsonConvert.DeserializeObject<Settings>(System.IO.File.ReadAllText("settings.json"));
                txtPathOuputFileDumpMemory.Text = settings.PathOuputFileDumpMemory;
                if (settings.PathOuputFileDumpMemory != String.Empty)
                {
                    trwOutPutFiles.ItemsSource = new List<RegRipperGUI.DTOs.File>() { FileHandler.Process(settings.PathOuputFileDumpMemory, RegRipperGUI.Enumerations.ProcessMode.Parallel) };
                }
            }
            pathRegRipper = $"{AppDomain.CurrentDomain.BaseDirectory}RegRipper";
            if (!System.IO.Directory.Exists(pathRegRipper))
            {
                System.IO.Directory.CreateDirectory(pathRegRipper);
                workerTerminal.DoWork += workerDownloader_DoWork;
                workerTerminal.RunWorkerCompleted += workerDownloader_RunWorkerCompleted;
                workerTerminal.RunWorkerAsync(new { path = pathRegRipper });
                dlhNotify.IsOpen = true;
            }
        }

        private void workerDownloader_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            dlhNotify.IsOpen = false;
        }

        private void workerDownloader_DoWork(object? sender, DoWorkEventArgs e)
        {
            string pathRegRipper = ((dynamic)e.Argument).path;
            var contentsUrl = $"https://api.github.com/repos/keydet89/RegRipper3.0/contents";
            DownloadFiles(contentsUrl, pathRegRipper);
        }

        private void DownloadFiles(string? contentsUrl, string? pathRegRipper)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApplication", "1"));
            var contentsJson = httpClient.GetStringAsync(contentsUrl).Result;
            var contents = (JArray)JsonConvert.DeserializeObject(contentsJson);
            foreach (var file in contents)
            {
                var fileType = (string)file["type"];
                if (fileType == "dir")
                {
                    var directoryContentsUrl = (string)file["url"];
                    // use this URL to list the contents of the folder
                    Console.WriteLine($"DIR: {directoryContentsUrl}");
                    var relativePath = pathRegRipper + directoryContentsUrl.Replace(contentsUrl, "").Replace("?ref=master", string.Empty).Replace("/", "\\");
                    if (!System.IO.Directory.Exists(relativePath))
                        System.IO.Directory.CreateDirectory(relativePath);
                    DownloadFiles(directoryContentsUrl, relativePath);
                }
                else if (fileType == "file")
                {
                    var downloadUrl = (string)file["download_url"];
                    var name = (string)file["name"];
                    Console.WriteLine($"DOWNLOAD: {downloadUrl}");
                    DownloadFile(pathRegRipper, name, downloadUrl);
                }
            }
        }

        private void DownloadFile(string? pathRegRipper, string? name, string? downloadFileUrl)
        {
            var destinationFilePath = System.IO.Path.GetFullPath("file.zip");
            var processMsgHander = new ProgressMessageHandler(new HttpClientHandler());

            processMsgHander.HttpSendProgress += (sender, e) =>
            {
                //add your codes base on e.BytesTransferred and e.ProgressPercentage
            };

            lblMessage.Dispatcher.Invoke(new System.Action(() =>
            {
                lblMessage.Content = $"File downloding '{name}'";
            }));

            processMsgHander.HttpReceiveProgress += (sender, e) =>
            {
                pgbNotification.Dispatcher.Invoke(new System.Action(() =>
                {
                    pgbNotification.Value = e.ProgressPercentage;
                }));
            };
            var client = new HttpClient(processMsgHander);
            byte[] fileBytes = client.GetByteArrayAsync(downloadFileUrl).Result;
            System.IO.File.WriteAllBytes($"{pathRegRipper}\\{name}", fileBytes);
        }

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
                    var items = RegRipperGUI.Handlers.RegRipperHandler.AddIns(pathRegRipper);
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
                        if (item.IsSelected)
                        {
                            var command = $"{pathRegRipper}\\rip.exe";
                            var parametros = $"-r {selectedNode.PathFullName} -p {item.Name}";
                            body.Append(RegRipperHandler.RunAddIn(selectedNode, item, command, parametros));
                        }
                    }
                    if (body.ToString() != string.Empty)
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

        private void txtFilter_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
                try
                {
                    var selectedNode = trwOutPutFiles.SelectedItem as RegRipperGUI.DTOs.File;
                    if (selectedNode != null && !selectedNode.IsFolder)
                    {
                        var items = RegRipperGUI.Handlers.RegRipperHandler.AddIns(pathRegRipper);
                        items = items.Where(c => c.Filters.Contains("All") || c.Filters.Contains(selectedNode.Name, StringComparer.CurrentCultureIgnoreCase)).ToList();
                        if(txtFilter.Text.IsNotEmpty())
                            items = items.Where(c => txtFilter.Text == "*" || c.Description.ToLower().Contains(txtFilter.Text.ToLower())).ToList();
                        trwAddIns.ItemsSource = items;
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