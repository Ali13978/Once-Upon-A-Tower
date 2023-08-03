using System;
using System.IO;
using UnityEngine;

public class StickerCamera : MonoBehaviour
{
	public string filename;

	public void TakeScreenshot()
	{
		ScreenshotCoroutine();
	}

	private Texture2D Screenshot()
	{
		int width = Screen.currentResolution.width;
		int height = Screen.currentResolution.height;
		Camera component = GetComponent<Camera>();
		RenderTexture renderTexture = new RenderTexture(width, height, 32);
		renderTexture.antiAliasing = 8;
		component.targetTexture = renderTexture;
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, mipChain: false);
		component.Render();
		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
		texture2D.Apply();
		component.targetTexture = null;
		RenderTexture.active = null;
		UnityEngine.Object.DestroyImmediate(renderTexture);
		return texture2D;
	}

	private void ScreenshotCoroutine()
	{
		Texture2D tex = Screenshot();
		byte[] bytes = tex.EncodeToPNG();
		DateTime now = DateTime.Now;
		string path = $"Screenshot {now.Year}-{now.Month}-{now.Day} {now.Hour}.{now.Minute}.{now.Second}.png";
		File.WriteAllBytes(path, bytes);
	}
}
