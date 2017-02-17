using GoogleAnalytics.Core;
using Plugin.Analytics.Abstractions;
using System;
using System.Threading.Tasks;
#if WINDOWS_PHONE
// WindowsPhone8
using System.Windows;
#else
using Windows.UI.Xaml;
#endif

namespace Plugin.Analytics
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class AnalyticsImplementation : IAnalytics
    {
        public static VerbosityLevel Verbosity;
        public static Tracker EasyTracker;
        public static int UserIdDimensionIndex;

        public static void Init(int verbosity, string trackingId, int localDispatchPeriod = 0, bool trackUncaughtExceptions = true, int userIdDimensionIndex = 0)
        {
            Verbosity = (VerbosityLevel)verbosity;

            var config = new GoogleAnalytics.EasyTrackerConfig();

            config.TrackingId = trackingId;
            config.DispatchPeriod = new TimeSpan(localDispatchPeriod * 1000);
            config.ReportUncaughtExceptions = false;

            GoogleAnalytics.EasyTracker.Current.Config = config;

            EasyTracker = GoogleAnalytics.EasyTracker.GetTracker();

            UserIdDimensionIndex = userIdDimensionIndex;

            if (trackUncaughtExceptions)
            {

                Application.Current.UnhandledException += (sender, e) =>
                {
#if WINDOWS_PHONE
                    var ex = e.ExceptionObject;
#else
                    var ex = e.Exception;
#endif
                    TrackUnhandledException(ex);
                };

                TaskScheduler.UnobservedTaskException += (sender, e) =>
                {
                    var ex = e.Exception;
                    TrackUnhandledException(ex);
                };
            }
        }

        static void TrackUnhandledException(Exception ex)
        {
            SetUserIDDimension();

            var errorMessage = CrossAnalytics.Current.ParseException(ex);

            EasyTracker.SendException(errorMessage, true);
            EasyTracker.SendEvent("Crashes", errorMessage, ex.ToString(), 0);
        }

        static void SetUserIDDimension()
        {
            if (UserIdDimensionIndex > 0)
            {
                if (!string.IsNullOrEmpty(EasyTracker.UserId))
                    EasyTracker.SetCustomDimension(UserIdDimensionIndex, EasyTracker.UserId);
            }
        }

        public void TrackUser(string userId)
        {
            if (Verbosity != VerbosityLevel.AnalyticsOff)
            {
                EasyTracker.UserId = userId;
            }
        }

        public void TrackScreen(string screenName)
        {
            if (Verbosity == VerbosityLevel.ReportAll)
            {
                SetUserIDDimension();

                EasyTracker.SendView(screenName);
            }
        }

        public void TrackEvent(string eventCategory, string eventAction, string eventLabel = "AppEvent", long eventValue = 0)
        {
            if (Verbosity == VerbosityLevel.ReportAll)
            {
                SetUserIDDimension();

                EasyTracker.SendEvent(eventCategory, eventAction, eventLabel, eventValue);
            }
        }

        public void TrackTime(string timingCategory, string timingName, long timingInterval, string timinglabel = "AppSpeed")
        {
            if (Verbosity >= VerbosityLevel.TimeTracking)
            {
                SetUserIDDimension();

                EasyTracker.SendTiming(new TimeSpan(timingInterval), timingCategory, timingName, timinglabel);
            }
        }

        public void TrackException(Exception ex, bool isFatal)
        {
            if (Verbosity != VerbosityLevel.AnalyticsOff)
            {
                SetUserIDDimension();

                EasyTracker.SendException(ParseException(ex), isFatal);
            }
        }

        public string ParseException(Exception ex)
        {
            // root cause, returns current ex if InnerException is null
            var e = ex.GetBaseException();
            var errorMessage = e.GetType().ToString();

#if WINDOWS_PHONE_APP
            // WindowsPhone81
#elif WINDOWS_APP
            //Windows81
#else
            // UWP, WINDOWS_PHONE 8
            var stacktrace = new System.Diagnostics.StackTrace(e, false);
            var frames = stacktrace.GetFrames();
            var frame = frames[0];
            errorMessage += " (@" + frame.GetMethod().DeclaringType.FullName + ":" + frame.GetMethod().Name + ")";
#endif

            errorMessage += " {" + e.Message + "}";

            return errorMessage;
        }

        public void TrackTransaction(string transactionName, string transactionId, long transactionCount = 1)
        {
            throw new NotImplementedException();
        }
    }
}