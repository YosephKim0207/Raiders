using System;
using System.Collections;
using FullInspector;
using UnityEngine;

// Token: 0x02000238 RID: 568
[InspectorDropdownName("ManfredsRival/ShieldSlam2")]
public class ManfredsRivalShieldSlam2 : ManfredsRivalShieldSlam1
{
	// Token: 0x0600088C RID: 2188 RVA: 0x000294D4 File Offset: 0x000276D4
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		base.FireExpandingLine(new Vector2(-0.6f, -1f), new Vector2(0.6f, -1f), 10);
		base.FireExpandingLine(new Vector2(-0.7f, -1f), new Vector2(-0.8f, -0.9f), 3);
		base.FireExpandingLine(new Vector2(0.7f, -1f), new Vector2(0.8f, -0.9f), 3);
		base.FireExpandingLine(new Vector2(-0.8f, -0.9f), new Vector2(-0.8f, 0.2f), 12);
		base.FireExpandingLine(new Vector2(0.8f, -0.9f), new Vector2(0.8f, 0.2f), 12);
		base.FireExpandingLine(new Vector2(-0.8f, 0.2f), new Vector2(-0.15f, 1f), 10);
		base.FireExpandingLine(new Vector2(0.8f, 0.2f), new Vector2(0.15f, 1f), 10);
		base.FireExpandingLine(new Vector2(-0.15f, 1f), new Vector2(0.15f, 1f), 5);
		base.FireSpinningLine(new Vector2(0f, -1.5f), new Vector2(0f, 1.5f), 4);
		base.FireSpinningLine(new Vector2(-0.6f, -0.4f), new Vector2(0.6f, -0.4f), 2);
		yield return base.Wait(40);
		base.FireSpinningLine(new Vector2(0f, -1.5f), new Vector2(0f, 1.5f), 4);
		base.FireSpinningLine(new Vector2(-0.6f, -0.4f), new Vector2(0.6f, -0.4f), 2);
		yield return base.Wait(20);
		yield break;
	}
}
