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
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {


            LCA_Tool.BuildingComponents.Walls walls = new LCA_Tool.BuildingComponents.Walls();
            LCA_Tool.BuildingComponents.Floors floors = new LCA_Tool.BuildingComponents.Floors();
            LCA_Tool.BuildingComponents.Roofs roofs = new LCA_Tool.BuildingComponents.Roofs();
            LCA_Tool.BuildingComponents.Ceilings ceilings = new LCA_Tool.BuildingComponents.Ceilings();
            LCA_Tool.BuildingComponents.Building building = new LCA_Tool.BuildingComponents.Building();


            walls.Execute(commandData, ref message, elements);
            floors.Execute(commandData, ref message, elements);
            roofs.Execute(commandData, ref message, elements);
            ceilings.Execute(commandData, ref message, elements);
            building.Execute(commandData, ref message, elements);

            //using (System.Diagnostics.Process pProcess = new System.Diagnostics.Process())
            //{
            //    pProcess.StartInfo.FileName = @"C:\Program Files\SBi\LCAbyg 5 (64 bit) (5.2.1.0)\lcabyg.exe";
            //    pProcess.StartInfo.Arguments = ""; //argument
            //    pProcess.StartInfo.UseShellExecute = false;
            //    pProcess.StartInfo.RedirectStandardOutput = true;
            //    pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
            //    pProcess.StartInfo.CreateNoWindow = false; //not diplay a windows
            //    pProcess.Start();
            //    string output = pProcess.StandardOutput.ReadToEnd(); //The output result
            //    pProcess.WaitForExit();
            //}

            return Result.Succeeded;
        }
    }
}


//return Result.Succeeded;