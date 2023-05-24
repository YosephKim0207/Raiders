using System;
using UnityEngine;

// Token: 0x02000C1A RID: 3098
[Serializable]
public class tk2dUILayoutItem
{
	// Token: 0x0600429A RID: 17050 RVA: 0x0015835C File Offset: 0x0015655C
	public static tk2dUILayoutItem FixedSizeLayoutItem()
	{
		return new tk2dUILayoutItem
		{
			fixedSize = true
		};
	}

	// Token: 0x040034DC RID: 13532
	public tk2dBaseSprite sprite;

	// Token: 0x040034DD RID: 13533
	public tk2dUIMask UIMask;

	// Token: 0x040034DE RID: 13534
	public tk2dUILayout layout;

	// Token: 0x040034DF RID: 13535
	public GameObject gameObj;

	// Token: 0x040034E0 RID: 13536
	public bool snapLeft;

	// Token: 0x040034E1 RID: 13537
	public bool snapRight;

	// Token: 0x040034E2 RID: 13538
	public bool snapTop;

	// Token: 0x040034E3 RID: 13539
	public bool snapBottom;

	// Token: 0x040034E4 RID: 13540
	public bool fixedSize;

	// Token: 0x040034E5 RID: 13541
	public float fillPercentage = -1f;

	// Token: 0x040034E6 RID: 13542
	public float sizeProportion = 1f;

	// Token: 0x040034E7 RID: 13543
	public bool inLayoutList;

	// Token: 0x040034E8 RID: 13544
	public int childDepth;

	// Token: 0x040034E9 RID: 13545
	public Vector3 oldPos = Vector3.zero;
}
