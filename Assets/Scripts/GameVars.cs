using System;

public static class GameVars
{
	public static int[] PrizeCoins = new int[7]
	{
		1000,
		2000,
		4000,
		8000,
		16000,
		32000,
		64000
	};

	public static TimeSpan SaveMeMaxDuration = TimeSpan.FromSeconds(6.0);

	public static TimeSpan MinTimeBetweenAds = TimeSpan.FromSeconds(90.0);

	public static int CoinsPerLevel = 500;

	public static int MissionCount = 3;

	public static float[] StoreLevelMultiplier = new float[11]
	{
		1f,
		1.1f,
		1.2f,
		1.3f,
		1.4f,
		1.5f,
		1.6f,
		1.7f,
		1.8f,
		1.9f,
		2f
	};

	public static int SaveMesPerFlask = 10;

	public static int NotificationSoundStart = 10;

	public static int NotificationSoundEnd = 22;

	public static TimeSpan SaveInvulnerabilityTime = TimeSpan.FromMilliseconds(500.0);

	public static TimeSpan[] Reminders = new TimeSpan[3]
	{
		TimeSpan.FromDays(1.0),
		TimeSpan.FromDays(5.0),
		TimeSpan.FromDays(14.0)
	};

	public static int StartSaveMeCoins = 0;

	public static int CharacterDealSaveMeCoins = 5;

	public static TimeSpan FirstTimeBetweenGifts = TimeSpan.FromMinutes(10.0);

	public static TimeSpan TimeBetweenGifts = TimeSpan.FromHours(24.0);

	public static float CharacterGiftChance = 0.15f;

	public static float ChickenGiftChance = 0.15f;

	public static TimeSpan LuckyTimeCooldown = TimeSpan.FromMinutes(3.0);

	public static TimeSpan FirstTimeBetweenRateUs = TimeSpan.FromMinutes(7.0);

	public static TimeSpan TimeBetweenRateUs = TimeSpan.FromDays(7.0);
}
