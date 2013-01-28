using System;
using CSJ2K;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WPF.CSJ2K.Test
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog
				          {
					          Filter = "JPEG 2000 files (*.jp2)|*.jp2",
					          Multiselect = false,
					          Title = "Select JPEG 2000 file"
				          };
			if (!dlg.ShowDialog(this).GetValueOrDefault()) return;

			using (var stream = dlg.OpenFile())
			{
				var image = (WriteableBitmap)J2kImage.FromStream(stream);
				DecodedImage.Source = image;
				ImageName.Text = dlg.FileName;
			}
		}
	}
}
