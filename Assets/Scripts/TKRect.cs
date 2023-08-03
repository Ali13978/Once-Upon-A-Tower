using UnityEngine;

public struct TKRect
{
	public float x;

	public float y;

	public float width;

	public float height;

	public float xMin => x;

	public float xMax => x + width;

	public float yMin => y;

	public float yMax => y + height;

	public Vector2 center => new Vector2(x + width / 2f, y + height / 2f);

	public TKRect(float x, float y, float width, float height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
		updateRectWithRuntimeScaleModifier();
	}

	public TKRect(float width, float height, Vector2 center)
	{
		this.width = width;
		this.height = height;
		x = center.x - width / 2f;
		y = center.y - height / 2f;
		updateRectWithRuntimeScaleModifier();
	}

	private void updateRectWithRuntimeScaleModifier()
	{
		Vector2 runtimeScaleModifier = TouchKit.instance.runtimeScaleModifier;
		x *= runtimeScaleModifier.x;
		y *= runtimeScaleModifier.y;
		width *= runtimeScaleModifier.x;
		height *= runtimeScaleModifier.y;
	}

	public TKRect copyWithExpansion(float allSidesExpansion)
	{
		return copyWithExpansion(allSidesExpansion, allSidesExpansion);
	}

	public TKRect copyWithExpansion(float xExpansion, float yExpansion)
	{
		float num = xExpansion;
		Vector2 runtimeScaleModifier = TouchKit.instance.runtimeScaleModifier;
		xExpansion = num * runtimeScaleModifier.x;
		float num2 = yExpansion;
		Vector2 runtimeScaleModifier2 = TouchKit.instance.runtimeScaleModifier;
		yExpansion = num2 * runtimeScaleModifier2.y;
		TKRect result = default(TKRect);
		result.x = x - xExpansion;
		result.y = y - yExpansion;
		result.width = width + xExpansion * 2f;
		result.height = height + yExpansion * 2f;
		return result;
	}

	public bool contains(Vector2 point)
	{
		if (x <= point.x && y <= point.y && xMax >= point.x && yMax >= point.y)
		{
			return true;
		}
		return false;
	}

	public override string ToString()
	{
		return $"TKRect: x: {x}, xMax: {xMax}, y: {y}, yMax: {yMax}, width: {width}, height: {height}, center: {center}";
	}
}
