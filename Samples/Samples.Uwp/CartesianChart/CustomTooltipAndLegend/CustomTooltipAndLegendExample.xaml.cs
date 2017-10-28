using LiveCharts;
using LiveCharts.Configurations;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.CustomTooltipAndLegend
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomTooltipAndLegendExample : Page
    {
        public CustomTooltipAndLegendExample()
        {
            InitializeComponent();

            Customers = new ChartValues<CustomerVm>
            {
                new CustomerVm
                {
                    Name = "Irvin",
                    LastName = "Hale",
                    Phone = 123456789,
                    PurchasedItems = 8
                },
                new CustomerVm
                {
                    Name = "Malcolm",
                    LastName = "Revees",
                    Phone = 098765432,
                    PurchasedItems = 3
                },
                new CustomerVm
                {
                    Name = "Anne",
                    LastName = "Rios",
                    Phone = 758294026,
                    PurchasedItems = 6
                },
                new CustomerVm
                {
                    Name = "Vivian",
                    LastName = "Howell",
                    Phone = 309382739,
                    PurchasedItems = 3
                },
                new CustomerVm
                {
                    Name = "Caleb",
                    LastName = "Roy",
                    Phone = 682902826,
                    PurchasedItems = 2
                }
            };

            Labels = new[] { "Irvin", "Malcolm", "Anne", "Vivian", "Caleb" };

            //let create a mapper so LiveCharts know how to plot our CustomerViewModel class
            var customerVmMapper = Mappers.Xy<CustomerVm>()
                .X((value, index) => index) // lets use the position of the item as X
                .Y(value => value.PurchasedItems); //and PurchasedItems property as Y

            //lets save the mapper globally
            Charting.For<CustomerVm>(customerVmMapper);

            DataContext = this;
        }

        public ChartValues<CustomerVm> Customers { get; set; }
        public string[] Labels { get; set; }
    }
}
