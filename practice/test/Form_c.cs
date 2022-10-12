using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practice.test
{
    public partial class Form_c : Form
    {
        public Form_c()
        {
            InitializeComponent();
        }

        #region 通用练习题

        private void button_Cancel1_Click(object sender, EventArgs e)
        {
            Form_cancel1 dlg = new Form_cancel1();
            dlg.ShowDialog(this);
        }

        private void button_cancel2_Click(object sender, EventArgs e)
        {
            Form_cancel2 dlg = new Form_cancel2();
            dlg.ShowDialog(this);
        }

        private void button_cancel3_Click(object sender, EventArgs e)
        {
            Form_cancel3 dlg = new Form_cancel3();
            dlg.ShowDialog(this);
        }

        private void button_cancel4_Click(object sender, EventArgs e)
        {
            Form_cancel4 dlg = new Form_cancel4();
            dlg.ShowDialog(this);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form5 dlg = new Form5();
            dlg.ShowDialog(this);
        }

        #endregion
    }
}
