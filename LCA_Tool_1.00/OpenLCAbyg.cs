using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;
using System.IO;
using Autodesk.Revit.DB.Analysis;
using BoundarySegment = Autodesk.Revit.DB.BoundarySegment;
using System.Text.RegularExpressions;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using LCA_Tool.BuildingComponents;
using System.Windows.Media.Imaging;

namespace LCA_Tool
{
    [Transaction(TransactionMode.Manual)]
    public class OpenLCAbyg : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            using (System.Diagnostics.Process pProcess = new System.Diagnostics.Process())
            {
                pProcess.StartInfo.FileName = @"C:\Program Files\SBi\LCAbyg 5 (64 bit) (5.2.1.0)\lcabyg.exe";
                pProcess.StartInfo.Arguments = ""; //argument
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
                pProcess.StartInfo.CreateNoWindow = false; //not diplay a windows
                pProcess.Start();
                string output = pProcess.StandardOutput.ReadToEnd(); //The output result
                pProcess.WaitForExit();
            }

            return Result.Succeeded;
        }
    }
}
