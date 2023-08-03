using System;

[Serializable]
public class SectionInfo
{
	public int BuildIndex;

	public string Name;

	public string Path;

	public int Width;

	public float Weight = 1f;

	public string[] Tags;

	[NonSerialized]
	public float ComputedWeight;

	public bool Testing => Path.Contains("/Testing/");
}
