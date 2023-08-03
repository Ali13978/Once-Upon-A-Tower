using UnityEngine;

namespace Flux
{
	[FEvent("Audio/Play Audio")]
	public class FPlayAudioEvent : FEvent
	{
		[SerializeField]
		private AudioClip _audioClip;

		[Range(0f, 1f)]
		[SerializeField]
		private float _volume = 1f;

		[SerializeField]
		private bool _loop;

		[SerializeField]
		[HideInInspector]
		private int _startOffset;

		[SerializeField]
		private bool _speedDeterminesPitch = true;

		private AudioSource _source;

		public AudioClip AudioClip => _audioClip;

		public bool Loop => _loop;

		public int StartOffset => _startOffset;

		public bool SpeedDeterminesPitch
		{
			get
			{
				return _speedDeterminesPitch;
			}
			set
			{
				_speedDeterminesPitch = value;
			}
		}

		public override string Text
		{
			get
			{
				return (!(_audioClip == null)) ? _audioClip.name : "!Missing!";
			}
			set
			{
			}
		}

		protected override void OnTrigger(float timeSinceTrigger)
		{
			_source = Owner.GetComponent<AudioSource>();
			if (_source == null)
			{
				_source = Owner.gameObject.AddComponent<AudioSource>();
			}
			_source.volume = _volume;
			_source.loop = _loop;
			_source.clip = _audioClip;
			if (Sequence.IsPlaying)
			{
				_source.Play();
			}
			_source.time = (float)_startOffset * Sequence.InverseFrameRate + timeSinceTrigger;
			if (SpeedDeterminesPitch)
			{
				_source.pitch = Sequence.Speed * Time.timeScale;
			}
		}

		protected override void OnPause()
		{
			_source.Pause();
		}

		protected override void OnResume()
		{
			if (Sequence.IsPlaying)
			{
				_source.Play();
			}
		}

		protected override void OnFinish()
		{
			if (_source.clip == _audioClip && _source.isPlaying)
			{
				_source.Stop();
				_source.clip = null;
			}
		}

		protected override void OnStop()
		{
			if (_source.clip == _audioClip && _source.isPlaying)
			{
				_source.Stop();
				_source.clip = null;
			}
		}

		public override int GetMaxLength()
		{
			if (_loop || _audioClip == null)
			{
				return base.GetMaxLength();
			}
			return Mathf.RoundToInt(_audioClip.length * (float)Sequence.FrameRate);
		}

		public int GetMaxStartOffset()
		{
			if (_audioClip == null)
			{
				return 0;
			}
			int num = Mathf.RoundToInt(_audioClip.length * (float)Sequence.FrameRate);
			if (_loop)
			{
				return num;
			}
			return num - base.Length;
		}
	}
}
