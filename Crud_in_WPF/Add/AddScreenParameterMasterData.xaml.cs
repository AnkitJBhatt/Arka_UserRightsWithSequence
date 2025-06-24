using ArkaPRP83.Core.Helper;
using log4net;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Crud_in_WPF.Add
{
    public partial class AddScreenParameterMasterData : Window
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int screenId;
        private string cs = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public AddScreenParameterMasterData()
        {
            try
            {

                InitializeComponent();
                addScreenList();
                screenName.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void addScreenList()
        {
            try
            {

                DataTable dt = SqlHelper.ExecuteDataTable(cs, CommandType.StoredProcedure, "GetScreenNameNotDeleted");
                screenName.ItemsSource = dt.DefaultView;
                screenName.DisplayMemberPath = "Name";
                screenName.SelectedValuePath = "Id";
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void screenName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (screenName.SelectedValue != null)
            {
                screenId = Convert.ToInt32(screenName.SelectedValue);
            }
        }
        private void addNewScreenPrmt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = screenId;
                string name = txtSM_Name.Text.ToString();
                string x = txtSequence.Text;
                int sequenceNumber;
                bool IsActive = isActive.IsChecked ?? false;
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(x))
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
                if (checkSequenceNumber(screenId, sequenceNumber))
                {
                    MessageBox.Show("Already a screen exist at this sequence Number , Please enter another Sequence Number!!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (!string.IsNullOrEmpty(name) && id > 0)
                {
                    Add(id, name, IsActive, sequenceNumber);
                }
                else
                {
                    MessageBox.Show("Please fill all columns!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private bool checkSequenceNumber(int screenId, int sequenceNumber)
        {
            try
            {
                int row = (int)SqlHelper.ExecuteScalar(cs, CommandType.StoredProcedure, "GetCountSequenceExistScreenParameterMaster",
                   screenId, sequenceNumber);
                if (row > 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
                throw ex;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Add(int masterId, string Name, bool isActive, int sequenceNumber)
        {
            try
            {
                bool isDeleted = false;
                int row = SqlHelper.ExecuteNonQuery(cs, "InsertIntoScreenParameterMaster", masterId, Name, isActive, isDeleted, sequenceNumber);
                if (row > 0)
                {
                    MessageBox.Show("Data inserted successfully!!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtSM_Name.Clear();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Data insertion failed!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSM_Name.Clear();
            txtSM_Name.Focus();
        }

        private void txtSM_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSM_Name.Text))
                txtplaceholder.Visibility = Visibility.Visible;
            else
                txtplaceholder.Visibility = Visibility.Hidden;
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
