using System;
using System.Threading.Tasks;
using Google.Analytics;
using Plugin.Analytics.Abstractions;

namespace Plugin.Analytics
{
    /// <summary>
    /// Implementation for Analytics
    /// </summary>
    public class AnalyticsImplementation : IAnalytics
    {
		public static VerbosityLevel Verbosity;
		public static ITracker Tracker;
		public static int UserIdDimensionIndex;

		public static void Init(int verbosity, string trackingId, int localDispatchPeriod = 120, bool trackUncaughtExceptions = true, int userIdDimensionIndex = 0)
		{
			Verbosity = (VerbosityLevel)verbosity;

			Gai.SharedInstance.DispatchInterval = localDispatchPeriod;
			Gai.SharedInstance.TrackUncaughtExceptions = false;

			Tracker = Gai.SharedInstance.GetTracker(trackingId);

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
			SetUserIDDimension();

			var errorMessage = CrossAnalytics.Current.ParseException(ex);

			Tracker.Send(DictionaryBuilder.CreateException(errorMessage, true).Build());
			Tracker.Send(DictionaryBuilder.CreateEvent("Crashes", errorMessage, ex.ToString(), null).Build());
			Gai.SharedInstance.Dispatch();
		}

		static void SetUserIDDimension()
		{
			if (UserIdDimensionIndex > 0)
			{
				var userId = string.Empty;

				try
				{
					userId = Tracker.Get(GaiConstants.UserId);
				}
				catch (Exception obj)
				{
					Console.WriteLine(obj.Message);
				}

				if (!string.IsNullOrEmpty(userId))
					Tracker.Set(Fields.CustomDimension((nuint)UserIdDimensionIndex), userId);
			}
		}

		public void TrackUser(string userId)
		{
			if (Verbosity != VerbosityLevel.AnalyticsOff)
			{
				Tracker.Set(GaiConstants.UserId, userId);
			}
		}

		public void TrackScreen(string screenName)
		{
			if (Verbosity == VerbosityLevel.ReportAll)
			{
				SetUserIDDimension();

				Tracker.Set(GaiConstants.ScreenName, screenName);
				Tracker.Send(DictionaryBuilder.CreateScreenView().Build());
			}
		}

		public void TrackEvent(string eventCategory, string eventAction, string eventLabel = "AppEvent", long eventValue = 0)
		{
			if (Verbosity == VerbosityLevel.ReportAll)
			{
				SetUserIDDimension();

				Tracker.Send(DictionaryBuilder.CreateEvent(eventCategory, eventAction, eventLabel, eventValue).Build());
				Gai.SharedInstance.Dispatch();
			}
		}

		public void TrackTime(string timingCategory, string timingName, long timingInterval, string timingLabel = "AppSpeed")
		{
			if (Verbosity >= VerbosityLevel.TimeTracking)
			{
				SetUserIDDimension();

				Tracker.Send(DictionaryBuilder.CreateTiming(timingCategory, timingInterval, timingName, timingLabel).Build());
			}
		}

		public void TrackException(Exception ex, bool isFatal)
		{
			if (Verbosity == VerbosityLevel.ReportAll)
			{
				SetUserIDDimension();

				Tracker.Send(DictionaryBuilder.CreateException(ParseException(ex), isFatal).Build());
			}
		}

		public string ParseException(Exception ex)
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
			SetUserIDDimension();

			Tracker.Send(DictionaryBuilder.CreateEvent("Transactions", transactionName, transactionId, transactionCount).Build());
			Gai.SharedInstance.Dispatch();
		}
    }
}