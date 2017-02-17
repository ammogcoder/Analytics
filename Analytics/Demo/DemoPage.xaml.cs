using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.Analytics;
using Xamarin.Forms;

namespace Demo
{
	public partial class DemoPage : ContentPage
	{
		public DemoPage()
		{
			InitializeComponent();

			CrossAnalytics.Current.TrackScreen("DemoPage");
		}

		async void Handle_Clicked(object sender, System.EventArgs e)
		{
			CrossAnalytics.Current.TrackEvent("ButtonClicked", "MainButton");

			var timer = new Stopwatch();
			timer.Start();
			await Task.Delay(1000);
			timer.Stop();

			CrossAnalytics.Current.TrackTime("GET", "DataSync", timer.ElapsedMilliseconds);

			CrossAnalytics.Current.TrackTransaction("Sync", "02-17-2017T12:14:22", 1);
			              
			try
			{
				int result1 = int.Parse("pepe");
			}
			catch (System.Exception ex)
			{
				CrossAnalytics.Current.TrackException(ex, false);
			}

			int result2 = int.Parse("web");
		}
	}
}
