using LiveCharts.Wpf.Charts.Chart;
using Microsoft.Windows.Design.Features;
using Microsoft.Windows.Design.Metadata;

[assembly: ProvideMetadata(typeof(WpfView.Designer.Metadata))]
namespace WpfView.Designer
{
    // Container for any general design-time metadata to initialize.
    // Designers look for a type in the design-time assembly that 
    // implements IProvideAttributeTable. If found, designers instantiate 
    // this class and access its AttributeTable property automatically.
    internal class Metadata : IProvideAttributeTable
    {
        // Accessed by the designer to register any design-time metadata.
        public AttributeTable AttributeTable
        {
            get
            {
                AttributeTableBuilder builder = new AttributeTableBuilder();

                builder.AddCustomAttributes(
                    typeof(Chart),
                    new FeatureAttribute(typeof(ChartAdornerProvider)));

                return builder.CreateTable();
            }
        }
    }
}
