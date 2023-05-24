using System;
using UnityEngine;

// Token: 0x0200181E RID: 6174
public static class BraveCameraUtility
{
	// Token: 0x170015D8 RID: 5592
	// (get) Token: 0x060091A0 RID: 37280 RVA: 0x003D94EC File Offset: 0x003D76EC
	// (set) Token: 0x060091A1 RID: 37281 RVA: 0x003D9550 File Offset: 0x003D7750
	public static float ASPECT
	{
		get
		{
			if (BraveCameraUtility.OverrideAspect != null)
			{
				return BraveCameraUtility.OverrideAspect.Value;
			}
			if (GameManager.Options.CurrentPreferredScalingMode == GameOptions.PreferredScalingMode.FORCE_PIXEL_PERFECT || GameManager.Options.CurrentPreferredScalingMode == GameOptions.PreferredScalingMode.PIXEL_PERFECT)
			{
				return 1.7777778f;
			}
			return Mathf.Max(1.7777778f, (float)Screen.width / (float)Screen.height);
		}
		set
		{
		}
	}

	// Token: 0x170015D9 RID: 5593
	// (get) Token: 0x060091A2 RID: 37282 RVA: 0x003D9554 File Offset: 0x003D7754
	public static int H_PIXELS
	{
		get
		{
			return Mathf.RoundToInt(480f * (BraveCameraUtility.ASPECT / 1.7777778f));
		}
	}

	// Token: 0x170015DA RID: 5594
	// (get) Token: 0x060091A3 RID: 37283 RVA: 0x003D956C File Offset: 0x003D776C
	public static int V_PIXELS
	{
		get
		{
			return Mathf.RoundToInt((float)BraveCameraUtility.H_PIXELS / BraveCameraUtility.ASPECT);
		}
	}

	// Token: 0x060091A4 RID: 37284 RVA: 0x003D9580 File Offset: 0x003D7780
	public static Rect GetRect()
	{
		if (GameManager.Options.CurrentPreferredScalingMode != GameOptions.PreferredScalingMode.UNIFORM_SCALING && GameManager.Options.CurrentPreferredScalingMode != GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST)
		{
			int num = Mathf.FloorToInt((float)Screen.width / (float)BraveCameraUtility.H_PIXELS);
			int num2 = Mathf.FloorToInt((float)Screen.height / (float)BraveCameraUtility.V_PIXELS);
			int num3 = Mathf.Min(num, num2);
			int num4 = BraveCameraUtility.H_PIXELS * num3;
			int num5 = BraveCameraUtility.V_PIXELS * num3;
			float num6 = 1f - (float)num4 / (float)Screen.width;
			float num7 = 1f - (float)num5 / (float)Screen.height;
			return new Rect(num6 / 2f, num7 / 2f, 1f - num6, 1f - num7);
		}
		float num8 = (float)Screen.width / (float)Screen.height;
		float num9 = 0f;
		float num10 = 0f;
		if (Screen.width % 16 == 0 && Screen.height % 9 == 0 && Screen.width / 16 == Screen.height / 9)
		{
			return new Rect(0f, 0f, 1f, 1f);
		}
		if (num8 > BraveCameraUtility.ASPECT)
		{
			num9 = 1f - BraveCameraUtility.ASPECT / num8;
		}
		else if (num8 < BraveCameraUtility.ASPECT)
		{
			num10 = 1f - num8 / BraveCameraUtility.ASPECT;
		}
		return new Rect(num9 / 2f, num10 / 2f, 1f - num9, 1f - num10);
	}

	// Token: 0x060091A5 RID: 37285 RVA: 0x003D9700 File Offset: 0x003D7900
	public static Vector2 GetRectSize()
	{
		if (GameManager.Options.CurrentPreferredScalingMode != GameOptions.PreferredScalingMode.UNIFORM_SCALING && GameManager.Options.CurrentPreferredScalingMode != GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST)
		{
			int num = Mathf.FloorToInt((float)Screen.width / (float)BraveCameraUtility.H_PIXELS);
			int num2 = Mathf.FloorToInt((float)Screen.height / (float)BraveCameraUtility.V_PIXELS);
			int num3 = Mathf.Min(num, num2);
			int num4 = BraveCameraUtility.H_PIXELS * num3;
			int num5 = BraveCameraUtility.V_PIXELS * num3;
			float num6 = 1f - (float)num4 / (float)Screen.width;
			float num7 = 1f - (float)num5 / (float)Screen.height;
			return new Vector2(1f - num6, 1f - num7);
		}
		float num8 = (float)Screen.width / (float)Screen.height;
		float num9 = 0f;
		float num10 = 0f;
		if (Screen.width % 16 == 0 && Screen.height % 9 == 0 && Screen.width / 16 == Screen.height / 9)
		{
			return Vector2.one;
		}
		if (num8 > BraveCameraUtility.ASPECT)
		{
			num9 = 1f - BraveCameraUtility.ASPECT / num8;
		}
		else if (num8 < BraveCameraUtility.ASPECT)
		{
			num10 = 1f - num8 / BraveCameraUtility.ASPECT;
		}
		return new Vector2(1f - num9, 1f - num10);
	}

	// Token: 0x060091A6 RID: 37286 RVA: 0x003D984C File Offset: 0x003D7A4C
	public static Vector2 ConvertGameViewportToScreenViewport(Vector2 pos)
	{
		Rect rect = BraveCameraUtility.GetRect();
		return new Vector2(pos.x * rect.width + rect.x, pos.y * rect.height + rect.y);
	}

	// Token: 0x060091A7 RID: 37287 RVA: 0x003D9894 File Offset: 0x003D7A94
	public static IntVector2 GetTargetScreenResolution(IntVector2 inResolution)
	{
		float num = (float)inResolution.x / (float)inResolution.y;
		if (inResolution.x % 16 == 0 && inResolution.y % 9 == 0 && inResolution.x / 16 == inResolution.y / 9)
		{
			return inResolution;
		}
		if (num > BraveCameraUtility.ASPECT)
		{
			float num2 = num / BraveCameraUtility.ASPECT;
			float num3 = (float)inResolution.y * num2;
			return new IntVector2(inResolution.x, Mathf.RoundToInt(num3));
		}
		if (num < BraveCameraUtility.ASPECT)
		{
			float num4 = num / BraveCameraUtility.ASPECT;
			float num5 = (float)inResolution.x / num4;
			return new IntVector2(Mathf.RoundToInt(num5), inResolution.y);
		}
		return inResolution;
	}

	// Token: 0x060091A8 RID: 37288 RVA: 0x003D9954 File Offset: 0x003D7B54
	public static void MaintainCameraAspect(Camera c)
	{
		c.transparencySortMode = TransparencySortMode.Orthographic;
		if (GameManager.Options == null || (GameManager.Options.CurrentPreferredScalingMode != GameOptions.PreferredScalingMode.UNIFORM_SCALING && GameManager.Options.CurrentPreferredScalingMode != GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST))
		{
			int num = Mathf.FloorToInt((float)Screen.width / (float)BraveCameraUtility.H_PIXELS);
			int num2 = Mathf.FloorToInt((float)Screen.height / (float)BraveCameraUtility.V_PIXELS);
			int num3 = Mathf.Max(1, Mathf.Min(num, num2));
			int num4 = BraveCameraUtility.H_PIXELS * num3;
			int num5 = BraveCameraUtility.V_PIXELS * num3;
			float num6 = 1f - (float)num4 / (float)Screen.width;
			float num7 = 1f - (float)num5 / (float)Screen.height;
			c.rect = new Rect(num6 / 2f, num7 / 2f, 1f - num6, 1f - num7);
			if (BraveCameraUtility.m_cachedMultiple != num3)
			{
				dfGUIManager.ForceResolutionUpdates();
			}
			BraveCameraUtility.m_cachedMultiple = num3;
		}
		else
		{
			float num8 = (float)Screen.width / (float)Screen.height;
			float aspect = BraveCameraUtility.ASPECT;
			float num9 = 0f;
			float num10 = 0f;
			bool flag = false;
			if (Screen.width % 16 == 0 && Screen.height % 9 == 0 && Screen.width / 16 == Screen.height / 9)
			{
				c.rect = new Rect(0f, 0f, 1f, 1f);
				flag = true;
			}
			else if (num8 > aspect)
			{
				num9 = 1f - aspect / num8;
			}
			else if (num8 < aspect)
			{
				num10 = 1f - num8 / aspect;
			}
			if (!flag)
			{
				c.rect = new Rect(num9 / 2f, num10 / 2f, 1f - num9, 1f - num10);
			}
		}
		float num11 = GameManager.Options.DisplaySafeArea;
		num11 = Mathf.Clamp01(num11);
		float width = c.rect.width;
		float height = c.rect.height;
		Rect rect = new Rect(c.rect.xMin + width * (0.5f * (1f - num11)), c.rect.yMin + height * (0.5f * (1f - num11)), width * num11, height * num11);
		c.rect = rect;
	}

	// Token: 0x060091A9 RID: 37289 RVA: 0x003D9BB4 File Offset: 0x003D7DB4
	public static void MaintainCameraAspectForceAspect(Camera c, float forcedAspect)
	{
		c.transparencySortMode = TransparencySortMode.Orthographic;
		if (GameManager.Options == null || (GameManager.Options.CurrentPreferredScalingMode != GameOptions.PreferredScalingMode.UNIFORM_SCALING && GameManager.Options.CurrentPreferredScalingMode != GameOptions.PreferredScalingMode.UNIFORM_SCALING_FAST))
		{
			int num = Mathf.FloorToInt((float)Screen.width / 480f);
			int num2 = Mathf.FloorToInt((float)Screen.height / 270f);
			int num3 = Mathf.Max(1, Mathf.Min(num, num2));
			int num4 = 480 * num3;
			int num5 = 270 * num3;
			float num6 = 1f - (float)num4 / (float)Screen.width;
			float num7 = 1f - (float)num5 / (float)Screen.height;
			c.rect = new Rect(num6 / 2f, num7 / 2f, 1f - num6, 1f - num7);
			if (BraveCameraUtility.m_cachedMultiple != num3)
			{
				dfGUIManager.ForceResolutionUpdates();
			}
			BraveCameraUtility.m_cachedMultiple = num3;
		}
		else
		{
			float num8 = (float)Screen.width / (float)Screen.height;
			float num9 = 0f;
			float num10 = 0f;
			bool flag = false;
			if (Screen.width % 16 == 0 && Screen.height % 9 == 0 && Screen.width / 16 == Screen.height / 9)
			{
				c.rect = new Rect(0f, 0f, 1f, 1f);
				flag = true;
			}
			else if (num8 > forcedAspect)
			{
				num9 = 1f - forcedAspect / num8;
			}
			else if (num8 < forcedAspect)
			{
				num10 = 1f - num8 / forcedAspect;
			}
			if (!flag)
			{
				c.rect = new Rect(num9 / 2f, num10 / 2f, 1f - num9, 1f - num10);
			}
		}
		float num11 = GameManager.Options.DisplaySafeArea;
		num11 = Mathf.Clamp01(num11);
		float width = c.rect.width;
		float height = c.rect.height;
		Rect rect = new Rect(c.rect.xMin + width * (0.5f * (1f - num11)), c.rect.yMin + height * (0.5f * (1f - num11)), width * num11, height * num11);
		c.rect = rect;
	}

	// Token: 0x060091AA RID: 37290 RVA: 0x003D9E08 File Offset: 0x003D8008
	public static Camera GenerateBackgroundCamera(Camera c)
	{
		Camera component = new GameObject("BackgroundCamera", new Type[] { typeof(Camera) }).GetComponent<Camera>();
		component.transform.position = new Vector3(-1000f, -1000f, 0f);
		component.orthographic = true;
		component.orthographicSize = 1f;
		component.depth = -5f;
		component.clearFlags = CameraClearFlags.Color;
		component.backgroundColor = Color.black;
		component.cullingMask = -1;
		component.rect = new Rect(0f, 0f, 1f, 1f);
		return component;
	}

	// Token: 0x040099D8 RID: 39384
	public static float? OverrideAspect;

	// Token: 0x040099D9 RID: 39385
	private static int m_cachedMultiple = -1;
}
