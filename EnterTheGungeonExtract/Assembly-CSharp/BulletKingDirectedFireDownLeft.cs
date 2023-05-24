using System;
using System.Collections;
using FullInspector;

// Token: 0x020000F0 RID: 240
[InspectorDropdownName("Bosses/BulletKing/DirectedFireDownLeft")]
public class BulletKingDirectedFireDownLeft : BulletKingDirectedFire
{
	// Token: 0x06000397 RID: 919 RVA: 0x00011DD8 File Offset: 0x0000FFD8
	protected override IEnumerator Top()
	{
		yield return base.Wait(10);
		base.DirectedShots(-1.3125f, -0.4375f, -24f);
		yield return base.Wait((!base.IsHard) ? 6 : 4);
		base.DirectedShots(-1.5f, -0.1875f, -34f);
		yield return base.Wait((!base.IsHard) ? 6 : 4);
		base.DirectedShots(-1.75f, 0.25f, -44f);
		yield return base.Wait((!base.IsHard) ? 12 : 8);
		base.DirectedShots(-1.3125f, -0.4375f, -28f);
		yield return base.Wait((!base.IsHard) ? 6 : 4);
		base.DirectedShots(-1.5f, -0.1875f, -34f);
		yield return base.Wait((!base.IsHard) ? 6 : 4);
		base.DirectedShots(-1.75f, 0.25f, -42f);
		yield break;
	}
}
