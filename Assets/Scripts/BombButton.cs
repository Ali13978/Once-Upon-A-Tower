using TMPro;
using UnityEngine;

public class BombButton : GuiButton
{
	public TextMeshPro CountText;

	public GameObject BombPrefab;

	private int lastBombs = -1;

	private Bomb bomb;

	protected override void Start()
	{
		base.Start();
		bomb = Object.Instantiate(BombPrefab).GetComponent<Bomb>();
		Click = OnClick;
	}

	protected override void Update()
	{
		base.Update();
		if (lastBombs != SingletonMonoBehaviour<Game>.Instance.Digger.Bombs)
		{
			lastBombs = SingletonMonoBehaviour<Game>.Instance.Digger.Bombs;
			CountText.text = SingletonMonoBehaviour<Game>.Instance.Digger.Bombs.ToString();
		}
	}

	public void OnClick()
	{
		if (SingletonMonoBehaviour<Game>.Instance.Digger.Bombs > 0)
		{
			bomb.Coord = SingletonMonoBehaviour<Game>.Instance.Digger.Coord;
			SingletonMonoBehaviour<Game>.Instance.Digger.Bombs--;
			if ((bool)SingletonMonoBehaviour<Game>.Instance.Digger.BombAudio)
			{
				SingletonMonoBehaviour<Game>.Instance.Digger.BombAudio.Play();
			}
			bomb.Activate(SingletonMonoBehaviour<Game>.Instance.Digger);
		}
	}
}
