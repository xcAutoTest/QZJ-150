using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace childTest
{
    public partial class action : Form
    {
        public action()
        {
            InitializeComponent();
        }

        private void buttonBzqOn_Click(object sender, EventArgs e)
        {
            string sendstring = "";
            controller.straightCar(out sendstring);
        }

        private void buttonBzqOff_Click(object sender, EventArgs e)
        {
            string sendstring = "";
            controller.backStraight(out sendstring);
        }

        private void buttonZpOn_Click(object sender, EventArgs e)
        {
            string sendstring = "";
            controller.lockTable(out sendstring);
        }

        private void buttonZpOff_Click(object sender, EventArgs e)
        {
            string sendstring = "";
            controller.unLockTable(out sendstring);
        }
    }
}
