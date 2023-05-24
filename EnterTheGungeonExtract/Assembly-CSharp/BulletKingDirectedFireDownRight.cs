using System;
using System.Collections;
using FullInspector;

// Token: 0x020000F3 RID: 243
[InspectorDropdownName("Bosses/BulletKing/DirectedFireDownRight")]
public class BulletKingDirectedFireDownRight : BulletKingDirectedFire
{
	// Token: 0x060003A0 RID: 928 RVA: 0x00012090 File Offset: 0x00010290
	protected override IEnumerator Top()
	{
		yield return base.Wait(10);
		base.DirectedShots(1.875f, 0.25f, 44f);
		yield return base.Wait((!base.IsHard) ? 6 : 4);
		base.DirectedShots(1.625f, -0.1875f, 34f);
		yield return base.Wait((!base.IsHard) ? 6 : 4);
		base.DirectedShots(1.4275f, -0.4375f, 24f);
		yield return base.Wait((!base.IsHard) ? 12 : 8);
		base.DirectedShots(1.875f, 0.25f, 42f);
		yield return base.Wait((!base.IsHard) ? 6 : 4);
		base.DirectedShots(1.625f, -0.1875f, 34f);
		yield return base.Wait((!base.IsHard) ? 6 : 4);
		base.DirectedShots(1.4275f, -0.4375f, 28f);
		yield break;
	}
}
