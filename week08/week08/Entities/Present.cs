using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using week08.Abstractions;
using System.Drawing;

namespace week08.Entities
{
    public class Present : Toy
    {
        public SolidBrush PresentColor { get; private set; }
        public SolidBrush PresentColor1 { get; private set; }
        public Present(Color color, Color color1)
        {
            PresentColor = new SolidBrush(color);
            PresentColor1 = new SolidBrush(color1);
        }
        protected override void DrawImage(Graphics g)
        {
            g.FillRectangle(PresentColor,0,0,Width,Height);
            g.FillRectangle(PresentColor1, 20, 0, Width/5, Height);
            g.FillRectangle(PresentColor1, 0, 20, Width, Height/5);
        }
    }
}
