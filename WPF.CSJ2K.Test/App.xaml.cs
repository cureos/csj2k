using System.Composition;
using System.Composition.Hosting;
using System.Windows;
using CSJ2K;
using CSJ2K.Util;

namespace WPF.CSJ2K.Test
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private void App_OnStartup(object sender, StartupEventArgs e)
		{
			new ContainerConfiguration().WithAssembly(typeof(WriteableBitmapWrapperCreator).Assembly)
			                            .CreateContainer().SatisfyImports(new CSJ2KSetup());
		}
	}
}
