using System;

// Token: 0x02000E2A RID: 3626
[Serializable]
public class GameActorSpeedEffect : GameActorEffect
{
	// Token: 0x06004CB6 RID: 19638 RVA: 0x001A382C File Offset: 0x001A1A2C
	public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1f)
	{
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

	// Token: 0x06004CB7 RID: 19639 RVA: 0x001A38A8 File Offset: 0x001A1AA8
	public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
	{
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

	// Token: 0x06004CB8 RID: 19640 RVA: 0x001A3924 File Offset: 0x001A1B24
	public void ModifyVelocity(SpeculativeRigidbody myRigidbody)
	{
		if (this.OnlyAffectPlayerWhenGrounded)
		{
			PlayerController playerController = myRigidbody.gameActor as PlayerController;
			if (playerController && (!playerController.IsGrounded || playerController.IsSlidingOverSurface))
			{
				return;
			}
		}
		myRigidbody.Velocity *= this.SpeedMultiplier;
	}

	// Token: 0x040042C3 RID: 17091
	public float SpeedMultiplier = 1f;

	// Token: 0x040042C4 RID: 17092
	public float CooldownMultiplier = 1f;

	// Token: 0x040042C5 RID: 17093
	public bool OnlyAffectPlayerWhenGrounded;
}
