# Google Analytics Plugin for Xamarin and Windows

* Available on NuGet: https://www.nuget.org/packages/Plugin.Analytics/ [![NuGet](https://img.shields.io/nuget/v/Plugin.Analytics.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.Analytics/)

**Requirements**

|Platform|Supported|Version|Component|
| ------------------- | :-----------: | :-----------: | :------------------: |
|Xamarin.iOS Unified|Yes|iOS 8.1+|Google Analytics Component for iOS|
|Xamarin.Android|Yes|API 15+|Google Play Service - Analytics|
|Windows|Yes|8+|Google Analytics SDK 1.3|

#### Setup

* Install in your PCL project and Client projects and call Init() at app startup.

```
//Android
AnalyticsImplementation.Init(verbosity, context, "XX-XXXXXXXX-X", localDispatchPeriod, trackUncaughtExceptions, enableAutoActivityTracking, userIdDimensionIndex);

//iOS
AnalyticsImplementation.Init(verbosity, "XX-XXXXXXXX-X", localDispatchPeriod, trackUncaughtExceptions, userIdDimensionIndex);

//Windows
AnalyticsImplementation.Init(verbosity, "XX-XXXXXXXX-X", localDispatchPeriod, trackUncaughtExceptions, userIdDimensionIndex);
```

#### Verbosity

Use this parameter to automatically configure the type of hits you send to Google Analytics.
You can completelly disable tracking (only crashes will be logged), or disable screens and events, tracking UserID and Timing only.

```
public enum VerbosityLevel
{
	/// <summary>
	/// Analytics Disabled
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
```

#### LocalDispatchPeriod

Data collected using the Google Analytics SDK for Android is stored locally before being dispatched on a separate thread to Google Analytics using the dispatch period interval.

#### TrackUncaughtExceptions

If true, unhandled exceptions will be automatically tracked.

#### UserIdDimensionIndex

This parameter is intended to automatically group Screens, Events, Timing, Crashes and Exceptions by UserID.

Requirements:

- A Custom Dimension: https://support.google.com/analytics/answer/2709829?hl=en
- User tracking enabled: https://developers.google.com/analytics/devguides/collection/ios/v3/user-id
- A UserID enabled View: https://support.google.com/analytics/answer/3123666?hl=en

Steps:

1 - Create a Custom Dimension named UserID and take note of it's index (use Hit scope).

2 - Call Init() passing the Index of the UserID Custom Dimension.

3 - Call TrackUser after initializing the plugin.

```
CrossAnalytics.Current.TrackUser("userId");
```

4 - Customize default reports to group by UserID.

Example:

- In your Google Analytics account, select app UserID enabled view.
- Go to REPORTING tab.
- Select "Behavior/Screens" report from the left menu.
- Click "Customize" (first option from the menu at the top).
- Go to "Dimensions Drilldowns".
- Click "+ add dimension".
- Select the previously created UserID Custom Dimension.
- Drag it to the top of dimension's list.
- Click "Save".

These steps apply to "Behavior/Events/Top Events" and "Behavior/Crashes and Exceptions" reports also.

To have a custom "AppSpeed" report follow these ones:

- Go to CUSTOMIZATIONS tab.
- Select "Overview" from the left menu.
- Click "New custom report"
- Title it "AppSpeed".
- Name the Report tab "Explorer"
- Go to "Metrics Group".
- Click "+ add metric"
- Add "Avg. User Timing (sec)" from "Behavior" section.
- Go to "Dimensions Drilldowns".
- Click "+ add dimension".
- Select the previously created UserID Custom Dimension.
- Click "+ add dimension".
- Add "Timing Category", "Timing Variable" and "Timing Label" from "Engagement" section.
- Click "Save".

Now you will see Screens, Events, Timing, Crashes and Exceptions grouped by UserID.

**AndroidManifest.xml**

```xml
<!-- Get permission for reliable local dispatching on non-Google Play devices. -->
<uses-permission android:name="android.permission.WAKE_LOCK" />
<application android:label="Demo">
	<!-- https://developers.google.com/analytics/devguides/collection/android/v4/dispatch#background -->
	<!-- Needed for MobileIron wrapped apk -->
	<!-- Register AnalyticsReceiver and AnalyticsService to support background dispatching on non-Google Play devices. -->
	<receiver android:name="com.google.android.gms.analytics.AnalyticsReceiver" android:enabled="true">
		<intent-filter>
			<action android:name="com.google.android.gms.analytics.ANALYTICS_DISPATCH" />
		</intent-filter>
	</receiver>
	<service android:name="com.google.android.gms.analytics.AnalyticsService" android:enabled="true" android:exported="false" />
</application>
```

#### Usage

**TrackUser(string userId)**

You only need to set UserID on a tracker once. By setting it on the tracker, the ID will be sent with all subsequent hits.

@param UserID Label identifying the user.

```
CrossAnalytics.Current.TrackUser("userId");
```

**TrackScreen(string screenName)**

In Google Analytics represent content users are viewing within your app. Measuring screen views allows you to see which content is being viewed most by your users, and how they are navigating between different pieces of content.

@param ScreenName The name of an application screen.

```
CrossAnalytics.Current.TrackScreen("Main Screen");
```
           
**TrackEvent(string eventCategory, string eventAction, string eventLabel = "AppEvent", long eventValue = 0)**

Events are a useful way to collect data about a user's interaction with interactive components of your app, like button presses or the use of a particular item in a game.

@params EventCategory, EventAction and EventLabel completely upon requirements.

@param EventValue is optional. For example, you could use it to provide the event time in seconds.

```
CrossAnalytics.Current.TrackEvent("Screen Lifecycle", "OnAppearing", 123456787980898);
```

**TrackTime(string timingCategory, string timingName, long timingInterval, string timingLabel = "AppSpeed")**

Measuring user timings provides a native way to measure a period of time in Google Analytics. This can be useful to measure resource load times.

User timing data can be found primarily in the App Speed User Timings report.

@params TimingCategory, TimingName and TimingLabel completely upon requirements.

@param TimingInterval The time it takes to load a resource.

```
CrossAnalytics.Current.TrackTime("Mapping", "GetTimeTypes", 200);
```

**TrackException(Exception ex, bool isFatal)**

Crash and exception measurement allows you to measure the number and type of caught and uncaught crashes and exceptions that occur in your app.

Exceptions will appear at "Behavior/Crashes and Exceptions" grouped by app version, in the format:

Type (@class:method) {message}

```
CrossAnalytics.Current.TrackException(ex, false);
```

Crashes represent instances where your app encountered unexpected conditions at runtime and are often fatal, causing the app to crash and are sent to Google Analytics automatically by setting the TrackUncaughtExceptions configuration value to true.

Also, for a detailed information, you will find associated events at "Behavior/Events/Overview" under “Crashes” category. This section will provide a full StackTrace in the "Event Label" column.

- Event Action = Type (@class:method) {message} 
- Event Label = ex.StackTrace

##### Documentation

https://analytics.google.com

https://developers.google.com/analytics/devguides/collection/

**Android**

https://developers.google.com/analytics/devguides/collection/android/v4/

**iOS**

https://developers.google.com/analytics/devguides/collection/ios/v3/

**Create and edit Custom Dimensions and Metrics**

https://support.google.com/analytics/answer/2709829?hl=en&ref_topic=2709827

**Scope**

https://support.google.com/analytics/answer/2709828#scope

**Google Analytics SDK for Windows**

https://googleanalyticssdk.codeplex.com/

#### Contributors
* [alexrainman](https://github.com/alexrainman)

Thanks!

#### License
Licensed under repo license
