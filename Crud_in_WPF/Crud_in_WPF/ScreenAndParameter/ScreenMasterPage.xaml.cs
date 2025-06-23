using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using Crud_in_WPF.Add;
using Crud_in_WPF.Update;
using ArkaPRP83.Core.Helper;
using System.Windows.Media;
using System.Configuration;
using log4net;

namespace Crud_in_WPF.ScreenAndParameter
{
    public partial class ScreenMasterPage : Window
    {
        private int screenId;
        string cs = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ScreenMasterPage()
        {
            try
            {
                logger.Info("Enter Screen Master Page.");
                InitializeComponent();
                screenMasterData();

            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }

        }

        public void screenMasterData()
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataTable(cs, CommandType.StoredProcedure, "GetScreenName");
                dataGridScreenMasterPage.ItemsSource = dt.DefaultView;
                dataGridScreenMasterPage.SelectedValuePath = "Id";
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }

            //using (SqlConnection con = new SqlConnection(cs))
            //{
            //    con.Open();
            //    SqlCommand cmd = new SqlCommand("GetScreenName", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    SqlDataAdapter da = new SqlDataAdapter(cmd);
            //    DataTable dt = new DataTable();
            //    da.Fill(dt);
            //    dataGridScreenMasterPage.ItemsSource = dt.DefaultView;
            //    dataGridScreenMasterPage.SelectedValuePath = "Id";
            //}
        }

        private void dataGridScreenMasterPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dataGridScreenMasterPage.SelectedValue != null)
                {

                    screenId = Convert.ToInt32(dataGridScreenMasterPage.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                logger.Info("Clicked Add Screen Master Button.");
                AddScreenMasterData add = new AddScreenMasterData();
                add.ShowDialog();
                screenMasterData();
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
                logger.Info("Clicked Edit Screen Master Button.");
                if (screenId > 0)
                {
                    EditScreenMasterData edit = new EditScreenMasterData(screenId);
                    edit.ShowDialog();
                    screenMasterData();
                }
                else
                {
                    MessageBox.Show("Select an item first to Update!!", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                logger.Info("Clicked Delete Screen Master Button.");
                if (screenId > 0)
                {
                    MessageBoxResult result = MessageBox.Show("Are You Sure to delete this Screen?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        screenDelete(screenId);
                        screenMasterData();
                    }
                    else
                    {
                        screenMasterData();
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

        private void screenDelete(int screenId)
        {

            if (screenId > 0)
            {
                try
                {
                    int row = SqlHelper.ExecuteNonQuery(cs, "DeleteScreenMaster", screenId);
                    if (row > 0)
                    {
                        MessageBox.Show("Data Deleted Successfully!!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Data didn't Deleted!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            logger.Info("Data Deleted of Screen Master Page Successfully.");

                    }
                    //using (SqlConnection con = new SqlConnection(cs))
                    //{
                    //    con.Open();;
                    //    SqlCommand cmd = new SqlCommand("DeleteScreenMaster", con);
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = screenId });
                    //    int row = cmd.ExecuteNonQuery();
                    //    if (row > 0)
                    //    {
                    //        MessageBox.Show("Data Deleted Successfully!!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("Data didn't Delete!!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
