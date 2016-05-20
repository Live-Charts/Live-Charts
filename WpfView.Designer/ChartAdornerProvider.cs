using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Windows.Design.Interaction;
using Microsoft.Windows.Design.Model;

namespace WpfView.Designer
{
    public class ChartAdornerProvider
    {
        // The following class implements an adorner provider for the 
        // AutoSizeButton control. The adorner is a CheckBox control, which 
        // changes the Height and Width of the AutoSizeButton to "Auto",
        // which is represented by double.NaN.
        public class AutoSizeAdornerProvider : PrimarySelectionAdornerProvider
        {
            private bool settingProperties;
            private ModelItem adornedControlModel;
            private CheckBox autoSizeCheckBox;
            private AdornerPanel autoSizeAdornerPanel;

            // The constructor sets up the adorner control. 
            public AutoSizeAdornerProvider()
            {
                autoSizeCheckBox = new CheckBox();
                autoSizeCheckBox.Content = "AutoSize";
                autoSizeCheckBox.IsChecked = true;
                autoSizeCheckBox.FontFamily = AdornerFonts.FontFamily;
                autoSizeCheckBox.FontSize = AdornerFonts.FontSize;
                autoSizeCheckBox.Background = AdornerResources.FindResource(
                    AdornerColors.RailFillBrushKey) as Brush;
            }

            // The following method is called when the adorner is activated.
            // It creates the adorner control, sets up the adorner panel,
            // and attaches a ModelItem to the AutoSizeButton.
            protected override void Activate(ModelItem item)
            {
                // Save the ModelItem and hook into when it changes.
                // This enables updating the slider position when 
                // a new background value is set.
                adornedControlModel = item;
                adornedControlModel.PropertyChanged +=
                    new System.ComponentModel.PropertyChangedEventHandler(
                        AdornedControlModel_PropertyChanged);

                // All adorners are placed in an AdornerPanel
                // for sizing and layout support.
                AdornerPanel panel = this.Panel;

                // Set up the adorner's placement.
                AdornerPanel.SetAdornerHorizontalAlignment(autoSizeCheckBox, AdornerHorizontalAlignment.OutsideLeft);
                AdornerPanel.SetAdornerVerticalAlignment(autoSizeCheckBox, AdornerVerticalAlignment.OutsideTop);

                // Listen for changes to the checked state.
                autoSizeCheckBox.Checked += new RoutedEventHandler(autoSizeCheckBox_Checked);
                autoSizeCheckBox.Unchecked += new RoutedEventHandler(autoSizeCheckBox_Unchecked);

                // Run the base implementation.
                base.Activate(item);
            }

            // The Panel utility property demand-creates the 
            // adorner panel and adds it to the provider's 
            // Adorners collection.
            private AdornerPanel Panel
            {
                get
                {
                    if (this.autoSizeAdornerPanel == null)
                    {
                        autoSizeAdornerPanel = new AdornerPanel();

                        // Add the adorner to the adorner panel.
                        autoSizeAdornerPanel.Children.Add(autoSizeCheckBox);

                        // Add the panel to the Adorners collection.
                        Adorners.Add(autoSizeAdornerPanel);
                    }

                    return this.autoSizeAdornerPanel;
                }
            }

            // The following code handles the Checked event.
            // It autosizes the adorned control's Height and Width.
            private void autoSizeCheckBox_Checked(object sender, RoutedEventArgs e)
            {
                this.SetHeightAndWidth(true);
            }

            // The following code handles the Unchecked event.
            // It sets the adorned control's Height and Width to a hard-coded value.
            private void autoSizeCheckBox_Unchecked(object sender, RoutedEventArgs e)
            {
                this.SetHeightAndWidth(false);
            }

            // The SetHeightAndWidth utility method sets the Height and Width
            // properties through the model and commits the change.
            private void SetHeightAndWidth(bool autoSize)
            {
                settingProperties = true;

                try
                {
                    using (ModelEditingScope batchedChange = adornedControlModel.BeginEdit())
                    {
                        ModelProperty widthProperty =
                            adornedControlModel.Properties["Width"];

                        ModelProperty heightProperty =
                            adornedControlModel.Properties["Height"];

                        if (autoSize)
                        {
                            widthProperty.ClearValue();
                            heightProperty.ClearValue();
                        }
                        else
                        {
                            widthProperty.SetValue(20d);
                            heightProperty.SetValue(20d);
                        }

                        batchedChange.Complete();
                    }
                }
                finally
                {
                    settingProperties = false;
                }
            }

            // The following method deactivates the adorner.
            protected override void Deactivate()
            {
                adornedControlModel.PropertyChanged -=
                    new System.ComponentModel.PropertyChangedEventHandler(
                        AdornedControlModel_PropertyChanged);

                base.Deactivate();
            }

            // The following method handles the PropertyChanged event.
            private void AdornedControlModel_PropertyChanged(
                object sender,
                System.ComponentModel.PropertyChangedEventArgs e)
            {
                if (settingProperties)
                {
                    return;
                }

                if (e.PropertyName == "Height" || e.PropertyName == "Width")
                {
                    double h = (double) adornedControlModel.Properties["Height"].ComputedValue;
                    double w = (double) adornedControlModel.Properties["Width"].ComputedValue;

                    autoSizeCheckBox.IsChecked = (h == double.NaN && w == double.NaN) ? true : false;
                }
            }
        }
    }
}
