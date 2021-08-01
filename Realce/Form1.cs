using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Realce
{



    public partial class Form1 : Form
    {
        Bitmap bmp;
        myPixels bmpTeste;
        int maximum;
        int minimum;
        int[] colorTable = new int[256];

        public Form1()
        {
            InitializeComponent();

            //for (double i = 0; i < 1.0; i += 0.1)
            //{
            //    comboBox1.Items.Add(i);
            //}

            

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bmp = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = bmp;
                //textBox1.Text = "0";

                trackBar1.Value = maximum = 255;
                trackBar2.Value = minimum = 0;
                label1.Text = maximum.ToString() ;
                label2.Text = minimum.ToString() ;
                
                comboBox1.Visible = true;
                

                trackBar1.Visible = true;
                trackBar2.Visible = true;

                bmpTeste = new myPixels(bmp);
            }

            realceLinear();

            radioButton1.Checked = true;


        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if(trackBar1.Visible == true)
            {
                maximum = trackBar1.Value;

                if (radioButton1.Checked)
                {
                    realceLinear();
                }

                if (radioButton2.Checked)
                {
                    realceExponencial();
                }

                if (radioButton3.Checked)
                {
                    realceLog();
                }

                if (radioButton4.Checked)
                {
                    
                    //comboBox1.SelectedValue = 0;

                    funcaoGama();
                }



                label1.Text = maximum.ToString();
            }
            
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            if(trackBar2.Visible == true)
            {
                minimum = trackBar2.Value;

                if (radioButton1.Checked)
                {
                    realceLinear();
                }

                if (radioButton2.Checked)
                {
                    realceExponencial();
                }

                if (radioButton3.Checked)
                {
                    realceLog();
                }

                if (radioButton4.Checked)
                {
                    //comboBox1.Visible = true;
                    //comboBox1.SelectedValue = 0;

                    funcaoGama();
                }

                label2.Text = minimum.ToString();
            }
        }

        private void realceLinear()
        {
            double a, b;
            int range = maximum - minimum;
            a = 255.0 / range;
            b = -a * minimum;
            int intVal;
            for (int i = 0; i < 256; ++i)
            {
                if (i <= minimum)
                    colorTable[i] = 0;
                else if (i > maximum)
                    colorTable[i] = 255;
                else
                {
                    intVal = Convert.ToInt32(a * i + b);
                    if (intVal > 255) intVal = 255;
                    if (intVal < 0) intVal = 0;
                    colorTable[i] = (byte)intVal;
                }
            }

            mostraImagem();

           

        }

        private void mostraImagem()
        {
            
            for (int i = 0; i < bmp.Width - 1; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color k = bmpTeste.bmpOrigem.GetPixel(i, j);

                    int grey = (k.R + k.G + k.B) / 3;

                    bmpTeste.bmpDestino.SetPixel(i, j, Color.FromArgb(colorTable[grey], colorTable[grey], colorTable[grey]));
                }

            }
            pictureBox1.Image = bmpTeste.bmpDestino;
        }



        private void realceExponencial()
        {
            double scale1 = Math.Log10(256.0) / 255.0;
            double a, b;
            a = Math.Exp(scale1 * minimum) - 1.0;
            b = Math.Exp(scale1 * maximum) - 1.0;
            double scale2 = 255.0 / (b - a);
            int intVal;
            for (int i = 0; i < 256; ++i)
            {
                if (i <= minimum)
                    colorTable[i] = 0;
                else if (i > maximum)
                    colorTable[i] = 255;
                else
                {
                    intVal = Convert.ToInt32(Math.Round(scale2 * (Math.Exp(scale1
                   * i) - 1.0 - a)));
                    if (intVal > 255) intVal = 255;
                    if (intVal < 0) intVal = 0;
                    colorTable[i] = (byte)intVal;

                }

            }

            mostraImagem();

        }

        private void realceLog()
        {
            double a, b;
            double lHigh = Math.Log10(maximum + 1.0);
            double lLow = Math.Log10(minimum + 1.0);
            double range = lHigh - lLow;
            a = 255.0 / range;
            b = -a * lLow;
            int intVal;
            for (int i = 0; i < 256; ++i)
            {
                if (i <= minimum)
                    colorTable[i] = 0;
                else if (i > maximum)
                    colorTable[i] = 255;
                else
                {
                    intVal = Convert.ToInt32(a * Math.Log10(i + 1) + b);
                    if (intVal > 255) intVal = 255;
                    if (intVal < 0) intVal = 0;
                    colorTable[i] = (byte)intVal;
                }
            }

            mostraImagem();
        }

        void funcaoGama()
        {
            double gamma = Convert.ToDouble(comboBox1.Text);
            double a, b;
            a = Math.Pow(minimum, gamma);
            b = Math.Pow(maximum, gamma);
            double scale = 255.0 / (b - a);
            int intVal;
            for (int i = 0; i < 256; ++i)
            {
                if (i <= minimum)
                    colorTable[i] = 0;
                else if (i > maximum)
                    colorTable[i] = 255;
                else
                {
                    intVal = Convert.ToInt32(Math.Round(scale * (Math.Pow(i,
                   gamma) - a)));
                    if (intVal > 255) intVal = 255;
                    if (intVal < 0) intVal = 0;
                    colorTable[i] = (byte)intVal;

                }
            }


                    mostraImagem();


        }


        private void button2_Click(object sender, EventArgs e)
        {
            realceLinear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(radioButton4.Checked)
                funcaoGama();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                realceExponencial();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                realceLinear();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
                realceLog();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                comboBox1.SelectedIndex = 0;
                funcaoGama();

            }
        }
    }
}

