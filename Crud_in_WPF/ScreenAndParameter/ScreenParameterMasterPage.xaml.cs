using System;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using Crud_in_WPF.Add;
using Crud_in_WPF.Update;
using ArkaPRP83.Core.Helper;
using System.Configuration;
using log4net;

namespace Crud_in_WPF.ScreenAndParameter
{
    public partial class ScreenParameterMasterPage : Window
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int ScreenParameterMaster_Id;
        private int ScreenMaster_Id;
        string cs = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public ScreenParameterMasterPage()
        {
            try
            {
                logger.Info("Enter Screen Parameter Master Page.");
                InitializeComponent();
                addDropdownListData();
                screenParameterMasterData();
                screenMasterName.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void addDropdownListData()
        {

            try
            {
                DataTable dt = SqlHelper.ExecuteDataTable(cs, CommandType.StoredProcedure, "GetScreenNameNotDeleted");
                screenMasterName.ItemsSource = dt.DefaultView;
                screenMasterName.DisplayMemberPath = "Name";
                screenMasterName.SelectedValuePath = "Id";
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void screenMasterName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (screenMasterName.SelectedValue != null)
                {
                    ScreenMaster_Id = Convert.ToInt32(screenMasterName.SelectedValue);
                    screenParameterMasterData(ScreenMaster_Id);
                }

            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }
        public void screenParameterMasterData(int id = 1)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataTable(cs, "GetScreenParameterNameBasedOnScreenMaster", id);
                dataGridScreenParameterMasterPage.ItemsSource = dt.DefaultView;
                dataGridScreenParameterMasterPage.SelectedValuePath = "Id";
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }


        private void dataGridScreenParameterMasterPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridScreenParameterMasterPage.SelectedValue != null)
            {
                ScreenParameterMaster_Id = Convert.ToInt32(dataGridScreenParameterMasterPage.SelectedValue);
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                logger.Info("Clicked Add Screen Parameter Master Button.");
                AddScreenParameterMasterData addScreenParemeterMasterData = new AddScreenParameterMasterData();
                addScreenParemeterMasterData.ShowDialog();
                screenParameterMasterData(ScreenMaster_Id);
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                logger.Info("Clicked Edit Screen Parameter Master Button.");
                if (ScreenParameterMaster_Id > 0 && ScreenMaster_Id > 0)
                {
                    EditScreenParameterMasterData edit = new EditScreenParameterMasterData(ScreenParameterMaster_Id, ScreenMaster_Id);
                    edit.ShowDialog();
                    screenParameterMasterData(ScreenMaster_Id);
                }
                else
                    MessageBox.Show("Plaese select an item for Edit!!!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                logger.Info("Clicked Delete Screen Parameter Master Button.");
                if (ScreenParameterMaster_Id > 0)
                {
                    MessageBoxResult result = MessageBox.Show("Are You sure to delete this Parameter?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        screenParameterDelete(ScreenParameterMaster_Id);
                        screenParameterMasterData(ScreenMaster_Id);
                    }
                    else
                    {
                        screenParameterMasterData(ScreenMaster_Id);
                    }
                }
                else
                {
                    MessageBox.Show("Select an item first to Delete!!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void screenParameterDelete(int ScreenParameterMaster_Id)
        {
            try
            {

                if (ScreenParameterMaster_Id > 0)
                {
                    int row = SqlHelper.ExecuteNonQuery(cs, "DeleteScreenParameterMaster", ScreenParameterMaster_Id);
                    if (row > 0)
                    {
                        MessageBox.Show("Data Deleted Successfully!!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Data didn't Delete!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
