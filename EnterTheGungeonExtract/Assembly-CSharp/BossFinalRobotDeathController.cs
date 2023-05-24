using System;
using UnityEngine;

// Token: 0x02000FE0 RID: 4064
public class BossFinalRobotDeathController : BraveBehaviour
{
	// Token: 0x060058A3 RID: 22691 RVA: 0x0021E214 File Offset: 0x0021C414
	public void Start()
	{
		base.healthHaver.OnPreDeath += this.OnBossDeath;
		base.healthHaver.OverrideKillCamTime = new float?(1f);
	}

	// Token: 0x060058A4 RID: 22692 RVA: 0x0021E244 File Offset: 0x0021C444
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060058A5 RID: 22693 RVA: 0x0021E24C File Offset: 0x0021C44C
	private void OnBossDeath(Vector2 dir)
	{
		UnityEngine.Object.FindObjectOfType<RobotPastController>().OnBossKilled(base.transform);
	}
}
