using System;
using UnityEngine;

// Token: 0x02000D78 RID: 3448
[Serializable]
public abstract class BehaviorBase
{
	// Token: 0x060048E1 RID: 18657 RVA: 0x00184C14 File Offset: 0x00182E14
	public virtual void Start()
	{
	}

	// Token: 0x060048E2 RID: 18658 RVA: 0x00184C18 File Offset: 0x00182E18
	public virtual void Upkeep()
	{
	}

	// Token: 0x060048E3 RID: 18659 RVA: 0x00184C1C File Offset: 0x00182E1C
	public virtual bool OverrideOtherBehaviors()
	{
		return false;
	}

	// Token: 0x060048E4 RID: 18660 RVA: 0x00184C20 File Offset: 0x00182E20
	public virtual BehaviorResult Update()
	{
		return BehaviorResult.Continue;
	}

	// Token: 0x060048E5 RID: 18661 RVA: 0x00184C24 File Offset: 0x00182E24
	public virtual ContinuousBehaviorResult ContinuousUpdate()
	{
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x060048E6 RID: 18662 RVA: 0x00184C28 File Offset: 0x00182E28
	public virtual void EndContinuousUpdate()
	{
	}

	// Token: 0x060048E7 RID: 18663 RVA: 0x00184C2C File Offset: 0x00182E2C
	public virtual void OnActorPreDeath()
	{
	}

	// Token: 0x060048E8 RID: 18664 RVA: 0x00184C30 File Offset: 0x00182E30
	public virtual void Destroy()
	{
	}

	// Token: 0x060048E9 RID: 18665 RVA: 0x00184C34 File Offset: 0x00182E34
	public virtual void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter)
	{
		this.m_gameObject = gameObject;
		this.m_aiActor = aiActor;
		this.m_aiShooter = aiShooter;
		this.m_aiAnimator = gameObject.GetComponent<AIAnimator>();
	}

	// Token: 0x060048EA RID: 18666 RVA: 0x00184C58 File Offset: 0x00182E58
	public virtual void SetDeltaTime(float deltaTime)
	{
		this.m_deltaTime = deltaTime;
	}

	// Token: 0x060048EB RID: 18667 RVA: 0x00184C64 File Offset: 0x00182E64
	public virtual bool UpdateEveryFrame()
	{
		return this.m_updateEveryFrame;
	}

	// Token: 0x060048EC RID: 18668 RVA: 0x00184C6C File Offset: 0x00182E6C
	public virtual bool IgnoreGlobalCooldown()
	{
		return this.m_ignoreGlobalCooldown;
	}

	// Token: 0x060048ED RID: 18669 RVA: 0x00184C74 File Offset: 0x00182E74
	public virtual bool IsOverridable()
	{
		return true;
	}

	// Token: 0x060048EE RID: 18670 RVA: 0x00184C78 File Offset: 0x00182E78
	protected void DecrementTimer(ref float timer, bool useCooldownFactor = false)
	{
		float num = this.m_deltaTime;
		if (this.m_aiActor && useCooldownFactor)
		{
			num *= this.m_aiActor.behaviorSpeculator.CooldownScale;
		}
		timer = Mathf.Max(timer - num, 0f);
	}

	// Token: 0x04003D0D RID: 15629
	protected GameObject m_gameObject;

	// Token: 0x04003D0E RID: 15630
	protected AIActor m_aiActor;

	// Token: 0x04003D0F RID: 15631
	protected AIShooter m_aiShooter;

	// Token: 0x04003D10 RID: 15632
	protected AIAnimator m_aiAnimator;

	// Token: 0x04003D11 RID: 15633
	protected float m_deltaTime;

	// Token: 0x04003D12 RID: 15634
	protected bool m_updateEveryFrame;

	// Token: 0x04003D13 RID: 15635
	protected bool m_ignoreGlobalCooldown;
}
