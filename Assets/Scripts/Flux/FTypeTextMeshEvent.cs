using TMPro;
using UnityEngine;

namespace Flux
{
	[FEvent("TextMesh/Type", typeof(FTextMeshTrack))]
	public class FTypeTextMeshEvent : FEvent
	{
		private string initialText;

		private TextMeshPro textMesh;

		protected override void OnInit()
		{
			base.OnInit();
			textMesh = Owner.GetComponent<TextMeshPro>();
			if (textMesh != null)
			{
				initialText = textMesh.text;
			}
		}

		protected override void OnTrigger(float timeSinceTrigger)
		{
			base.OnTrigger(timeSinceTrigger);
			textMesh = Owner.GetComponent<TextMeshPro>();
			if (textMesh != null)
			{
				initialText = textMesh.text;
			}
		}

		protected override void OnUpdateEvent(float timeSinceTrigger)
		{
			if (textMesh == null)
			{
				return;
			}
			if (SingletonMonoBehaviour<Gui>.HasInstance() && SingletonMonoBehaviour<Gui>.Instance.TextLoop != null && !SingletonMonoBehaviour<Gui>.Instance.TextLoop.isPlaying)
			{
				SingletonMonoBehaviour<Gui>.Instance.TextLoop.Play();
			}
			int num = Mathf.RoundToInt(timeSinceTrigger / base.LengthTime * (float)initialText.Length);
			if (num < 0)
			{
				num = 0;
			}
			if (num > initialText.Length)
			{
				num = initialText.Length;
			}
			int num2 = num - 1;
			while (num2 >= 0 && initialText[num2] != '>')
			{
				if (initialText[num2] == '<')
				{
					num = num2;
					break;
				}
				num2--;
			}
			textMesh.text = initialText.Substring(0, num);
		}

		protected override void OnStop()
		{
			base.OnStop();
			if (textMesh != null)
			{
				textMesh.text = initialText;
			}
			if (SingletonMonoBehaviour<Gui>.HasInstance() && SingletonMonoBehaviour<Gui>.Instance.TextLoop != null)
			{
				SingletonMonoBehaviour<Gui>.Instance.TextLoop.Stop();
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
			if (textMesh != null)
			{
				textMesh.text = initialText;
			}
			if (SingletonMonoBehaviour<Gui>.HasInstance() && SingletonMonoBehaviour<Gui>.Instance.TextLoop != null)
			{
				SingletonMonoBehaviour<Gui>.Instance.TextLoop.Stop();
			}
		}

		protected override void OnPause()
		{
			base.OnPause();
			if (SingletonMonoBehaviour<Gui>.HasInstance() && SingletonMonoBehaviour<Gui>.Instance.TextLoop != null)
			{
				SingletonMonoBehaviour<Gui>.Instance.TextLoop.Stop();
			}
		}
	}
}
