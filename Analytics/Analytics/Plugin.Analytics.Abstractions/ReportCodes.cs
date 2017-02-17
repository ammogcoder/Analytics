using System;
namespace Plugin.Analytics.Abstractions
{
	public struct ReportCodes
	{
		public static string Generic
		{
			get
			{
				return "[Generic] Generic Application Error - Unexpected code.";
			}
		}

		public static string G_1000
		{
			get
			{
				return "[G-1000] Generic Application Error - Unexpected code.";
			}
		}

		public static string NR_1000
		{
			get
			{
				return "[NR-1000] Network Error - Transient connection.";
			}
		}

		public static string NR_1001
		{
			get
			{
				return "[NR-1001] Network Error - Connection required.";
			}
		}

		public static string NR_1002
		{
			get
			{
				return "[NR-1002] Network Error - Intervention required.";
			}
		}

		public static string C_1000
		{
			get
			{
				return "[C-1000] Communication Error - Bad request.";
			}
		}

		public static string S_1000
		{
			get
			{
				return "[S-1000] Server Error - Request exception.";
			}
		}

		public static string S_1001
		{
			get
			{
				return "[S-1001] Server Error - Task canceled.";
			}
		}

		public static string S_1002
		{
			get
			{
				return "[S-1002] Server Error - Generic response failure.";
			}
		}

		public static string S_1004
		{
			get
			{
				return "[S-1004] Server Error - Bad format.";
			}
		}

		public static string E_1000
		{
			get
			{
				return "[E-1000] Encryption Error - Keychain access.";
			}
		}

		public static string E_1001
		{
			get
			{
				return "[E-1001] Encryption Error - Encrypt error.";
			}
		}

		public static string E_1002
		{
			get
			{
				return "[E-1002] Encryption Error - Decrypt error.";
			}
		}

		public static string I_1000
		{
			get
			{
				return "[I-1000] I/O Error - File not found.";
			}
		}

		public static string I_1001
		{
			get
			{
				return "[I-1001] I/O Error - Unable to store data locally, device is low on storage.";
			}
		}

		public static string U_1000
		{
			get
			{
				return "[U-1000] Usage Tracking - Session time.";
			}
		}

		public static string U_1001
		{
			get
			{
				return "[U-1001] Usage Tracking - User ID.";
			}
		}

		public static string U_1002
		{
			get
			{
				return "[U-1002] Usage Tracking - Path.";
			}
		}

		public static string SP_1000
		{
			get
			{
				return "[SP-1000] Service Performance - Response time.";
			}
		}

		public static string SP_1001
		{
			get
			{
				return "[SP-1001] Service Performance - Success.";
			}
		}

		public static string SP_1002
		{
			get
			{
				return "[SP-1002] Service Performance - Failure.";
			}
		}

		public static string G_1001
		{
			get
			{
				return "[G-1001] Invalid Operation - System exception.";
			}
		}
	}
}

