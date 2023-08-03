using UnityEngine;

namespace Flux
{
	[FEvent("Renderer/Tween Color", typeof(FRendererTrack))]
	public class FRendererColorEvent : FRendererEvent
	{
		[SerializeField]
		private FTweenColor _tween;

		[SerializeField]
		private bool _shared;

		private Renderer _renderer;

		private Color _spriteRendererColor;

		protected override void OnInit()
		{
			base.OnInit();
			_renderer = ((FRendererTrack)_track).Renderer;
			if (_renderer is SpriteRenderer)
			{
				_spriteRendererColor = ((SpriteRenderer)_renderer).color;
			}
		}

		protected override void SetDefaultValues()
		{
			if (base.PropertyName == null)
			{
				base.PropertyName = "_Color";
			}
			if (_tween == null)
			{
				_tween = new FTweenColor(new Color(1f, 1f, 1f, 0f), Color.white);
			}
		}

		protected override void ApplyProperty(float t)
		{
			if ((bool)_renderer)
			{
				Color value = _tween.GetValue(t);
				if (_renderer is SpriteRenderer)
				{
					((SpriteRenderer)_renderer).color = value;
				}
				else if (_renderer is ParticleSystemRenderer || _shared)
				{
					_renderer.sharedMaterial.SetColor(base.PropertyName, value);
				}
				else
				{
					_matPropertyBlock.SetColor(base.PropertyName, value);
				}
			}
		}

		protected override void OnStop()
		{
			if (_renderer is SpriteRenderer)
			{
				((SpriteRenderer)_renderer).color = _spriteRendererColor;
			}
			else
			{
				ApplyProperty(0f);
			}
		}
	}
}
