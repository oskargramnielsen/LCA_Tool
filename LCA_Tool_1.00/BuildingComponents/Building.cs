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
using LCA_Tool;

namespace LCA_Tool.BuildingComponents
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class Building
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc).WhereElementIsNotElementType();

            StringBuilder sb = new StringBuilder(); // stringbuilder sættes op til at kunne tilføje tekst til en liste

            double gfa = 0;
            string test = "";
            string gfaMeters = "";
            double areaTot = 0;

            foreach (Element element in collector)
            {
                try // try bruges da ellers mange elementer ville give fejl hvis man prøver at tage deres categori, hvis de ikke har en.
                {
                    if (element.Category.Name == "Floors")
                    {

                        //TOTAL FLOOR AREA //

                       Floor floor = element as Floor;

                        double area = Math.Round(UnitUtils.Convert(floor.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble(),
                            DisplayUnitType.DUT_SQUARE_FEET, DisplayUnitType.DUT_SQUARE_METERS), 1);

                        areaTot = area + areaTot;

                        //sb.AppendLine(string.Format("floor id: {0}", element.Id.ToString()) + Environment.NewLine);

                    }

                    else if (element.Category.Name == "Rooms")
                    {

                        gfa = gfa + element.get_Parameter(BuiltInParameter.ROOM_AREA).AsDouble();

                        gfaMeters = Math.Round(UnitUtils.Convert(gfa, DisplayUnitType.DUT_SQUARE_FEET, DisplayUnitType.DUT_SQUARE_METERS), 1).ToString();

                    }
                }

                catch
                {

                }

            }


            //----------------------------------------------------------------------------------------------------------------------------------//
            //Building:

            string id_building = Guid.NewGuid().ToString("N");
            string scenarioName = "TEST123";
            string locked = "Unlocked";
            string description = "Unknown";
            string building_type = "Other";
            string heated_floor_area = gfaMeters;
            string gross_area = gfaMeters;// not correct - should be updated according to definition
            string gross_area_above_ground = gfaMeters; // not correct - should be updated according to definition
            string storeys_above_ground = "Unknown";// Not found
            string storeys_below_ground = "Unknown"; // Not found
            string storey_height = "2800"; // Not found
            string initial_year = "2023";  // Not found
            string calculation_timespan = "50";
            string calculation_mode = "Normal";
            string outside_area = "Unknown"; // Not found
            string plot_area = "Unknown";// Not found
            string energy_class = "Unknown"; // Not found


            string building_project = "Building: " + Environment.NewLine + "ID: " + id_building + Environment.NewLine + "Scenario name: " +
                scenarioName + Environment.NewLine + "Description: " + description + Environment.NewLine + "Building Type: " + building_type
                + Environment.NewLine + "Heated floor area: " + heated_floor_area;

            //----------------------------------------------------------------------------------------------------------------------------------//
            //Project:

            string id_project = "123456789";
            string name = "TEST123";
            string address = "Testvej 1, 1111 Testbyen";
            string owner = "TestOwner";
            string lca_advisor = "test";
            string building_regulation_version = "2022";

            string project = "Project: " + Environment.NewLine + "ID: " + id_project + Environment.NewLine + "Name: " + name +
                Environment.NewLine + "Address: " + address + Environment.NewLine + "Owner: " + owner + Environment.NewLine + "LCA supervisor: " +
                lca_advisor + Environment.NewLine + "BR version: " + building_regulation_version;
            // see appendix B6 in json guide for template

            //----------------------------------------------------------------------------------------------------------------------------------//


            //sb.AppendLine(building_project + Environment.NewLine + Environment.NewLine + Environment.NewLine + project);

            sb.AppendLine("Building quantities successfully exported to LCAbyg");

            TaskDialog.Show("Message", sb.ToString());

            string building_info = "GFA," + gfaMeters + ",Total area," + areaTot.ToString();

            File.WriteAllText(@"C:\Users\oskar\OneDrive - Danmarks Tekniske Universitet\Kandidat Speciale\Programmering\RevitQuantities\Building.txt", building_info); // gem som tekstfil, indsæt egen sti



            return Result.Succeeded;
        }
    }
}
