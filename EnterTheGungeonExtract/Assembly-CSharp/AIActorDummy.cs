using System;
using UnityEngine;

// Token: 0x02000F90 RID: 3984
public class AIActorDummy : AIActor
{
	// Token: 0x17000C3A RID: 3130
	// (get) Token: 0x0600564F RID: 22095 RVA: 0x0020E4CC File Offset: 0x0020C6CC
	public override bool InBossAmmonomiconTab
	{
		get
		{
			return this.isInBossTab;
		}
	}

	// Token: 0x04004F43 RID: 20291
	public bool isInBossTab;

	// Token: 0x04004F44 RID: 20292
	public GameObject realPrefab;
}
