using System;
using UnityEngine;

// Token: 0x02000FC0 RID: 4032
public class BashelliskBodyPickupController : BraveBehaviour
{
	// Token: 0x060057D6 RID: 22486 RVA: 0x00218504 File Offset: 0x00216704
	public void Awake()
	{
		base.aiActor.PreventBlackPhantom = true;
	}

	// Token: 0x060057D7 RID: 22487 RVA: 0x00218514 File Offset: 0x00216714
	public void Update()
	{
		ShootBehavior shootBehavior = base.aiActor.behaviorSpeculator.AttackBehaviors[0] as ShootBehavior;
		if (base.aiActor.CanTargetEnemies)
		{
			shootBehavior.Cooldown = 0.15f;
			shootBehavior.BulletName = "fast";
		}
		else
		{
			shootBehavior.Cooldown = 1.5f;
			shootBehavior.BulletName = "default";
		}
	}

	// Token: 0x060057D8 RID: 22488 RVA: 0x00218580 File Offset: 0x00216780
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040050D3 RID: 20691
	public Transform center;

	// Token: 0x040050D4 RID: 20692
	public AIActorBuffEffect buffEffect;
}
