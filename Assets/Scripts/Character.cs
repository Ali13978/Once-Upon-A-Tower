using I2.Loc;
using System;
using System.IO;
using UnityEngine.Purchasing;

[Serializable]
public class Character : IProduct
{
	public string Name;

	public string Scene;

	public bool Enabled;

	public int MinPreviousCharacters;

	public string DisplayName
	{
		get
		{
			string text = ScriptLocalization.Get("Character" + Name);
			return (!string.IsNullOrEmpty(text)) ? text : Name;
		}
	}

	public string Description => ScriptLocalization.Get("Character" + Name + "Description");

	public string SceneName => Path.GetFileNameWithoutExtension(Scene);

	public string ReferenceName => "Character" + Name;

	public string ProductId => "tower.character." + Name.ToLowerInvariant();

	public ProductType ProductType => ProductType.NonConsumable;
}
