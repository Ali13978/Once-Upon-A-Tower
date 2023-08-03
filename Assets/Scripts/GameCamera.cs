using UnityEngine;

public class GameCamera : MonoBehaviour
{
	public Camera Camera;

	public float SmoothTime = 0.5f;

	public float MaxSpeed = 10f;

	public float StartOffset;

	public float Offset = -12f;

	public float MaxOffsetPosition = 20f;

	public Collider TargetFrame;

	public ParticleSystem ScreenFlash;

	public float DefaultSize;

	public float MinWidthSize = 4.8f;

	public Animation ShakeAnimation;

	public Transform ShakeReference;

	private float HeightOffset;

	private Vector3 velocity;

	private float sizeVelocity;

	private float shakeMultiplier;

	private Vector3 unshakedPosition;

	public bool Stable => velocity.magnitude < 1f;

	private float ActualSize
	{
		get
		{
			if (DefaultSize * Camera.aspect > MinWidthSize)
			{
				return DefaultSize;
			}
			return MinWidthSize / Camera.aspect;
		}
	}

	private void Start()
	{
		if (Camera == null)
		{
			Camera = GetComponentInChildren<Camera>();
		}
		unshakedPosition = base.transform.position;
	}

	private void Update()
	{
		Camera.enabled = !SingletonMonoBehaviour<Game>.Instance.FullscreenView();
		if (!(SingletonMonoBehaviour<Game>.Instance.Digger == null))
		{
			Vector3 position;
			float orthographicSize;
			FindTarget(SingletonMonoBehaviour<Game>.Instance.Digger.transform, out position, out orthographicSize);
			unshakedPosition = Vector3.SmoothDamp(unshakedPosition, position, ref velocity, SmoothTime, MaxSpeed);
			base.transform.position = unshakedPosition + ShakeReference.transform.localPosition * shakeMultiplier;
			Camera.orthographicSize = Mathf.SmoothDamp(Camera.orthographicSize, orthographicSize, ref sizeVelocity, SmoothTime, MaxSpeed);
		}
	}

	public void Shake(float multiplier)
	{
		shakeMultiplier = multiplier;
		ShakeAnimation.Play();
	}

	public void Focus()
	{
		if (!(SingletonMonoBehaviour<Game>.Instance.Digger == null) || !(TargetFrame == null))
		{
			shakeMultiplier = 0f;
			Transform target = null;
			if (SingletonMonoBehaviour<Game>.Instance.Digger != null)
			{
				target = SingletonMonoBehaviour<Game>.Instance.Digger.transform;
			}
			Vector3 position;
			float orthographicSize;
			FindTarget(target, out position, out orthographicSize);
			Vector3 position2 = position;
			base.transform.position = position2;
			unshakedPosition = position2;
			Camera.orthographicSize = orthographicSize;
		}
	}

	private void FindTarget(Transform target, out Vector3 position, out float orthographicSize)
	{
		position = unshakedPosition;
		orthographicSize = ActualSize;
		if (TargetFrame == null && target != null)
		{
			float startOffset = StartOffset;
			float offset = Offset;
			Vector3 position2 = target.position;
			float num = Mathf.Lerp(startOffset, offset, Mathf.Clamp01((0f - position2.y) / MaxOffsetPosition));
			position.x = 0f;
			Vector3 position3 = target.position;
			position.y = position3.y + num;
		}
		else if (TargetFrame != null)
		{
			Vector3 center = TargetFrame.bounds.center;
			position.x = center.x;
			Vector3 center2 = TargetFrame.bounds.center;
			position.y = center2.y;
			Vector3 extents = TargetFrame.bounds.extents;
			float a = extents.x / Camera.aspect;
			Vector3 extents2 = TargetFrame.bounds.extents;
			orthographicSize = Mathf.Max(a, extents2.y);
		}
	}
}
