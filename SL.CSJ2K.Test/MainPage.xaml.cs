using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CSJ2K;

namespace SL.CSJ2K.Test
{
	public partial class MainPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog
			{
				Filter = "JPEG 2000 files (*.jp2)|*.jp2",
				Multiselect = false
			};
			if (!dlg.ShowDialog().GetValueOrDefault()) return;

			using (var stream = dlg.File.OpenRead())
			{
				var image = (WriteableBitmap)J2kImage.FromStream(stream);
				DecodedImage.Source = image;
				ImageName.Text = dlg.File.Name;
			}
		}
	}
}
