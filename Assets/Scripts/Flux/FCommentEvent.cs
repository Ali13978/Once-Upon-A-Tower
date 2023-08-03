using UnityEngine;

namespace Flux
{
	[FEvent("Misc/Comment", typeof(FCommentTrack))]
	public class FCommentEvent : FEvent
	{
		[SerializeField]
		private string _comment = "!Comment!";

		[SerializeField]
		private Color _color = new Color(0.15f, 0.6f, 0.95f, 0.8f);

		public override string Text
		{
			get
			{
				return _comment;
			}
			set
			{
				_comment = value;
			}
		}

		public Color Color
		{
			get
			{
				return _color;
			}
			set
			{
				_color = value;
			}
		}

		protected override void OnTrigger(float timeSinceTrigger)
		{
			FCommentTrack fCommentTrack = (FCommentTrack)base.Track;
			if (fCommentTrack.Label != null)
			{
				fCommentTrack.Label.text = Text;
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			ClearText();
		}

		protected override void OnStop()
		{
			base.OnStop();
			ClearText();
		}

		private void ClearText()
		{
			FCommentTrack fCommentTrack = (FCommentTrack)base.Track;
			if (fCommentTrack.Label != null)
			{
				fCommentTrack.Label.text = string.Empty;
			}
		}
	}
}
