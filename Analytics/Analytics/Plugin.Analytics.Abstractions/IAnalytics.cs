using System;

namespace Plugin.Analytics.Abstractions
{
    /// <summary>
    /// Interface for Analytics
    /// </summary>
    public interface IAnalytics
    {
		/*
         * You only need to set User ID on a tracker once.
         * By setting it on the tracker, the ID will be sent with all subsequent hits
         */
		void TrackUser(string userId);

		/*
         * @param ScreenName The name of an application screen.
         */
		void TrackScreen(string screenName);

		/*
         * @param EventCategory completely upon requirements.
         */
		void TrackEvent(string eventCategory, string eventAction, string eventLabel = "AppEvent", long eventValue = 0);

		/* 
		 * @params TimingCategory, TimingName completely upon requirements.
         * @param TimingInterval The time it takes to load a resource.
         */
		void TrackTime(string timingCategory, string timingName, long timingInterval, string timingLabel = "AppSpeed");

		void TrackException(Exception ex, bool isFatal);
		string ParseException(Exception ex);

		/*
		 * @param transactionName
		 * @param transactionId (strings separated by comma)
		 * @cound the number of transactions (default 1)
		 */
		void TrackTransaction(string transactionName, string transactionId, long transactionCount = 1);
    }
}
