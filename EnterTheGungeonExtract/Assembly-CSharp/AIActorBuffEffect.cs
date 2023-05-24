using System;

// Token: 0x02000E16 RID: 3606
[Serializable]
public class AIActorBuffEffect : GameActorEffect
{
	// Token: 0x06004C67 RID: 19559 RVA: 0x001A0868 File Offset: 0x0019EA68
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

	// Token: 0x06004C68 RID: 19560 RVA: 0x001A0914 File Offset: 0x0019EB14
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

	// Token: 0x06004C69 RID: 19561 RVA: 0x001A09C0 File Offset: 0x0019EBC0
	public void ModifyVelocity(SpeculativeRigidbody myRigidbody)
	{
		myRigidbody.Velocity *= this.SpeedMultiplier;
	}

	// Token: 0x04004245 RID: 16965
	public float SpeedMultiplier = 1f;

	// Token: 0x04004246 RID: 16966
	public float CooldownMultiplier = 1f;

	// Token: 0x04004247 RID: 16967
	public float HealthMultiplier = 1f;

	// Token: 0x04004248 RID: 16968
	public bool KeepHealthPercentage = true;
}
