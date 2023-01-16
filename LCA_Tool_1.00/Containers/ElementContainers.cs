using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCA_Tool.Containers
{
    public class ElementContainers
    {

        public List<object> ElementContainersList { get; set; } = new List<object>();


        public void AddElementsToContainers(ElementContainer elementContainer)
        {

            ElementContainersList.Add(elementContainer);

        }
    }
}
