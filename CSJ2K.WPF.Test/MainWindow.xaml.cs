// Copyright (c) 2007-2016 CSJ2K contributors.
// Licensed under the BSD 3-Clause License.

using CSJ2K;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Media;

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
                var image = J2kImage.FromStream(stream).As<ImageSource>();
                DecodedImage.Source = image;
                ImageName.Text = dlg.FileName;
            }
        }
    }
}
