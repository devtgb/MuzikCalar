using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using WMPLib;

namespace MuzikCalar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        XmlDocument doc = new XmlDocument();
        int sarkisayisi = 0;
        int sira = 0;
        int index;
       
        
        private void Form1_Load(object sender, EventArgs e)
        {
            XmlRadyoGetir();
        }

      
        /// <summary>
        /// XML den radyoları ve MMS adreslerini çeken method.
        /// </summary>
        void XmlRadyoGetir()
        {
            //doc.Load("C:\\Users\\tugba\\Desktop\\radiodb.xml");
            doc.Load(Application.StartupPath + "\\" + "radiodb.xml");
            XmlElement root = doc.DocumentElement;
            XmlNodeList kayitlar = root.SelectNodes("/data/row");

            foreach (XmlNode secilen in kayitlar)
            {
                ListViewItem lv = new ListViewItem();
                lv.Text = secilen.Attributes["url"].InnerText;
                lv.SubItems.Add(secilen.Attributes["name"].InnerText);
                listView1.Items.Add(lv);
                
            }
        }     
/// <summary>
/// Radyonun çalınmasını sağlayan method.
/// </summary>
/// <param name="url"></param>
        void RadyoCal(string url)
        {
            axWindowsMediaPlayer1.URL = url;
          
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)

            {
                RadyoCal(listView1.SelectedItems[0].Text.ToString());

            }
            
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
                openFileDialog1.Filter = "Media File(*.mpg,*.dat,*.avi,*.wmv,*.wav,*.mp3)|*.wav;*.mp3;*.mpg;*.dat;*.avi;*.wmv"; //seçeceğimiz dosyalar arasında filtreleme yapar. 
                //openFileDialog1.InitialDirectory = Application.StartupPath;  //Debug klasöründen başlanılmasını sağlıyor. 
                openFileDialog1.Title = "Dosya Seç";
                openFileDialog1.RestoreDirectory = true;//En son açılan dizi gözata basıldığı zaman tekrar açılır.
                //openFileDialog1.ShowDialog();
                openFileDialog1.CheckFileExists = false;
                if (openFileDialog1.ShowDialog().ToString() == "OK") { 
                //çoklu seçim için
                string[] safefilenames = openFileDialog1.SafeFileNames;
                string[] filenames = openFileDialog1.FileNames;
                openFileDialog1.CheckFileExists = true;
                openFileDialog1.CheckPathExists = true;

                sira = lstbilgisayar.Items.Count; //listviewdaki kolonlari sayiyoruz.
                for (int i = 0; i < filenames.Length; i++)
                {
                    sarkisayisi++;
                    lstbilgisayar.Items.Add(sarkisayisi.ToString());
                    lstbilgisayar.Items[i].SubItems.Add(safefilenames[i]);
                    lstbilgisayar.Items[i].SubItems.Add(filenames[i]);

                }

                for (int i = 0; i < openFileDialog1.SafeFileNames.Length; i++)
                {

                    lstbilgisayar.Items[sira].SubItems.Add(openFileDialog1.SafeFileNames[i].ToString());

                }
                lstbilgisayar.Items[sira].SubItems.Add(openFileDialog1.FileName.ToString());
                openFileDialog1.FileName = "";
                }
           
        }
        

        private void lstbilgisayar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
                {
            if (lstbilgisayar.SelectedItems.Count > 0)
            {
                index = lstbilgisayar.SelectedItems[0].Index;
                RadyoCal(lstbilgisayar.Items[index].SubItems[2].Text);
                textBox1.Text ="Oynatılıyor: "+ " " + lstbilgisayar.Items[index].SubItems[1].Text;

            }
            }
                catch (Exception ex)
                {

                    lstbilgisayar.SelectedItems[0].Remove();
                    //MessageBox.Show(ex.ToString());
                }
            
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                WMPLib.IWMPPlaylist playlist = axWindowsMediaPlayer1.playlistCollection.newPlaylist("myplaylist");
                WMPLib.IWMPMedia media;                                      
                for (int i = 0; i < lstbilgisayar.Items.Count; i++)
                {
                        media = axWindowsMediaPlayer1.newMedia(lstbilgisayar.Items[i].SubItems[2].Text);
                        playlist.appendItem(media);                  
                }
               
                axWindowsMediaPlayer1.currentPlaylist = playlist;
                axWindowsMediaPlayer1.settings.autoStart = true;
                axWindowsMediaPlayer1.Ctlcontrols.next();
                axWindowsMediaPlayer1.Ctlcontrols.play();
                }
               
            }
       
        }

     
        }
    

