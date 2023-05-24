using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000D17 RID: 3351
public class CompanionRadialBurstBehavior : AttackBehaviorBase
{
	// Token: 0x060046B6 RID: 18102 RVA: 0x00170840 File Offset: 0x0016EA40
	public override BehaviorResult Update()
	{
		base.Update();
		base.DecrementTimer(ref this.m_cooldownTimer, false);
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		if (this.m_cooldownTimer > 0f)
		{
			return BehaviorResult.Continue;
		}
		RoomHandler currentRoom = this.m_aiActor.CompanionOwner.CurrentRoom;
		List<AIActor> activeEnemies = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear);
		if (activeEnemies == null)
		{
			return BehaviorResult.Continue;
		}
		bool flag = false;
		float num = this.DetectRadius * this.DetectRadius;
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			if ((activeEnemies[i].CenterPosition - this.m_aiActor.CenterPosition).sqrMagnitude < num)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.m_aiActor.StartCoroutine(this.DoRadialBurst());
			this.m_cooldownTimer = this.Cooldown;
			return BehaviorResult.SkipRemainingClassBehaviors;
		}
		return BehaviorResult.Continue;
	}

	// Token: 0x060046B7 RID: 18103 RVA: 0x0017092C File Offset: 0x0016EB2C
	private IEnumerator DoRadialBurst()
	{
		if (!string.IsNullOrEmpty(this.BurstAnimation))
		{
			this.m_aiAnimator.PlayUntilFinished(this.BurstAnimation, false, null, -1f, false);
		}
		yield return new WaitForSeconds(this.AnimationDelay);
		if (this.IgnitesEnemies)
		{
			Exploder.DoRadialIgnite(this.IgnitionEffect, this.m_aiActor.CenterPosition, this.WaveRadius, null);
		}
		if (this.m_aiActor.CompanionOwner)
		{
			PlayerController companionOwner = this.m_aiActor.CompanionOwner;
			Vector2? vector = new Vector2?(this.m_aiActor.CenterPosition);
			companionOwner.ForceBlank(25f, 0.5f, false, true, vector, true, -1f);
		}
		yield break;
	}

	// Token: 0x060046B8 RID: 18104 RVA: 0x00170948 File Offset: 0x0016EB48
	public override float GetMaxRange()
	{
		return this.DetectRadius;
	}

	// Token: 0x060046B9 RID: 18105 RVA: 0x00170950 File Offset: 0x0016EB50
	public override float GetMinReadyRange()
	{
		return 0f;
	}

	// Token: 0x060046BA RID: 18106 RVA: 0x00170958 File Offset: 0x0016EB58
	public override bool IsReady()
	{
		return this.m_cooldownTimer <= 0f;
	}

	// Token: 0x0400396F RID: 14703
	[CheckAnimation(null)]
	public string BurstAnimation;

	// Token: 0x04003970 RID: 14704
	public float AnimationDelay = 0.125f;

	// Token: 0x04003971 RID: 14705
	public float DetectRadius = 8f;

	// Token: 0x04003972 RID: 14706
	public float WaveRadius = 15f;

	// Token: 0x04003973 RID: 14707
	public float Cooldown = 15f;

	// Token: 0x04003974 RID: 14708
	public bool IgnitesEnemies;

	// Token: 0x04003975 RID: 14709
	public GameActorFireEffect IgnitionEffect;

	// Token: 0x04003976 RID: 14710
	private float m_cooldownTimer;
}
