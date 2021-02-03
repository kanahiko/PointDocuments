using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PointDocuments
{
    /// <summary>
    /// Логика взаимодействия для Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        int waitTime = 500;
        int circleSize = 10;
        int smalCircleSize = 5;
        public List<Dot> points;
        public List<Dot> newPoints;

        double sizeX;
        double sizeY;
        Random rand = new Random();

        BackgroundWorker backgroundWorker1;

        bool isAdding;
        bool isDeleting;
        double deleteCircle = 75;
        public Test()
        {
            InitializeComponent();
            InitializeBackgroundWorker();

            isAdding = false;
            isDeleting = false;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (points == null)
            {
                points = new List<Dot>();
                newPoints = new List<Dot>();
                sizeX = PointCanvas.ActualWidth;
                sizeY = PointCanvas.ActualHeight;

                points.Add(new Dot(rand.NextDouble() * sizeX, rand.NextDouble() * sizeY));
                points.Add(new Dot(rand.NextDouble() * sizeX, rand.NextDouble() * sizeY));
                points.Add(new Dot(rand.NextDouble() * sizeX, rand.NextDouble() * sizeY));

                DrawPoints();
            }
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (points.Count < 3)
            {
                return;
            }
            AddButton.IsEnabled = backgroundWorker1.IsBusy;
            RemoveButton.IsEnabled = backgroundWorker1.IsBusy;
            ClearButton.IsEnabled = backgroundWorker1.IsBusy;

            if (backgroundWorker1.IsBusy)
            {
                StartButton.Content = "Start";
                backgroundWorker1.CancelAsync();
            }
            else
            {
                rand = new Random();

                newPoints.Clear();
                DrawPoints();
                CreateStartingPoint();

                StartButton.Content = "Stop";
                backgroundWorker1.RunWorkerAsync();
            }
        }

        void CreateStartingPoint()
        {
            Dot newPoint = new Dot(rand.NextDouble() * sizeX, rand.NextDouble() * sizeY);
            DrawCircle(newPoint.x, newPoint.y, smalCircleSize, smalCircleSize, PointCanvas, Brushes.Red);
            newPoints.Add(newPoint);
        }

        void AddPoint()
        {         
            int index = (rand.Next(1, points.Count * 10))/10;
            Dot newPoint = new Dot(newPoints[newPoints.Count - 1], points[index], points.Count - 1);
            DrawCircle(newPoint.x, newPoint.y, smalCircleSize, smalCircleSize, PointCanvas, Brushes.Red);
            newPoints.Add(newPoint);
            PointsCountLabel.Content = "Points:" + newPoints.Count;
        }

        void DrawPoints()
        {
            PointCanvas.Children.Clear();

            for (int i = 0; i < points.Count; i++) 
            {
                DrawCircle(points[i].x, points[i].y, circleSize, circleSize, PointCanvas, Brushes.Black);
            }
        }

        void DrawCircle(double x, double y, int width, int height ,Canvas cv, Brush color)
        {
            Ellipse circle = new Ellipse()
            {
                Width = width,
                Height = height,
                Stroke = color,
                StrokeThickness = 6
            };

            cv.Children.Add(circle);

            circle.SetValue(Canvas.LeftProperty, x);
            circle.SetValue(Canvas.TopProperty, y);
        }


        //---------------------------------------
        // This event handler is where the time-consuming work is done.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while(true)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(waitTime);
                    worker.ReportProgress(10);
                }
            }
        }

        // This event handler updates the progress.
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            AddPoint();
        }

        // This event handler deals with the results of the background operation.
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                //resultLabel.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                //resultLabel.Text = "Error: " + e.Error.Message;
            }
            else
            {
                //resultLabel.Text = "Done!";
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            waitTime = (int)((Slider)sender).Value;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            points.Clear();
            PointCanvas.Children.Clear();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                if (isDeleting)
                {
                    isDeleting = false;
                    RemoveButton.Content = "Remove points";
                }
                isAdding = !isAdding;
                if (isAdding)
                {
                    AddButton.Content = "Stop Creating";
                }
                else
                {
                    AddButton.Content = "Create points";
                }
            }
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                if (isAdding)
                {
                    isAdding = false;
                    AddButton.Content = "Create points";
                }

                isDeleting = !isDeleting;
                if (isDeleting)
                {
                    RemoveButton.Content = "Stop removing";
                }
                else
                {
                    RemoveButton.Content = "Remove points";
                }
            }
        }

        private void PointCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!backgroundWorker1.IsBusy && (isAdding || isDeleting))
            {
                System.Windows.Point p = Mouse.GetPosition(PointCanvas);
                if (isAdding)
                {
                    points.Add(new Dot(p.X, p.Y));

                    DrawPoints();
                }

                if (isDeleting)
                {
                    double minDistance = 1000000d;
                    int index = -1;
                    for (int i = 0; i < points.Count; i++)
                    {
                        double distance = points[i].GetSquareDistance(p.X, p.Y);
                        if (distance <= deleteCircle && distance < minDistance)
                        {
                            minDistance = distance;
                            index = i;
                        }
                    }

                    if (index != -1)
                    {
                        points.RemoveAt(index);
                        DrawPoints();
                    }
                }
            }
        }

    }
}

public class Dot
{
    public double x;
    public double y;

    public Dot(double x, double y)
    {
        this.x = x;
        this.y = y;
    }
    public Dot(Dot a, Dot b, double divider)
    {
        this.x = (a.x + b.x) / divider;
        this.y = (a.y + b.y) / divider;
    }
    public double GetSquareDistance(double x2, double y2)
    {
        return ((x - x2) * (x - x2)) + ((y - y2)*(y - y2));
    }

}
