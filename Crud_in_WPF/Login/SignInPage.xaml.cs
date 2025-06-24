using ArkaPRP83.Core.Helper;
using log4net;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Crud_in_WPF.Login
{
    public partial class SignInPage : Window
    {
        string cs = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public SignInPage()
        {
            logger.Info("----Welcome to NKP----\n");
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnFocus_Click(object sender, RoutedEventArgs e)
        {
            txtUserName.Focus();
        }
        private void txtUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
                txtplaceholder.Visibility = Visibility.Visible;
            else
                txtplaceholder.Visibility = Visibility.Hidden;
        }

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtShowPassword.Text))
                txtplaceholder2.Visibility = Visibility.Visible;
            else
                txtplaceholder2.Visibility = Visibility.Hidden;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                logger.Info("Clicked Login Button");
                //string userName = txtUserName.Text;
                string userName = "SuperAdmin123";
                string passWord;
                if (txtPassword.Visibility == Visibility.Visible)
                {
                    passWord = "SuperAdmin@123";
                    //passWord = txtPassword.Password;
                    if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
                    {
                        MessageBox.Show("Please fill both fields!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    else
                    {
                        string encryptedPass = PasswordHelper.Encrypt(passWord);
                        //string D = PasswordHelper.Decrypt("8QbHuvWlGo1t0vgjZU6+Lw==");  //SuperAdmin@123
                        bool loginResult = Login(userName, encryptedPass);
                        if (loginResult)
                        {
                            MainWindow mw = new MainWindow();
                            mw.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Wrong Credentials!! Please Enter Valid UserName and Password",
                                "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                if (txtShowPassword.Visibility == Visibility.Visible)
                {
                    passWord = txtShowPassword.Text.ToString().Trim();
                    if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
                    {
                        MessageBox.Show("Please fill both fields!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    else
                    {
                        string encryptedPass = PasswordHelper.Encrypt(passWord);
                        //string D = PasswordHelper.Decrypt("8QbHuvWlGo1t0vgjZU6+Lw==");  //SuperAdmin@123
                        bool loginResult = Login(userName, encryptedPass);
                        if (loginResult)
                        {
                            MainWindow mw = new MainWindow();
                            mw.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Wrong Credentials!! Please Enter Valid UserName and Password",
                                "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }

        }
        public bool Login(string username, string password)        /* DESKTOP-QI77AJ8\SQLEXPRESS*/
        {
            try
            {

                int row = (int)SqlHelper.ExecuteScalar(cs, System.Data.CommandType.StoredProcedure, "CheckUserExist", username, password);
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

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Password))
                txtplaceholder2.Visibility = Visibility.Visible;
            else
                txtplaceholder2.Visibility = Visibility.Hidden;
        }

        private void eyeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Visibility == Visibility.Visible)
            {
                txtShowPassword.Text = txtPassword.Password; // Copy password to TextBox
                txtPassword.Visibility = Visibility.Collapsed;
                txtShowPassword.Visibility = Visibility.Visible;
            }
            else
            {
                txtPassword.Password = txtShowPassword.Text; // Copy  TextBox to password
                txtPassword.Visibility = Visibility.Visible;
                txtShowPassword.Visibility = Visibility.Collapsed;
            }
        }
    }
}
