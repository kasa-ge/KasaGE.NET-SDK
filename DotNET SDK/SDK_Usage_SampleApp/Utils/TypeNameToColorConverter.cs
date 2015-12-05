using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SDK_Usage_SampleApp.Utils
{
	public class TypeNameToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.ToString().Contains("Exception") 
				? new SolidColorBrush(Color.FromRgb(205, 92, 92)) 
				: new SolidColorBrush(Color.FromRgb(46, 139, 87));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}