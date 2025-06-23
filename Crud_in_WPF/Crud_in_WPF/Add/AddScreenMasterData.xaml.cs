using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System;
using ArkaPRP83.Core.Helper;
using System.Configuration;
using log4net;

namespace Crud_in_WPF.Add
{
    public partial class AddScreenMasterData : Window
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string cs = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public AddScreenMasterData()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                txtInput.Clear();
                txtInput.Focus();

            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtInput.Text))
                txtplaceholder.Visibility = Visibility.Visible;
            else
                txtplaceholder.Visibility = Visibility.Hidden;
        }

        private void addNewScreenMaster_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string Name = txtInput.Text;
                string x = txtSequence.Text;
                int sequenceNumber;
                bool isActive = isActiveCheck.IsChecked ?? false;
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(x))
                {
                    MessageBox.Show("Please Fill both fields!!", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!int.TryParse(x, out sequenceNumber) || sequenceNumber == 0 || sequenceNumber > 100)
                {
                    MessageBox.Show("Sequence number contain only an integer Value and number between 1-100 !!",
                            "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (checkSequenceNumber(sequenceNumber))
                {
                    MessageBox.Show("Already a screen exist at this sequence Number , Please enter another Sequence Number!!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                bool result = Add(Name, isActive, sequenceNumber);
                if (result == true)
                {
                    MessageBox.Show("Data Added successfully!!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Data insertion failed!!", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private bool checkSequenceNumber(int sequenceNumber)
        {
            try
            {
                int row = (int)SqlHelper.ExecuteScalar(cs, "GetCountSequenceExistScreenMaster", sequenceNumber);
                if (row > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
                return false;
            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private bool Add(string Name, bool IsActive, int sequenceNumber)
        {
            try
            {
                bool isDeleted = false;
                int row = SqlHelper.ExecuteNonQuery(cs, "InsertIntoScreenMaster", Name, IsActive, isDeleted, sequenceNumber);
                if (row > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private void txtSequence_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSequence.Text))
                txtplaceholder2.Visibility = Visibility.Visible;
            else
                txtplaceholder2.Visibility = Visibility.Hidden;
        }

        private void btnClear2_Click(object sender, RoutedEventArgs e)
        {
            txtSequence.Clear();
            txtSequence.Focus();
        }
    }
}
