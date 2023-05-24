using System;
using System.Collections;
using FullInspector;

// Token: 0x020000ED RID: 237
[InspectorDropdownName("Bosses/BulletKing/DirectedFireRight")]
public class BulletKingDirectedFireRight : BulletKingDirectedFire
{
	// Token: 0x0600038E RID: 910 RVA: 0x00011B20 File Offset: 0x0000FD20
	protected override IEnumerator Top()
	{
		yield return base.Wait(10);
		base.DirectedShots(2.125f, 2.375f, 98f);
		yield return base.Wait((!base.IsHard) ? 6 : 4);
		base.DirectedShots(2.1875f, 1.3125f, 90f);
		yield return base.Wait((!base.IsHard) ? 6 : 4);
		base.DirectedShots(1.875f, 0.25f, 82f);
		yield return base.Wait((!base.IsHard) ? 12 : 8);
		base.DirectedShots(2.125f, 2.375f, 94f);
		yield return base.Wait((!base.IsHard) ? 6 : 4);
		base.DirectedShots(2.1875f, 1.3125f, 90f);
		yield return base.Wait((!base.IsHard) ? 6 : 4);
		base.DirectedShots(1.875f, 0.25f, 86f);
		yield break;
	}
}
