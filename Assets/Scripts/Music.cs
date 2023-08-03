using UnityEngine;

public class Music : MonoBehaviour
{
	public AudioSource[] StoreMusic;

	public AudioSource StoreMusicOut;

	public AudioSource AmbientNight;

	public AudioSource AmbientDay;

	public float MinClipTime = 5f;

	public float MaxClipTime = 20f;

	public AudioSource[] AmbientClips;

	private float nextClipTime;

	private void Start()
	{
		UpdateClipTime();
	}

	private void UpdateClipTime()
	{
		nextClipTime = Time.time + UnityEngine.Random.Range(MinClipTime, MaxClipTime);
	}

	private void Update()
	{
		int num = ShouldPlayStoreMusic();
		if (SingletonMonoBehaviour<Gui>.Instance.Ready && Gui.Views.WheelView.Visible)
		{
			num = -1;
		}
		if (num >= 0)
		{
			if (num >= StoreMusic.Length)
			{
				num = StoreMusic.Length - 1;
			}
			AudioSource audioSource = StoreMusic[num];
			if (!audioSource.isPlaying)
			{
				audioSource.volume = 0f;
				audioSource.Play();
				StartCoroutine(SingletonMonoBehaviour<Game>.Instance.FadeAudio(audioSource, 1f, 1f));
			}
		}
		for (int i = 0; i < StoreMusic.Length; i++)
		{
			if (num == i)
			{
				continue;
			}
			AudioSource audioSource2 = StoreMusic[i];
			if (!audioSource2.isPlaying)
			{
				continue;
			}
			if (audioSource2.volume == 0f)
			{
				audioSource2.Stop();
			}
			else if (audioSource2.volume > 0f)
			{
				if (audioSource2.volume == 1f && !Gui.Views.WheelView.Visible)
				{
					StoreMusicOut.Play();
				}
				audioSource2.volume = Mathf.Clamp01(audioSource2.volume - Time.deltaTime / 0.2f);
			}
		}
		bool flag = SingletonMonoBehaviour<Game>.Instance.Ready && num < 0 && SingletonMonoBehaviour<Game>.Instance.Digger != null && SingletonMonoBehaviour<Game>.Instance.Digger.gameObject.activeSelf && !SingletonMonoBehaviour<Game>.Instance.FullscreenViewOrTransition();
		if (AmbientDay != AmbientNight)
		{
			UpdateAmbient(AmbientNight, flag && !SingletonMonoBehaviour<Game>.Instance.CurrentCustomizer.IsDay);
			UpdateAmbient(AmbientDay, flag && SingletonMonoBehaviour<Game>.Instance.CurrentCustomizer.IsDay);
		}
		else
		{
			UpdateAmbient(AmbientDay, flag);
		}
		if (Time.time > nextClipTime)
		{
			if (flag && AmbientClips != null && AmbientClips.Length > 0)
			{
				Random.InitState((int)(Time.realtimeSinceStartup * 1000f));
				int num2 = UnityEngine.Random.Range(0, AmbientClips.Length);
				AmbientClips[num2].Play();
			}
			UpdateClipTime();
		}
	}

	private int ShouldPlayStoreMusic()
	{
		if (!SingletonMonoBehaviour<Game>.Instance.Ready)
		{
			return -1;
		}
		if (SingletonMonoBehaviour<World>.Instance.IsLoading)
		{
			return SingletonMonoBehaviour<World>.Instance.LoadingLevel - 2;
		}
		Digger digger = SingletonMonoBehaviour<Game>.Instance.Digger;
		if (digger == null)
		{
			return -1;
		}
		TileMap section = SingletonMonoBehaviour<World>.Instance.GetSection(digger.Coord.Section);
		if (section == null)
		{
			return -1;
		}
		if (section.IsLoadingSection || (section.IsTransition && digger.Coord.y < section.Rows - 2))
		{
			return SaveGame.Instance.WorldLevel - 2;
		}
		return -1;
	}

	private void UpdateAmbient(AudioSource ambient, bool play)
	{
		if (ambient.isPlaying && !play)
		{
			if (ambient.volume == 0f)
			{
				ambient.Stop();
			}
			else if (ambient.volume > 0f)
			{
				ambient.volume = Mathf.Clamp01(ambient.volume - Time.deltaTime / 0.2f);
			}
		}
		else if (!ambient.isPlaying && play)
		{
			ambient.volume = 0f;
			ambient.Play();
			StartCoroutine(SingletonMonoBehaviour<Game>.Instance.FadeAudio(ambient, 1f, 1f));
		}
	}
}
