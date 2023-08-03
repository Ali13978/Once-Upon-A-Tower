using System;
using UnityEngine;

namespace Flux
{
	[Serializable]
	public struct FrameRange
	{
		[SerializeField]
		private int _start;

		[SerializeField]
		private int _end;

		public int Start
		{
			get
			{
				return _start;
			}
			set
			{
				_start = value;
			}
		}

		public int End
		{
			get
			{
				return _end;
			}
			set
			{
				_end = value;
			}
		}

		public int Length
		{
			get
			{
				return _end - _start;
			}
			set
			{
				End = _start + value;
			}
		}

		public FrameRange(int start, int end)
		{
			_start = start;
			_end = end;
		}

		public int Cull(int i)
		{
			return Mathf.Clamp(i, _start, _end);
		}

		public bool Contains(int i)
		{
			return i >= _start && i <= _end;
		}

		public bool ContainsExclusive(int i)
		{
			return i > _start && i < _end;
		}

		public bool Collides(FrameRange range)
		{
			return _start < range._end && _end > range._start;
		}

		public bool Overlaps(FrameRange range)
		{
			return range.End >= _start && range.Start <= _end;
		}

		public FrameRangeOverlap GetOverlap(FrameRange range)
		{
			if (range._start >= _start)
			{
				if (range._end <= _end)
				{
					return FrameRangeOverlap.ContainsFull;
				}
				if (range._start > _end)
				{
					return FrameRangeOverlap.MissOnRight;
				}
				return FrameRangeOverlap.ContainsStart;
			}
			if (range._end < _start)
			{
				return FrameRangeOverlap.MissOnLeft;
			}
			if (range._end > _end)
			{
				return FrameRangeOverlap.IsContained;
			}
			return FrameRangeOverlap.ContainsEnd;
		}

		public static bool operator ==(FrameRange a, FrameRange b)
		{
			return a._start == b._start && a._end == b._end;
		}

		public static bool operator !=(FrameRange a, FrameRange b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return (FrameRange)obj == this;
		}

		public override int GetHashCode()
		{
			return _start + _end;
		}

		public override string ToString()
		{
			return $"[{_start}; {_end}]";
		}
	}
}
