using System;
using UnityEngine;

// Token: 0x02000BC6 RID: 3014
[Serializable]
public class tk2dSpriteCollectionDefault
{
	// Token: 0x04003269 RID: 12905
	public bool additive;

	// Token: 0x0400326A RID: 12906
	public Vector3 scale = new Vector3(1f, 1f, 1f);

	// Token: 0x0400326B RID: 12907
	public tk2dSpriteCollectionDefinition.Anchor anchor = tk2dSpriteCollectionDefinition.Anchor.MiddleCenter;

	// Token: 0x0400326C RID: 12908
	public tk2dSpriteCollectionDefinition.Pad pad;

	// Token: 0x0400326D RID: 12909
	public tk2dSpriteCollectionDefinition.ColliderType colliderType;
}
