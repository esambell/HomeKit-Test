using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HomeKit_Test;

namespace Tests
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            HomeKit temp = new HomeKit(false);

            UInt32[] A = new UInt32[] { 0xffffffff, 0xffff };
            UInt32[] B = new UInt32[] { 0x100 };
            UInt32[] q, r;
            temp.UInt32ArrayDiv(A, B, out q, out r);

            textBox1.AppendText("q:\r\n");
            foreach (UInt32 i in q) textBox1.AppendText(i.ToString("X8") + "\r\n");
            textBox1.AppendText("r:\r\n");
            foreach (UInt32 i in r) textBox1.AppendText(i.ToString("X8") + "\r\n");


        }
    }
}
