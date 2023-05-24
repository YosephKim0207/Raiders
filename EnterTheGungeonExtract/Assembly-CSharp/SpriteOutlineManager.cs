using System;
using UnityEngine;

// Token: 0x0200153D RID: 5437
public static class SpriteOutlineManager
{
	// Token: 0x06007C69 RID: 31849 RVA: 0x003211BC File Offset: 0x0031F3BC
	public static void AddSingleOutlineToSprite(tk2dBaseSprite targetSprite, IntVector2 pixelOffset, Color outlineColor)
	{
		SpriteOutlineManager.AddSingleOutlineToSprite<tk2dSprite>(targetSprite, pixelOffset, outlineColor, 0.4f, 0f);
	}

	// Token: 0x06007C6A RID: 31850 RVA: 0x003211D0 File Offset: 0x0031F3D0
	public static void AddOutlineToSprite(tk2dBaseSprite targetSprite, Color outlineColor)
	{
		SpriteOutlineManager.AddOutlineToSprite(targetSprite, outlineColor, 0.4f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
	}

	// Token: 0x06007C6B RID: 31851 RVA: 0x003211E4 File Offset: 0x0031F3E4
	public static void AddOutlineToSprite<T>(tk2dBaseSprite targetSprite, Color outlineColor) where T : tk2dBaseSprite
	{
		SpriteOutlineManager.AddOutlineToSprite<T>(targetSprite, outlineColor, 0.4f, 0f, null);
	}

	// Token: 0x06007C6C RID: 31852 RVA: 0x003211F8 File Offset: 0x0031F3F8
	public static void AddOutlineToSprite<T>(tk2dBaseSprite targetSprite, Color outlineColor, Material overrideOutlineMaterial) where T : tk2dBaseSprite
	{
		SpriteOutlineManager.AddOutlineToSprite<T>(targetSprite, outlineColor, 0.4f, 0f, overrideOutlineMaterial);
	}

	// Token: 0x06007C6D RID: 31853 RVA: 0x0032120C File Offset: 0x0031F40C
	public static bool HasOutline(tk2dBaseSprite targetSprite)
	{
		foreach (tk2dBaseSprite tk2dBaseSprite in targetSprite.GetComponentsInChildren<tk2dBaseSprite>(true))
		{
			if (!(tk2dBaseSprite.transform.parent != targetSprite.transform))
			{
				if (tk2dBaseSprite.IsOutlineSprite)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06007C6E RID: 31854 RVA: 0x00321268 File Offset: 0x0031F468
	public static Material GetOutlineMaterial(tk2dBaseSprite targetSprite)
	{
		if (targetSprite == null)
		{
			return null;
		}
		Transform transform = targetSprite.transform.Find("BraveOutlineSprite");
		if (transform != null)
		{
			return transform.GetComponent<tk2dBaseSprite>().renderer.sharedMaterial;
		}
		foreach (tk2dBaseSprite tk2dBaseSprite in targetSprite.GetComponentsInChildren<tk2dBaseSprite>(true))
		{
			if (!(tk2dBaseSprite.transform.parent != targetSprite.transform))
			{
				if (tk2dBaseSprite.IsOutlineSprite)
				{
					return tk2dBaseSprite.renderer.sharedMaterial;
				}
			}
		}
		return null;
	}

	// Token: 0x06007C6F RID: 31855 RVA: 0x0032130C File Offset: 0x0031F50C
	public static tk2dSprite[] GetOutlineSprites(tk2dBaseSprite targetSprite)
	{
		return SpriteOutlineManager.GetOutlineSprites<tk2dSprite>(targetSprite);
	}

	// Token: 0x06007C70 RID: 31856 RVA: 0x00321314 File Offset: 0x0031F514
	public static int ChangeOutlineLayer(tk2dBaseSprite targetSprite, int targetLayer)
	{
		tk2dSprite[] outlineSprites = SpriteOutlineManager.GetOutlineSprites(targetSprite);
		int num = -1;
		if (outlineSprites != null)
		{
			for (int i = 0; i < outlineSprites.Length; i++)
			{
				if (outlineSprites[i])
				{
					num = outlineSprites[i].gameObject.layer;
					outlineSprites[i].gameObject.layer = targetLayer;
				}
			}
		}
		return num;
	}

	// Token: 0x06007C71 RID: 31857 RVA: 0x00321374 File Offset: 0x0031F574
	public static T[] GetOutlineSprites<T>(tk2dBaseSprite targetSprite) where T : tk2dBaseSprite
	{
		if (targetSprite == null)
		{
			return null;
		}
		Transform transform = targetSprite.transform.Find("BraveOutlineSprite");
		if (transform != null)
		{
			return new T[] { transform.GetComponent<T>() };
		}
		T[] componentsInChildren = targetSprite.GetComponentsInChildren<T>(true);
		T[] array = new T[4];
		int num = 0;
		foreach (T t in componentsInChildren)
		{
			if (!(t.transform.parent != targetSprite.transform))
			{
				if (t.IsOutlineSprite)
				{
					array[num] = t;
					num++;
				}
			}
		}
		return array;
	}

	// Token: 0x06007C72 RID: 31858 RVA: 0x00321440 File Offset: 0x0031F640
	public static void UpdateSingleOutlineSprite(tk2dBaseSprite targetSprite, IntVector2 newPixelOffset)
	{
		Transform transform = targetSprite.transform.Find("OutlineSprite0");
		if (transform != null)
		{
			transform.localPosition = newPixelOffset.ToVector3() * 0.0625f;
		}
	}

	// Token: 0x06007C73 RID: 31859 RVA: 0x00321484 File Offset: 0x0031F684
	public static void AddSingleOutlineToSprite<T>(tk2dBaseSprite targetSprite, IntVector2 pixelOffset, Color outlineColor, float zOffset, float luminanceCutoff = 0f) where T : tk2dBaseSprite
	{
		SpriteOutlineManager.HandleSingleOutlineAddition<T>(targetSprite, null, pixelOffset, 0, outlineColor, zOffset, luminanceCutoff);
	}

	// Token: 0x06007C74 RID: 31860 RVA: 0x00321494 File Offset: 0x0031F694
	private static void HandleInitialLayer(tk2dBaseSprite sourceSprite, GameObject outlineObject)
	{
		int num = sourceSprite.gameObject.layer;
		if (num == 22)
		{
			num = 21;
		}
		outlineObject.layer = num;
	}

	// Token: 0x06007C75 RID: 31861 RVA: 0x003214C0 File Offset: 0x0031F6C0
	private static void HandleBraveOutlineAddition<T>(tk2dBaseSprite targetSprite, Color outlineColor, float zOffset, float luminanceCutoff = 0f) where T : tk2dBaseSprite
	{
		Transform transform = targetSprite.transform;
		GameObject gameObject = new GameObject("BraveOutlineSprite");
		Transform transform2 = gameObject.transform;
		transform2.parent = transform;
		transform2.localPosition = Vector3.zero;
		transform2.localRotation = Quaternion.identity;
		if (targetSprite.ignoresTiltworldDepth)
		{
			transform2.localPosition = transform2.localPosition.WithZ(1f);
		}
		T t = gameObject.AddComponent<T>();
		t.IsOutlineSprite = true;
		t.IsBraveOutlineSprite = true;
		t.usesOverrideMaterial = true;
		t.depthUsesTrimmedBounds = targetSprite.depthUsesTrimmedBounds;
		t.SetSprite(targetSprite.Collection, targetSprite.spriteId);
		t.ignoresTiltworldDepth = targetSprite.ignoresTiltworldDepth;
		SpriteOutlineManager.HandleInitialLayer(targetSprite, gameObject);
		t.scale = targetSprite.scale;
		Material material = new Material(ShaderCache.Acquire("Brave/Internal/SinglePassOutline"));
		material.mainTexture = targetSprite.renderer.sharedMaterial.mainTexture;
		material.SetColor("_OverrideColor", outlineColor);
		material.SetFloat("_LuminanceCutoff", luminanceCutoff);
		material.DisableKeyword("OUTLINE_OFF");
		material.EnableKeyword("OUTLINE_ON");
		t.renderer.material = material;
		t.HeightOffGround = -zOffset;
		targetSprite.AttachRenderer(t);
		targetSprite.UpdateZDepth();
		SpriteOutlineManager.HandleSpriteChanged(targetSprite);
	}

	// Token: 0x06007C76 RID: 31862 RVA: 0x0032163C File Offset: 0x0031F83C
	private static Material HandleSingleOutlineAddition<T>(tk2dBaseSprite targetSprite, Material sharedMaterialToUse, IntVector2 pixelOffset, int outlineIndex, Color outlineColor, float zOffset, float luminanceCutoff = 0f) where T : tk2dBaseSprite
	{
		Transform transform = targetSprite.transform;
		Vector3 vector = pixelOffset.ToVector3() * 0.0625f;
		GameObject gameObject = new GameObject(SpriteOutlineManager.m_outlineNames[outlineIndex]);
		Transform transform2 = gameObject.transform;
		transform2.parent = transform;
		transform2.localPosition = Vector3.Scale(vector, targetSprite.scale);
		transform2.localRotation = Quaternion.identity;
		if (targetSprite.ignoresTiltworldDepth)
		{
			transform2.localPosition = transform2.localPosition.WithZ(1f);
		}
		T t = gameObject.AddComponent<T>();
		t.IsOutlineSprite = true;
		t.usesOverrideMaterial = true;
		t.depthUsesTrimmedBounds = targetSprite.depthUsesTrimmedBounds;
		t.SetSprite(targetSprite.Collection, targetSprite.spriteId);
		t.ignoresTiltworldDepth = targetSprite.ignoresTiltworldDepth;
		SpriteOutlineManager.HandleInitialLayer(targetSprite, gameObject);
		t.scale = targetSprite.scale;
		Material material = sharedMaterialToUse;
		if (material == null)
		{
			material = new Material(ShaderCache.Acquire("tk2d/SpriteOutlineCutout"));
			material.mainTexture = targetSprite.renderer.sharedMaterial.mainTexture;
			material.SetColor("_OverrideColor", outlineColor);
			material.SetFloat("_LuminanceCutoff", luminanceCutoff);
		}
		t.renderer.material = material;
		t.HeightOffGround = -zOffset;
		targetSprite.AttachRenderer(t);
		targetSprite.UpdateZDepth();
		return material;
	}

	// Token: 0x06007C77 RID: 31863 RVA: 0x003217C4 File Offset: 0x0031F9C4
	private static Material HandleSingleScaledOutlineAddition<T>(tk2dBaseSprite targetSprite, Material sharedMaterialToUse, IntVector2 pixelOffset, int outlineIndex, Color outlineColor, float zOffset, float luminanceCutoff = 0f) where T : tk2dBaseSprite
	{
		Transform transform = targetSprite.transform;
		Vector3 vector = pixelOffset.ToVector3() * 0.0625f;
		bool flipX = targetSprite.FlipX;
		bool flipY = targetSprite.FlipY;
		Vector3 vector2 = Vector3.Scale(new Vector3((float)((!flipX) ? 1 : (-1)), (float)((!flipY) ? 1 : (-1)), 1f), targetSprite.scale);
		GameObject gameObject = new GameObject(SpriteOutlineManager.m_outlineNames[outlineIndex]);
		Transform transform2 = gameObject.transform;
		transform2.parent = transform;
		transform2.localPosition = Vector3.Scale(vector, targetSprite.scale);
		transform2.localRotation = Quaternion.identity;
		transform2.localScale = Vector3.one;
		if (targetSprite.ignoresTiltworldDepth)
		{
			transform2.localPosition = transform2.localPosition.WithZ(1f);
		}
		T t = gameObject.AddComponent<T>();
		t.IsOutlineSprite = true;
		t.usesOverrideMaterial = true;
		t.depthUsesTrimmedBounds = targetSprite.depthUsesTrimmedBounds;
		t.SetSprite(targetSprite.Collection, targetSprite.spriteId);
		t.ignoresTiltworldDepth = targetSprite.ignoresTiltworldDepth;
		SpriteOutlineManager.HandleInitialLayer(targetSprite, gameObject);
		t.scale = vector2;
		Material material = sharedMaterialToUse;
		if (material == null)
		{
			material = new Material(ShaderCache.Acquire("tk2d/SpriteOutlineCutout"));
			material.mainTexture = targetSprite.renderer.sharedMaterial.mainTexture;
			material.SetColor("_OverrideColor", outlineColor);
			material.SetFloat("_LuminanceCutoff", luminanceCutoff);
		}
		t.renderer.material = material;
		t.HeightOffGround = -zOffset;
		targetSprite.AttachRenderer(t);
		targetSprite.UpdateZDepth();
		return material;
	}

	// Token: 0x06007C78 RID: 31864 RVA: 0x003219A0 File Offset: 0x0031FBA0
	public static void ForceRebuildMaterial(tk2dBaseSprite outlineSprite, tk2dBaseSprite sourceSprite, Color c, float luminanceCutoff = 0f)
	{
		Material material = null;
		if (material == null)
		{
			material = new Material(ShaderCache.Acquire("tk2d/SpriteOutlineCutout"));
			material.mainTexture = sourceSprite.renderer.sharedMaterial.mainTexture;
			material.SetColor("_OverrideColor", c);
			material.SetFloat("_LuminanceCutoff", luminanceCutoff);
		}
		outlineSprite.renderer.material = material;
	}

	// Token: 0x06007C79 RID: 31865 RVA: 0x00321A08 File Offset: 0x0031FC08
	public static void ForceUpdateOutlineMaterial(tk2dBaseSprite outlineSprite, tk2dBaseSprite sourceSprite)
	{
		if (!sourceSprite || !outlineSprite)
		{
			return;
		}
		Material sharedMaterial = outlineSprite.renderer.sharedMaterial;
		sharedMaterial.mainTexture = sourceSprite.renderer.sharedMaterial.mainTexture;
		outlineSprite.renderer.sharedMaterial = sharedMaterial;
	}

	// Token: 0x06007C7A RID: 31866 RVA: 0x00321A5C File Offset: 0x0031FC5C
	public static void AddScaledOutlineToSprite<T>(tk2dBaseSprite targetSprite, Color outlineColor, float zOffset, float luminanceCutoff) where T : tk2dBaseSprite
	{
		if (GameManager.Instance.Dungeon != null && GameManager.Instance.Dungeon.debugSettings.DISABLE_OUTLINES)
		{
			return;
		}
		if (SpriteOutlineManager.HasOutline(targetSprite))
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(targetSprite, true);
		}
		IntVector2[] cardinals = IntVector2.Cardinals;
		Material material = null;
		for (int i = 0; i < 4; i++)
		{
			material = SpriteOutlineManager.HandleSingleScaledOutlineAddition<T>(targetSprite, material, cardinals[i], i, outlineColor, zOffset, luminanceCutoff);
		}
		targetSprite.SpriteChanged += SpriteOutlineManager.HandleSpriteChanged;
		targetSprite.UpdateZDepth();
		SpriteOutlineManager.HandleSpriteChanged(targetSprite);
	}

	// Token: 0x06007C7B RID: 31867 RVA: 0x00321B0C File Offset: 0x0031FD0C
	public static void AddOutlineToSprite(tk2dBaseSprite targetSprite, Color outlineColor, float zOffset, float luminanceCutoff = 0f, SpriteOutlineManager.OutlineType outlineType = SpriteOutlineManager.OutlineType.NORMAL)
	{
		if (GameManager.Instance.Dungeon != null && GameManager.Instance.Dungeon.debugSettings.DISABLE_OUTLINES)
		{
			return;
		}
		if (SpriteOutlineManager.HasOutline(targetSprite))
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(targetSprite, true);
		}
		if (outlineType == SpriteOutlineManager.OutlineType.NORMAL)
		{
			SpriteOutlineManager.HandleBraveOutlineAddition<tk2dSprite>(targetSprite, outlineColor, zOffset, luminanceCutoff);
		}
		else if (outlineType == SpriteOutlineManager.OutlineType.EEVEE)
		{
			IntVector2[] cardinals = IntVector2.Cardinals;
			Material material = null;
			for (int i = 0; i < 4; i++)
			{
				material = SpriteOutlineManager.HandleSingleOutlineAddition<tk2dSprite>(targetSprite, material, cardinals[i], i, outlineColor, zOffset, luminanceCutoff);
			}
			material.shader = Shader.Find("Brave/PlayerShaderEevee");
			material.SetTexture("_EeveeTex", targetSprite.transform.parent.GetComponent<CharacterAnimationRandomizer>().CosmicTex);
		}
		targetSprite.SpriteChanged += SpriteOutlineManager.HandleSpriteChanged;
		targetSprite.UpdateZDepth();
		SpriteOutlineManager.HandleSpriteChanged(targetSprite);
		if (!targetSprite.renderer.enabled)
		{
			SpriteOutlineManager.ToggleOutlineRenderers(targetSprite, false);
		}
	}

	// Token: 0x06007C7C RID: 31868 RVA: 0x00321C20 File Offset: 0x0031FE20
	public static void AddOutlineToSprite<T>(tk2dBaseSprite targetSprite, Color outlineColor, float zOffset, float luminanceCutoff = 0f, Material overrideOutlineMaterial = null) where T : tk2dBaseSprite
	{
		if (GameManager.Instance.Dungeon != null && GameManager.Instance.Dungeon.debugSettings.DISABLE_OUTLINES)
		{
			return;
		}
		if (SpriteOutlineManager.HasOutline(targetSprite))
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(targetSprite, true);
		}
		IntVector2[] cardinals = IntVector2.Cardinals;
		Material material = overrideOutlineMaterial;
		for (int i = 0; i < 4; i++)
		{
			material = SpriteOutlineManager.HandleSingleOutlineAddition<T>(targetSprite, material, cardinals[i], i, outlineColor, zOffset, luminanceCutoff);
		}
		targetSprite.SpriteChanged += SpriteOutlineManager.HandleSpriteChanged;
		targetSprite.UpdateZDepth();
		SpriteOutlineManager.HandleSpriteChanged(targetSprite);
	}

	// Token: 0x06007C7D RID: 31869 RVA: 0x00321CD0 File Offset: 0x0031FED0
	public static void HandleSpriteChanged(tk2dBaseSprite targetSprite)
	{
		if (SpriteOutlineManager.m_atlasDataID == -1)
		{
			SpriteOutlineManager.m_atlasDataID = Shader.PropertyToID("_AtlasData");
		}
		Transform transform = targetSprite.transform;
		Vector3 scale = targetSprite.scale;
		bool flag = false;
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			tk2dBaseSprite component = child.GetComponent<tk2dBaseSprite>();
			if (component && component.IsBraveOutlineSprite)
			{
				flag = true;
				tk2dSpriteDefinition currentSpriteDef = targetSprite.GetCurrentSpriteDef();
				Vector4 vector = new Vector4(1f, 1f, 0f, 0f);
				if (currentSpriteDef.flipped == tk2dSpriteDefinition.FlipMode.Tk2d)
				{
					vector = new Vector4(vector.x, vector.y, 1f, 1f);
				}
				vector.x *= targetSprite.scale.x;
				vector.y *= targetSprite.scale.y;
				tk2dBaseSprite component2 = child.GetComponent<tk2dBaseSprite>();
				component2.scale = scale;
				component2.SetSprite(targetSprite.Collection, targetSprite.spriteId);
				component2.renderer.material.SetVector(SpriteOutlineManager.m_atlasDataID, vector);
			}
		}
		if (!flag)
		{
			for (int j = 0; j < 4; j++)
			{
				Transform transform2 = transform.Find(SpriteOutlineManager.m_outlineNames[j]);
				if (transform2 != null)
				{
					tk2dBaseSprite component3 = transform2.GetComponent<tk2dBaseSprite>();
					component3.scale = scale;
					component3.SetSprite(targetSprite.Collection, targetSprite.spriteId);
				}
			}
		}
	}

	// Token: 0x06007C7E RID: 31870 RVA: 0x00321E6C File Offset: 0x0032006C
	public static void ToggleOutlineRenderers(tk2dBaseSprite targetSprite, bool value)
	{
		foreach (tk2dBaseSprite tk2dBaseSprite in targetSprite.GetComponentsInChildren<tk2dBaseSprite>(true))
		{
			if (tk2dBaseSprite.IsOutlineSprite)
			{
				tk2dBaseSprite.renderer.enabled = value;
			}
		}
	}

	// Token: 0x06007C7F RID: 31871 RVA: 0x00321EB0 File Offset: 0x003200B0
	public static void RemoveOutlineFromSprite(tk2dBaseSprite targetSprite, bool deparent = false)
	{
		Transform transform = targetSprite.transform;
		targetSprite.SpriteChanged -= SpriteOutlineManager.HandleSpriteChanged;
		bool flag = false;
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			tk2dBaseSprite component = child.GetComponent<tk2dBaseSprite>();
			if (component && component.IsBraveOutlineSprite)
			{
				flag = true;
				if (deparent)
				{
					child.parent = null;
				}
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
		if (!flag)
		{
			for (int j = 0; j < 4; j++)
			{
				Transform transform2 = transform.Find(SpriteOutlineManager.m_outlineNames[j]);
				if (!(transform2 != null))
				{
					break;
				}
				if (deparent)
				{
					transform2.parent = null;
				}
				UnityEngine.Object.Destroy(transform2.gameObject);
			}
		}
	}

	// Token: 0x04007F66 RID: 32614
	private static string[] m_outlineNames = new string[] { "OutlineSprite0", "OutlineSprite1", "OutlineSprite2", "OutlineSprite3" };

	// Token: 0x04007F67 RID: 32615
	private static int m_atlasDataID = -1;

	// Token: 0x0200153E RID: 5438
	public enum OutlineType
	{
		// Token: 0x04007F6D RID: 32621
		NORMAL,
		// Token: 0x04007F6E RID: 32622
		EEVEE
	}
}
