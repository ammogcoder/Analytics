using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Gms.Analytics;
using Plugin.Analytics.Abstractions;

namespace Plugin.Analytics
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class AnalyticsImplementation : IAnalytics
    {
		public static VerbosityLevel Verbosity;
		public static GoogleAnalytics GAInstance;
		public static Tracker GATracker;
		public static int UserIdDimensionIndex;

		public static void Init(int verbosity, Context context, string trackingId, int localDispatchPeriod = 1800, bool trackUncaughtExceptions = true, bool enableAutoActivityTracking = false, int userIdDimensionIndex = 0)
		{
			Verbosity = (VerbosityLevel)verbosity;

			GAInstance = GoogleAnalytics.GetInstance(context);
			GAInstance.SetLocalDispatchPeriod(localDispatchPeriod);

			GATracker = GAInstance.NewTracker(trackingId);
			GATracker.EnableAutoActivityTracking(enableAutoActivityTracking);
			GATracker.EnableExceptionReporting(false);

			if (trackUncaughtExceptions)
			{
				AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
				{
					var ex = (Exception)e.ExceptionObject;
					TrackUnhandledException(ex);
				};

				TaskScheduler.UnobservedTaskException += (sender, e) =>
				{
					var ex = e.Exception;
					TrackUnhandledException(ex);
				};
			}

			UserIdDimensionIndex = userIdDimensionIndex;
		}

		static void TrackUnhandledException(Exception ex)
		{
			var builder_ex = new HitBuilders.ExceptionBuilder();

			var errorMessage = CrossAnalytics.Current.ParseException(ex);

			SetUserIDDimension(builder_ex);

			builder_ex.SetDescription(errorMessage);
			builder_ex.SetFatal(true);
			GATracker.Send(builder_ex.Build());

			var builder_ev = new HitBuilders.EventBuilder();

			SetUserIDDimension(builder_ev);

			builder_ev.SetCategory("Crashes");
			builder_ev.SetAction(errorMessage);
			builder_ev.SetLabel(ex.ToString());
			GATracker.Send(builder_ev.Build());
		}

		static void SetUserIDDimension(HitBuilders.HitBuilder builder)
		{
			if (UserIdDimensionIndex > 0)
			{
				var userId = string.Empty;

				try
				{
					userId = GATracker.Get("&uid");
				}
				catch (Exception obj)
				{
					Console.WriteLine(obj.Message);
				}

				if (!string.IsNullOrEmpty(userId))
					builder.SetCustomDimension(UserIdDimensionIndex, userId);
			}
		}

		public void TrackUser(string userId)
		{
            if (Verbosity != VerbosityLevel.AnalyticsOff)
            {
                GATracker.Set("&uid", userId);
			}
		}

		public void TrackScreen(string screenName)
		{
			if (Verbosity == VerbosityLevel.ReportAll)
			{
				var builder = new HitBuilders.ScreenViewBuilder();

				SetUserIDDimension(builder);

				GATracker.SetScreenName(screenName);
				GATracker.Send(builder.Build());
			}
		}

		public void TrackEvent(string eventCategory, string eventAction, string eventLabel = "AppEvent", long eventValue = 0)
		{
			if (Verbosity == VerbosityLevel.ReportAll)
			{
				var builder = new HitBuilders.EventBuilder();

				SetUserIDDimension(builder);

				builder.SetCategory(eventCategory);
				builder.SetAction(eventAction);
				builder.SetLabel(eventLabel);

				builder.SetValue(eventValue);

				GATracker.Send(builder.Build());
			}
		}

		public void TrackTime(string timingCategory, string timingName, long timingInterval, string timingLabel = "AppSpeed")
		{
			if (Verbosity >= VerbosityLevel.TimeTracking)
			{
				var builder = new HitBuilders.TimingBuilder();

				SetUserIDDimension(builder);

				builder.SetCategory(timingCategory);
				builder.SetVariable(timingName);
				builder.SetLabel(timingLabel);

				builder.SetValue(timingInterval);

				GATracker.Send(builder.Build());
			}
		}

		public void TrackException(Exception ex, bool isFatal)
		{
            if (Verbosity != VerbosityLevel.AnalyticsOff)
            {
                var builder = new HitBuilders.ExceptionBuilder();

				SetUserIDDimension(builder);

				builder.SetDescription(ParseException(ex));
				builder.SetFatal(isFatal);

				GATracker.Send(builder.Build());
			}
		}

		public string ParseException(System.Exception ex)
		{
			// root cause, returns current ex if InnerException is null
			var e = ex.GetBaseException();

			// Type (@class:method) {message}
			var errorMessage = e.GetType().ToString();

			if (e.TargetSite != null)
			{
				errorMessage += " (@" + e.TargetSite.DeclaringType.FullName + ":" + e.TargetSite.Name + ")";
			}

			errorMessage += " {" + e.Message + "}";

			return errorMessage;
		}

		public void TrackTransaction(string transactionName, string transactionId, long transactionCount = 1)
		{
            if (Verbosity != VerbosityLevel.AnalyticsOff)
            {
                var builder = new HitBuilders.EventBuilder();

                SetUserIDDimension(builder);

                builder.SetCategory("Transactions");
                builder.SetAction(transactionName);
                builder.SetLabel(transactionId);

                builder.SetValue(transactionCount);

                GATracker.Send(builder.Build());
            }
		}
	}
}