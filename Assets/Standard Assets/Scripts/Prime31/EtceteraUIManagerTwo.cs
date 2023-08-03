using UnityEngine;

namespace Prime31
{
	public class EtceteraUIManagerTwo : MonoBehaviourGUI
	{
		private int _fiveSecondNotificationId;

		private void OnGUI()
		{
			beginColumn();
			if (GUILayout.Button("Show Inline Web View"))
			{
				EtceteraAndroid.inlineWebViewShow("http://prime31.com/", 160, 430, Screen.width - 160, Screen.height - 100);
			}
			if (GUILayout.Button("Close Inline Web View"))
			{
				EtceteraAndroid.inlineWebViewClose();
			}
			if (GUILayout.Button("Set Url of Inline Web View"))
			{
				EtceteraAndroid.inlineWebViewSetUrl("http://google.com");
			}
			if (GUILayout.Button("Set Frame of Inline Web View"))
			{
				EtceteraAndroid.inlineWebViewSetFrame(80, 50, 300, 400);
			}
			if (GUILayout.Button("Get First 25 Contacts"))
			{
				EtceteraAndroid.loadContacts(0, 25);
			}
			GUILayout.Label("Request M Permissions");
			if (GUILayout.Button("Request Permission"))
			{
				EtceteraAndroid.requestPermissions(new string[1]
				{
					"android.permission.READ_PHONE_STATE"
				});
			}
			if (GUILayout.Button("Should Show Permission Rationale"))
			{
				bool flag = EtceteraAndroid.shouldShowRequestPermissionRationale("android.permission.READ_PHONE_STATE");
				UnityEngine.Debug.Log("shouldShowRequestPermissionRationale: " + flag);
			}
			if (GUILayout.Button("Check Permission"))
			{
				bool flag2 = EtceteraAndroid.checkSelfPermission("android.permission.READ_PHONE_STATE");
				UnityEngine.Debug.Log("checkSelfPermission: " + flag2);
			}
			endColumn(hasSecondColumn: true);
			if (toggleButtonState("Camera Capture"))
			{
				notificationsUI();
			}
			else
			{
				cameraCaptureUI();
			}
			GUILayout.Space(30f);
			toggleButton("Camera Capture", "Notifications");
			endColumn();
			if (bottomRightButton("Previous Scene"))
			{
				MonoBehaviourGUI.loadLevel("EtceteraTestScene");
			}
		}

		private void notificationsUI()
		{
			GUILayout.Label("Notifications");
			if (GUILayout.Button("Schedule Notification in 5s"))
			{
				AndroidNotificationConfiguration androidNotificationConfiguration = new AndroidNotificationConfiguration(5L, "Notification Title - 5 Seconds", "The subtitle of the notification", "Ticker text gets ticked");
				androidNotificationConfiguration.extraData = "five-second-note";
				androidNotificationConfiguration.groupKey = "my-note-group";
				AndroidNotificationConfiguration androidNotificationConfiguration2 = androidNotificationConfiguration;
				androidNotificationConfiguration2.sound = false;
				androidNotificationConfiguration2.vibrate = false;
				_fiveSecondNotificationId = EtceteraAndroid.scheduleNotification(androidNotificationConfiguration2);
				UnityEngine.Debug.Log("notificationId: " + _fiveSecondNotificationId);
			}
			if (GUILayout.Button("Schedule Group Summary Notification in 5s"))
			{
				AndroidNotificationConfiguration androidNotificationConfiguration = new AndroidNotificationConfiguration(5L, "Group Summary Title", "Group Summary Subtitle - Stuff Happened", "Ticker text");
				androidNotificationConfiguration.extraData = "group-summary-note";
				androidNotificationConfiguration.groupKey = "my-note-group";
				androidNotificationConfiguration.isGroupSummary = true;
				AndroidNotificationConfiguration config = androidNotificationConfiguration;
				EtceteraAndroid.scheduleNotification(config);
			}
			if (GUILayout.Button("Cancel 5s Notification"))
			{
				EtceteraAndroid.cancelNotification(_fiveSecondNotificationId);
			}
			if (GUILayout.Button("Check for Notifications"))
			{
				EtceteraAndroid.checkForNotifications();
			}
			if (GUILayout.Button("Cancel All Notifications"))
			{
				EtceteraAndroid.cancelAllNotifications();
			}
		}

		private void cameraCaptureUI()
		{
			GUILayout.Label("Camera Capture");
			if (GUILayout.Button("Start Camera Capture"))
			{
				EtceteraAndroid.startCameraCapture(false, 160, 100, Screen.width - 180, Screen.height - 120);

			}
			if (GUILayout.Button("Stop Camera Capture"))
			{
				EtceteraAndroid.stopCameraCapture();
			}
			if (GUILayout.Button("Set Frame of Camera Preview"))
			{
				EtceteraAndroid.cameraCaptureSetFrame(40, 40, 300, 200);
			}
			if (GUILayout.Button("Set Frame of Camera Preview"))
			{
				EtceteraAndroid.cameraCaptureSetFrame(300, 300, 800, 800);
			}
		}
	}
}
