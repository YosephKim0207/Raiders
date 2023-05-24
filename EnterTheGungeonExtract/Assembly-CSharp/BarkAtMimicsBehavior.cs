using System;
using UnityEngine;

// Token: 0x02000DE3 RID: 3555
public class BarkAtMimicsBehavior : MovementBehaviorBase
{
	// Token: 0x06004B49 RID: 19273 RVA: 0x001978AC File Offset: 0x00195AAC
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_repathTimer, false);
	}

	// Token: 0x06004B4A RID: 19274 RVA: 0x001978C4 File Offset: 0x00195AC4
	public override BehaviorResult Update()
	{
		PlayerController primaryPlayer = GameManager.Instance.PrimaryPlayer;
		if (primaryPlayer == null)
		{
			return BehaviorResult.Continue;
		}
		if (primaryPlayer.CurrentRoom == null)
		{
			return BehaviorResult.Continue;
		}
		if (primaryPlayer.CurrentRoom.IsSealed && this.DisableInCombat)
		{
			this.m_aiAnimator.EndAnimationIf(this.BarkAnimation);
			return BehaviorResult.Continue;
		}
		Chest chest = null;
		for (int i = 0; i < StaticReferenceManager.AllChests.Count; i++)
		{
			if (StaticReferenceManager.AllChests[i] && !StaticReferenceManager.AllChests[i].IsOpen && StaticReferenceManager.AllChests[i].IsMimic && StaticReferenceManager.AllChests[i].GetAbsoluteParentRoom() == primaryPlayer.CurrentRoom)
			{
				chest = StaticReferenceManager.AllChests[i];
				break;
			}
		}
		if (chest == null || chest.specRigidbody == null)
		{
			this.m_aiAnimator.EndAnimationIf(this.BarkAnimation);
			return BehaviorResult.Continue;
		}
		this.m_aiAnimator.EndAnimationIf("pet");
		float num = Vector2.Distance(chest.specRigidbody.UnitCenter, this.m_aiActor.CenterPosition);
		if (num <= this.IdealRadius)
		{
			this.m_aiActor.ClearPath();
			if (!this.m_aiAnimator.IsPlaying(this.BarkAnimation))
			{
				this.m_aiAnimator.PlayUntilCancelled(this.BarkAnimation, false, null, -1f, false);
			}
			return BehaviorResult.SkipRemainingClassBehaviors;
		}
		if (this.m_repathTimer <= 0f)
		{
			this.m_repathTimer = this.PathInterval;
			this.m_aiActor.PathfindToPosition(chest.specRigidbody.UnitCenter, null, true, null, null, null, false);
		}
		return BehaviorResult.SkipRemainingClassBehaviors;
	}

	// Token: 0x040040B0 RID: 16560
	public float PathInterval = 0.25f;

	// Token: 0x040040B1 RID: 16561
	public bool DisableInCombat = true;

	// Token: 0x040040B2 RID: 16562
	public float IdealRadius = 3f;

	// Token: 0x040040B3 RID: 16563
	public string BarkAnimation = "bark";

	// Token: 0x040040B4 RID: 16564
	private float m_repathTimer;
}
