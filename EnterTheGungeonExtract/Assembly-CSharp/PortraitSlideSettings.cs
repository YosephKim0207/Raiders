using System;
using UnityEngine;

// Token: 0x0200102A RID: 4138
[Serializable]
public class PortraitSlideSettings
{
	// Token: 0x04005424 RID: 21540
	[StringTableString("enemies")]
	public string bossNameString;

	// Token: 0x04005425 RID: 21541
	[StringTableString("enemies")]
	public string bossSubtitleString;

	// Token: 0x04005426 RID: 21542
	[StringTableString("enemies")]
	public string bossQuoteString;

	// Token: 0x04005427 RID: 21543
	public Texture bossArtSprite;

	// Token: 0x04005428 RID: 21544
	public IntVector2 bossSpritePxOffset;

	// Token: 0x04005429 RID: 21545
	public IntVector2 topLeftTextPxOffset;

	// Token: 0x0400542A RID: 21546
	public IntVector2 bottomRightTextPxOffset;

	// Token: 0x0400542B RID: 21547
	public Color bgColor = Color.blue;
}
