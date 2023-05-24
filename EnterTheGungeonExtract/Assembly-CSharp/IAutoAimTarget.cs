using System;
using UnityEngine;

// Token: 0x02000D07 RID: 3335
public interface IAutoAimTarget
{
	// Token: 0x17000A4A RID: 2634
	// (get) Token: 0x06004658 RID: 18008
	bool IsValid { get; }

	// Token: 0x17000A4B RID: 2635
	// (get) Token: 0x06004659 RID: 18009
	Vector2 AimCenter { get; }

	// Token: 0x17000A4C RID: 2636
	// (get) Token: 0x0600465A RID: 18010
	Vector2 Velocity { get; }

	// Token: 0x17000A4D RID: 2637
	// (get) Token: 0x0600465B RID: 18011
	bool IgnoreForSuperDuperAutoAim { get; }

	// Token: 0x17000A4E RID: 2638
	// (get) Token: 0x0600465C RID: 18012
	float MinDistForSuperDuperAutoAim { get; }
}
