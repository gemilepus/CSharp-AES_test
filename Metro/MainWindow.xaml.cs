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

namespace Metro
{
    public partial class MainWindow : MetroWindow
    {

        // DataGrid
        List<mTable> mDataTable = new List<mTable>();

        public MainWindow()
        {
            InitializeComponent();

            // Data Binding
            this.DataContext = this;

        }


        private void mDataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            e.NewItem = new mTable
            {
              mTable_IsEnable = true,
                        mTable_Mode = "",
                        mTable_Action = "",
                        mTable_Time = 0
            };
        }

        public class mTable
        {
            public bool   mTable_IsEnable { get; set; }
            public string mTable_Mode { get; set; }
            public string mTable_Action { get; set; }
            public int mTable_Time { get; set; }
        }

        
        private void mDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString(); 
        }


       
        private void Btn_open_Click(object sender, RoutedEventArgs e)
        {
            string fileContent = string.Empty;
            string filePath = string.Empty;

            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

            openFileDialog.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            openFileDialog.Filter = "txt files (*.txt)|*.txt"; // "txt files (*.txt)|*.txt|All files (*.*)|*.*"
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
                System.IO.File.WriteAllText(filePath+ ".Encrypt", ToReturn);


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

        private void mDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            int columnIndex = mDataGrid.Columns.IndexOf(mDataGrid.CurrentCell.Column);
            if (columnIndex < 0) {

                return;
            }


            if (mDataGrid.Columns[columnIndex].Header.ToString().Equals(" "))
            {
                int tableIndex = mDataGrid.Items.IndexOf(mDataGrid.CurrentItem);


                try
                {
                    if (tableIndex < mDataTable.Count())
                    {
                        //Table Clear
                        mDataGrid.DataContext = null;
                        mDataTable.RemoveAt(tableIndex);
                        mDataGrid.DataContext = mDataTable;
                    }
                }
                catch
                {

                }

            }

            if (mDataGrid.Columns[columnIndex].Header.ToString().Equals("+"))
            {
                // Get index
                int tableIndex = mDataGrid.Items.IndexOf(mDataGrid.CurrentItem);

                try
                {
                    if (tableIndex < mDataTable.Count() - 1)
                    {
                        // Insert Item
                        mDataGrid.DataContext = null;
                        mDataTable.Insert(tableIndex + 1, new mTable() { mTable_IsEnable = true, mTable_Mode = "", mTable_Action = "", mTable_Time = 0 });
                        mDataGrid.DataContext = mDataTable;
                    }
                    else {
                        mDataGrid.DataContext = null;
                        mDataTable.Add(new mTable() { mTable_IsEnable = true, mTable_Mode = "", mTable_Action = "", mTable_Time = 0 });
                        mDataGrid.DataContext = mDataTable;
                    }                  
                }
                catch
                {

                }

              
            }


        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
      
        }
 
    }
}