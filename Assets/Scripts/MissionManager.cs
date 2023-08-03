using Achievements;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionManager : SingletonMonoBehaviour<MissionManager>
{
	public const int MissionsPerSet = 3;

	private List<MissionSet> MissionDef = new List<MissionSet>();

	public int Level => SaveGame.Instance.MissionLevel;

	public MissionSet CurrentMissionSet
	{
		get
		{
			if (Level > MissionDef.Count)
			{
				return null;
			}
			return MissionDef[Level - 1];
		}
	}

	public MissionSet NextMissionSet
	{
		get
		{
			if (Level + 1 > MissionDef.Count)
			{
				return null;
			}
			return MissionDef[Level];
		}
	}

	private void AddMissionSet(MissionType type0, int count0, MissionType type1, int count1, MissionType type2, int count2)
	{
		MissionSet missionSet = new MissionSet();
		missionSet.Missions = new MissionStatus[3];
		missionSet.Missions[0] = new MissionStatus
		{
			Mission = new Mission
			{
				Type = type0,
				Count = count0
			},
			Progress = 0
		};
		missionSet.Missions[1] = new MissionStatus
		{
			Mission = new Mission
			{
				Type = type1,
				Count = count1
			},
			Progress = 0
		};
		missionSet.Missions[2] = new MissionStatus
		{
			Mission = new Mission
			{
				Type = type2,
				Count = count2
			},
			Progress = 0
		};
		MissionDef.Add(missionSet);
	}

	protected override void Awake()
	{
		base.Awake();
		AddMissionSet(MissionType.BreakFloor, 10, MissionType.KillEnemy, 5, MissionType.PassLevel, 1);
		AddMissionSet(MissionType.HitBackFireball, 1, MissionType.BreakFireballCube, 5, MissionType.PassLevel, 2);
		AddMissionSet(MissionType.BreakWheelbarrows, 3, MissionType.StepOnEnemy, 5, MissionType.KillAllEnemiesInLevel, 1);
		AddMissionSet(MissionType.KillEnemiesWithBombs, 2, MissionType.KillFirefly, 15, MissionType.PassLevelNoCoins, 1);
		AddMissionSet(MissionType.NearMissDragonFire, 1, MissionType.BreakFloor, 50, MissionType.Pacifist, 1);
		AddMissionSet(MissionType.RunOverEnemy, 1, MissionType.KillEnemy, 25, MissionType.PassLevel, 4);
		AddMissionSet(MissionType.HitFallingWildPig, 1, MissionType.StepOnEnemy, 15, MissionType.ChickenPassLevel, 1);
		AddMissionSet(MissionType.ScarePidgeon, 5, MissionType.HitBackThwomp, 1, MissionType.KillWhileFalling, 15);
		AddMissionSet(MissionType.BreakSpikeFromAbove, 1, MissionType.KillFromBelow, 5, MissionType.PassLevel, 5);
		AddMissionSet(MissionType.MakeTwoCombo, 3, MissionType.BreakFloor, 100, MissionType.Pacifist, 2);
		AddMissionSet(MissionType.BreakCrabFromAbove, 1, MissionType.KillEnemy, 50, MissionType.PassLevel, 6);
		AddMissionSet(MissionType.DragonKillPigeon, 1, MissionType.KillWhileFalling, 30, MissionType.ChickenPassLevel, 2);
		AddMissionSet(MissionType.KillEnemyWithGiftBox, 1, MissionType.HitBackFireball, 5, MissionType.PassLevel, 7);
		AddMissionSet(MissionType.HitFallingWildPig, 3, MissionType.HitBackThwomp, 5, MissionType.PassLevel, 8);
		AddMissionSet(MissionType.KillEnemiesWithBombs, 20, MissionType.KillAllEnemiesInLevel, 2, MissionType.PassLevel, 9);
		AddMissionSet(MissionType.MakeThreeCombo, 3, MissionType.KillEnemy, 100, MissionType.ChickenPassLevel, 6);
		AddMissionSet(MissionType.RunOverEnemy, 5, MissionType.StepOnEnemy, 40, MissionType.Pacifist, 3);
		AddMissionSet(MissionType.PassLevelNoCoins, 5, MissionType.BreakSpike, 50, MissionType.PassLevel, 11);
		AddMissionSet(MissionType.KillAllEnemiesInLevel, 4, MissionType.MakeThreeCombo, 5, MissionType.EscapeTower, 1);
		AddMissionSet(MissionType.Pacifist, 4, MissionType.HitBackFireball, 20, MissionType.EscapeTowerWithChicken, 1);
		bool[] completedMissions = SaveGame.Instance.CompletedMissions;
		for (int i = 0; i < completedMissions.Length; i++)
		{
			if (completedMissions[i])
			{
				MissionStatus missionStatus = CurrentMissionSet.Missions[i];
				missionStatus.Progress = missionStatus.Mission.Count;
				missionStatus.ShownCompleted = true;
				missionStatus.ShownCompletedOnSet = true;
			}
		}
		CheckLevelUpdate();
	}

	public void CheckLevelUpdate()
	{
		if (CurrentMissionSet != null && CurrentMissionSet.Completed)
		{
			SaveGame.Instance.MissionLevel++;
			bool[] completedMissions = new bool[3];
			SaveGame.Instance.CompletedMissions = completedMissions;
			SingletonMonoBehaviour<AchievementManager>.Instance.NotifyMissionLevel(SaveGame.Instance.MissionLevel);
		}
	}

	public void UpdateProgress(MissionType type)
	{
		if (CurrentMissionSet == null || type == MissionType.None || !SaveGame.Instance.TutorialComplete)
		{
			return;
		}
		MissionStatus[] missions = CurrentMissionSet.Missions;
		for (int i = 0; i < missions.Count(); i++)
		{
			MissionStatus missionStatus = missions[i];
			if (missionStatus.Mission.Type == type)
			{
				missionStatus.Progress++;
				if (missionStatus.Completed && !missionStatus.ShownCompleted)
				{
					bool[] completedMissions = SaveGame.Instance.CompletedMissions;
					completedMissions[i] = true;
					SaveGame.Instance.CompletedMissions = completedMissions;
					Gui.Views.InGameMissionCompleteView.ShowMissionCompleted(missionStatus);
				}
			}
		}
	}

	public void NotifyBreak(WorldObject wo, Vector3 direction)
	{
		switch (wo.WorldObjectType)
		{
		case WorldObjectType.Floor:
		case WorldObjectType.CrackedFloor:
			UpdateProgress(MissionType.BreakFloor);
			break;
		case WorldObjectType.FireballCube:
			UpdateProgress(MissionType.BreakFireballCube);
			break;
		case WorldObjectType.Spike:
			UpdateProgress(MissionType.BreakSpike);
			break;
		case WorldObjectType.Coin:
			UpdateProgress(MissionType.KillFirefly);
			break;
		case WorldObjectType.Cart:
			UpdateProgress(MissionType.BreakWheelbarrows);
			break;
		}
		if (direction == Vector3.down)
		{
			switch (wo.WorldObjectType)
			{
			case WorldObjectType.Spike:
				UpdateProgress(MissionType.BreakSpikeFromAbove);
				break;
			case WorldObjectType.Crab:
				UpdateProgress(MissionType.BreakCrabFromAbove);
				break;
			}
		}
	}
}
