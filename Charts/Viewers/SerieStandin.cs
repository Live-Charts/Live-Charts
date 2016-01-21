using System.ComponentModel;
using System.Windows.Media;
using LiveCharts.Annotations;

namespace LiveCharts.Viewers
{
	public class SerieStandin : INotifyPropertyChanged
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