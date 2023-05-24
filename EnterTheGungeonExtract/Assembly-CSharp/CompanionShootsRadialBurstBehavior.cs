using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000D19 RID: 3353
public class CompanionShootsRadialBurstBehavior : AttackBehaviorBase
{
	// Token: 0x060046C2 RID: 18114 RVA: 0x00170B04 File Offset: 0x0016ED04
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

	// Token: 0x060046C3 RID: 18115 RVA: 0x00170BF0 File Offset: 0x0016EDF0
	private IEnumerator DoRadialBurst()
	{
		if (!string.IsNullOrEmpty(this.BurstAudioEvent))
		{
			AkSoundEngine.PostEvent(this.BurstAudioEvent, this.m_aiActor.gameObject);
		}
		if (!string.IsNullOrEmpty(this.BurstAnimation))
		{
			this.m_aiAnimator.PlayUntilFinished(this.BurstAnimation, false, null, -1f, false);
		}
		yield return new WaitForSeconds(this.AnimationDelay);
		this.Burst.DoBurst(this.m_aiActor.CompanionOwner, new Vector2?(this.m_aiActor.CenterPosition), null);
		yield break;
	}

	// Token: 0x060046C4 RID: 18116 RVA: 0x00170C0C File Offset: 0x0016EE0C
	public override float GetMaxRange()
	{
		return this.DetectRadius;
	}

	// Token: 0x060046C5 RID: 18117 RVA: 0x00170C14 File Offset: 0x0016EE14
	public override float GetMinReadyRange()
	{
		return 0f;
	}

	// Token: 0x060046C6 RID: 18118 RVA: 0x00170C1C File Offset: 0x0016EE1C
	public override bool IsReady()
	{
		return this.m_cooldownTimer <= 0f;
	}

	// Token: 0x0400397B RID: 14715
	[CheckAnimation(null)]
	public string BurstAnimation;

	// Token: 0x0400397C RID: 14716
	public float AnimationDelay = 0.125f;

	// Token: 0x0400397D RID: 14717
	public float DetectRadius = 8f;

	// Token: 0x0400397E RID: 14718
	public float WaveRadius = 15f;

	// Token: 0x0400397F RID: 14719
	public float Cooldown = 15f;

	// Token: 0x04003980 RID: 14720
	public RadialBurstInterface Burst;

	// Token: 0x04003981 RID: 14721
	public string BurstAudioEvent;

	// Token: 0x04003982 RID: 14722
	private float m_cooldownTimer;
}
