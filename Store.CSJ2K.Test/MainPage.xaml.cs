using System;
using System.IO;
using CSJ2K;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Store.CSJ2K.Test
{
	using Windows.UI.Xaml.Media;

	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.  The Parameter
		/// property is typically used to configure the page.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
		}

		private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			try
			{
				var picker = new FileOpenPicker { SuggestedStartLocation = PickerLocationId.DocumentsLibrary };
				picker.FileTypeFilter.Add(".jp2");
				picker.FileTypeFilter.Add(".ppm");
				picker.FileTypeFilter.Add(".pgm");
				picker.FileTypeFilter.Add(".pgx");

				var file = await picker.PickSingleFileAsync();
				if (file == null) return;

				using (var stream = await file.OpenAsync(FileAccessMode.Read))
				{
					ImageSource image;
					if (file.FileType.Equals(".jp2"))
					{
						image = (ImageSource)J2kImage.FromStream(stream.AsStreamForRead());
					}
					else
					{
						// If not already encoded, encode before decoding
						var bytes = J2kImage.ToBytes(stream.AsStreamForRead());
						image = (ImageSource)J2kImage.FromBytes(bytes);
					}
					DecodedImage.Source = image;
					ImageName.Text = file.Path;
				}
			}
			catch (Exception exc)
			{
				DecodedImage.Source = null;
				ImageName.Text = "Could not display file, reason: " + exc.Message;
			}
		}
	}
}
