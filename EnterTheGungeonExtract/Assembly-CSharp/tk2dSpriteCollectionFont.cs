using System;
using UnityEngine;

// Token: 0x02000BCA RID: 3018
[Serializable]
public class tk2dSpriteCollectionFont
{
	// Token: 0x06003FF3 RID: 16371 RVA: 0x00144EE4 File Offset: 0x001430E4
	public void CopyFrom(tk2dSpriteCollectionFont src)
	{
		this.active = src.active;
		this.bmFont = src.bmFont;
		this.texture = src.texture;
		this.dupeCaps = src.dupeCaps;
		this.flipTextureY = src.flipTextureY;
		this.charPadX = src.charPadX;
		this.data = src.data;
		this.editorData = src.editorData;
		this.materialId = src.materialId;
		this.gradientCount = src.gradientCount;
		this.gradientTexture = src.gradientTexture;
		this.useGradient = src.useGradient;
	}

	// Token: 0x170009AF RID: 2479
	// (get) Token: 0x06003FF4 RID: 16372 RVA: 0x00144F84 File Offset: 0x00143184
	public string Name
	{
		get
		{
			if (this.bmFont == null || this.texture == null)
			{
				return "Empty";
			}
			if (this.data == null)
			{
				return this.bmFont.name + " (Inactive)";
			}
			return this.bmFont.name;
		}
	}

	// Token: 0x170009B0 RID: 2480
	// (get) Token: 0x06003FF5 RID: 16373 RVA: 0x00144FEC File Offset: 0x001431EC
	public bool InUse
	{
		get
		{
			return this.active && this.bmFont != null && this.texture != null && this.data != null && this.editorData != null;
		}
	}

	// Token: 0x0400328D RID: 12941
	public bool active;

	// Token: 0x0400328E RID: 12942
	public TextAsset bmFont;

	// Token: 0x0400328F RID: 12943
	public Texture2D texture;

	// Token: 0x04003290 RID: 12944
	public bool dupeCaps;

	// Token: 0x04003291 RID: 12945
	public bool flipTextureY;

	// Token: 0x04003292 RID: 12946
	public int charPadX;

	// Token: 0x04003293 RID: 12947
	public tk2dFontData data;

	// Token: 0x04003294 RID: 12948
	public tk2dFont editorData;

	// Token: 0x04003295 RID: 12949
	public int materialId;

	// Token: 0x04003296 RID: 12950
	public bool useGradient;

	// Token: 0x04003297 RID: 12951
	public Texture2D gradientTexture;

	// Token: 0x04003298 RID: 12952
	public int gradientCount = 1;
}
