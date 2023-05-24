using System;
using System.Collections;
using FullInspector;

// Token: 0x020000EA RID: 234
[InspectorDropdownName("Bosses/BulletKing/DirectedFireLeft")]
public class BulletKingDirectedFireLeft : BulletKingDirectedFire
{
	// Token: 0x06000385 RID: 901 RVA: 0x00011868 File Offset: 0x0000FA68
	protected override IEnumerator Top()
	{
		yield return base.Wait(10);
		base.DirectedShots(-2.0625f, 2.375f, -98f);
		yield return base.Wait((!base.IsHard) ? 6 : 4);
		base.DirectedShots(-2.125f, 1.3125f, -90f);
		yield return base.Wait((!base.IsHard) ? 6 : 4);
		base.DirectedShots(-1.75f, 0.25f, -82f);
		yield return base.Wait((!base.IsHard) ? 12 : 8);
		base.DirectedShots(-2.0625f, 2.375f, -94f);
		yield return base.Wait((!base.IsHard) ? 6 : 4);
		base.DirectedShots(-2.125f, 1.3125f, -90f);
		yield return base.Wait((!base.IsHard) ? 6 : 4);
		base.DirectedShots(-1.75f, 0.25f, -86f);
		yield break;
	}
}
