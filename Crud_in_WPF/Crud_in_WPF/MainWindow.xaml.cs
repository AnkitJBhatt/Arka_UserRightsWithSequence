using Crud_in_WPF.ScreenAndParameter;
using log4net;
using System;
using System.Windows;
using System.Windows.Input;

namespace Crud_in_WPF
{
    public partial class MainWindow : Window
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public MainWindow()
        {
            log4net.Config.XmlConfigurator.Configure();
            logger.Info("Enter Select Screen Page.");
            InitializeComponent();
        }
        private void btnScreenMaster_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                logger.Info("Clicked Screen Master Button.");
                ScreenMasterPage SMP = new ScreenMasterPage();
                this.Hide();
                SMP.ShowDialog();
                this.Show();

            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void btnScreenMasterPrmtr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                logger.Info("Clicked Screen Parameter Master Button.");
                ScreenParameterMasterPage SMPP = new ScreenParameterMasterPage();
                this.Hide();
                SMPP.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                logger.Info("Clicked Cancel Button.");
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                logger.Error("Error Message - " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
