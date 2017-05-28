using System;
using System.Windows.Forms;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace CrystalSiegeEntry
{
    public partial class Form1 : Form
    {
      //   private static String url_main = "http://localhost:62074";
        private static String url_main = "http://crystalsiege.eu";
        public Form1()
        {
            

            InitializeComponent();
            //
            Image image = CrystalSiegeEntry.Properties.Resources.crystal_siege_cover_by_spidivonmarder_d8cr0cv;

            var destRect = new Rectangle(0, 0, Grafika.Width, Grafika.Height);
            var destImage = new Bitmap(Grafika.Width, Grafika.Height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            this.Grafika.BackgroundImage = destImage;
            //Test connect

            if (TestConnection())
            {
                Label_connect.Text = "Connected";
            }
            else
            {
                Label_connect.Text = "Not connected";
            }
        }

        public bool TestConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CrystalSiegeEntry.Properties.Settings.Connection"].ConnectionString;          
            try
            {
                SqlDataAdapter da;
                using (da = new SqlDataAdapter("select * from Person", connectionString))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds.Tables[0] != null ? true : false;
                }
            }
            catch
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String user, pass;
            string url = url_main + "/api/graph";
          //  url = url_main+"/Account/Login";
            if (this.textBoxUser.Text != "" && this.textBoxPassword.Text != "")
            {
                user = this.textBoxUser.Text;
                pass = this.textBoxPassword.Text;
             //   pass = Encrypt(this.textBoxPassword.Text);

                String str = "{ \"username\": \"" + user + "\", \"password\": \"" + pass + "\"  }";
                var httpContent = new StringContent(str, Encoding.UTF8, "application/json");
                Send(url, httpContent, user, pass);
            //    System.Diagnostics.Process.Start(@""+url_main+"/Account/Login?username=" + user + "&password=" + pass);
            }

        }
        private static string Encrypt(string clearText)
        {
            string EncryptionKey = DateTime.Today.Year * DateTime.Today.Month + "";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        static async void Send(string url, StringContent cont, string user, string pass)
        {
            string r = await PostResponseString(url,cont);
            if (r != "\"fail\"")
            {
                pass = Encrypt(pass);
              //  r = Encrypt(r);
                System.Diagnostics.Process.Start(url_main + "/Account/Connect?username=" + user + "&password=" + pass + "&secure=" + r);// + "&secure=" + r);                
            }
        }
        static async Task<string> PostResponseString(string url, StringContent values)
        {
            string contents = "";
            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.PostAsync(url, values))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        contents = await response.Content.ReadAsStringAsync();
                    }
                }
                /*
                using (HttpResponseMessage response = await client.GetAsync(url))
                {                    
                    
                }*/
            }
            return contents;
            //
            /*POST
            var httpClient = new HttpClient();

            var parameters = new Dictionary<string, string>();
            parameters["text"] = text;

            var response = await httpClient.PostAsync(BaseUri, new FormUrlEncodedContent(parameters));
            var contents = await response.Content.ReadAsStringAsync();

            return contents;*/
        }

        private async void przypomnijHasłoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string url = url_main + "/api/graph?action=" + Encrypt("Przypomnij mi hasło");
        /*    string r = await GetResponseString(url);
            if (r == "\"ok\"")
            {
                Form2 form = new Form2();
                form.Show();
            }*///trzeba mieć konto pocztowa, najlepiej te oferowaną przez stronę
        }

        static async Task<string> GetResponseString(string url)
        {
            var contents = "";
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        contents = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return contents;
        }
    }
}

    