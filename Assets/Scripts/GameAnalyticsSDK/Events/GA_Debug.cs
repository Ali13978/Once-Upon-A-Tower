using System.Collections.Generic;
using UnityEngine;

namespace GameAnalyticsSDK.Events
{
	public static class GA_Debug
	{
		public static int MaxErrorCount = 10;

		private static int _errorCount;

		private static bool _showLogOnGUI;

		public static List<string> Messages;

		public static void HandleLog(string logString, string stackTrace, LogType type)
		{
			if (_showLogOnGUI)
			{
				if (Messages == null)
				{
					Messages = new List<string>();
				}
				Messages.Add(logString);
			}
			if (GameAnalytics.SettingsGA.SubmitErrors && _errorCount < MaxErrorCount && type != LogType.Log)
			{
				_errorCount++;
				string str = logString.Replace('"', '\'').Replace('\n', ' ').Replace('\r', ' ');
				string str2 = stackTrace.Replace('"', '\'').Replace('\n', ' ').Replace('\r', ' ');
				SubmitError(str + " " + str2, type);
			}
		}

		private static void SubmitError(string message, LogType type)
		{
			GAErrorSeverity severity = GAErrorSeverity.Info;
			switch (type)
			{
			case LogType.Assert:
				severity = GAErrorSeverity.Info;
				break;
			case LogType.Error:
				severity = GAErrorSeverity.Error;
				break;
			case LogType.Exception:
				severity = GAErrorSeverity.Critical;
				break;
			case LogType.Log:
				severity = GAErrorSeverity.Debug;
				break;
			case LogType.Warning:
				severity = GAErrorSeverity.Warning;
				break;
			}
			GA_Error.NewEvent(severity, message);
		}

		public static void EnabledLog()
		{
			_showLogOnGUI = true;
		}
	}
}
