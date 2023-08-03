using System;

namespace Flux
{
	[Flags]
	public enum CacheMode
	{
		Editor = 0x1,
		RuntimeForward = 0x2,
		RuntimeBackwards = 0x4
	}
}
