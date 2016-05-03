// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using CSJ2K;

namespace Store.CSJ2K.Test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        private FileOpenPicker picker;

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
            var j2KExts = new[] { ".jp2", ".j2k", ".j2c" };

            try
            {
                var files = await picker.PickMultipleFilesAsync();
                if (files == null) return;

                var streams =
                    await
                    Task.WhenAll(
                        files.Select(async file => (await file.OpenAsync(FileAccessMode.Read)).AsStreamForRead()));

                ImageSource image;
                if (streams.Length == 1
                    && j2KExts.Any(ext => files[0].FileType.Equals(ext, StringComparison.OrdinalIgnoreCase)))
                {
                    image = J2kImage.FromStream(streams[0]).As<ImageSource>();
                }
                else
                {
                    // If not already encoded, encode before decoding
                    var bytes = J2kImage.ToBytes(J2kImage.CreateEncodableSource(streams));
                    image = J2kImage.FromBytes(bytes).As<ImageSource>();
                }
                DecodedImage.Source = image;
                ImageName.Text = files[0].Path;
            }
            catch (Exception exc)
            {
                DecodedImage.Source = null;
                ImageName.Text = "Could not display file, reason: " + exc.Message;
            }
        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            picker = new FileOpenPicker { SuggestedStartLocation = PickerLocationId.DocumentsLibrary };
            picker.FileTypeFilter.Add(".jp2");
            picker.FileTypeFilter.Add(".j2k");
            picker.FileTypeFilter.Add(".j2c");
            picker.FileTypeFilter.Add(".ppm");
            picker.FileTypeFilter.Add(".pgm");
            picker.FileTypeFilter.Add(".pgx");
        }
    }
}
