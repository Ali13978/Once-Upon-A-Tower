using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
	public class FSequencePlayer : MonoBehaviour
	{
		[SerializeField]
		private List<FSequence> _sequences = new List<FSequence>();

		[SerializeField]
		[Tooltip("Init all the sequences at the start? Use this to avoid frame drops at runtime")]
		private bool _initAllOnStart = true;

		[SerializeField]
		private bool _playOnStart = true;

		[SerializeField]
		[Tooltip("At which update rate should we update the sequences and check if they are finished")]
		private AnimatorUpdateMode _updateMode;

		private int _currentSequence = -1;

		public List<FSequence> Sequences => _sequences;

		public bool InitAllOnStart
		{
			get
			{
				return _initAllOnStart;
			}
			set
			{
				_initAllOnStart = value;
			}
		}

		public bool PlayOnStart
		{
			get
			{
				return _playOnStart;
			}
			set
			{
				_playOnStart = value;
			}
		}

		public AnimatorUpdateMode UpdateMode
		{
			get
			{
				return _updateMode;
			}
			set
			{
				_updateMode = value;
			}
		}

		public bool IsPlaying => _currentSequence >= 0;

		private void Start()
		{
			if (InitAllOnStart)
			{
				InitAll();
			}
			if (PlayOnStart)
			{
				Play();
			}
		}

		public void InitAll()
		{
			foreach (FSequence sequence in _sequences)
			{
				if (sequence != null)
				{
					sequence.Init();
				}
			}
		}

		public void Play()
		{
			Play(0);
		}

		public void Play(int sequenceIndex)
		{
			if (IsPlaying)
			{
				_sequences[_currentSequence].Pause();
			}
			_currentSequence = sequenceIndex;
			_sequences[_currentSequence].UpdateMode = UpdateMode;
			if (!_sequences[_currentSequence].IsStopped)
			{
				_sequences[_currentSequence].Stop();
			}
			_sequences[_currentSequence].Play();
		}

		public void Stop(bool reset)
		{
			foreach (FSequence sequence in _sequences)
			{
				sequence.Stop(reset);
			}
		}

		private void CheckSequence()
		{
			if (IsPlaying && _sequences[_currentSequence].IsFinished)
			{
				_currentSequence++;
				if (_currentSequence < _sequences.Count)
				{
					Play(_currentSequence);
				}
				else
				{
					_currentSequence = -1;
				}
			}
		}

		private void LateUpdate()
		{
			if (UpdateMode != AnimatorUpdateMode.AnimatePhysics)
			{
				CheckSequence();
			}
		}

		private void FixedUpdate()
		{
			if (UpdateMode == AnimatorUpdateMode.AnimatePhysics)
			{
				CheckSequence();
			}
		}
	}
}
