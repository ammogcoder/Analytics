
namespace Plugin.Analytics.Abstractions
{
	public enum VerbosityLevel
	{
		/// <summary>
		/// Insights Disabled
		/// </summary>
		AnalyticsOff = 0,
		/// <summary>
		/// Report user id only
		/// </summary>
		UserIdentificationOnly = 1,
		/// <summary>
		/// Report user id plus time tracking
		/// </summary>
		TimeTracking = 2,
		/// <summary>
		/// Report all
		/// </summary>
		ReportAll = 3
	}
}

