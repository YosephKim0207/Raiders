using System;
using UnityEngine;

namespace DaikonForge.Tween.Components
{
	// Token: 0x020004E9 RID: 1257
	public interface ITweenPropertyBase
	{
		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001E14 RID: 7700
		// (set) Token: 0x06001E15 RID: 7701
		GameObject Target { get; set; }

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001E16 RID: 7702
		// (set) Token: 0x06001E17 RID: 7703
		string ComponentType { get; set; }

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001E18 RID: 7704
		// (set) Token: 0x06001E19 RID: 7705
		string MemberName { get; set; }
	}
}
