using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCA_Tool.Containers
{
    public class ElementContainer
    {
        public string Type { get; set; }
        public string ElementId { get; set; }
        public string LayerId { get; set; }
        public string MaterialId { get; set; }
        public string Name { get; set; }
        public double Area { get; set; }
        public double Thickness { get; set; }

        public ElementContainer(string type, string elementId, string layerId, string materialId, string name, double area, double thickness)
        {
            Type = type;
            ElementId = elementId;
            LayerId = layerId;
            MaterialId = materialId;
            Name = name;
            Area = area;
            Thickness = thickness;
        }
    }
}
