using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using week08.Abstractions;
using System.Drawing;
namespace week08.Entities
{
    class PresentFactory : IToyFactory
    {
        public Color PresentColor { get; set; }
        public Color PresentColor1 { get; set; }
        public Toy CreateNew()
        {
            return new Present(PresentColor,PresentColor1);
        }
    }
}
