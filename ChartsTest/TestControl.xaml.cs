using System.Collections.ObjectModel;
using System.Windows;

namespace ChartsTest
{
    /// <summary>
    /// Interaction logic for TestControl.xaml
    /// </summary>
    public partial class TestControl 
    {

        public static readonly DependencyProperty CollectionProperty = DependencyProperty.Register(
            "Collection", typeof (ObservableCollection<string>), typeof (TestControl), 
            new PropertyMetadata(new ObservableCollection<string>())); 

        public ObservableCollection<string> Collection
        {
            get { return (ObservableCollection<string>) GetValue(CollectionProperty); }
            set { SetValue(CollectionProperty, value); }
        }

        public TestControl()
        {
            InitializeComponent();
            ItemsControl.DataContext = this;
        }
    }
}
