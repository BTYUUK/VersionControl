using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using week08.Abstractions;
using week08.Entities;

namespace week08
{
    public partial class Form1 : Form
    {
        List<Toy> _toys = new List<Toy>();
        
        private IToyFactory _toyFactory;

        public IToyFactory Factory
        {
            get { return _toyFactory; }
            set 
            {
                _toyFactory = value;
                DisplayNext();
            }
        }

        Toy _nextToy;
        public Form1()
        {
            InitializeComponent();
            Factory = new CarFactory();
        }

        private void createTimer_Tick(object sender, EventArgs e)
        {
            var toy = Factory.CreateNew();
            _toys.Add(toy);
            toy.Left = -toy.Width;
            mainPanel.Controls.Add(toy);

        }

        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            var jobb = 0;
            foreach (var toy in _toys)
            {
                toy.MoveToy();
                if (toy.Left > jobb)
                {
                    jobb = toy.Left;
                }
                if (jobb >1000)
                {
                    var oldestToy = _toys[0];
                    mainPanel.Controls.Remove(oldestToy);
                    _toys.Remove(oldestToy);
                }
            }
        }

        public void DisplayNext()
        {
            if (_nextToy != null)
            {
                Controls.Remove(_nextToy);
            }
            _nextToy = Factory.CreateNew();
            _nextToy.Top = label1.Top + label1.Height + 20;
            _nextToy.Left = label1.Left;
            Controls.Add(_nextToy);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Factory = new CarFactory();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Factory = new BallFactory
            {
                Ballcolor = button3.BackColor
            };
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var colorPicker = new ColorDialog();
            colorPicker.Color = button.BackColor;
            if (colorPicker.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            button.BackColor = colorPicker.Color;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Factory = new PresentFactory()
            {
                PresentColor = button5.BackColor,
                PresentColor1 = button6.BackColor
            };
        }
    }
}
