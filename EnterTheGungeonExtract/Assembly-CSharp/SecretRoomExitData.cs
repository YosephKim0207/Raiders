using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000F67 RID: 3943
public class SecretRoomExitData
{
	// Token: 0x060054F5 RID: 21749 RVA: 0x0020359C File Offset: 0x0020179C
	public SecretRoomExitData(GameObject g, DungeonData.Direction d)
	{
		this.colliderObject = g;
		this.exitDirection = d;
	}

	// Token: 0x04004DF8 RID: 19960
	public GameObject colliderObject;

	// Token: 0x04004DF9 RID: 19961
	public DungeonData.Direction exitDirection;
}
