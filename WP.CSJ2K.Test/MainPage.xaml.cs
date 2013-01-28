using System.Windows;
using System.Windows.Media.Imaging;
using CSJ2K;
using Microsoft.Phone.Tasks;

namespace WP.CSJ2K.Test
{
	public partial class MainPage
	{
		#region FIELDS

		private readonly PhotoChooserTask _photoChooserTask;

		#endregion 

		// Constructor
		public MainPage()
		{
			InitializeComponent();

			_photoChooserTask = new PhotoChooserTask();
			_photoChooserTask.Completed += PhotoChooserTaskOnCompleted;

			// Sample code to localize the ApplicationBar
			//BuildLocalizedApplicationBar();
		}

		void PhotoChooserTaskOnCompleted(object sender, PhotoResult e)
		{
			if (e.Error == null && e.ChosenPhoto != null)
			{
				using (var stream = e.ChosenPhoto)
				{
					var image = (WriteableBitmap)J2kImage.FromStream(stream);
					DecodedImage.Source = image;
					ImageName.Text = e.OriginalFileName;
				}
			}
		}

		// Sample code for building a localized ApplicationBar
		//private void BuildLocalizedApplicationBar()
		//{
		//    // Set the page's ApplicationBar to a new instance of ApplicationBar.
		//    ApplicationBar = new ApplicationBar();

		//    // Create a new button and set the text value to the localized string from AppResources.
		//    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
		//    appBarButton.Text = AppResources.AppBarButtonText;
		//    ApplicationBar.Buttons.Add(appBarButton);

		//    // Create a new menu item with the localized string from AppResources.
		//    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
		//    ApplicationBar.MenuItems.Add(appBarMenuItem);
		//}
		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			_photoChooserTask.Show();
		}
	}
}