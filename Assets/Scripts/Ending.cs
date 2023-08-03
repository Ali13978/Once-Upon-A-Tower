using Flux;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
	public FSequence EndingSequence;

	public List<Transform> TowerSinks;

	public AnimationCurve SinkCurve;

	public GameObject CharacterOutro;

	private void Start()
	{
		CharacterOutro.SetActive(value: false);
	}

	public void DoEnding()
	{
        LeaderManager.instance.UploadScoreOnLeaderBoard();
		StartCoroutine(EndingCoroutine());
	}

	private IEnumerator EndingCoroutine()
	{
		SwapCharacter();
		StartCoroutine(EscapeChickenCoroutine());
		Gui.Views.InGameStatsView.HideAnimated();
		Gui.Views.PauseButtonView.HideAnimated();
		Gui.Views.ItemsView.HideAnimated();
		SingletonMonoBehaviour<Game>.Instance.Digger.Armor = false;
		yield return null;
		SingletonMonoBehaviour<GameInput>.Instance.Enabled = false;
		SingletonMonoBehaviour<Game>.Instance.Digger.Invulnerability = true;
		SingletonMonoBehaviour<Game>.Instance.Digger.gameObject.SetActive(value: false);
		CharacterOutro.SetActive(value: true);
		EndingSequence.Play();
		while (EndingSequence.IsPlaying)
		{
			yield return null;
		}
		SaveGame.Instance.WorldLevel = 1;
		SaveGame.Instance.Save();
		Gui.Views.CharacterFree.ShowAnimated();
		bool done = false;
		Gui.Views.CharacterFree.PlayButton.Click = delegate
		{
			done = true;
		};
		while (!done)
		{
			yield return null;
		}
		SaveGame.Instance.CurrentCharacter = Characters.ChooseNewCharacter();
		SingletonMonoBehaviour<GameInput>.Instance.Enabled = true;
		Gui.Views.CharacterFree.HideAnimated();
		Gui.Views.LoseView.ShowAnimated();
	}

	private IEnumerator EscapeChickenCoroutine()
	{
		if (SingletonMonoBehaviour<Game>.Instance.Chicken == null || SingletonMonoBehaviour<Game>.Instance.Chicken.Broken)
		{
			yield break;
		}
		Chicken chicken = SingletonMonoBehaviour<Game>.Instance.Chicken;
		chicken.enabled = false;
		chicken.Walker.enabled = false;
		chicken.Animator.SetFloat("velocity", 1f);
		chicken.Animator.SetBool("moving", value: true);
		chicken.Animator.SetTrigger("move");
		while (true)
		{
			Vector3 position = chicken.Animator.transform.position;
			if (!(position.x < 100f))
			{
				break;
			}
			chicken.Animator.transform.position += Vector3.right * chicken.Walker.MoveSpeed * Time.deltaTime;
			yield return null;
		}
		chicken.gameObject.SetActive(value: false);
	}

	public void SinkTower()
	{
		if (Application.isPlaying)
		{
			StartCoroutine(SinkTowerCoroutine());
		}
	}

	private IEnumerator SinkTowerCoroutine()
	{
		List<Transform> transforms = new List<Transform>();
		foreach (Transform towerSink in TowerSinks)
		{
			if (towerSink != null)
			{
				transforms.Add(towerSink);
			}
		}
		TileMap endSection = GetComponentInParent<TileMap>();
		foreach (TileMap section in SingletonMonoBehaviour<World>.Instance.Sections)
		{
			if (section != null)
			{
				section.DontDestroy = true;
				if (section != endSection)
				{
					transforms.Add(section.transform);
				}
			}
		}
		float startTime = Time.time;
		float duration = SinkCurve.keys[SinkCurve.keys.Length - 1].time;
		float previous = SinkCurve.Evaluate(0f);
		while (Time.time - startTime < duration)
		{
			yield return null;
			float now = SinkCurve.Evaluate(Time.time - startTime);
			float diff = now - previous;
			for (int i = 0; i < transforms.Count; i++)
			{
				transforms[i].position += Vector3.down * diff;
			}
			previous = now;
		}
		for (int j = 0; j < transforms.Count; j++)
		{
			for (int k = 0; k < transforms[j].childCount; k++)
			{
				transforms[j].GetChild(k).gameObject.SetActive(value: false);
			}
		}
	}

	private void SwapCharacter()
	{
		while (CharacterOutro.transform.childCount > 0)
		{
			UnityEngine.Object.DestroyImmediate(CharacterOutro.transform.GetChild(0).gameObject);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(SingletonMonoBehaviour<Game>.Instance.Digger.Animator.gameObject);
		while (gameObject.transform.childCount > 0)
		{
			Transform child = gameObject.transform.GetChild(0);
			child.SetParent(CharacterOutro.transform, worldPositionStays: false);
			SkinnedMeshRenderer[] componentsInChildren = child.GetComponentsInChildren<SkinnedMeshRenderer>();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren)
			{
				skinnedMeshRenderer.updateWhenOffscreen = true;
			}
		}
		UnityEngine.Object.Destroy(gameObject);
	}
}
