using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;


namespace Ex2
{
    public partial class Form1 : Form
    {
        //список из одн массивов коэф-тов неравенств граней (сделать все неравнества меньше либо равно)
        double a;
        double alph, beta;
        double eyeX, eyeY, eyeZ;
        double R;
        double phi, theta;
        double rate;
        double m, h;
        double T;

        double[] P = { 0.0, 0.0, 0.0 };//начальные положения игроков
        double[] E = { 5.0, 8.0, -7.0 };

        double[] q = new double[3]; //y(0) - x(0) = E(0) - P(0)
        double[] v = new double[3];
        double[] u = new double[3];

        double[] a0 = { 6.0, 6.0, 9.0 };
        double[] a1 = { -5.0, 5.0, 5.0 };
        double[] a2 = { 7.0, -7.0, 7.0 };
        double[] a3 = { 8.0, 6.0, -6.0 };
        double[] a4 = { -5.0, -5.0, 7.0 };
        double[] a5 = { 7.0, -7.0, -5.0 };
        double[] a6 = { -6.0, 6.0, -6.0 };
        double[] a7 = { -7.0, -7.0, -8.0 };
        double[] a8 = { -1.0, -1.0, -1.0 };
        double[] a9 = { -2.0, -2.0, -2.0 };


        //double[] a0 = { 1.0, 1.0, 1.0 }; 
        //double[] a1 = { 1.0, 1.0, 1.0 };
        //double[] a2 = { 1.0, 1.0, 1.0 };
        //double[] a3 = { 1.0, 1.0, 1.0 };


        //double[] a0 = { 4.0, 0.0, -1.1 }; //элементы множества M
        //double[] a1 = { -4.0, -4.0, -1.1 };
        //double[] a2 = { -4.0, 4.0, -1.1 };
        //double[] a3 = { 0.0, 0.0, 4.0 };
        //double[] a4 = { 0.1, 0.1, 0.1 };

        //double[] a0 = { 10.0, 8.0, 4.0 }; //элементы множества M
        //double[] a1 = { -4.0, -5.0, 5.0 };
        //double[] a2 = { -4.0, 5.0, -6.0 };
        //double[] a3 = { 3.0, -7.0, 7.0 };
        //double[] a4 = { -6.0, 8.0, 1.0 };
        //double[] a5 = { 7.0, -9.0, 3.0 };
        //double[] a6 = { -5.0, 1.0, 2.0 };




        //double[] a0 = { 1.0, 1.0, 0.0 };
        //double[] a1 = { 1.0, -1.0, 0.0 };
        //double[] a2 = { -1.0, 0.0, 0.0 };
        //double[] a3 = { 0.0, 0.0, 1.0 };

        //double c1, c2;

        int n;

        //double[] a0 = { 6.0, 3.0, 2.0 }; 
        //double[] a1 = { 2.0, 2.0, 2.0 };
        //double[] a2 = { 2.0, 6.0, 2.0 };
        //double[] a3 = { 3.0, 3.0, 6.0 };


        //List<double> u = new List<double>();
        //List<double> v = new List<double>();


        List<double[]> M = new List<double[]>(); //M = co{a_i}
        List<double[]> SLAI = new List<double[]>();


        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            a = 10.0;
            h = 0.01;
            m = 40;
            rate = 20.0;
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGBA | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Gl.glClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            //Gl.glOrtho(-1.5 *a, 1.5 * a,  -1.5 * a, 1.5 * a, -3.6 * a, 3.6 * a);
            Glu.gluPerspective(90.0, 1.0, 0.1, 200.0);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glEnable(Gl.GL_DEPTH_TEST);

            

            q[0] = E[0] - P[0];
            q[1] = E[1] - P[1];
            q[2] = E[2] - P[2];

            phi = 2.0* Math.PI;
            theta =  2.0* Math.PI ;
            

            v = EscaperControl(theta, phi);

            M.Add(a0);
            M.Add(a1);
            M.Add(a2);
            M.Add(a3);
            M.Add(a4);
            M.Add(a5);
            M.Add(a6);
            M.Add(a7);
            M.Add(a8);
            M.Add(a9);

            n = M.Count;

            SLAI = GetSLAI(M);

            //c2 = check(M[0], M[1], M[2], M[3]);
            //c1 =EdgeDeterminant(M[0], M[1], M[2], M[3]);

            //v.Add(FuncX(theta, phi));//theta [0,pi]; phi[0,2pi]
            //v.Add(FuncY(theta, phi));
            //v.Add(FuncZ(theta));
            //u.Add(v[0]);
            ////u.Add(v[0] * k + b);
            ///
            //if (CheckBelonging(M) == false)
            //    MessageBox.Show("V not in U", "warning");
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            alph = trackBar1.Value;
            beta = trackBar2.Value;
            R = trackBar3.Value;

            label1.Text = alph.ToString();
            label2.Text = beta.ToString();
            label3.Text = R.ToString();

            eyeX = 1.8 * a * Math.Cos(Math.PI * beta / 180.0) * Math.Cos(Math.PI * 2.0 * alph / 180.0) * (R / 10.0);
            eyeY = 1.8 * a * Math.Cos(Math.PI * beta / 180.0) * Math.Sin(Math.PI * 2.0 * alph / 180.0) * (R / 10.0);
            eyeZ = 1.8 * a * Math.Sin(Math.PI * beta / 180.0) * (R / 10.0);

            Draw();
        }

        double[] EscaperControl(double theta, double phi)
        {
            double[] v = { FuncX(theta, phi), FuncY(theta, phi), FuncZ(theta) };
            return v;
        }

        //система уравнений для управления убегающего
        double FuncX(double theta, double phi)
        {
            return sin(theta) * cos(phi);
        }
        double FuncY(double theta, double phi)
        {
            return sin(theta) * sin(phi);
        }
        double FuncZ(double theta)
        {
            return cos(theta);
        }
        //
        double cos(double phi)
        {
            return Math.Cos(phi);
        }
        double sin(double phi)
        {
            return Math.Sin(phi);
        }


        void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glPushMatrix();
            Glu.gluLookAt(eyeX, eyeY, eyeZ, 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);


            DrawCoords();

            u[0] = v[0] + Lambda(M) * q[0];
            u[1] = v[1] + Lambda(M) * q[1];
            u[2] = v[2] + Lambda(M) * q[2];
            label4.Text = T.ToString();
            label5.Text = u[0].ToString();
            label6.Text = u[1].ToString();
            label7.Text = u[2].ToString();
            DrawEscaperSet();
            DrawPursuerSet();
            DrawCapture();

            Gl.glPopMatrix();
            AnT.Invalidate();
        }

        void DrawCoords()
        {
            Gl.glColor3f(0.0f, 0.0f, 0.0f);

            Gl.glBegin(Gl.GL_LINES);

            Gl.glVertex3d(-a, -a, -a);
            Gl.glVertex3d(-a, -a, a);

            Gl.glVertex3d(-a, -a, -a);
            Gl.glVertex3d(a, -a, -a);

            Gl.glVertex3d(-a, -a, -a);
            Gl.glVertex3d(-a, a, -a);

            Gl.glVertex3d(-a, a, -a);
            Gl.glVertex3d(-a, a, a);

            Gl.glVertex3d(-a, a, -a);
            Gl.glVertex3d(a, a, -a);

            Gl.glVertex3d(a, a, -a);
            Gl.glVertex3d(a, a, a);

            Gl.glVertex3d(a, a, a);
            Gl.glVertex3d(-a, a, a);

            Gl.glVertex3d(-a, -a, a);
            Gl.glVertex3d(-a, a, a);

            Gl.glVertex3d(-a, -a, a);
            Gl.glVertex3d(a, -a, a);

            Gl.glVertex3d(a, -a, a);
            Gl.glVertex3d(a, -a, -a);

            Gl.glVertex3d(a, -a, a);
            Gl.glVertex3d(a, a, a);

            Gl.glVertex3d(a, -a, -a);
            Gl.glVertex3d(a, a, -a);

            Gl.glEnd();

            Gl.glPointSize(9.0f); //начальные положения игроков
            Gl.glColor3f(1.0f, 0.0f, 0.0f);

            Gl.glBegin(Gl.GL_POINTS);

            Gl.glVertex3d(P[0], P[1], P[2]);

            Gl.glColor3f(0.0f, 0.0f, 1.0f);
            Gl.glVertex3d(E[0], E[1], E[2]);
            Gl.glEnd();


        }

        void DrawPursuerSet()
        {
            for (int i = 0; i <= n - 1; i++)
            {
                for (int j = i+1; j <= n - 1; j++)
                {
                    for (int k = j+1; k <= n - 1; k++)
                    {
                        if(CheckIfEdge(M[i],M[j],M[k]))
                        { 
                            Gl.glColor3f(1.0f, 0.0f, 0.0f);
                            Gl.glPointSize(9.0f);
                            Gl.glBegin(Gl.GL_POINTS);
                            Gl.glVertex3d(M[i][0], M[i][1], M[i][2]);
                            Gl.glVertex3d(M[j][0], M[j][1], M[j][2]);
                            Gl.glVertex3d(M[k][0], M[k][1], M[k][2]);
                            Gl.glEnd();


                            Gl.glPointSize(3.0f);
                            Gl.glBegin(Gl.GL_LINES);
                            Gl.glVertex3d(M[i][0], M[i][1], M[i][2]);
                            Gl.glVertex3d(M[j][0], M[j][1], M[j][2]);

                            Gl.glVertex3d(M[i][0], M[i][1], M[i][2]);
                            Gl.glVertex3d(M[k][0], M[k][1], M[k][2]);

                            Gl.glVertex3d(M[j][0], M[j][1], M[j][2]);
                            Gl.glVertex3d(M[k][0], M[k][1], M[k][2]);
                            Gl.glEnd();
                        }


                    }
                }
            }

            
        }

        void DrawEscaperSet()
        {
            double h = 2 * Math.PI / rate;
            Gl.glBegin(Gl.GL_LINES);

            Gl.glColor3f(0.0f, 0.0f, 1.0f);
            Gl.glPointSize(3.0f);

            for (double i = 0.0; i <= 2 * Math.PI; i += h)
            {
                for (double j = 0.0; j <= Math.PI; j += h)
                {
                    Gl.glVertex3d(FuncX(j, i) + P[0], FuncY(j, i) + P[1], FuncZ(j) + P[2]);
                    Gl.glVertex3d(FuncX(j + h, i) + P[0], FuncY(j + h, i) + P[1], FuncZ(j + h) + P[2]);

                    Gl.glVertex3d(FuncX(i, j + h) + P[0], FuncY(i, j + h) + P[1], FuncZ(i) + P[2]);
                    Gl.glVertex3d(FuncX(i, j) + P[0], FuncY(i, j) + P[1], FuncZ(i) + P[2]);

                    Gl.glVertex3d(FuncX(j, i) + E[0], FuncY(j, i) + E[1], FuncZ(j) + E[2]);
                    Gl.glVertex3d(FuncX(j + h, i) + E[0], FuncY(j + h, i) + E[1], FuncZ(j + h) + E[2]);

                    Gl.glVertex3d(FuncX(i, j + h) + E[0], FuncY(i, j + h) + E[1], FuncZ(i) + E[2]);
                    Gl.glVertex3d(FuncX(i, j) + E[0], FuncY(i, j) + E[1], FuncZ(i) + E[2]);
                }
            }
            Gl.glEnd();
        }

        double[] CalculateCoefficients(double[] a0, double[] a1, double[] a2)
        {
            double p0 = (a1[1] - a0[1]) * (a2[2] - a0[2]) - (a2[1] - a0[1]) * (a1[2] - a0[2]);

            double p1 = -(a1[0] - a0[0]) * (a2[2] - a0[2]) - (a2[0] - a0[0]) * (a1[2] - a0[2]);

            double p2 = (a1[0] - a0[0]) * (a2[1] - a0[1]) - (a2[0] - a0[0]) * (a1[1] - a0[1]);

            double alpha = -a0[0] * ((a1[1] - a0[1]) * (a2[2] - a0[2]) - (a2[1] - a0[1]) * (a1[2] - a0[2])) +
                a0[1] * (-(a1[0] - a0[0]) * (a2[2] - a0[2]) - (a2[0] - a0[0]) * (a1[2] - a0[2])) - 
                a0[2] * ((a1[0] - a0[0]) * (a2[1] - a0[1]) - (a2[0] - a0[0]) * (a1[1] - a0[1]));

            double[] d = { p0, p1, p2, alpha };

            return d;
        }

        List<double[]> GetSLAI(List<double[]> M)
        {
            List<double[]> SLAI = new List<double[]>();

            for (int i = 0; i <= n - 1; i++)
            {
                for (int j = i + 1; j <= n - 1; j++)
                {
                    for (int k = j + 1; k <= n - 1; k++)
                    {
                        double p0 = CalculateCoefficients(M[i], M[j], M[k])[0];
                        double p1 = CalculateCoefficients(M[i], M[j], M[k])[1];
                        double p2 = CalculateCoefficients(M[i], M[j], M[k])[2];
                        double alpha = CalculateCoefficients(M[i], M[j], M[k])[3];
                        if (p0 * M[i][0] + p1 * M[i][1] + p2 * M[i][2] + alpha > 0.0)
                        {
                            p0 = -p0;
                            p1 = -p1;
                            p2 = -p2;
                            alpha = -alpha;
                        }
                        SLAI.Add(CalculateCoefficients(M[i], M[j], M[k]));
                    }
                }
            }
            return SLAI;
        }

        double Lambda(List<double[]> M)
        {
            double lambda = 0.0;
            int counter = 0;
            for (int i = 0; i <= SLAI.Count() - 1; i++)
            {
                //double p0 = SLAI[i][0];
                //double p1 = SLAI[i][1];
                //double p2 = SLAI[i][2];
                //double alpha = SLAI[i][3];

                lambda = (-SLAI[i][3] - (v[0] * SLAI[i][0] + v[1] * SLAI[i][1] + v[2] * SLAI[i][2])) /
                    (q[0] * SLAI[i][0] + q[1] * SLAI[i][1] + q[2] * SLAI[i][2]);

                for (int j = 0; j <= SLAI.Count() - 1; j++)
                {
                    if (lambda <= 0)
                        break;
                    if (((v[0] + lambda * q[0]) * SLAI[i][0] + (v[1] + lambda * q[1]) * SLAI[i][1] + (v[2] + lambda * q[2]) * SLAI[i][2]) + SLAI[i][3] <= 0.0)
                        counter ++;
                    if (counter == SLAI.Count())
                        return lambda;
                }
            }
            return lambda;
        }

        //SAVE
        private void button1_Click(object sender, EventArgs e)
        {
            //Draw();
            //timer1.Stop();

            Bitmap image = new Bitmap(AnT.Width, AnT.Height);
            BitmapData imgData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);

            Gl.glPushClientAttrib(Gl.GL_CLIENT_PIXEL_STORE_BIT);
            Gl.glPixelStoref(Gl.GL_PACK_ALIGNMENT, 4);

            Gl.glReadPixels(0, 0, image.Width, image.Height, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, imgData.Scan0);
            Gl.glPopClientAttrib();

            image.UnlockBits(imgData);
            image.RotateFlip(RotateFlipType.RotateNoneFlipY);


            // формат можно поменять (меняется и в расширении названия и второй параметр метода Save)

            image.Save("D:\\Documents and Settings\\Администратор\\Рабочий стол\\Презентация\\sc.jpg", ImageFormat.Jpeg);
        }

        void DrawCapture()
        {
            T = 1.0 / Lambda(M);

            Gl.glColor3f(0.0f, 0.0f, 1.0f);
            Gl.glPointSize(9.0f);
            Gl.glBegin(Gl.GL_POINTS);

            Gl.glVertex3d(E[0] + v[0] * T, E[1] + v[1] * T, E[2] + v[2] * T);
            Gl.glVertex3d(E[0] + v[0] * T, E[1] + v[1] * T, E[2] + v[2] * T);
            
            Gl.glEnd();

            Gl.glPointSize(3.0f);
            Gl.glBegin(Gl.GL_LINES);

            Gl.glVertex3d(E[0], E[1], E[2]);
            Gl.glVertex3d(E[0] + v[0] * T, E[1] + v[1] * T, E[2] + v[2] * T);

            Gl.glColor3f(1.0f, 0.0f, 0.0f);
            Gl.glVertex3d(P[0], P[1], P[2]);
            Gl.glVertex3d(E[0] + v[0] * T, E[1] + v[1] * T, E[2] + v[2] * T);
            Gl.glEnd();

        }

        bool CheckIfEdge(double[] a0, double[] a1, double[] a2)
        {
            int more = 0;
            int less = 0;

            for (int i = 0; i <= n - 1; i++)
            {
                if (EdgeDeterminant(a0, a1, a2, M[i]) > 0)
                    more++;
                if (EdgeDeterminant(a0, a1, a2, M[i]) < 0)
                    less++;
            }

            if ((more == 0) || (less == 0))
                return true;
            else if ((more == 0) && (less == 0))
                return false;
            else return false;
        }

       

        double EdgeDeterminant(double[] a0, double[] a1, double[] a2, double[] a4)//a4 - точка для проверки
        {
            double x = a4[0];
            double y = a4[1];
            double z = a4[2];

            double io = (x - a0[0]) * (a1[1] - a0[1]) * (a2[2] - a0[2]) +
                (a1[0] - a0[0]) * (a2[1] - a0[1]) * (z - a0[2]) +
                (y - a0[1]) * (a1[2] - a0[2]) * (a2[0] - a0[0]) -
                (z - a0[2]) * (a1[1] - a0[1]) * (a2[0] - a0[0]) -
                (a1[0] - a0[0]) * (y - a0[1]) * (a2[2] - a0[2]) -
                (a2[1] - a0[1]) * (a1[2] - a0[2]) * (x - a0[0]);

            return io;
                

            //(x - a) * (e - b) * (j - c) +
            //    (d - a) * (h - b) * (z - c) +
            //    (y - b) * (f - c) * (g - a) -
            //    (z - c) * (e - b) * (g - a) -
            //    (d - a) * (y - c) * (j - c) -
            //    (h - b) * (f - c) * (x - a);
        }

        bool CheckBelonging(List<double[]> M)
        {
            for (int i = 0; i <= n - 1; i++)
            {
                for (int j = i; j <= n - 1; j++)
                {
                    for (int k = j; k <= n - 1; k++)
                    {
                        if (CheckIfEdge(M[i], M[j], M[k]) == true)
                        {
                            if (DistanceToZero(M[i], M[j], M[k]) < 1.0) return false;
                        }
                    }
                }
            }

            return true;
        }

        double DistanceToPoint(double[] a0, double[] a1, double[] a2, double x, double y, double z)
        {
            double A = (a0[1] * a1[2] - a0[1] * a2[2] - a0[2] * a1[1] + a0[2] * a2[1] + a1[1] * a2[2] - a1[2] * a2[1]);
            double B = (-a0[0] * a1[2] + a0[0] * a2[2] + a0[2] * a1[0] - a0[2] * a2[0] - a1[0] * a2[2] + a1[2] * a2[0]);
            double C = (a0[0] * a1[1] - a0[0] * a2[1] - a0[1] * a1[0] + a0[1] * a2[0] + a1[0] * a2[1] - a1[1] * a2[0]);
            double D = -a0[0] * a1[1] * a2[2] + a0[0] * a1[2] * a2[1] + a0[1] * a1[0] * a2[2] - a0[1] * a1[2] * a2[0] - a0[2] * a1[0] * a2[1] + a0[2] * a1[1] * a2[0];

            return (A * x + B * y + C * z + D) / (Math.Sqrt(A * A + B * B + C * C));
        }
        double DistanceToZero(double[] a0, double[] a1, double[] a2)
        {
            double x = 0.0;
            double y = 0.0;
            double z = 0.0;

            double A = (a0[1] * a1[2] - a0[1] * a2[2] - a0[2] * a1[1] + a0[2] * a2[1] + a1[1] * a2[2] - a1[2] * a2[1]);
            double B = (-a0[0] * a1[2] + a0[0] * a2[2] + a0[2] * a1[0] - a0[2] * a2[0] - a1[0] * a2[2] + a1[2] * a2[0]);
            double C = (a0[0] * a1[1] - a0[0] * a2[1] - a0[1] * a1[0] + a0[1] * a2[0] + a1[0] * a2[1] - a1[1] * a2[0]);
            double D = -a0[0] * a1[1] * a2[2] + a0[0] * a1[2] * a2[1] +
                        a0[1] * a1[0] * a2[2] - a0[1] * a1[2] * a2[0] -
                        a0[2] * a1[0] * a2[1] + a0[2] * a1[1] * a2[0];

            return (A * x + B * y + C * z + D) / (Math.Sqrt(A * A + B * B + C * C));
        }



        int NumberOfEdges(List<double[]> M1)
        {
            int q0 = 0;
            int n = M1.Count;

            for (int i = 0; i <= n - 1; i++)
            {
                for (int j = i+1; j <= n - 1; j++)
                {
                    for (int k = j+1; k <= n - 1; k++)
                    {
                        if (CheckIfEdge(M1[i], M1[j], M1[k]))
                        {
                            q0++;
                        }
                    }
                }
            }
            return q0;
        }

        double Norm(double[] q)
        {
            return Math.Sqrt(q[0] * q[0] + q[1] * q[1] + q[2] * q[2]);
        }




        //double check(double[] a0, double[] a1, double[] a2, double[] a4)
        //{

        //    double x = a4[0];
        //    double y = a4[1];
        //    double z = a4[2];

        //    //            return x * (a2[2] * a1[1] - a2[2] * a0[1] - a0[2] * a1[1] - a1[2] * a2[1] + a1[2] * a0[1] + a0[2] * a2[1]) +
        //    //y * (a2[0] * a1[2] - a2[0] * a0[2] - a0[0] * a1[2] + a0[2] * a1[0] - a2[2] * a1[0] + a2[2] * a0[0]) +
        //    //z*(a2[1] * a1[0] - a2[1] * a0[0] - a0[1] * a1[0] + a1[0] * a0[0] - a2[0] * a1[1] + a2[0] * a0[1] + a0[0] * a1[1] - a0[0] * a0[1]) -
        //    //a2[2] * a1[1] * a0[0] -a0[2] * a2[1] * a1[0] - a0[2] * a1[0] * a0[0] - a2[0] * a1[2] * a0[1] + a0[0] * a0[1] * a0[2] + a2[2] * a0[1] * a1[0] + a0[0] * a1[2] * a2[1];


        //    return  x * (a0[1] * a1[2] - a0[1] * a2[2] - a0[2] * a1[1] + a0[2] * a2[1] + a1[1] * a2[2] - a1[2] * a2[1]) +
        //            y * (-a0[0] * a1[2] + a0[0] * a2[2] + a0[2] * a1[0] - a0[2] * a2[0] - a1[0] * a2[2] + a1[2] * a2[0]) +
        //            z * (a0[0] * a1[1] - a0[0] * a2[1] - a0[1] * a1[0] + a0[1] * a2[0] + a1[0] * a2[1] - a1[1] * a2[0]) - 
        //            a0[0] * a1[1] * a2[2] + a0[0] * a1[2] * a2[1] + a0[1] * a1[0] * a2[2] - a0[1] * a1[2] * a2[0] - a0[2] * a1[0] * a2[1] + a0[2] * a1[1] * a2[0];
        //}

        //A = (a0[1] * a1[2] - a0[1] * a2[2] - a0[2] * a1[1] + a0[2] * a2[1] + a1[1] * a2[2] - a1[2] * a2[1])
        //B = (-a0[0] * a1[2] + a0[0] * a2[2] + a0[2] * a1[0] - a0[2] * a2[0] - a1[0] * a2[2] + a1[2] * a2[0])
        //C = (a0[0] * a1[1] - a0[0] * a2[1] - a0[1] * a1[0] + a0[1] * a2[0] + a1[0] * a2[1] - a1[1] * a2[0])
        //D =  - a0[0] * a1[1] * a2[2] + a0[0] * a1[2] * a2[1] + a0[1] * a1[0] * a2[2] - a0[1] * a1[2] * a2[0] - a0[2] * a1[0] * a2[1] + a0[2] * a1[1] * a2[0]


    }
}
