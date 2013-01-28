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
		    var picker = new FileOpenPicker { SuggestedStartLocation = PickerLocationId.DocumentsLibrary };
		    picker.FileTypeFilter.Add(".jp2");
		    var file = await picker.PickSingleFileAsync();
		    if (file == null) return;

		    using (var stream = await file.OpenAsync(FileAccessMode.Read))
		    {
			    var image = (WriteableBitmap)J2kImage.FromStream(stream.AsStreamForRead());
				DecodedImage.Source = image;
			    ImageName.Text = file.Path;
		    }
	    }

	    private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
	    {
			StoreJ2kImage.Initialize();
	    }
    }
}
