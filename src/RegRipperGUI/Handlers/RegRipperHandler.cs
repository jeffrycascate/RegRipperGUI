using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegRipperGUI.Handlers
{
    public class RegRipperHandler
    {
        internal static List<DTOs.AddIn>  AddIns(string path)
        {
            List<DTOs.AddIn> items = new List<DTOs.AddIn>(); 

            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = $"{path}\\rip.exe";
            process.StartInfo.Arguments = "-l";
            process.StartInfo.WorkingDirectory = "";
            process.Start();
            var addIns = process.StandardOutput.ReadToEnd().Split(new String[] { "\r\n\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in addIns)
            {
                var array = item.Split(new String[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
                items.Add(new DTOs.AddIn { Name = array[0].Split(" ")[1].Trim() , Filters  = Filters(array[0].Split(" ")[3].Trim()) ,  Description = array[1].Trim().Replace("- ", "") });
            }
            return items.OrderByDescending( c=> c.Name).ToList() ;   
        }

        private static List<string> Filters(string v)
        {
            return new List<string>( v.Replace("[", "").Replace("]", "").Split(",", StringSplitOptions.RemoveEmptyEntries));   
        }

        internal static string RunAddIn(DTOs.File selectedNode, DTOs.AddIn item, string command, string parametros)
        {
            StringBuilder body = new StringBuilder();   
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = command;
            process.StartInfo.Arguments = parametros;
            process.StartInfo.WorkingDirectory = "";
            process.Start();
            body.AppendLine($"Path del fichero extraccion: {selectedNode.PathFullName}, AddIn usado: {item.Name} -Description:'{item.Description}'");
            body.AppendLine(process.StandardOutput.ReadToEnd());
            process.Close();
            return body.ToString();
        }
    }
}
