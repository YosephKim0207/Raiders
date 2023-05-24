using System;
using UnityEngine;

// Token: 0x02001213 RID: 4627
public class SimpleSpritePositioner : DungeonPlaceableBehaviour
{
	// Token: 0x06006781 RID: 26497 RVA: 0x002880D4 File Offset: 0x002862D4
	public void Start()
	{
		base.transform.localRotation = Quaternion.Euler(0f, 0f, this.Rotation);
		if (base.sprite)
		{
			base.sprite.UpdateZDepth();
			base.sprite.ForceRotationRebuild();
		}
	}

	// Token: 0x04006362 RID: 25442
	public float Rotation;
}
