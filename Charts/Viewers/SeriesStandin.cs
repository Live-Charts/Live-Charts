//The MIT License(MIT)

//Copyright(c) 2015 Greg Dennis

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.ComponentModel;
using System.Windows.Media;
using LiveCharts.Annotations;

namespace LiveCharts.Viewers
{
	public class SeriesStandin : INotifyPropertyChanged
	{
	    private Brush _stroke;
	    private Brush _fill;
	    private string _title;

	    public string Title
	    {
	        get { return _title; }
	        set
	        {
	            _title = value;
	            OnPropertyChanged(Title);
	        }
	    }

	    public Brush Stroke
	    {
	        get { return _stroke; }
	        set
	        {
	            _stroke = value;
	            OnPropertyChanged("Stroke");
	        }
	    }

	    public Brush Fill
	    {
	        get { return _fill; }
	        set
	        {
	            _fill = value;
	            OnPropertyChanged("Fill");
	        }
	    }

	    public event PropertyChangedEventHandler PropertyChanged;

	    [NotifyPropertyChangedInvocator]
	    protected virtual void OnPropertyChanged(string propertyName)
	    {
	        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	    }
	}
}