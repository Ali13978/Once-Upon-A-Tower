using System.Collections.Generic;
using UnityEngine;

public class Util
{
	private static List<string> CrappyGPUs = new List<string>
	{
		"Mali400",
		"Adreno306",
		"MaliT720",
		"Adreno305",
		"Mali450",
		"Adreno405",
		"MaliT760",
		"Adreno304",
		"PowerVRSGX544",
		"MaliT830",
		"PowerVRG62xx",
		"Adreno505",
		"VivanteGC1000",
		"Adreno320",
		"VideocoreIV",
		"MaliT624",
		"VivanteGC7000",
		"PowerVRSGX540",
		"Adreno203",
		"MaliT628",
		"PowerVRSGX531",
		"Adreno200",
		"MaliT622",
		"PowerVRG64xx",
		"Adreno225",
		"Tegra2",
		"Adreno308",
		"VivanteGC2000",
		"Adreno220",
		"MaliG71",
		"Mali300",
		"Adreno205"
	};

	public static bool IsIPhoneX => false;

	public static bool UseMultipleRenderTargets => SystemInfo.supportedRenderTargetCount > 1 && (SystemInfo.graphicsDeviceName == null || !SystemInfo.graphicsDeviceName.StartsWith("Mali-"));

	public static bool IsLowPerformanceDevice
	{
		get
		{
			if (SystemInfo.systemMemorySize < 3000)
			{
				return true;
			}
			foreach (string crappyGPU in CrappyGPUs)
			{
				if (SystemInfo.graphicsDeviceName.Contains(crappyGPU))
				{
					return true;
				}
			}
			return false;
		}
	}
}
