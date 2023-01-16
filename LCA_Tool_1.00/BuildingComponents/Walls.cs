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
using Autodesk.Revit.ApplicationServices;
using LCA_Tool.Containers;


namespace LCA_Tool.BuildingComponents
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class Walls
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc).WhereElementIsNotElementType();

            StringBuilder sb = new StringBuilder(); // stringbuilder sættes op til at kunne tilføje tekst til en liste



            int elementId = 0; // bruges til at angive væg nummer

            int[] df_id = new int[] { };
            string[] df_area = new string[] { };
            var wall_csv = new StringBuilder();

            string uniqueIdElement = Guid.NewGuid().ToString("N");

            wall_csv.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6}", "element id", "layer id", "type", "area (m2)",
                "layer width", "material name", "material id"));


            ElementContainers elementContainers = new ElementContainers();
            //ElementCounter elementCounter = new ElementCounter();

            int numOfWalls = 0;

            foreach (Element element in collector)
            {

                try // try bruges da ellers mange elementer ville give fejl hvis man prøver at tage deres categori, hvis de ikke har en.
                {
                    if (element.Category.Name == "Walls")
                    {
                        string materials = "";
                        string materialIds = "";
                        string uniqueIds = "";
                        string layerWidths = "";
                        string constructionId = "10a52123-48d7-466a-9622-d463511a6df0";

                        Wall wall = element as Wall;

                        string id = wall.Id.ToString();
                        string width = UnitUtils.Convert(Math.Round(wall.WallType.Width, 0), DisplayUnitType.DUT_DECIMAL_FEET, DisplayUnitType.DUT_MILLIMETERS).ToString();
                        double areaFeet = wall.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();
                        double area = Math.Round(UnitUtils.Convert(areaFeet, DisplayUnitType.DUT_SQUARE_FEET, DisplayUnitType.DUT_SQUARE_METERS), 1);

                        int layerId = 0; // bruges til at angive lag i væggen

                        string type = "Walls";


                        CompoundStructure structure = wall.WallType.GetCompoundStructure();
                        IList<CompoundStructureLayer> layers = structure.GetLayers();

                        elementId++;

                        foreach (CompoundStructureLayer layer in layers)
                        {

                            Material layerWallMaterial = doc.GetElement(layer.MaterialId) as Material;

                            string materialId = layerWallMaterial.LookupParameter("MaterialID").AsString(); // angiver materialeID'et

                            if (materialId == null)
                            {
                                materialId = "28185835-607b-5336-bb15-b10f3eb840e5";
                            }
                            else
                            {
                                numOfWalls++;
                            }


                            string material = layerWallMaterial.Name; // angiver materiale navnet
                            string uniqueId = Guid.NewGuid().ToString("N");
                            double layerWidth = Math.Round(UnitUtils.Convert(layer.Width, DisplayUnitType.DUT_DECIMAL_FEET,
                                DisplayUnitType.DUT_MILLIMETERS), 1);

                            layerId++;

                            string materialModified = material.Replace(",", ".");

                            ElementContainer elementContainer = new ElementContainer(type, elementId.ToString(), layerId.ToString(), materialId, material, area, layerWidth);

                            elementContainers.AddElementsToContainers(elementContainer);



                        }
                    }
                }

                catch { }

            }


            //elementCounter.TotalNumElements = collector.GetElementCount();
            //elementCounter.WallNumElements = numOfWalls;


            string json = JsonConvert.SerializeObject(elementContainers.ElementContainersList);

            File.WriteAllText(@"C:\Users\oskar\OneDrive - Danmarks Tekniske Universitet\Kandidat Speciale\Programmering\RevitQuantities\json_wall.json", json);



            return Result.Succeeded;
        }
    }
}