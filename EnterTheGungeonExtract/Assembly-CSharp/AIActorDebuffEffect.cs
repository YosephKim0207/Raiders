using System;

// Token: 0x02000E17 RID: 3607
[Serializable]
public class AIActorDebuffEffect : GameActorEffect
{
	// Token: 0x06004C6B RID: 19563 RVA: 0x001A0A0C File Offset: 0x0019EC0C
	public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1f)
	{
		HealthHaver healthHaver = actor.healthHaver;
		float num = actor.healthHaver.GetMaxHealth() * this.HealthMultiplier;
		bool keepHealthPercentage = this.KeepHealthPercentage;
		healthHaver.SetHealthMaximum(num, null, keepHealthPercentage);
		if (this.SpeedMultiplier != 1f)
		{
			SpeculativeRigidbody specRigidbody = actor.specRigidbody;
			specRigidbody.OnPreMovement = (Action<SpeculativeRigidbody>)Delegate.Combine(specRigidbody.OnPreMovement, new Action<SpeculativeRigidbody>(this.ModifyVelocity));
		}
		if (this.CooldownMultiplier != 1f && actor.behaviorSpeculator)
		{
			actor.behaviorSpeculator.CooldownScale /= this.CooldownMultiplier;
		}
	}

	// Token: 0x06004C6C RID: 19564 RVA: 0x001A0AB8 File Offset: 0x0019ECB8
	public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
	{
		HealthHaver healthHaver = actor.healthHaver;
		float num = actor.healthHaver.GetMaxHealth() / this.HealthMultiplier;
		bool keepHealthPercentage = this.KeepHealthPercentage;
		healthHaver.SetHealthMaximum(num, null, keepHealthPercentage);
		if (this.SpeedMultiplier != 1f)
		{
			SpeculativeRigidbody specRigidbody = actor.specRigidbody;
			specRigidbody.OnPreMovement = (Action<SpeculativeRigidbody>)Delegate.Remove(specRigidbody.OnPreMovement, new Action<SpeculativeRigidbody>(this.ModifyVelocity));
		}
		if (this.CooldownMultiplier != 1f && actor.behaviorSpeculator)
		{
			actor.behaviorSpeculator.CooldownScale *= this.CooldownMultiplier;
		}
	}

	// Token: 0x06004C6D RID: 19565 RVA: 0x001A0B64 File Offset: 0x0019ED64
	public void ModifyVelocity(SpeculativeRigidbody myRigidbody)
	{
		myRigidbody.Velocity *= this.SpeedMultiplier;
	}

	// Token: 0x04004249 RID: 16969
	public float SpeedMultiplier = 1f;

	// Token: 0x0400424A RID: 16970
	public float CooldownMultiplier = 1f;

	// Token: 0x0400424B RID: 16971
	public float HealthMultiplier = 1f;

	// Token: 0x0400424C RID: 16972
	public bool KeepHealthPercentage = true;
}
