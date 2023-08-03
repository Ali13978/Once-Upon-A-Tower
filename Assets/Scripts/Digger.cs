using GameAnalyticsSDK;
using System;
using System.Collections;
using UnityEngine;

public class Digger : WorldObject
{
	private enum ActionType
	{
		Wait,
		Success,
		Failure
	}

	[HideInInspector]
	public bool IsDead;

	public bool WaitingSave;

	public ParticleSystem DustParticles;

	public ParticleSystem UnbreakHitLeft;

	public ParticleSystem UnbreakHitRight;

	public ParticleSystem UnbreakHitDown;

	public ParticleSystem UnbreakHitUp;

	public ParticleSystem DragParticles;

	public ParticleSystem HitSidesParticles;

	public ParticleSystem[] HardBootsLoopParticles;

	public ParticleSystem[] HitSidesLoopParticles;

	public ParticleSystem HitDistanceParticles;

	public ParticleSystem[] FireHammerParticles;

	public ParticleSystem[] ArmorParticles;

	public ParticleSystem ArmorBreakParticles;

	public AudioSource MoveAudio;

	public AudioSource HitUnbreakableAudio;

	public AudioSource SwingAudio;

	public AudioSource SideMoveAudio;

	public AudioSource SaveMeAudio;

	public AudioSource HammerFallAudio;

	public AudioSource ArmorBreakAudio;

	public AudioSource FireHammerAudio;

	public AudioSource BombAudio;

	public AudioSourceController ArmorLoopAudio;

	public float HitDelay = 0.2f;

	public float DownHitDelay = 0.2f;

	public GameObject MeshObject;

	public int Bombs;

	public bool HardBoots;

	public bool HitSidesWhenLanding;

	public int HitDistance;

	public CoinMagnet CoinMagnet;

	public GameObject BackShield;

	public bool Armor;

	public string[] BuiltInItems;

	public bool Invulnerability;

	private Vector3 lookDirection;

	private Coord lastMoveCoord;

	private float lastMoveTime;

	[HideInInspector]
	public float SavedTime;

	[HideInInspector]
	public Coord SavedCoord;

	private Action queued;

	private float queueTime;

	private MovingCube movingCube;

	public bool hitting;

	private WorldObject hittingWo;

	private float secondsSinceRunStart;

	private bool runStarted;

	public Vector3 LookDirection
	{
		set
		{
			lookDirection = value;
		}
	}

	public float SecondsSinceRunStart => secondsSinceRunStart;

	private void Start()
	{
		Walker.Animator = Animator;
		SavedTime = 0f;
		SavedCoord = new Coord(0, 0, 0);
	}

	public void Initialize(Vector3 position, Coord coord)
	{
		if (position == Vector3.zero && coord == Coord.None)
		{
			Initialize();
			return;
		}
		SingletonMonoBehaviour<Game>.Instance.Digger.transform.position = position;
		SingletonMonoBehaviour<Game>.Instance.Digger.MoveCoord(coord);
		base.Initialize();
	}

	public override void Initialize()
	{
		TileMap tileMap = SingletonMonoBehaviour<World>.Instance.Sections.Find((TileMap s) => s != null && !s.IsLoadingSection);
		if (tileMap != null && tileMap.StartPosition != null)
		{
			SingletonMonoBehaviour<Game>.Instance.Digger.transform.position = tileMap.StartPosition.position;
			SingletonMonoBehaviour<Game>.Instance.Digger.MoveCoord(tileMap.GetCoordFromLocalPosition(tileMap.StartPosition.localPosition));
		}
		else if (tileMap == null)
		{
			UnityEngine.Debug.LogError("Digger.Initialize: was called without sections");
		}
		else
		{
			UnityEngine.Debug.LogError("Digger.Initialize: first section doesn't have a start position");
		}
		base.Initialize();
		if (BuiltInItems == null)
		{
			return;
		}
		for (int i = 0; i < BuiltInItems.Length; i++)
		{
			ItemDef item = SingletonMonoBehaviour<Game>.Instance.GetItem(BuiltInItems[i]);
			if ((bool)item)
			{
				item.Activate(this);
			}
		}
	}

	private void Update()
	{
		if (!IsDead)
		{
			if (SingletonMonoBehaviour<GameInput>.Instance.Left)
			{
				runStarted = true;
				MoveLeft();
			}
			if (SingletonMonoBehaviour<GameInput>.Instance.Right)
			{
				runStarted = true;
				MoveRight();
			}
			if (SingletonMonoBehaviour<GameInput>.Instance.Down)
			{
				runStarted = true;
				MoveDown();
			}
			if (SingletonMonoBehaviour<GameInput>.Instance.Up)
			{
				runStarted = true;
				MoveUp();
			}
			if (runStarted)
			{
				secondsSinceRunStart += Time.deltaTime;
			}
		}
	}

	private void FixedUpdate()
	{
		UpdateParticleEmission(HitSidesLoopParticles, !IsDead && HitSidesWhenLanding);
		UpdateParticleEmission(HardBootsLoopParticles, !IsDead && HardBoots);
		UpdateParticleEmission(ArmorParticles, !IsDead && Armor, clear: true);
		UpdateParticleEmission(FireHammerParticles, !IsDead && HitDistance > 0 && (!Walker.AirControl || Walker.IsGrounded));
		ArmorLoopAudio.TargetVolume = ((!IsDead && Armor) ? 1 : 0);
		if (IsDead)
		{
			return;
		}
		if (!Walker.Moving && !hitting && queued != null)
		{
			if (Time.time - queueTime < 0.3f)
			{
				queued();
			}
			queued = null;
		}
		if (Coord.x != lastMoveCoord.x || Time.time - lastMoveTime > 0.5f || TryAirAttack(Walker.Direction))
		{
			Walker.Stop();
		}
		if (DragParticles != null)
		{
			var emission = DragParticles.emission;
			emission.enabled = (Mathf.Abs(Walker.Velocity.x) > 2f);
		}
		SideMoveAudio.volume = ((!Walker.IsGrounded) ? 0f : Mathf.Clamp01(Mathf.Abs(Walker.Velocity.x)));
		Animator.SetFloat("parachute", Walker.AirControl ? 1 : 0);
		Animator.SetBool("hammerfall", HitSidesWhenLanding);
	}

	private void UpdateParticleEmission(ParticleSystem[] particleSystems, bool emit, bool clear = false)
	{
		foreach (ParticleSystem particleSystem in particleSystems)
		{
			var emission = particleSystem.emission;
			bool isEmissionEnabled = emission.enabled;

			if ((emit && isEmissionEnabled) || (!emit && !isEmissionEnabled))
			{
				// Code block where the emission is handled
			}
			{
				var _temp_val_278 = particleSystem.emission; _temp_val_278.enabled = emit;
				if (!emit && clear)
				{
					particleSystem.Clear();
				}
			}
		}
	}

	private void MoveLeft()
	{
		if (!IsDead)
		{
			MoveSide(Vector3.left);
		}
	}

	public void MoveRight()
	{
		if (!IsDead)
		{
			MoveSide(Vector3.right);
		}
	}

	private void MoveSide(Vector3 direction)
	{
		lookDirection = direction;
		if (TryAirAttack(direction))
		{
			return;
		}
		for (int i = 0; i <= HitDistance; i++)
		{
			WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(Coord + direction * (i + 1));
			if (tile != null)
			{
				if (tile is Chicken && i == 0)
				{
					tile.RemoveCoord();
					break;
				}
				if (tile.BreakableBy(this))
				{
					StartCoroutine(HitCoroutine(direction, tile));
					WorldObject tile2 = SingletonMonoBehaviour<World>.Instance.GetTile((Coord + direction + Vector3.down).Normalize());
					WorldObject tile3 = SingletonMonoBehaviour<World>.Instance.GetTile((Coord + direction + Vector3.up).Normalize());
					if (!tile.Broken || tile2 == null || !tile2.Hermetic || (tile3 != null && tile3.Walker != null))
					{
						return;
					}
					break;
				}
				break;
			}
		}
		Walker.Move(direction);
		lastMoveCoord = Coord;
		lastMoveTime = Time.time;
	}

	private bool TryAirAttack(Vector3 direction)
	{
		if (!Walker.IsGrounded && !Walker.AirControl && (direction == Vector3.left || direction == Vector3.right))
		{
			WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(Coord + direction);
			if (tile != null && tile.BreakableBy(this))
			{
				StartCoroutine(HitCoroutine(direction, tile));
				return true;
			}
		}
		return false;
	}

	public override bool CanMoveTo(Coord newCoord, WorldObject tile)
	{
		if (tile is Chicken)
		{
			return true;
		}
		return base.CanMoveTo(newCoord, tile);
	}

	public override bool OnMoveTo(Vector3 direction, WorldObject wo)
	{
		if (wo is Chicken)
		{
			return true;
		}
		if (wo.BreakableBy(this))
		{
			StartCoroutine(HitCoroutine(direction, wo));
		}
		else if (!HitUnbreakableAudio.isPlaying || HitUnbreakableAudio.time > 0.3f)
		{
			SingletonMonoBehaviour<Game>.Instance.MakeNoise(new Noise(wo.transform.position));
			if (wo is FinalDragonHitBox)
			{
				((FinalDragonHitBox)wo).Dragon.OnHammerHit();
			}
			else
			{
				HitUnbreakableAudio.Play();
			}
			Animator.SetTrigger((!(direction == Vector3.left)) ? "wallHitRight" : "wallHitLeft");
			if ((bool)UnbreakHitLeft && direction == Vector3.left)
			{
				UnbreakHitLeft.Play();
			}
			else if ((bool)UnbreakHitRight && direction == Vector3.right)
			{
				UnbreakHitRight.Play();
			}
		}
		return false;
	}

	private IEnumerator HitCoroutine(Vector3 direction, WorldObject wo)
	{
		if ((bool)SwingAudio)
		{
			SwingAudio.Play();
		}
		Animator.SetTrigger((!(direction == Vector3.left)) ? "hitRight" : "hitLeft");
		hittingWo = wo;
		hitting = true;
		if (HitDelay > 0f)
		{
			yield return new WaitForSeconds(HitDelay);
		}
		DistanceHit(direction);
		hittingWo = null;
		hitting = false;
	}

	private bool CheckValidHit(Coord coord, WorldObject wo)
	{
		if (wo != null && !wo.gameObject.activeInHierarchy)
		{
			UnityEngine.Debug.LogError("Hitting inactive element " + wo, wo);
			wo.RemoveCoord();
			return false;
		}
		if (wo != null && wo != SingletonMonoBehaviour<World>.Instance.GetTile(wo.Coord))
		{
			UnityEngine.Debug.LogError("Hitting element in inconsistent coord " + wo, wo);
			TileMap section = SingletonMonoBehaviour<World>.Instance.GetSection(coord.Section);
			if (section != null)
			{
				section.Remove(coord);
			}
			return false;
		}
		if (wo != null && wo.Broken)
		{
			UnityEngine.Debug.LogError("Hitting broken element " + wo, wo);
			wo.RemoveCoord();
			return false;
		}
		return true;
	}

	private bool DistanceHit(Vector3 direction)
	{
		if (IsDead)
		{
			return false;
		}
		bool result = false;
		if (hittingWo is Fireball && hittingWo.Broken)
		{
			hittingWo.OnHit(direction, this);
			result = true;
		}
		if (hittingWo is DragonFireball)
		{
			hittingWo.OnHit(direction, this);
			return true;
		}
		if (SingletonMonoBehaviour<Game>.Instance.Chicken != null && !SingletonMonoBehaviour<Game>.Instance.Chicken.Broken && direction == Vector3.up && SingletonMonoBehaviour<Game>.Instance.Chicken.Coord == (Coord + Coord.Up).Normalize())
		{
			SingletonMonoBehaviour<Game>.Instance.Chicken.OnHit(direction, this);
		}
		for (int i = 0; i <= HitDistance; i++)
		{
			Coord coord = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(Coord + direction * (i + 1));
			WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(coord);
			Vector3 positionFromCoord = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(coord);
			if (HitDistance > 0 && HitDistanceParticles != null && i == 0 && (tile == null || tile.BreakableBy(this)))
			{
				HitDistanceParticles.transform.position = positionFromCoord;
				HitDistanceParticles.transform.localRotation = Quaternion.FromToRotation(Vector3.right, direction);
				HitDistanceParticles.Play();
				if (FireHammerAudio != null)
				{
					FireHammerAudio.Play();
				}
			}
			if (!CheckValidHit(coord, tile) || !(tile != null) || !(tile != this))
			{
				continue;
			}
			SingletonMonoBehaviour<Game>.Instance.MakeNoise(new Noise(tile.transform.position));
			result = true;
			tile.OnHit(direction, this);
			if (tile.Broken && HitDistance > 0 && HitDistanceParticles != null)
			{
				tile.Breakable.LightFire();
			}
			if (tile.Broken && tile is Enemy)
			{
				if (direction == Vector3.up)
				{
					SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.KillFromBelow);
				}
				if (!Walker.IsGrounded)
				{
					SingletonMonoBehaviour<MissionManager>.Instance.UpdateProgress(MissionType.KillWhileFalling);
				}
			}
			if (!tile.Broken || tile is PushCube)
			{
				break;
			}
			Coord coord2 = (coord + direction).Normalize();
			WorldObject tile2 = SingletonMonoBehaviour<World>.Instance.GetTile(coord2);
			if (tile.Hermetic && direction == Vector3.down && !(tile2 is Enemy) && !(tile2 is Spike))
			{
				break;
			}
		}
		return result;
	}

	public override void OnHit(Vector3 direction, WorldObject hitter, WorldObject medium = null)
	{
		if ((double)(Time.fixedTime - SavedTime) < GameVars.SaveInvulnerabilityTime.TotalSeconds || Invulnerability || (direction == Vector3.up && HardBoots) || (direction == lookDirection && BackShield != null && BackShield.activeSelf))
		{
			return;
		}
		if (Armor)
		{
			Armor = false;
			SaveGame.Instance.SetItemActive("Armor", value: false);
			if ((bool)ArmorBreakParticles)
			{
				ArmorBreakParticles.Play();
			}
			if ((bool)ArmorBreakAudio)
			{
				ArmorBreakAudio.Play();
			}
			hitter.OnHit(-direction, this);
		}
		else if (!IsDead && hitter != hittingWo)
		{
			StartCoroutine(DieRoutine(direction));
		}
	}

	private IEnumerator DieRoutine(Vector3 direction)
	{
		if (IsDead)
		{
			yield break;
		}
		Breakable.Break(direction);
		IsDead = true;
		WaitingSave = true;
		queued = null;
		GameAnalytics.NewDesignEvent("gameplay:gameover:level" + SaveGame.Instance.WorldLevel + "duration", SecondsSinceRunStart);
		GameAnalytics.NewDesignEvent("gameplay:gameover:level" + SaveGame.Instance.WorldLevel + "coins", Coins);
		GameAnalytics.NewDesignEvent("gameplay:gameover:duration", SecondsSinceRunStart);
		if (DragParticles != null)
		{
			var emission = DragParticles.emission;
			emission.enabled = false;
		}
		SavedCoord = Coord;
		SideMoveAudio.volume = 0f;
		StartCoroutine(SingletonMonoBehaviour<Game>.Instance.FadeTime(0.1f, 0.8f, 0.01f));
		SingletonMonoBehaviour<Game>.Instance.GameCamera.ScreenFlash.Play();
		SingletonMonoBehaviour<Game>.Instance.HideInGameButtons();
		int resumeLevel = SaveGame.Instance.WorldLevel;
		SaveGame.Instance.WorldLevel = 1;
		yield return new WaitForSecondsRealtime(1.6f);
		ActionType actionType = ActionType.Wait;
		Character character = Characters.ByName(SaveGame.Instance.CurrentCharacter);
		if (!SaveGame.Instance.CharacterOwned(character.Name) && Purchaser.Instance.CanBuyProduct(character.ProductId))
		{
			yield return Gui.Views.CharacterDeal.ShowCoroutine(character.Name, delegate
			{
				actionType = ActionType.Success;
			}, delegate
			{
				actionType = ActionType.Failure;
			});
		}
		else
		{
			actionType = ActionType.Failure;
		}
		while (actionType == ActionType.Wait)
		{
			yield return null;
		}
		if (actionType == ActionType.Failure)
		{
			actionType = ActionType.Wait;
			if (Gui.Views.SaveMeView.CanSave)
			{
				yield return Gui.Views.SaveMeView.ShowCoroutine(delegate
				{
					actionType = ActionType.Success;
				}, delegate
				{
					actionType = ActionType.Failure;
				});
			}
			else
			{
				actionType = ActionType.Failure;
			}
			while (actionType == ActionType.Wait)
			{
				yield return null;
			}
		}
		if (actionType == ActionType.Success)
		{
			SaveGame.Instance.WorldLevel = resumeLevel;
			SingletonMonoBehaviour<Game>.Instance.Digger.SaveMe();
			SingletonMonoBehaviour<Game>.Instance.ShowInGameButtons();
		}
		else
		{
			SingletonMonoBehaviour<Game>.Instance.FixCurrentCharacter();
			SingletonMonoBehaviour<Game>.Instance.ResetFadeTime();
			SingletonMonoBehaviour<Game>.Instance.AudioMixer.SetFloat("worldVolume", -80f);
			if (Gui.Views.LoseMissionsView.ShouldShow())
			{
				yield return Gui.Views.LoseMissionsView.ShowAnimatedCoroutine();
			}
			if (Coins > 0 && SaveGame.Instance.TutorialComplete)
			{
				Gui.Views.LoseView.ShowAnimated();
			}
			else
			{
				SingletonMonoBehaviour<Game>.Instance.Restart();
			}
		}
		WaitingSave = false;
	}

	public void SaveProgress()
	{
		SaveGame.Instance.DiggerCoins = Coins;
		SaveGame.Instance.DiggerBombs = Bombs;
		for (int i = 0; i < SingletonMonoBehaviour<Game>.Instance.Items.Count; i++)
		{
			SaveGame.Instance.SetItemActive(SingletonMonoBehaviour<Game>.Instance.Items[i].name, SingletonMonoBehaviour<Game>.Instance.Items[i].IsActive(this));
		}
		MissionSet currentMissionSet = SingletonMonoBehaviour<MissionManager>.Instance.CurrentMissionSet;
		if (currentMissionSet != null)
		{
			SaveGame.Instance.Mission1Progress = currentMissionSet.Missions[0].Progress;
			SaveGame.Instance.Mission2Progress = currentMissionSet.Missions[1].Progress;
			SaveGame.Instance.Mission3Progress = currentMissionSet.Missions[2].Progress;
		}
	}

	public void SaveMe()
	{
		SavedTime = Time.fixedTime;
		SaveGame.Instance.SaveMeCount++;
		Breakable.Unbreak();
		IsDead = false;
		base.transform.position = SingletonMonoBehaviour<World>.Instance.GetPositionFromCoord(SavedCoord);
		if ((bool)SaveMeAudio)
		{
			SaveMeAudio.Play();
		}
		for (int num = SavedCoord.y + 1; num > SavedCoord.y - 20; num--)
		{
			Coord coord = new Coord(SavedCoord.Section, 0, num);
			coord = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(coord);
			if (!(coord == Coord.None))
			{
				TileMap section = SingletonMonoBehaviour<World>.Instance.GetSection(coord.Section);
				if (!(section == null))
				{
					while (coord.x < section.Columns)
					{
						WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(coord);
						if ((bool)tile)
						{
							tile.Disarm();
						}
						coord.x++;
					}
				}
			}
		}
		if (!FloorCube(SavedCoord + Vector3.down) && !FloorCube(SavedCoord + Vector3.down * 2f))
		{
			Coord coord2 = SingletonMonoBehaviour<World>.Instance.NormalizeCoord(SavedCoord + Vector3.down);
			WorldObject tile2 = SingletonMonoBehaviour<World>.Instance.GetTile(coord2);
			if (tile2 != null)
			{
				if ((bool)tile2.Breakable)
				{
					tile2.Breakable.Break(Vector3.zero);
				}
				else
				{
					tile2.RemoveCoord();
					tile2.gameObject.SetActive(value: false);
				}
			}
			TileMap section2 = SingletonMonoBehaviour<World>.Instance.GetSection(coord2.Section);
			if (!section2.IsLoadingSection)
			{
				section2.InstantiateFromPrefab("Cube", coord2);
			}
		}
		MoveCoord(SavedCoord);
		StartCoroutine(SingletonMonoBehaviour<Game>.Instance.FadeTime(1f, 1f, 0.1f));
	}

	private bool FloorCube(Coord coord)
	{
		coord = coord.Normalize();
		WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(coord);
		return tile != null && Array.IndexOf(SingletonMonoBehaviour<World>.Instance.FloorIds, tile.PrefabId) >= 0;
	}

	private void MoveDown()
	{
		if (!IsDead)
		{
			if (hitting)
			{
				queued = MoveDown;
				queueTime = Time.time;
			}
			else
			{
				StartCoroutine(HitUpDownCoroutine(Vector3.down, DownHitDelay));
			}
		}
	}

	private void MoveUp()
	{
		if (!IsDead)
		{
			if (hitting)
			{
				queued = MoveUp;
				queueTime = Time.time;
			}
			else
			{
				StartCoroutine(HitUpDownCoroutine(Vector3.up, HitDelay));
			}
		}
	}

	private IEnumerator HitUpDownCoroutine(Vector3 direction, float delay)
	{
		lookDirection = Vector3.forward;
		hittingWo = SingletonMonoBehaviour<World>.Instance.GetTile(Coord + direction);
		hitting = true;
		string dirName = (!(direction == Vector3.up)) ? "Down" : "Up";
		bool unbreakable = hittingWo != null && !hittingWo.BreakableBy(this);
		if (!unbreakable && direction != Vector3.down)
		{
			hittingWo = null;
		}
		Animator.SetTrigger(((!unbreakable) ? "hit" : "wallHit") + dirName);
		if (!unbreakable && (bool)SwingAudio)
		{
			SwingAudio.Play();
		}
		if (Walker.IsGrounded || direction != Vector3.down)
		{
			float startTime = Time.fixedTime;
			WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
			while (Time.fixedTime - startTime < delay)
			{
				hittingWo = SingletonMonoBehaviour<World>.Instance.GetTile(Coord + direction);
				yield return waitForFixedUpdate;
			}
		}
		if (unbreakable && (!HitUnbreakableAudio.isPlaying || HitUnbreakableAudio.time > 0.3f))
		{
			HitUnbreakableAudio.Play();
			if ((bool)UnbreakHitLeft && direction == Vector3.up)
			{
				UnbreakHitUp.Play();
			}
			else if ((bool)UnbreakHitRight && direction == Vector3.down)
			{
				UnbreakHitDown.Play();
			}
		}
		bool hitSomething = DistanceHit(direction);
		hittingWo = null;
		if (direction == Vector3.down && !hitSomething)
		{
			yield return new WaitForSeconds(0.3f);
		}
		hitting = false;
	}

	public override void OnGrounded()
	{
		DustParticles.Play();
		MoveAudio.Play();
		if (HitSidesWhenLanding)
		{
			if ((bool)HammerFallAudio)
			{
				HammerFallAudio.Play();
			}
			HitSidesParticles.Play();
			WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(Coord + Coord.Left);
			if (tile != null && !tile.Hermetic)
			{
				tile.OnHit(Vector3.left, this);
			}
			WorldObject tile2 = SingletonMonoBehaviour<World>.Instance.GetTile(Coord + Coord.Right);
			if (tile2 != null && !tile2.Hermetic)
			{
				tile2.OnHit(Vector3.right, this);
			}
		}
	}

	public override void IncrementCoins(WorldObject source, int coins)
	{
		if (source is Enemy && SingletonMonoBehaviour<Game>.Instance.Combo > 1)
		{
			coins *= SingletonMonoBehaviour<Game>.Instance.Combo;
		}
		Coins += coins;
	}
}
