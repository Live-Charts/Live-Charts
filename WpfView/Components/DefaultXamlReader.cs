using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace LiveCharts.Wpf.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class DefaultXamlReader
    {
        /// <summary>
        /// Creates the specified type.
        /// </summary>
        /// <returns></returns>
        public static DataTemplate DataLabelTemplate()
        {
            var stringReader = new StringReader(
                @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                    <TextBlock Text=""{Binding FormattedText}""></TextBlock>
                  </DataTemplate>");

            var xmlReader = XmlReader.Create(stringReader);
            return XamlReader.Load(xmlReader) as DataTemplate;
        }
    }
}
