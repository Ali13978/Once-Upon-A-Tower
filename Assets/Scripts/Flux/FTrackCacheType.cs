using System;

namespace Flux
{
	[Flags]
	public enum FTrackCacheType
	{
		None = 0x0,
		Editor = 0x1,
		Runtime = 0x2
	}
}
