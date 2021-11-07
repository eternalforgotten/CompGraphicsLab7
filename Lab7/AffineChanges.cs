using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7
{
    class AffineChanges
    {
        static private float[,] MatrixColumnFromPoint3D(Point3D p)
        {
            return new float[,] { { p.X }, { p.Y }, { p.Z }, { 1 } };
        }


        static private void RecreateFigure(Figure fig, float[,] matr)
        {
            List<Point3D> p = new List<Point3D>();
            for (int i = 0; i < fig.Vertexes.Count; ++i)
            {
                var matrixPoint = Projection.MultMatrix(matr, MatrixColumnFromPoint3D(fig.Vertexes[i]));
                Point3D newPoint = new Point3D(matrixPoint[0, 0] / matrixPoint[3, 0], matrixPoint[1, 0] / matrixPoint[3, 0], matrixPoint[2, 0] / matrixPoint[3, 0]);
                fig.Vertexes[i] = newPoint;
            }
        }
        static private double TransformAngle(double angle)
        {
            return angle * Math.PI / 180;
        }

        static public void Translate(Figure fig, float tx, float ty, float tz)
        {
            float[,] trans = {       { 1, 0, 0, tx },
                                     { 0, 1, 0, ty },
                                     { 0, 0, 1, tz },
                                     { 0, 0, 0,  1 }};

            RecreateFigure(fig, trans);
        }


        static public void Scale(Figure figure, float mx, float my, float mz)
        {
            float[,] scale = { { mx,  0,  0,  0 },
                               {  0, my,  0,  0 },
                               {  0,  0, mz,  0 },
                               {  0,  0,  0,  1 }};

            RecreateFigure(figure, scale);
        }

        static public void Rotate(Figure fig, float anX, float anY, float anZ)
        {
            Point3D locPoint = fig.Center();
            float locX = locPoint.X,
                  locY = locPoint.Y,
                  locZ = locPoint.Z;

            Translate(fig, -locX, -locY, -locZ);

            float loccos = (float)Math.Cos(anX * Math.PI / 180);
            float locsin = (float)Math.Sin(anX * Math.PI / 180);

            float[,] matrixX = { { 1,  0,   0,  0},
                                 { 0, loccos,-locsin, 0},
                                 { 0, locsin, loccos, 0},
                                 { 0,  0,   0,  1}};

            loccos = (float)Math.Cos(anY * Math.PI / 180);
            locsin = (float)Math.Sin(anY * Math.PI / 180);

            float[,] matrixY = { { loccos, 0, locsin, 0},
                                 {  0,  1,  0,  0},
                                 {-locsin, 0, loccos, 0},
                                 {  0,  0,  0,  1}};

            locsin = (float)Math.Sin(anZ * Math.PI / 180);
            loccos = (float)Math.Cos(anZ * Math.PI / 180);
            float[,] matrixZ = { { loccos, -locsin, 0, 0},
                                 { locsin,  loccos, 0, 0},
                                 {  0,    0,  1, 0},
                                 {  0,    0,  0, 1}};

            RecreateFigure(fig, Projection.MultMatrix(Projection.MultMatrix(matrixX, matrixY), matrixZ));

            Translate(fig, locX, locY, locZ);
        }

        public static void RotateCentral(Figure fig, float anX, float angleY, float angleZ)
        {
            var curcent = fig.Center();
            Translate(fig, -curcent.X, -curcent.Y, -curcent.Z);
            Rotate(fig, anX, angleY, angleZ);
            Translate(fig, curcent.X, curcent.Y, curcent.Z);
        }

        public static void RotateFigureAboutLine(Figure fig, float an, Edge ed)
        {
            var loc_l = ed.To - ed.From;
            var len = Math.Sqrt(Math.Pow(loc_l.X, 2) + Math.Pow(loc_l.Y, 2) + Math.Pow(loc_l.Z, 2));
            var xl = (float)(loc_l.X / len);
            var yn = (float)(loc_l.Y / len);
            var zn = (float)(loc_l.Z / len);

            float locsin = (float)Math.Sin(an * Math.PI / 180);
            float cos = (float)Math.Cos(an * Math.PI / 180);



            float[,] m = { { xl*xl+cos*(1-xl*xl),   xl*(1-cos)*yn-zn*locsin, xl*(1-cos)*zn+yn*locsin, 0 },
                              { xl*(1-cos)*yn+zn*locsin, yn*yn+cos*(1-yn*yn),   yn*(1-cos)*zn-xl*locsin, 0 },
                              { xl*(1-cos)*zn-yn*locsin, yn*(1-cos)*zn+xl*locsin, zn*zn+cos*(1-zn*zn),   0 },
                              { 0,                 0,                 0,                 1 }};

            RecreateFigure(fig, m);
        }
        public static void Reflect(Figure figure, string plane)
        {
            float[,] matrix;
            switch (plane)
            {
                case "xy":
                    matrix = new float[,] {{ 1, 0,  0, 0 },
                                           { 0, 1,  0, 0 },
                                           { 0, 0, -1, 0 },
                                           { 0, 0,  0, 1 }};
                    break;
                case "xz":
                    matrix = new float[,] {{ 1,  0, 0, 0 },
                                           { 0, -1, 0, 0 },
                                           { 0,  0, 1, 0 },
                                           { 0,  0, 0, 1 }};
                    break;
                case "yz":
                    matrix = new float[,] {{ -1, 0, 0, 0 },
                                           {  0, 1, 0, 0 },
                                           {  0, 0, 1, 0 },
                                           {  0, 0, 0, 1 }};
                    break;
                default:
                    matrix = new float[,] {{ 1, 0, 0, 0 },
                                           { 0, 1, 0, 0 },
                                           { 0, 0, 1, 0 },
                                           { 0, 0, 0, 1 }};
                    break;
            }
            RecreateFigure(figure, matrix);
        }

        public static void ScaleCenter(Figure figure, float a)
        {

            float[,] scale = { { a, 0, 0, 0 },
                               { 0, a, 0, 0 },
                               { 0, 0, a, 0 },
                               { 0, 0, 0, 1 }};

            RecreateFigure(figure, scale);
        }
    }
}
