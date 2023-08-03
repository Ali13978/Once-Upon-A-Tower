using Rewired.Dev;

namespace RewiredConsts
{
	public static class Action
	{
		[ActionIdFieldInfo(categoryName = "Default", friendlyName = "Left")]
		public const int Left = 0;

		[ActionIdFieldInfo(categoryName = "Default", friendlyName = "Right")]
		public const int Right = 1;

		[ActionIdFieldInfo(categoryName = "Default", friendlyName = "Up")]
		public const int Up = 2;

		[ActionIdFieldInfo(categoryName = "Default", friendlyName = "Down")]
		public const int Down = 3;

		[ActionIdFieldInfo(categoryName = "Default", friendlyName = "Back")]
		public const int Back = 4;

		[ActionIdFieldInfo(categoryName = "Default", friendlyName = "Ok")]
		public const int Ok = 5;
	}
}
