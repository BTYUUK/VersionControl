using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using week08.Entities;

namespace week08
{
    public partial class Form1 : Form
    {
        List<Ball> _balls = new List<Ball>();
        public BallFactory Factory { get; set; }
        public Form1()
        {
            InitializeComponent();
            Factory = new BallFactory();
        }

        private void createTimer_Tick(object sender, EventArgs e)
        {
            var ball = Factory.CreateNew();
            _balls.Add(ball);
            ball.Left = -ball.Width;
            mainPanel.Controls.Add(ball);

        }

        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            var jobb = 0;
            foreach (var ball in _balls)
            {
                ball.MoveBall();
                if (ball.Left > jobb)
                {
                    jobb = ball.Left;
                }
                if (jobb >1000)
                {
                    var oldestBall = _balls[0];
                    mainPanel.Controls.Remove(oldestBall);
                    _balls.Remove(oldestBall);
                }
            }
        }
    }
}
