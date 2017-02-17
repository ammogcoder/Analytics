using Foundation;
using Plugin.Analytics;
using UIKit;

namespace Demo.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			AnalyticsImplementation.Init(3, "UA-87410133-2", 1, true, 1);
			CrossAnalytics.Current.TrackUser("alexrainman");

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
