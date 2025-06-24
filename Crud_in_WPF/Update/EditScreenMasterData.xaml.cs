using ArkaPRP83.Core.Helper;
using log4net;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Crud_in_WPF.Update
{
    public partial class EditScreenMasterData : Window
    {
        private int ScreenId;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string cs = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public EditScreenMasterData(int ScreenId)
        {

            try
            {

                InitializeComponent();
                this.ScreenId = ScreenId;
                loadData(ScreenId);
                checkbox();
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }

        }

        private void loadData(int id)
        {

            try
            {

                SqlDataReader reader = SqlHelper.ExecuteReader(cs, "GetScreenMasterByID", id);
                while (reader.Read())
                {
                    txtInputName.Text = reader["Name"].ToString();
                    checkIsActive.IsChecked = (bool)reader["IsActive"];
                    checkIsDeleted.IsChecked = (bool)reader["IsDeleted"];
                    txtSequence.Text = reader["SequenceNumber"].ToString();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }

        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = ScreenId;
                string name = txtInputName.Text.ToString().Trim();
                string x = txtSequence.Text;
                int sequenceNumber;
                bool isActive = checkIsActive.IsChecked ?? false;
                bool isDelete = checkIsDeleted.IsChecked ?? false;
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(txtSequence.Text))
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

                if (checkSequenceNumber(id, sequenceNumber))
                {
                    MessageBox.Show("Already a screen exist at this sequence Number , Please enter another Sequence Number!!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (isDelete == true)
                {
                    MessageBox.Show("Deleted Data can't editable. Firstly uncheck the Deleted Checkbox!!");
                    return;
                }
                if (id > 0 && !string.IsNullOrWhiteSpace(name))
                {
                    editData(id, name, isActive, isDelete, sequenceNumber);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private bool checkSequenceNumber(int id, int sequenceNumber)
        {
            try
            {
                int row = (int)SqlHelper.ExecuteScalar(cs, CommandType.StoredProcedure, "GetCountSequenceExistScreenMasterWithId",
                    sequenceNumber, id);
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
        private void editData(int id, string name, bool isactive, bool isDelete, int sequenceNumber)
        {
            try
            {
                int row = SqlHelper.ExecuteNonQuery(cs, "UpdateScreenMaster", id, name, isactive, isDelete, sequenceNumber);
                if (row > 0)
                {
                    MessageBox.Show("Data Updated Successfully!!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Data didn't Update!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtInputName.Clear();
            txtInputName.Focus();
        }

        private void checkbox()
        {
            if (checkIsDeleted.IsChecked == true)
            {
                checkIsDeleted.IsEnabled = true;
                checkIsActive.IsEnabled = true;
            }
            if (checkIsDeleted.IsChecked == false)
            {
                checkIsDeleted.IsEnabled = false;
                checkIsActive.IsEnabled = true;
            }
        }
        private void checkIsActive_Checked(object sender, RoutedEventArgs e)
        {
            bool check = checkIsActive.IsChecked ?? false;
        }

        private void checkIsDeleted_Checked(object sender, RoutedEventArgs e)
        {
            bool check = checkIsDeleted.IsChecked ?? false;
            if (check == true)
                checkIsActive.IsEnabled = true;
            else
            {
                checkIsActive.IsEnabled = true;
                checkIsDeleted.IsEnabled = false;
            }
        }

        private void btnClear2_Click(object sender, RoutedEventArgs e)
        {
            txtSequence.Clear();
            txtSequence.Focus();
        }

        private void txtSequence_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSequence.Text))
                txtplaceholder2.Visibility = Visibility.Visible;
            else
                txtplaceholder2.Visibility = Visibility.Hidden;
        }

        private void txtInputName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtInputName.Text))
                txtplaceholder.Visibility = Visibility.Visible;
            else
                txtplaceholder.Visibility = Visibility.Hidden;
        }
    }
}
