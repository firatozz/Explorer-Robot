using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Media;
using System.Net.Sockets;
using DirectShowLib;
using FaceDetect.Vision.Detection;
using OpenCvSharp;

using OpenCvSharp.Blob;
using OpenCvSharp.UserInterface;
using OpenCvSharp.CPlusPlus;
using System.Diagnostics;
using OpenCvSharp.Extensions;




namespace ExplorerRobot
{
    public partial class Form1 : Form
    {
        SerialPort serialport;

        byte [] veri = new byte[4];
     
        Thread t;
        
        public Form1()
        {
            InitializeComponent();
            serialport = new SerialPort();
            serialport.BaudRate = 9600;

          // TextWriter dosya = new StreamWriter(@"C:\ExplorerRobot.txt");
          //  StreamWriter SW = new StreamWriter("C:\ExplorerRobot.txt");
          ////  SW.WriteLine(richtextBox1.Text);
          //  SW.Close();

       
            
       
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = SerialPort.GetPortNames();

            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            //button13.Enabled = false;

            trackBar1.Enabled = false;
            trackBar1.Enabled = false;
            trackBar1.Enabled = false;

            int hiz = trackBar3.Value; ;        //trackBar3.Value;   //  timer control
            label8.Text = trackBar3.Value.ToString();    //trackBar3.Value.ToString();

            veri[3] = (byte)hiz;
           // veri[3] = Convert.ToByte(hiz);       
          
            
        }
        private void button13_Click(object sender, EventArgs e)
        {
            t = new Thread(new ThreadStart(getFrame));
            t.Start();
            
                try
                {
                    getFrame();
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "HATA !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

       
        


        private void getFrame() //IP camera
        {
                try
                {
                    string sourceURL = "http://192.168.8.1:8083/?action=snapshot";

                    byte[] buffer = new byte [1280 * 1200];
                    int read, total = 0;

                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sourceURL);
                    WebResponse resp = req.GetResponse();

                    Stream stream = resp.GetResponseStream();
                    while ((read = stream.Read(buffer, total, 1000)) != 0)
                    {
                        total += read;
                    }

                   Bitmap resim = (Bitmap)Bitmap.FromStream(new MemoryStream(buffer, 0, total));
                    

                    Bitmap bmp = (Bitmap)Bitmap.FromStream(new MemoryStream(buffer, 0, total));
                    pictureBox1.Image = bmp;
                    timer1.Enabled = true;
                    
                    IplImage resimipl = BitmapConverter.ToIplImage(resim);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        void gonder()
        {
            try
            {
                
               serialport.Write(veri, 0, 4);
                try
                {
                    Thread.Sleep(200);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "HATA !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "HATA !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!serialport.IsOpen)
            {
                try
                {
                    serialport.PortName = comboBox1.Text;
                    serialport.Open();
                    label2.Text = "Bağlandı.";
                    label2.ForeColor = Color.Green;

                    trackBar1.Value = 50; //  X servo merkez açısı
                    trackBar2.Value = 45;  // Y Servo

                    button1.Enabled = false;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button5.Enabled = true;
                    button6.Enabled = true;
                    button7.Enabled = true;
                    button8.Enabled = true;
                    button9.Enabled = true;
                    button10.Enabled = true;
                    button11.Enabled = true;
                    button12.Enabled = true;
                   // button13.Enabled = true;

                    trackBar1.Enabled = true;
                    trackBar1.Enabled = true;
                    trackBar1.Enabled = true;

                   

                    label6.Text = trackBar1.Value.ToString();
                    //veri[1] = (byte)trackBar1.Value;
                    veri[1] = Convert.ToByte(trackBar1.Value);
                    label5.Text = trackBar2.Value.ToString();
                    //veri[2] = (byte)trackBar2.Value;
                    veri[2] = Convert.ToByte(trackBar2.Value);

                    gonder();

                    //richTextBox1.Text += "Connected." + "  " + DateTime.Today + "  " + DateTime.Now.ToLongTimeString() + Environment.NewLine;
   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            veri[0] = 0;
            trackBar1.Value = 50; //  X servo center angle
            trackBar2.Value = 45;  // Y Servo
            label6.Text = trackBar1.Value.ToString();
            veri[1] = Convert.ToByte(trackBar1.Value);
            label5.Text = trackBar2.Value.ToString();
            veri[2] = Convert.ToByte(trackBar2.Value);
            gonder();

            serialport.Close();
            label2.Text = "Bağlantı Kesildi";
            label2.ForeColor = Color.Red;
            //richTextBox1.Text += "Connection failed." + "  " + DateTime.Today + "  " + DateTime.Now.ToLongTimeString() + Environment.NewLine;
            button1.Enabled = true;
            button2.Enabled = false;
            
        }
    
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (serialport.IsOpen == true)   //servo right-left control
            {
                int servox = trackBar1.Value;
                label6.Text = trackBar1.Value.ToString();
                veri[1] = Convert.ToByte(servox);
                gonder();
            }
            else 
            {
                MessageBox.Show("Ooopss ! Lütfen Önce Bağlantıyı Gerçekleştiriniz.","HATA", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (serialport.IsOpen == true)   //servo up-down control
            {
                int servoy = trackBar2.Value;
                label5.Text = trackBar2.Value.ToString();
                veri[2] = Convert.ToByte(servoy);
                gonder();
            }
            else // 
            {
                MessageBox.Show("Ooopss ! Lütfen Önce Bağlantıyı Gerçekleştiriniz.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void trackBar3_Scroll(object sender, EventArgs e)    //speed control
        {
            if (serialport.IsOpen == true)
            {
                int hiz = trackBar3.Value;
                label8.Text = trackBar3.Value.ToString();

                veri[3] = Convert.ToByte(hiz);
                // veri[3] = BitConverter.GetBytes(hiz);


                gonder();
            }
            else
            {
                MessageBox.Show("Oooppss ! Lütfen Önce Bağlantıyı Gerçekleştiriniz.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                button9.PerformClick();
                button9.BackColor = Color.Red;
            }
            else
            {
                button9.BackColor = Color.Gainsboro;
            }
          
            if (e.KeyCode == Keys.W)
            {
                button7.PerformClick();
                button7.BackColor = Color.Red;
            }
            else
            {
                button7.BackColor = Color.Gainsboro;
            }
            if (e.KeyCode == Keys.S)
            {
                button8.PerformClick();
                button8.BackColor = Color.Red;
            }
            else
            {
                button8.BackColor = Color.Gainsboro;
            }
            if (e.KeyCode == Keys.D)
            {
                button10.PerformClick();
                button10.BackColor = Color.Red;
            }
            else
            {
                button10.BackColor = Color.Gainsboro;
            }
            
            if (e.KeyCode == Keys.Space)
            {
                button11.PerformClick();
                button11.BackColor = Color.Red;
            }
            else
            {
                button11.BackColor = Color.Gainsboro;
            }
            if (e.KeyCode == Keys.Up)
            {
                button3.PerformClick();
                button3.BackColor = Color.Red;
            }
            else
            {
                button3.BackColor = Color.Gainsboro;
            }
            if (e.KeyCode == Keys.Down)
            {
                button4.PerformClick();
                button4.BackColor = Color.Red;
            }
            else
            {
                button4.BackColor = Color.Gainsboro;
            }
            if (e.KeyCode == Keys.Right)
            {
                button6.PerformClick();
                button6.BackColor = Color.Red;
            }
            else
            {
                button6.BackColor = Color.Gainsboro;
            }
            if (e.KeyCode == Keys.Left)
            {
                button5.PerformClick();
                button5.BackColor = Color.Red;
            }
            else
            {
                button5.BackColor = Color.Gainsboro;
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            //richTextBox1.Text += "Tank İleri Gidiyor..." + "   " + DateTime.Today + "  " + DateTime.Now.ToLongTimeString() + Environment.NewLine;
            veri[0] = 2;        // motor ileri
            gonder();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            //richTextBox1.Text += "Tank Sola Dönüyor..." + "   " + DateTime.Today + "  " + DateTime.Now.ToLongTimeString() + Environment.NewLine;
            veri[0] = 4;        // motor sol
            gonder();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //richTextBox1.Text += "Tank Geri Gidiyor..." + "   " + DateTime.Today + "  " + DateTime.Now.ToLongTimeString() + Environment.NewLine;
            veri[0] = 1;        //motor  geri
            gonder();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //richTextBox1.Text += "Tank Sağa Dönüyor..." + "   " + DateTime.Today + "  " + DateTime.Now.ToLongTimeString() + Environment.NewLine;
            veri[0] = 3;      // motor sağ
            gonder();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //richTextBox1.Text += "Tank Duruyor..." + "   " + DateTime.Today + "  " + DateTime.Now.ToLongTimeString() + Environment.NewLine;
            veri[0] = 0;     //motor dur
            gonder();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                trackBar2.Value += 2;  // servo down
                label5.Text = trackBar2.Value.ToString();
                veri[2] = Convert.ToByte(trackBar2.Value);
                gonder();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "HATA !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                trackBar2.Value -= 2;  // servo up
                label5.Text = trackBar2.Value.ToString();
                veri[2] = Convert.ToByte(trackBar2.Value);
                gonder();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "HATA !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                trackBar1.Value += 2;  // servo right
                label6.Text = trackBar1.Value.ToString();
                veri[1] = Convert.ToByte(trackBar1.Value);
                gonder();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "HATA !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                trackBar1.Value -= 2;  // servo left
                label6.Text = trackBar1.Value.ToString();
                veri[1] = Convert.ToByte(trackBar1.Value);
                gonder();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "HATA !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 
        }

        private void button12_Click(object sender, EventArgs e)
        {
            trackBar1.Value = 50; //  X servo center angle
            trackBar2.Value = 45;  // Y Servo
            label6.Text = trackBar1.Value.ToString();
            veri[1] = Convert.ToByte(trackBar1.Value);
            label5.Text = trackBar2.Value.ToString();
            veri[2] = Convert.ToByte(trackBar2.Value);
            gonder();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.W || e.KeyCode == Keys.S || e.KeyCode == Keys.D || e.KeyCode == Keys.Space)
            {
                veri[0] = 0;
                gonder();
                button7.BackColor = Color.Gainsboro;
                button8.BackColor = Color.Gainsboro;
                button9.BackColor = Color.Gainsboro;
                button10.BackColor = Color.Gainsboro;
                button11.BackColor = Color.Gainsboro;
            }
        }

        private void trackBar3_Scroll_1(object sender, EventArgs e)
        {

        }
    }
}

