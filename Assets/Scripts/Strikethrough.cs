using TMPro;
using UnityEngine;

public class Strikethrough : MonoBehaviour
{
	public TextMeshPro Text;

	public Transform Cube;

	public float lastWidth;

	public void Show()
	{
		PrepareCube();
		Cube.gameObject.SetActive(value: true);
	}

	public void Clear()
	{
		Cube.gameObject.SetActive(value: false);
	}

	private void PrepareCube()
	{
		Vector3 localScale = Cube.localScale;
		Vector3 size = Text.bounds.size;
		localScale.x = size.x;
		Cube.localScale = localScale;
	}
}
