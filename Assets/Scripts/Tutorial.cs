using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other == SingletonMonoBehaviour<Game>.Instance.Digger.Collider)
		{
			StartCoroutine(TutorialCoroutine());
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other == SingletonMonoBehaviour<Game>.Instance.Digger.Collider)
		{
			SaveGame.Instance.TutorialComplete = true;
			SingletonMonoBehaviour<Game>.Instance.ShowInGameButtons();
		}
	}

	private IEnumerator TutorialCoroutine()
	{
		while (!SingletonMonoBehaviour<Game>.Instance.Ready || !SingletonMonoBehaviour<Game>.Instance.Digger.gameObject.activeSelf)
		{
			yield return null;
		}
		Gui.Views.TutorialView.ShowAnimated();
		Digger digger = SingletonMonoBehaviour<Game>.Instance.Digger;
		while (!SaveGame.Instance.TutorialComplete)
		{
			if (digger.Walker.IsGrounded)
			{
				WorldObject down = SingletonMonoBehaviour<World>.Instance.GetTile(digger.Coord + Coord.Down);
				WorldObject up = SingletonMonoBehaviour<World>.Instance.GetTile(digger.Coord + Coord.Up);
				if (up is Coin)
				{
					yield return TutorialSequence(0f, "SwipeUp");
				}
				else if (down != null && down.BreakableBy(digger))
				{
					yield return TutorialSequence(1f, "SwipeDown");
				}
				else if (PathDown(digger, Vector3.right))
				{
					yield return TutorialSequence(1f, "SwipeRight");
				}
				else if (PathDown(digger, Vector3.left))
				{
					yield return TutorialSequence(1f, "SwipeLeft");
				}
			}
			else
			{
				Gui.Views.TutorialView.StopCurrentSequence();
			}
			yield return null;
		}
		GameTime.NormalSpeed(SlowMoPriority.Game);
		Gui.Views.TutorialView.HideAnimated();
	}

	private IEnumerator TutorialSequence(float wait, string name)
	{
		if (!Gui.Views.TutorialView.IsPlayingSequence(name))
		{
			Gui.Views.TutorialView.StopCurrentSequence();
		}
		Coord startCoord = SingletonMonoBehaviour<Game>.Instance.Digger.Coord;
		float startTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup - startTime < wait && SingletonMonoBehaviour<Game>.Instance.Digger.Coord == startCoord)
		{
			yield return null;
		}
		if (SingletonMonoBehaviour<Game>.Instance.Digger.Coord == startCoord && !Gui.Views.TutorialView.IsPlayingSequence(name))
		{
			Gui.Views.TutorialView.Play(name);
		}
	}

	private bool PathDown(WorldObject wo, Vector3 direction)
	{
		Coord coord = wo.Coord + direction;
		while (SingletonMonoBehaviour<World>.Instance.IsCoordValid(coord))
		{
			WorldObject tile = SingletonMonoBehaviour<World>.Instance.GetTile(coord);
			WorldObject tile2 = SingletonMonoBehaviour<World>.Instance.GetTile((coord + Coord.Down).Normalize());
			if (tile != null && !tile.BreakableBy(wo))
			{
				break;
			}
			if (tile2 == null || tile2.BreakableBy(wo))
			{
				return true;
			}
			coord += direction;
		}
		return false;
	}
}
