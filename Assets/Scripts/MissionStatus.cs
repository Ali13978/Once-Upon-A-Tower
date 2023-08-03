using I2.Loc;

public class MissionStatus
{
	public Mission Mission;

	public int Progress;

	public bool ShownCompleted;

	public bool ShownCompletedOnSet;

	public string Text
	{
		get
		{
			string text = "Mission." + Mission.Type.ToString();
			if (Mission.Count == 1)
			{
				text += ".1";
			}
			return ScriptLocalization.Get(text).Replace("{C}", Mission.Count.ToString());
		}
	}

	public bool Completed => ShownCompleted || Progress >= Mission.Count;
}
