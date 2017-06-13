using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageQuantization
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;
        Graph G;
        Stopwatch sw ;
        Stopwatch S1;
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                S1 = new Stopwatch();
                S1.Start();
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                G = new Graph(ImageMatrix);
                S1.Stop();
                MessageBox.Show(S1.Elapsed.ToString());
                G.ConstructMST();
                //long a = G.DetectTheNumberOfClusters();
                //this.txtNumberOfClusters.Text = a.ToString();
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
        }

        private void btnQuantize_Click(object sender, EventArgs e)
        {
            sw = new Stopwatch();
            sw.Start();
            //G.ConstructMST();
            G.Clustering(Convert.ToInt32 (txtNumberOfClusters.Text));
            G.ConstructTheNewImage(ImageMatrix);
            //MessageBox.Show(G.MSTWeight.ToString());
            sw.Stop();
            MessageBox.Show(sw.Elapsed.ToString());
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            G.K_MeanClustring(Convert.ToInt32(txtNumberOfClusters.Text));
            G.ConstructTheNewImage(ImageMatrix);
            
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {

        }

    }
}