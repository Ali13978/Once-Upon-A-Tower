using System;

namespace Flux
{
	public class FEventAttribute : Attribute
	{
		public string menu;

		public Type trackType;

		public FEventAttribute(string menu)
			: this(menu, typeof(FTrack))
		{
		}

		public FEventAttribute(string menu, Type trackType)
		{
			this.menu = menu;
			this.trackType = trackType;
		}
	}
}
