using System.Data.SqlClient;
using System.Windows;
using System.Data;
using ArkaPRP83.Core.Helper;
using System.Configuration;
using log4net;
using System;

namespace Crud_in_WPF.Update
{
    public partial class EditScreenParameterMasterData : Window
    {
        private readonly int parameter_Id;
        private readonly int ScreenId;
        string cs = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public EditScreenParameterMasterData(int parameter_Id, int ScreenId)
        {
            try
            {
                InitializeComponent();
                this.parameter_Id = parameter_Id;
                this.ScreenId = ScreenId;
                attachedData(this.parameter_Id);
                checkbox();

            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void attachedData(int id)
        {
            try
            {

                SqlDataReader reader = SqlHelper.ExecuteReader(cs, "GetScreenParameterDataForEdit", id);
                while (reader.Read())
                {
                    txtModelName.Text = reader["MainName"].ToString().Trim();
                    txtName.Text = reader["ProductName"].ToString().Trim();
                    txtSequence.Text = reader["SequenceNumber"].ToString();
                    checkIsActive.IsChecked = (bool)reader["IsActive"];
                    checkIsDelete.IsChecked = (bool)reader["IsDeleted"];
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }
        private void checkbox()
        {
            if (checkIsDelete.IsChecked == true)
            {
                checkIsDelete.IsEnabled = true;
                checkIsActive.IsEnabled = true;
            }
            if (checkIsDelete.IsChecked == false)
            {
                checkIsDelete.IsEnabled = false;
                checkIsActive.IsEnabled = true;
            }

        }


        private void checkIsActive_Unchecked(object sender, RoutedEventArgs e)
        {
            //checkIsDelete.IsEnabled = false;
        }

        private void checkIsDelete_Unchecked(object sender, RoutedEventArgs e)
        {
            checkIsDelete.IsEnabled = false;
        }

        private void checkIsActive_Checked(object sender, RoutedEventArgs e)
        {
            //checkIsDelete.IsEnabled = false;
        }

        private void checkIsDelete_Checked(object sender, RoutedEventArgs e)
        {
            checkIsActive.IsEnabled = true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int parameterId = parameter_Id;
                int screenId = ScreenId;
                string parameter = txtName.Text.ToString().Trim();
                string x = txtSequence.Text;
                int sequenceNumber;
                bool isActive = checkIsActive.IsChecked ?? false;
                bool isDeleted = checkIsDelete.IsChecked ?? false;
                if (string.IsNullOrEmpty(parameter) || string.IsNullOrEmpty(x))
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
                if (checkSequenceNumber(parameterId, screenId, sequenceNumber))
                {
                    MessageBox.Show("Already a screen exist at this sequence Number , Please enter another Sequence Number!!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (isDeleted == true)
                {
                    MessageBox.Show("Deleted Data can't editable. Firstly uncheck the Deleted Checkbox!!");
                    return;
                }
                if (!string.IsNullOrEmpty(parameter) && screenId > 0 && parameterId > 0)
                {
                    edit(parameterId, screenId, parameter, isActive, isDeleted, sequenceNumber);
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

        private bool checkSequenceNumber(int id, int screenId, int sequenceNumber)
        {
            try
            {

                int row = (int)SqlHelper.ExecuteScalar(cs, CommandType.StoredProcedure, "GetCountSequenceExistScreenParameterMasterById", id,
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

        private void edit(int parameterId, int screenId, string parameter, bool isActive, bool isDeleted, int sequenceNumber)
        {
            try
            {
                int row = SqlHelper.ExecuteNonQuery(cs, "UpdateScreenParameterMaster", parameterId, screenId, parameter, sequenceNumber, isActive, isDeleted);
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtName.Clear();
            txtName.Focus();
        }

        private void txtName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
                txtplaceholder.Visibility = Visibility.Visible;
            else
                txtplaceholder.Visibility = Visibility.Hidden;
        }
    }
}
