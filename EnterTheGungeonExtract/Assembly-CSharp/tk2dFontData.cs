using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B91 RID: 2961
[AddComponentMenu("2D Toolkit/Backend/tk2dFontData")]
public class tk2dFontData : MonoBehaviour
{
	// Token: 0x1700094B RID: 2379
	// (get) Token: 0x06003DD5 RID: 15829 RVA: 0x00135EBC File Offset: 0x001340BC
	public tk2dFontData inst
	{
		get
		{
			if (this.platformSpecificData == null || this.platformSpecificData.materialInst == null)
			{
				if (this.hasPlatformData)
				{
					string currentPlatform = tk2dSystem.CurrentPlatform;
					string text = string.Empty;
					for (int i = 0; i < this.fontPlatforms.Length; i++)
					{
						if (this.fontPlatforms[i] == currentPlatform)
						{
							text = this.fontPlatformGUIDs[i];
							break;
						}
					}
					if (text.Length == 0)
					{
						text = this.fontPlatformGUIDs[0];
					}
					this.platformSpecificData = tk2dSystem.LoadResourceByGUID<tk2dFontData>(text);
				}
				else
				{
					this.platformSpecificData = this;
				}
				this.platformSpecificData.Init();
			}
			return this.platformSpecificData;
		}
	}

	// Token: 0x06003DD6 RID: 15830 RVA: 0x00135F80 File Offset: 0x00134180
	private void Init()
	{
		if (this.needMaterialInstance)
		{
			if (this.spriteCollection)
			{
				tk2dSpriteCollectionData inst = this.spriteCollection.inst;
				for (int i = 0; i < inst.materials.Length; i++)
				{
					if (inst.materials[i] == this.material)
					{
						this.materialInst = inst.materialInsts[i];
						break;
					}
				}
				if (this.materialInst == null)
				{
					Debug.LogError("Fatal error - font from sprite collection is has an invalid material");
				}
			}
			else
			{
				this.materialInst = UnityEngine.Object.Instantiate<Material>(this.material);
				this.materialInst.hideFlags = HideFlags.DontSave;
			}
		}
		else
		{
			this.materialInst = this.material;
		}
	}

	// Token: 0x06003DD7 RID: 15831 RVA: 0x00136048 File Offset: 0x00134248
	public void ResetPlatformData()
	{
		if (this.hasPlatformData && this.platformSpecificData)
		{
			this.platformSpecificData = null;
		}
		this.materialInst = null;
	}

	// Token: 0x06003DD8 RID: 15832 RVA: 0x00136074 File Offset: 0x00134274
	private void OnDestroy()
	{
		if (this.needMaterialInstance && this.spriteCollection == null)
		{
			UnityEngine.Object.DestroyImmediate(this.materialInst);
		}
	}

	// Token: 0x06003DD9 RID: 15833 RVA: 0x001360A0 File Offset: 0x001342A0
	public void InitDictionary()
	{
		if (this.useDictionary && this.charDict == null)
		{
			this.charDict = new Dictionary<int, tk2dFontChar>(this.charDictKeys.Count);
			for (int i = 0; i < this.charDictKeys.Count; i++)
			{
				this.charDict[this.charDictKeys[i]] = this.charDictValues[i];
			}
		}
	}

	// Token: 0x06003DDA RID: 15834 RVA: 0x00136118 File Offset: 0x00134318
	public void SetDictionary(Dictionary<int, tk2dFontChar> dict)
	{
		this.charDictKeys = new List<int>(dict.Keys);
		this.charDictValues = new List<tk2dFontChar>();
		for (int i = 0; i < this.charDictKeys.Count; i++)
		{
			this.charDictValues.Add(dict[this.charDictKeys[i]]);
		}
	}

	// Token: 0x04003066 RID: 12390
	public const int CURRENT_VERSION = 2;

	// Token: 0x04003067 RID: 12391
	[HideInInspector]
	public int version;

	// Token: 0x04003068 RID: 12392
	public float lineHeight;

	// Token: 0x04003069 RID: 12393
	public tk2dFontChar[] chars;

	// Token: 0x0400306A RID: 12394
	[SerializeField]
	private List<int> charDictKeys;

	// Token: 0x0400306B RID: 12395
	[SerializeField]
	private List<tk2dFontChar> charDictValues;

	// Token: 0x0400306C RID: 12396
	public string[] fontPlatforms;

	// Token: 0x0400306D RID: 12397
	public string[] fontPlatformGUIDs;

	// Token: 0x0400306E RID: 12398
	private tk2dFontData platformSpecificData;

	// Token: 0x0400306F RID: 12399
	public bool hasPlatformData;

	// Token: 0x04003070 RID: 12400
	public bool managedFont;

	// Token: 0x04003071 RID: 12401
	public bool needMaterialInstance;

	// Token: 0x04003072 RID: 12402
	public bool isPacked;

	// Token: 0x04003073 RID: 12403
	public bool premultipliedAlpha;

	// Token: 0x04003074 RID: 12404
	public tk2dSpriteCollectionData spriteCollection;

	// Token: 0x04003075 RID: 12405
	public Dictionary<int, tk2dFontChar> charDict;

	// Token: 0x04003076 RID: 12406
	public bool useDictionary;

	// Token: 0x04003077 RID: 12407
	public tk2dFontKerning[] kerning;

	// Token: 0x04003078 RID: 12408
	public float largestWidth;

	// Token: 0x04003079 RID: 12409
	public Material material;

	// Token: 0x0400307A RID: 12410
	[NonSerialized]
	public Material materialInst;

	// Token: 0x0400307B RID: 12411
	public Texture2D gradientTexture;

	// Token: 0x0400307C RID: 12412
	public bool textureGradients;

	// Token: 0x0400307D RID: 12413
	public int gradientCount = 1;

	// Token: 0x0400307E RID: 12414
	public Vector2 texelSize;

	// Token: 0x0400307F RID: 12415
	[HideInInspector]
	public float invOrthoSize = 1f;

	// Token: 0x04003080 RID: 12416
	[HideInInspector]
	public float halfTargetHeight = 1f;
}
