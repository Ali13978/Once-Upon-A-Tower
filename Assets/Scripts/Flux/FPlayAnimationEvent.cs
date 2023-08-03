using UnityEngine;

namespace Flux
{
	[FEvent("Animation/Play Animation", typeof(FAnimationTrack))]
	public class FPlayAnimationEvent : FEvent
	{
		[HideInInspector]
		public AnimationClip _animationClip;

		[HideInInspector]
		[SerializeField]
		private bool _controlsAnimation;

		[HideInInspector]
		[Tooltip("How long is the transition blending?")]
		public int _blendLength;

		[HideInInspector]
		[Tooltip("What's the offset from where we play the animation?")]
		public int _startOffset;

		[HideInInspector]
		public int _stateHash;

		private Animator _animator;

		private FAnimationTrack _animTrack;

		public bool ControlsAnimation
		{
			get
			{
				return _controlsAnimation;
			}
			set
			{
				_controlsAnimation = (FUtility.IsAnimationEditable(_animationClip) && value);
			}
		}

		public override string Text
		{
			get
			{
				return (!(_animationClip == null)) ? _animationClip.name : "!Missing!";
			}
			set
			{
			}
		}

		protected override void OnTrigger(float timeSinceTrigger)
		{
			_animator = Owner.GetComponent<Animator>();
			_animTrack = (FAnimationTrack)_track;
			if (_animator.runtimeAnimatorController != _animTrack.AnimatorController)
			{
				_animator.runtimeAnimatorController = _animTrack.AnimatorController;
			}
			_animator.enabled = (_animationClip != null);
			int id = GetId();
			if (_animator.enabled)
			{
				_animator.SetLayerWeight(_animTrack.LayerId, 1f);
				if (id == 0 || _track.Events[id - 1].End < base.Start)
				{
					_animator.Play(_stateHash, _animTrack.LayerId, (float)_startOffset * Sequence.InverseFrameRate / _animationClip.length);
				}
				if (timeSinceTrigger > 0f)
				{
					_animator.Update(timeSinceTrigger - 0.001f);
				}
				else
				{
					_animator.Update(0f);
				}
			}
		}

		protected override void OnUpdateEvent(float timeSinceTrigger)
		{
			if (!_animator.enabled)
			{
				_animator.enabled = true;
			}
		}

		protected override void OnFinish()
		{
			if ((bool)_animator && (base.IsLastEvent || _track.GetEvent(GetId() + 1).Start != base.End))
			{
				_animator.enabled = false;
				_animator.SetLayerWeight(_animTrack.LayerId, 0f);
			}
		}

		protected override void OnStop()
		{
			int id = GetId();
			if ((bool)_animator && (id == 0 || _track.GetEvent(id - 1).End != base.Start))
			{
				_animator.SetLayerWeight(_animTrack.LayerId, 0f);
				_animator.enabled = false;
			}
		}

		protected override void OnPause()
		{
			_animator.enabled = false;
		}

		protected override void OnResume()
		{
			_animator.enabled = true;
		}

		public override int GetMaxLength()
		{
			if (FUtility.IsAnimationEditable(_animationClip) || _animationClip.isLooping)
			{
				return base.GetMaxLength();
			}
			return Mathf.RoundToInt(_animationClip.length * _animationClip.frameRate - (float)_startOffset);
		}

		public bool IsBlending()
		{
			int id = GetId();
			return id > 0 && _track != null && _track.Events[id - 1].End == base.Start && ((FAnimationTrack)_track).AnimatorController != null && ((FPlayAnimationEvent)_track.Events[id - 1])._animationClip != null && _animationClip != null;
		}

		public int GetMaxStartOffset()
		{
			if (_animationClip == null)
			{
				return 0;
			}
			return (!_animationClip.isLooping) ? (Mathf.RoundToInt(_animationClip.length * _animationClip.frameRate) - base.Length) : base.Length;
		}
	}
}
