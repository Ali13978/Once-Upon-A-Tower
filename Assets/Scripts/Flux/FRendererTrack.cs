using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
	public class FRendererTrack : FTrack
	{
		private class MaterialPropertyBlockInfo
		{
			public MaterialPropertyBlock _materialPropertyBlock = new MaterialPropertyBlock();

			public int _frameGotCleared;

			public void Clear(int frame)
			{
				_materialPropertyBlock.Clear();
				_frameGotCleared = frame;
			}
		}

		private static Dictionary<int, MaterialPropertyBlockInfo> _materialPropertyBlocks;

		private MaterialPropertyBlockInfo _matPropertyBlockInfo;

		private Renderer _renderer;

		public Renderer Renderer => _renderer;

		private static MaterialPropertyBlockInfo GetMaterialPropertyBlockInfo(int objInstanceId)
		{
			if (_materialPropertyBlocks == null)
			{
				_materialPropertyBlocks = new Dictionary<int, MaterialPropertyBlockInfo>();
			}
			MaterialPropertyBlockInfo value = null;
			if (_materialPropertyBlocks.TryGetValue(objInstanceId, out value))
			{
				return value;
			}
			value = new MaterialPropertyBlockInfo();
			_materialPropertyBlocks.Add(objInstanceId, value);
			return value;
		}

		public MaterialPropertyBlock GetMaterialPropertyBlock()
		{
			return (_matPropertyBlockInfo == null) ? null : _matPropertyBlockInfo._materialPropertyBlock;
		}

		public override void Init()
		{
			_renderer = ((!(Owner != null)) ? null : Owner.GetComponent<Renderer>());
			if (!(_renderer is SpriteRenderer))
			{
				_matPropertyBlockInfo = GetMaterialPropertyBlockInfo((!(Owner != null)) ? (-1) : Owner.GetInstanceID());
			}
			base.Init();
		}

		public override void UpdateEvents(int frame, float time)
		{
			if (_matPropertyBlockInfo != null && _matPropertyBlockInfo._frameGotCleared != frame)
			{
				_matPropertyBlockInfo.Clear(frame);
			}
			base.UpdateEvents(frame, time);
			if (_matPropertyBlockInfo != null)
			{
				_renderer.SetPropertyBlock(_matPropertyBlockInfo._materialPropertyBlock);
			}
		}

		public override void UpdateEventsEditor(int currentFrame, float currentTime)
		{
			if (_matPropertyBlockInfo != null && _matPropertyBlockInfo._frameGotCleared != currentFrame)
			{
				_matPropertyBlockInfo.Clear(currentFrame);
			}
			base.UpdateEventsEditor(currentFrame, currentTime);
			if (_matPropertyBlockInfo != null)
			{
				_renderer.SetPropertyBlock(_matPropertyBlockInfo._materialPropertyBlock);
			}
		}

		public override void Stop()
		{
			base.Stop();
			if (_matPropertyBlockInfo != null)
			{
				_matPropertyBlockInfo.Clear(_matPropertyBlockInfo._frameGotCleared);
				if (_renderer != null)
				{
					_renderer.SetPropertyBlock(_matPropertyBlockInfo._materialPropertyBlock);
				}
			}
		}
	}
}
