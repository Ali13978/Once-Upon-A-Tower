using I2.Loc;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizeUtil
{
	public static Dictionary<string, string> LanguageFonts = new Dictionary<string, string>
	{
		{
			"Japanese",
			"Arial Unicode Japanese"
		},
		{
			"Chinese (Traditional)",
			"Arial Unicode Chinese"
		},
		{
			"Chinese (Simplified)",
			"Arial Unicode Chinese"
		},
		{
			"Russian",
			"Arial Unicode Russian Turkish"
		},
		{
			"Turkish",
			"Arial Unicode Russian Turkish"
		},
		{
			"Korean",
			"Arial Unicode Korean"
		}
	};

	public static void LocalizeFont(TextMeshPro textMesh)
	{
		if (Application.isPlaying && LanguageFonts.TryGetValue(LocalizationManager.CurrentLanguage, out string value))
		{
			Material fontMaterial = textMesh.fontMaterial;
			textMesh.font = Resources.Load<TMP_FontAsset>(value);
			Material material = new Material(fontMaterial.shader);
			material.CopyPropertiesFromMaterial(fontMaterial);
			material.mainTexture = textMesh.font.atlas;
			if (material.GetFloat("_FaceDilate") > 0f)
			{
				material.SetFloat("_FaceDilate", material.GetFloat("_FaceDilate") / 4f);
			}
			if (material.GetFloat("_OutlineWidth") > 0f)
			{
				material.SetFloat("_OutlineWidth", material.GetFloat("_OutlineWidth") / 4f);
			}
			Renderer component = textMesh.GetComponent<Renderer>();
			if ((bool)component)
			{
				component.material = material;
			}
			textMesh.fontStyle &= (FontStyles)(-2);
			textMesh.fontMaterial = material;
			textMesh.lineSpacing = 0f;
			textMesh.characterSpacing = 0f;
			textMesh.SetAllDirty();
		}
	}
}
