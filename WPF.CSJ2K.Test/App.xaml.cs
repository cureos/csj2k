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
#if MEF
			System.ComponentModel.Composition.AttributedModelServices.SatisfyImportsOnce(
				new System.ComponentModel.Composition.Hosting.CompositionContainer(
					new System.ComponentModel.Composition.Hosting.AssemblyCatalog(typeof(CSJ2KSetup).Assembly)), new CSJ2KSetup());
#else
			CSJ2KSetup.RegisterCreators();
#endif
		}
	}
}
