using LiveCharts;
using LiveCharts.Configurations;
using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.ConstantChanges
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConstantChangesChart : Page, INotifyPropertyChanged
    {
        private double _axisMax;
        private double _axisMin;

        public ConstantChangesChart()
        {
            InitializeComponent();

            //To handle live data easily, in this case we built a specialized type
            //the MeasureModel class, it only contains 2 properties
            //DateTime and Value
            //We need to configure LiveCharts to handle MeasureModel class
            //The next code configures MEasureModel  globally, this means
            //that livecharts learns to plot MeasureModel and will use this config every time
            //a ChartValues instance uses this type.
            //this code ideally should only run once, when application starts is reccomended.
            //you can configure series in many ways, learn more at http://lvcharts.net/App/examples/v1/wpf/Types%20and%20Configuration

            var mapper = Mappers.Xy<MeasureModel>()
                .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
                .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<MeasureModel>(mapper);


            //the values property will store our values array
            ChartValues = new ChartValues<MeasureModel>();

            //lets set how to display the X Labels
            DateTimeFormatter = value => new DateTime((long)(value)).ToString("mm:ss");

            AxisStep = TimeSpan.FromSeconds(1).Ticks;
            SetAxisLimits(DateTime.Now);

            //The next code simulates data changes every 300 ms
            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(300)
            };
            Timer.Tick += TimerOnTick;
            IsDataInjectionRunning = false;
            R = new Random();

            DataContext = this;
        }

        public ChartValues<MeasureModel> ChartValues { get; set; }
        public Func<double, string> DateTimeFormatter { get; set; }

        public double AxisStep { get; set; }

        public double AxisMax
        {
            get { return _axisMax; }
            set
            {
                _axisMax = value;
                OnPropertyChanged("AxisMax");
            }
        }
        public double AxisMin
        {
            get { return _axisMin; }
            set
            {
                _axisMin = value;
                OnPropertyChanged("AxisMin");
            }
        }

        public DispatcherTimer Timer { get; set; }
        public bool IsDataInjectionRunning { get; set; }
        public Random R { get; set; }

        private void RunDataOnClick(object sender, RoutedEventArgs e)
        {
            if (IsDataInjectionRunning)
            {
                Timer.Stop();
                IsDataInjectionRunning = false;
            }
            else
            {
                Timer.Start();
                IsDataInjectionRunning = true;
            }
        }

        private void TimerOnTick(object sender, object eventArgs)
        {
            var now = DateTime.Now;

            ChartValues.Add(new MeasureModel
            {
                DateTime = now,
                Value = R.Next(0, 10)
            });

            SetAxisLimits(now);

            //lets only use the last 30 values
            if (ChartValues.Count > 30) ChartValues.RemoveAt(0);
        }

        private void SetAxisLimits(DateTime now)
        {
            AxisMax = now.Ticks + TimeSpan.FromSeconds(1).Ticks; // lets force the axis to be 100ms ahead
            AxisMin = now.Ticks - TimeSpan.FromSeconds(8).Ticks; //we only care about the last 8 seconds
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
