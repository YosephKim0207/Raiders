using System;
using UnityEngine;

// Token: 0x02000B94 RID: 2964
[Serializable]
public class tk2dTextMeshData
{
	// Token: 0x0400308C RID: 12428
	public int version;

	// Token: 0x0400308D RID: 12429
	public tk2dFontData font;

	// Token: 0x0400308E RID: 12430
	public string text = string.Empty;

	// Token: 0x0400308F RID: 12431
	public Color color = Color.white;

	// Token: 0x04003090 RID: 12432
	public Color color2 = Color.white;

	// Token: 0x04003091 RID: 12433
	public bool useGradient;

	// Token: 0x04003092 RID: 12434
	public int textureGradient;

	// Token: 0x04003093 RID: 12435
	public TextAnchor anchor = TextAnchor.LowerLeft;

	// Token: 0x04003094 RID: 12436
	public int renderLayer;

	// Token: 0x04003095 RID: 12437
	public Vector3 scale = Vector3.one;

	// Token: 0x04003096 RID: 12438
	public bool kerning;

	// Token: 0x04003097 RID: 12439
	public int maxChars = 16;

	// Token: 0x04003098 RID: 12440
	public bool inlineStyling;

	// Token: 0x04003099 RID: 12441
	public bool formatting;

	// Token: 0x0400309A RID: 12442
	public int wordWrapWidth;

	// Token: 0x0400309B RID: 12443
	public float spacing;

	// Token: 0x0400309C RID: 12444
	public float lineSpacing;
}
