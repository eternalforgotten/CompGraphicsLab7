﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Math;
using System.IO;
using Newtonsoft.Json;

namespace Lab7
{
    public partial class Form1 : Form
    {
        Graphics g;
        Pen pen;
        Projection projection;
        Figure curFigure;
        private List<Point3D> rotationPoints;


        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            pen = new Pen(Color.BlueViolet, 2);
            projection = new Projection();
            rotationPoints = new List<Point3D>();
            projBox.SelectedIndex = 0;
            radioButton1.Checked = true;
        }
        private void Draw()
        {
            if (curFigure != null)
            {
                g.Clear(Color.White);
                List<Edge> edges = projection.Project(curFigure, projBox.SelectedIndex);

                var centerX = pictureBox1.Width / 2;
                var centerY = pictureBox1.Height / 2;

                var figureLeftX = edges.Min(e => e.From.X < e.To.X ? e.From.X : e.To.X);
                var figureLeftY = edges.Min(e => e.From.Y < e.To.Y ? e.From.Y : e.To.Y);
                var figureRightX = edges.Max(e => e.From.X > e.To.X ? e.From.X : e.To.X);
                var figureRightY = edges.Max(e => e.From.Y > e.To.Y ? e.From.Y : e.To.Y);


                var figureCenterX = (figureRightX - figureLeftX) / 2;
                var figureCenterY = (figureRightY - figureLeftY) / 2;


                foreach (Edge line in edges)
                {
                    var p1 = line.From.To2DPoint();
                    var p2 = line.To.To2DPoint();
                    g.DrawLine(pen, p1.X + centerX - figureCenterX, p1.Y + centerY - figureCenterY, p2.X + centerX - figureCenterX, p2.Y + centerY - figureCenterY);
                }
                pictureBox1.Invalidate();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Point3D start = new Point3D(0, 0, 0);
            float len = 150;

            List<Point3D> points = new List<Point3D>
            {
                start,
                new Point3D(len, 0, 0),
                new Point3D(len, 0, len),
                new Point3D(0, 0, len),

                new Point3D(0, len, 0),
                new Point3D(len, len, 0),
                new Point3D(len, len, len),
                new Point3D(0, len, len)
            };

            curFigure = new Figure(points);
            curFigure.AddEdges(0, new List<int> { 1, 4 });
            curFigure.AddEdges(1, new List<int> { 2, 5 });
            curFigure.AddEdges(2, new List<int> { 6, 3 });
            curFigure.AddEdges(3, new List<int> { 7, 0 });
            curFigure.AddEdges(4, new List<int> { 5 });
            curFigure.AddEdges(5, new List<int> { 6 });
            curFigure.AddEdges(6, new List<int> { 7 });
            curFigure.AddEdges(7, new List<int> { 4 });

            curFigure.AddSurface(new List<int> { 0, 1, 2, 3 });
            curFigure.AddSurface(new List<int> { 1, 2, 6, 5 });
            curFigure.AddSurface(new List<int> { 0, 3, 7, 4 });
            curFigure.AddSurface(new List<int> { 4, 5, 6, 7 });
            curFigure.AddSurface(new List<int> { 2, 3, 7, 6 });
            curFigure.AddSurface(new List<int> { 0, 1, 5, 4 });

            Draw();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Point3D start = new Point3D(0, 0, 0);
            float len = 150;

            List<Point3D> points = new List<Point3D>
            {
                start,
                new Point3D(len, 0, len),
                new Point3D(len, len, 0),
                new Point3D(0, len, len),
            };

            curFigure = new Figure(points);
            curFigure.AddEdges(0, new List<int> { 1, 3, 2 });
            curFigure.AddEdges(1, new List<int> { 3 });
            curFigure.AddEdges(2, new List<int> { 1, 3 });


            curFigure.AddSurface(new List<int> { 0, 1, 2 });
            curFigure.AddSurface(new List<int> { 0, 1, 3 });
            curFigure.AddSurface(new List<int> { 0, 2, 3 });
            curFigure.AddSurface(new List<int> { 1, 2, 3 });


            Draw();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            pictureBox1.Invalidate();
            rotationPoints.Clear();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Point3D start = new Point3D(0, 0, 0);
            float len = 150;

            List<Point3D> points = new List<Point3D>
            {
                start,
                new Point3D(len , len , 0),
                new Point3D(-len, len , 0),
                new Point3D(0, len , -len ),
                new Point3D(0, len , len ),
                new Point3D(0,  2 *len, 0),
            };

            curFigure = new Figure(points);
            curFigure.AddEdges(0, new List<int> { 1, 3, 2, 4 });
            curFigure.AddEdges(5, new List<int> { 1, 3, 2, 4 });
            curFigure.AddEdges(1, new List<int> { 3 });
            curFigure.AddEdges(3, new List<int> { 2 });
            curFigure.AddEdges(2, new List<int> { 4 });
            curFigure.AddEdges(4, new List<int> { 1 });

            curFigure.AddSurface(new List<int> { 0, 1, 3 });
            curFigure.AddSurface(new List<int> { 0, 1, 4 });
            curFigure.AddSurface(new List<int> { 0, 2, 3 });
            curFigure.AddSurface(new List<int> { 0, 2, 4 });
            curFigure.AddSurface(new List<int> { 5, 1, 3 });
            curFigure.AddSurface(new List<int> { 5, 1, 4 });
            curFigure.AddSurface(new List<int> { 5, 2, 3 });
            curFigure.AddSurface(new List<int> { 5, 2, 4 });


            Draw();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            float x = float.Parse(textBox1.Text);
            float y = float.Parse(textBox2.Text);
            float z = float.Parse(textBox3.Text);
            AffineChanges.Translate(curFigure, x, y, z);
            Draw();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            float x = float.Parse(textBox1.Text) / 100;
            float y = float.Parse(textBox2.Text) / 100;
            float z = float.Parse(textBox3.Text) / 100;
            AffineChanges.Scale(curFigure, x, y, z);
            Draw();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            float x = float.Parse(textBox1.Text);
            float y = float.Parse(textBox2.Text);
            float z = float.Parse(textBox3.Text);
            AffineChanges.Rotate(curFigure, x, y, z);
            Draw();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AffineChanges.Reflect(curFigure, "xy");
            Draw();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AffineChanges.Reflect(curFigure, "yz");
            Draw();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AffineChanges.Reflect(curFigure, "xz");
            Draw();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            float a = float.Parse(textBox4.Text) / 100;
            AffineChanges.ScaleCenter(curFigure, a);
            Draw();
        }
        private void rotateBtn_Click(object sender, EventArgs e)
        {
            if (rotateOX.Checked)
            {
                AffineChanges.RotateCentral(curFigure, (float)rotateAngle.Value, 0, 0);
            }
            else if (rotateOY.Checked)
            {
                AffineChanges.RotateCentral(curFigure, 0, (float)rotateAngle.Value, 0);
            }
            else if (rotateOZ.Checked)
            {
                AffineChanges.RotateCentral(curFigure, 0, 0, (float)rotateAngle.Value);
            }
            else if (rotateOwn.Checked)

            {
                Edge ed = new Edge(float.Parse(rX1.Text), float.Parse(rY1.Text), float.Parse(rZ1.Text),
                    float.Parse(rX2.Text), float.Parse(rY2.Text), float.Parse(rZ2.Text));
                AffineChanges.RotateFigureAboutLine(curFigure, (float)rotateAngle.Value, ed);
            }
            Draw();
        }

        private void projBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (curFigure != null)
                Draw();
        }

        private void rotateOX_Click(object sender, EventArgs e)
        {
            rotateOY.Checked = rotateOZ.Checked = rotateOwn.Checked = false;
        }

        private void rotateOY_Click(object sender, EventArgs e)
        {
            rotateOX.Checked = rotateOZ.Checked = rotateOwn.Checked = false;
        }

        private void rotateOZ_Click(object sender, EventArgs e)
        {
            rotateOY.Checked = rotateOX.Checked = rotateOwn.Checked = false;
        }
        private void rotateOwn_Click(object sender, EventArgs e)
        {
            rotateOY.Checked = rotateOZ.Checked = rotateOX.Checked = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fName = saveFileDialog1.FileName;
                File.WriteAllText(fName, JsonConvert.SerializeObject(curFigure, Formatting.Indented), Encoding.UTF8);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fName = openFileDialog1.FileName;
                if (File.Exists(fName))
                {
                    curFigure = JsonConvert.DeserializeObject<Figure>(File.ReadAllText(fName, Encoding.UTF8));
                    Draw();
                }
            }
        }

        

        private void button15_Click(object sender, EventArgs e)
        {

            int count = int.Parse(textBox5.Text);
            char axis;
            if (radioButton1.Checked)
            {
                axis = 'x';
            }
            else if (radioButton2.Checked)
            {
                axis = 'y';
            }
            else
            {
                axis = 'z';
            }
            curFigure = FigureRotation.CreateRotationFigure(rotationPoints, count, axis);
            Draw();
        }


        private void radioButton1_Click(object sender, EventArgs e)
        {
            radioButton2.Checked = radioButton3.Checked = false;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = radioButton3.Checked = false;
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = radioButton2.Checked = false;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            float x = float.Parse(textBox6.Text);
            float y = float.Parse(textBox7.Text);
            float z = float.Parse(textBox8.Text);

            rotationPoints.Add(new Point3D(x, y, z));
        }
        delegate float func(float x, float y);
        private void button16_Click(object sender, EventArgs e)
        {
            float x0 = 0, y0 = 0, x1 = 0, y1 = 0,count = 0;
            try
            {
                float.TryParse(textBox9.Text, out x0);
                float.TryParse(textBox10.Text, out y0);
                float.TryParse(textBox11.Text, out x1);
                float.TryParse(textBox12.Text, out y1);
                float.TryParse(textBox13.Text, out count);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверное значение для графика");
                return;
            }
            func f;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    f = (x, y) => (float)(Cos(x + y));
                    break;
                case 1:
                    f = (x, y) => (float)(Sin(x + y));
                    break;
                default:
                    MessageBox.Show("График не выбран");
                    return;
            }
            float dx = (x1 - x0) / count;
            float dy = (y1 - y0) / count;
            float curx, cury = y0;

            List<Point3D> points = new List<Point3D>();            
            for (int i = 0; i <= count; i++)
            {
                curx = x0;
                for (int j = 0; j <= count; j++)
                {
                    points.Add(new Point3D(curx, cury, f(curx, cury)));
                    curx += dx;
                }
                cury += dy;
            }
            Figure figure = new Figure(points);
            int n = (int)count + 1;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    if (j != n-1)
                        figure.AddEdges(i*n + j, i*n + j+1);
                    if (i != n-1)
                        figure.AddEdges(i*n + j, (i+1)*n + j);
                    if (i != n-1 && j != n-1)
                        figure.AddSurface(new List<int> { i*n + j, i*n + j+1, (i+1)*n + j, (i+1)*n + j+1 });
                }

            AffineChanges.ScaleCenter(figure, 40);
            AffineChanges.RotateCentral(figure, 60, 0, 0);
            curFigure = figure;
            Draw();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
