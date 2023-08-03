using System.Collections.Generic;

namespace Prime31
{
	public class FacebookGameRequestContent
	{
		private string actionType;

		public string title = string.Empty;

		public string message = string.Empty;

		public string data = string.Empty;

		public string objectId = string.Empty;

		public List<string> recipients = new List<string>();

		public List<string> recipientSuggestions = new List<string>();

		public bool frictionlessRequestsEnabled;

		public FacebookGameRequestActionType requestActionType
		{
			set
			{
				switch (value)
				{
				case FacebookGameRequestActionType.AskFor:
					actionType = "AskFor";
					break;
				case FacebookGameRequestActionType.Send:
					actionType = "Send";
					break;
				case FacebookGameRequestActionType.Turn:
					actionType = "Turn";
					break;
				default:
					actionType = string.Empty;
					break;
				}
			}
		}
	}
}
