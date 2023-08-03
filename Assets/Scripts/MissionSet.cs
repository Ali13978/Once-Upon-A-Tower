public class MissionSet
{
	public MissionStatus[] Missions;

	public bool Completed
	{
		get
		{
			for (int i = 0; i < Missions.Length; i++)
			{
				if (!Missions[i].Completed)
				{
					return false;
				}
			}
			return true;
		}
	}
}
