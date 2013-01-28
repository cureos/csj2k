using System.Windows;
using CSJ2K;

namespace WPF.CSJ2K.Test
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private void App_OnStartup(object sender, StartupEventArgs e)
		{
			WpfJ2kImage.Initialize();
		}
	}
}
