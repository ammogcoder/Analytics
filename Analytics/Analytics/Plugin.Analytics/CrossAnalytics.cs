using Plugin.Analytics.Abstractions;
using System;

namespace Plugin.Analytics
{
  /// <summary>
  /// Cross platform Analytics implemenations
  /// </summary>
  public class CrossAnalytics
  {
    static Lazy<IAnalytics> Implementation = new Lazy<IAnalytics>(() => CreateAnalytics(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current settings to use
    /// </summary>
    public static IAnalytics Current
    {
      get
      {
        var ret = Implementation.Value;
        if (ret == null)
        {
          throw NotImplementedInReferenceAssembly();
        }
        return ret;
      }
    }

    static IAnalytics CreateAnalytics()
    {
#if PORTABLE
        return null;
#else
        return new AnalyticsImplementation();
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
