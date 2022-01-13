using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using System.Security.Cryptography;
using System.Text;

namespace Metro
{
    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {
            InitializeComponent();

            // Data Binding
            this.DataContext = this;

        }
    
       
        private void Btn_Encode_Click(object sender, RoutedEventArgs e)
        {
            string fileContent = string.Empty;
            string filePath = string.Empty;

            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

            openFileDialog.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();

            try
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
                //Read the contents of the file into a stream
                StreamReader reader = new StreamReader(filePath);

                // read test
                fileContent = reader.ReadToEnd();


                string textToEncrypt = fileContent;
                string ToReturn = "";
                string publickey = "sergyjik20211227";
                string secretkey = "JRFMDxNg0KW0N5Y5"; // 128bit
                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = secretkeyByte;
                    aesAlg.IV = publickeybyte;
                    Console.WriteLine(aesAlg);

                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, aesAlg.CreateEncryptor(secretkeyByte, publickeybyte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                System.IO.File.WriteAllText(filePath+ ".Encode", ToReturn);


                this.ShowMessageAsync("", "OK!");

            }
            catch
            {            
                this.ShowMessageAsync("", "ERROR!");
            }

        }

        private void Btn_Decode_Click(object sender, RoutedEventArgs e)
        {
            string fileContent = string.Empty;
            string filePath = string.Empty;

            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

            openFileDialog.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();

            try
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
                //Read the contents of the file into a stream
                StreamReader reader = new StreamReader(filePath);

                // read test
                fileContent = reader.ReadToEnd();

                string textToDecrypt = fileContent;
                string ToReturn = "";
                string publickey = "sergyjik20211227";
                string secretkey = "JRFMDxNg0KW0N5Y5";
                byte[] privatekeyByte = { };
                privatekeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = privatekeyByte;
                    aesAlg.IV = publickeybyte;

                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, aesAlg.CreateDecryptor(privatekeyByte, publickeybyte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
                System.IO.File.WriteAllText(filePath + ".Decode", ToReturn);


                this.ShowMessageAsync("", "OK!");

            }
            catch
            {
                this.ShowMessageAsync("", "ERROR!");
            }

        }

        private void Btn_About_Click(object sender, RoutedEventArgs e)
        {
            
            this.ShowMessageAsync("", "");
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
      
        }
 
    }
}