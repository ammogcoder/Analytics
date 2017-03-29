using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Analytics;

namespace Demo.Droid
{
	[Activity(Label = "Demo.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			//TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

		    //AnalyticsImplementation.Init(3, this, "UA-87410133-1", 1, true, false, 1);
			//CrossAnalytics.Current.TrackUser("alexrainman");

			LoadApplication(new App());
		}
	}
}
