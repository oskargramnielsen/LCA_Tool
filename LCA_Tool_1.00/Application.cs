using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Autodesk.Revit.UI;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.IO;


namespace LCA_Tool
{
    public class Application : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            RibbonPanel panel = RibbonPanel(application);
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            if (panel.AddItem(new PushButtonData("Export", "Export", thisAssemblyPath, "LCA_Tool.Command"))
            is PushButton button)
            {
                button.ToolTip = "Export";

                Uri uri = new Uri(Path.Combine(Path.GetDirectoryName(thisAssemblyPath), "Resources", "LCA_logo.ico"));
                BitmapImage bitmap = new BitmapImage(uri);
                button.LargeImage = bitmap;

            }
            
            if (panel.AddItem(new PushButtonData("Open LCAbyg", "Open LCAbyg", thisAssemblyPath, "LCA_Tool.OpenLCAbyg"))
            is PushButton button1)
            {
                button1.ToolTip = "Open LCAbyg";

                Uri uri = new Uri(Path.Combine(Path.GetDirectoryName(thisAssemblyPath), "Resources", "lcabyg.ico"));
                BitmapImage bitmap = new BitmapImage(uri);
                button1.LargeImage = bitmap;

            }

            return Result.Succeeded;

        }

        public RibbonPanel RibbonPanel(UIControlledApplication a)
        {
            string tab = "LCA Tools";
            RibbonPanel ribbonPanel = null;

            try
            {
                a.CreateRibbonTab(tab);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            try
            {
                a.CreateRibbonPanel(tab, "Export Data to LCAbyg");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            List<RibbonPanel> panels = a.GetRibbonPanels(tab);
            foreach (RibbonPanel p in panels.Where(p => p.Name == "Export Data to LCAbyg"))
            {
                ribbonPanel = p;
            }

            return ribbonPanel;
        }

    }
}