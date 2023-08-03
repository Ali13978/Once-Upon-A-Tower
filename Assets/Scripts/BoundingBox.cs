using UnityEngine;

public class BoundingBox : MonoBehaviour
{
	private int width;

	private int height;

	private Bounds appliedBounds;

	public bool DisableAlignment;

	public TextAlignment Alignment = TextAlignment.Center;

	public VerticalAlignment VerticalAlignment;

	public Bounds Bounds
	{
		get
		{
			Vector3 localScale = base.transform.localScale;
			localScale.z = 0f;
			return new Bounds(base.transform.position, localScale);
		}
	}

	private void Start()
	{
		Apply();
	}

	private void Update()
	{
		if (width != Screen.width || height != Screen.height)
		{
			Apply();
		}
	}

	public void Apply()
	{
		ApplyWithBounds(Bounds);
	}

	public void ApplyWithBounds(Bounds bounds)
	{
		if (width != Screen.width || height != Screen.height || !(appliedBounds == bounds))
		{
			appliedBounds = bounds;
			width = Screen.width;
			height = Screen.height;
			if (!DisableAlignment)
			{
				SingletonMonoBehaviour<Gui>.Instance.RunOnReady(delegate
				{
					Camera guiCamera = SingletonMonoBehaviour<Gui>.Instance.GuiCamera;
					float num = guiCamera.aspect;
					Bounds bounds2 = new Bounds(guiCamera.transform.position, new Vector3(guiCamera.orthographicSize * num * 2f, guiCamera.orthographicSize * 2f));
					if (Gui.AndroidTV)
					{
						bounds2.size *= 0.9f;
					}
					if (Util.IsIPhoneX && Screen.height > Screen.width)
					{
						Vector3 size = bounds2.size;
						float num2 = size.y * 0.03f;
						bounds2.size += Vector3.down * num2;
						Vector3 size2 = bounds2.size;
						float x = size2.x;
						Vector3 size3 = bounds2.size;
						num = x / size3.y;
						bounds2.center += Vector3.down * (num2 / 2f);
					}
					Vector3 translation = bounds2.center - bounds.center;
					GuiView component = base.transform.parent.GetComponent<GuiView>();
					if (!(component == null))
					{
						float num3 = -component.ZIndex;
						Vector3 position = base.transform.parent.position;
						translation.z = num3 - position.z;
						float num4 = num;
						Vector3 size4 = bounds.size;
						float x2 = size4.x;
						Vector3 size5 = bounds.size;
						float num5;
						if (num4 > x2 / size5.y)
						{
							Vector3 size6 = bounds2.size;
							float y = size6.y;
							Vector3 size7 = bounds.size;
							num5 = y / size7.y;
							if (Alignment == TextAlignment.Left)
							{
								float x3 = translation.x;
								Vector3 size8 = bounds.size;
								float num6 = size8.y * num;
								Vector3 size9 = bounds.size;
								translation.x = x3 - (num6 - size9.x) / 2f * num5;
							}
							else if (Alignment == TextAlignment.Right)
							{
								float x4 = translation.x;
								Vector3 size10 = bounds.size;
								float num7 = size10.y * num;
								Vector3 size11 = bounds.size;
								translation.x = x4 + (num7 - size11.x) / 2f * num5;
							}
						}
						else
						{
							Vector3 size12 = bounds2.size;
							float x5 = size12.x;
							Vector3 size13 = bounds.size;
							num5 = x5 / size13.x;
							if (VerticalAlignment == VerticalAlignment.Bottom)
							{
								float y2 = translation.y;
								Vector3 size14 = bounds.size;
								float num8 = size14.x / num;
								Vector3 size15 = bounds.size;
								translation.y = y2 - (num8 - size15.y) / 2f * num5;
							}
							else if (VerticalAlignment == VerticalAlignment.Top)
							{
								float y3 = translation.y;
								Vector3 size16 = bounds.size;
								float num9 = size16.x / num;
								Vector3 size17 = bounds.size;
								translation.y = y3 + (num9 - size17.y) / 2f * num5;
							}
						}
						base.transform.parent.Translate(translation);
						base.transform.parent.localScale = new Vector3(num5, num5, num5);
					}
				});
			}
		}
	}
}
