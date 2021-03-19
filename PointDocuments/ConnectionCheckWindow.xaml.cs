using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;
using System.Data.SqlClient;
using System.ComponentModel;

namespace PointDocuments
{
    /// <summary>
    /// Логика взаимодействия для ConnectionCheckWindow.xaml
    /// </summary>
    public partial class ConnectionCheckWindow : Window
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();
        double degrees = 0;
        System.Windows.Threading.DispatcherTimer animTimer;
        public ConnectionCheckWindow()
        {
            InitializeComponent();
            ShowInTaskbar = false;
            DatabaseHandler.isTested = false;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            SetupAnimTimer();
            StartWorker();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            animTimer.Stop();
            animTimer.IsEnabled = false;
            animTimer = null;
            //worker.CancelAsync();
            worker.Dispose();

            Content = null;
            e.Cancel = false;

        }

        private void SetupAnimTimer()
        {
            animTimer = new System.Windows.Threading.DispatcherTimer();
            animTimer.Tick += new EventHandler(RotateImage);
            animTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);

            animTimer.Start();
        }

        void RotateImage(object sender, EventArgs e)
        {
            degrees += 40;
            var rt = new System.Windows.Media.RotateTransform { Angle = degrees };
            WaitingImage.LayoutTransform = rt;
        }

        public void StartWorker()
        {
            if (!worker.IsBusy)
            {
                worker.DoWork += worker_DoWork;
                worker.WorkerReportsProgress = true;
                worker.ProgressChanged += Worker_ProgressChanged;
                worker.RunWorkerAsync();
            }
        }
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < 50)
            {
                DatabaseHandler.canConnect = false;
                OkErrorDelegate okErrorHandler  = Close;
                Util.ShowErrorMessage(okErrorHandler);
            }
            else
            {
                Close();
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
           bool result = CheckConnection();
           worker.ReportProgress(result ? 100 : 0);
        }

        bool CheckConnection()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string connectionString = config.ConnectionStrings.ConnectionStrings["PointDocumentationEntities"].ConnectionString;
            connectionString = connectionString.Substring(connectionString.IndexOf("data source"));
            connectionString = connectionString.Substring(0, connectionString.IndexOf("App=Entity"));
            connectionString = connectionString.Replace("\r\n\t\t", "");
#if DEBUG
            //connectionString = connectionString.Replace("data source=SRV-VIRT-B-APK", "data source=SRV-VIRT-B-APKK");
#endif
            SqlConnectionStringBuilder scb = new SqlConnectionStringBuilder(connectionString);
            scb.ConnectTimeout = 1;  // 5 seconds wait 0 = Infinite (better avoid)

            bool connected = false;

            using (SqlConnection cnn = new SqlConnection(scb.ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand("select 1", cnn)
                    {
                        CommandTimeout = 1
                    };
                    cnn.Open();
                    if (cnn.State == System.Data.ConnectionState.Open)
                    {
                        connected = true;
                    }
                }
                catch (Exception e)
                {
                }
            }
            DatabaseHandler.isTested = true;
            return connected;
        }
    }
}
