using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace InverseFinder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            BigInteger u = BigInteger.Parse(textBox2.Text);
            BigInteger v = BigInteger.Parse(textBox3.Text);
            BigInteger u3 = u;
            BigInteger v3 = v;
            BigInteger u1 = 1;
            BigInteger v1 = 0;

            BigInteger q = 0;
            BigInteger R = 0;
            BigInteger t1 = 0;
            BigInteger r = 1;
            BigInteger inv;
            bool iter = true;


            while (v3 != 0)
            {

                textBox1.AppendText("q:" + q.ToString() + "\tu1:" + u1.ToString() + "\tu3:" + u3.ToString() + "\tv1:" + v1.ToString() + "\tv3:" + v3.ToString() + "\tt1:" + t1.ToString()
                    + "\tr:" + r.ToString() + "\titer: " + iter.ToString() + "\r\n");
                q = u3 / v3;
                r = u3 % v3;
                // textBox1.AppendText(m.ToString() + "=" + Q.ToString() + ("(" + b.ToString() + ")+" + R.ToString()) + "\r\n");
                t1 = u1 + q * v1;
                u1 = v1;
                v1 = t1;
                u3 = v3;
                v3 = r;

                iter = !iter;

                //textBox1.AppendText("q:" + q.ToString()+ "\tu1:" + u1.ToString() + "\tu3:" + u3.ToString() + "\tv1:" + v1.ToString() + "\tv3:" + v3.ToString() + "\tt1:" + t1.ToString()
                //  + "\tt3:" + t3.ToString() + "\titer: " + iter.ToString()+ "\r\n");


                //textBox1.AppendText(t.ToString() + " " + newt.ToString() + "\r\n");


            }
            textBox1.AppendText("q:" + q.ToString() + "\tu1:" + u1.ToString() + "\tu3:" + u3.ToString() + "\tv1:" + v1.ToString() + "\tv3:" + v3.ToString() + "\tt1:" + t1.ToString()
                            + "\tr:" + r.ToString() + "\titer: " + iter.ToString() + "\r\n");
            
            
            if (!iter)
            {
                inv = v - u1;
            }
            else
            {
                inv = u1;
            }


            
            textBox1.AppendText(inv.ToString() + "\r\n");
        }
    }
}
