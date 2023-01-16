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
    public class Roofs
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            FilteredElementCollector collector = new FilteredElementCollector(doc).WhereElementIsNotElementType();
            ElementContainers elementContainers = new ElementContainers();

            int elementId = 0; // bruges til at angive væg nummer

            foreach (Element element in collector)
            {

                try // try bruges da ellers mange elementer ville give fejl hvis man prøver at tage deres categori, hvis de ikke har en.
                {
                    if (element.Category.Name == "Roofs")
                    {


                        //Roof roof = element as Roof;

                        var RF = doc.GetElement(element.GetTypeId()) as RoofType; // 

                        string id = element.Id.ToString();

                        double areaFeet = element.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();
                        double area = Math.Round(UnitUtils.Convert(areaFeet, DisplayUnitType.DUT_SQUARE_FEET, DisplayUnitType.DUT_SQUARE_METERS), 1);
                        string type = "Roofs";

                        int layerId = 0; // bruges til at angive lag i væggen

                        elementId++;

                        CompoundStructure structure = RF.GetCompoundStructure();
                        IList<CompoundStructureLayer> layers = structure.GetLayers();

                        foreach (CompoundStructureLayer layer in layers)
                        {

                            Material layerWallMaterial = doc.GetElement(layer.MaterialId) as Material;

                            layerId++;

                            string materialId = layerWallMaterial.LookupParameter("MaterialID").AsString(); // angiver materialeID'et
                            string material = layerWallMaterial.Name; // angiver materiale navnet
                            string uniqueId = Guid.NewGuid().ToString("N");
                            double layerWidth = Math.Round(UnitUtils.Convert(layer.Width, DisplayUnitType.DUT_DECIMAL_FEET,
                                DisplayUnitType.DUT_MILLIMETERS), 1);

                            ElementContainer elementContainer = new ElementContainer(type, elementId.ToString(), layerId.ToString(), materialId, material, area, layerWidth);

                            elementContainers.AddElementsToContainers(elementContainer);

                        }

                    }
                }

                catch { }

            }

            string json = JsonConvert.SerializeObject(elementContainers.ElementContainersList);

            File.WriteAllText(@"C:\Users\oskar\OneDrive - Danmarks Tekniske Universitet\Kandidat Speciale\Programmering\RevitQuantities\json_roof.json", json);

            return Result.Succeeded;
        }
    }
}