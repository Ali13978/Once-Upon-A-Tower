using UnityEngine;

[ExecuteInEditMode]
public class Follow : MonoBehaviour
{
	public Transform Target;

	public string TargetName;

	private bool alreadySearched;

	private void LateUpdate()
	{
		if (!alreadySearched && Target == null && !string.IsNullOrEmpty(TargetName))
		{
			alreadySearched = true;
			Target = Find(base.transform.root, TargetName);
			if (Target == null)
			{
				UnityEngine.Debug.LogError("Follow can't find TargetName " + TargetName);
			}
		}
		if (Application.isPlaying && !(Target == null))
		{
			base.transform.position = Target.position;
			base.transform.rotation = Target.rotation;
		}
	}

	private Transform Find(Transform t, string name)
	{
		if (t.name == name)
		{
			return t;
		}
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			Transform transform = Find(child, name);
			if ((bool)transform)
			{
				return transform;
			}
		}
		return null;
	}
}
