using Android.App;
using Android.Content;
using Android.Util;
using Android.OS;
using Android.Content.PM;
using Plugin.Analytics;
using System;

namespace Demo.Droid
{
	[Service(Exported = true, Enabled = true)]
	[IntentFilter(new[] { "com.mobileiron.HANDLE_CONFIG" })]
	public class AppConnectConfigService : IntentService
	{
		const string TAG = "AppConnectConfigService";

		public AppConnectConfigService() : base("AppConnectConfigService")
		{
		}

		public static void requestConfig(Context ctx)
		{
			var intent = new Intent("com.mobileiron.REQUEST_CONFIG");
			intent.SetPackage("com.forgepond.locksmith");
			intent.PutExtra("packageName", ctx.PackageName);
			ctx.StartService(intent);
		}

		protected override void OnHandleIntent(Intent intent)
		{

			/**
			 * This block of code should be uncommented in production so that you only receive configs from
			 * a trusted source.
			 */

			try
			{

				if (!bool.Parse(Java.Lang.JavaSystem.GetProperty("com.mobileiron.wrapped", "false")) &&
					!PackageName.Equals(PackageManager.GetPermissionInfo("com.mobileiron.CONFIG_PERMISSION", 0).PackageName))
				{
					Log.Debug(TAG, "Refusing intent as we don't own our permission?!");
					return;
				}

			}
			catch (PackageManager.NameNotFoundException ex)
			{
				Log.Debug(TAG, ex.Message + " " + "Refusing intent as we can't find our permission?!");
				return;
			}

			if ("com.mobileiron.HANDLE_CONFIG".Equals(intent.Action))
			{

				Log.Debug(TAG, "Received intent : " + intent + " from package " + intent.GetStringExtra("packageName"));

				Bundle config = intent.GetBundleExtra("config");

				if (config != null)
				{
					Log.Debug(TAG, "Config received");

					var alias = "";

					foreach (var key in config.KeySet())
					{
						if (key == "alias")
						{
							alias = config.GetString(key);
							Console.WriteLine("alias = {0}", config.GetString(key));
						}
					}

					AnalyticsImplementation.Init(3, this, "UA-87410133-1", 1, true, false, 1);
					CrossAnalytics.Current.TrackUser(alias);
				}
			}
		}
	}
}

