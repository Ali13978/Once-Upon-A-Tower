using System;
using UnityEngine.Events;

namespace Flux
{
	[Serializable]
	public class SequenceFinishedEvent : UnityEvent<FSequence>
	{
	}
}
