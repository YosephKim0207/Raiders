using System;
using FullInspector;
using UnityEngine;

// Token: 0x02000D9E RID: 3486
[InspectorDropdownName("Bosses/BossStatues/StompBehavior")]
public class BossStatuesStompBehavior : BossStatuesPatternBehavior
{
	// Token: 0x060049D1 RID: 18897 RVA: 0x0018AAD4 File Offset: 0x00188CD4
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		for (int i = 0; i < this.m_activeStatueCount; i++)
		{
			BossStatueController bossStatueController = this.m_activeStatues[i];
			if (bossStatueController && bossStatueController.healthHaver.IsAlive)
			{
				bossStatueController.IsStomping = false;
				bossStatueController.HangTime = 0f;
				bossStatueController.State = BossStatueController.StatueState.StandStill;
			}
		}
	}

	// Token: 0x060049D2 RID: 18898 RVA: 0x0018AB40 File Offset: 0x00188D40
	protected override void InitPositions()
	{
		for (int i = 0; i < this.m_activeStatueCount; i++)
		{
			BossStatueController bossStatueController = this.m_activeStatues[i];
			if (bossStatueController && bossStatueController.healthHaver.IsAlive)
			{
				PlayerController playerClosestToPoint = GameManager.Instance.GetPlayerClosestToPoint(bossStatueController.GroundPosition);
				if (playerClosestToPoint)
				{
					bossStatueController.Target = new Vector2?(playerClosestToPoint.specRigidbody.UnitCenter);
				}
				if (this.attackType == null)
				{
					bossStatueController.QueuedBulletScript.Add(null);
				}
				bossStatueController.IsStomping = true;
				bossStatueController.HangTime = this.HangTime;
			}
		}
		this.m_frameCount = 0;
	}

	// Token: 0x060049D3 RID: 18899 RVA: 0x0018ABF0 File Offset: 0x00188DF0
	protected override void UpdatePositions()
	{
		for (int i = 0; i < this.m_activeStatueCount; i++)
		{
			BossStatueController bossStatueController = this.m_activeStatues[i];
			if (bossStatueController && bossStatueController.healthHaver.IsAlive)
			{
				PlayerController playerClosestToPoint = GameManager.Instance.GetPlayerClosestToPoint(bossStatueController.GroundPosition);
				if (playerClosestToPoint)
				{
					bossStatueController.Target = new Vector2?(playerClosestToPoint.specRigidbody.UnitCenter);
				}
			}
		}
		this.m_frameCount++;
	}

	// Token: 0x060049D4 RID: 18900 RVA: 0x0018AC7C File Offset: 0x00188E7C
	protected override bool IsFinished()
	{
		if (this.m_frameCount < 3)
		{
			return false;
		}
		for (int i = 0; i < this.m_activeStatueCount; i++)
		{
			if (!this.m_activeStatues[i].IsGrounded)
			{
				return false;
			}
		}
		AkSoundEngine.PostEvent("Play_ENM_kali_shockwave_01", this.m_statuesController.bulletBank.gameObject);
		return true;
	}

	// Token: 0x060049D5 RID: 18901 RVA: 0x0018ACE4 File Offset: 0x00188EE4
	protected override void BeginState(BossStatuesPatternBehavior.PatternState state)
	{
		base.BeginState(state);
		if (state == BossStatuesPatternBehavior.PatternState.InProgress)
		{
			base.SetActiveState(BossStatueController.StatueState.WaitForAttack);
		}
	}

	// Token: 0x04003E37 RID: 15927
	public float HangTime = 1f;

	// Token: 0x04003E38 RID: 15928
	private int m_frameCount;
}
