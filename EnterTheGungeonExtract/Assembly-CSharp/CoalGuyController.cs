using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02001082 RID: 4226
public class CoalGuyController : BraveBehaviour
{
	// Token: 0x06005D0A RID: 23818 RVA: 0x0023A27C File Offset: 0x0023847C
	public void Start()
	{
		base.healthHaver.OnDamaged += this.OnDamaged;
		base.healthHaver.OnPreDeath += this.OnPreDeath;
	}

	// Token: 0x06005D0B RID: 23819 RVA: 0x0023A2AC File Offset: 0x002384AC
	protected override void OnDestroy()
	{
		if (base.healthHaver)
		{
			base.healthHaver.OnDamaged -= this.OnDamaged;
			base.healthHaver.OnPreDeath -= this.OnPreDeath;
		}
		base.OnDestroy();
	}

	// Token: 0x06005D0C RID: 23820 RVA: 0x0023A300 File Offset: 0x00238500
	private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		if ((damageTypes & CoreDamageTypes.Water) == CoreDamageTypes.Water)
		{
			return;
		}
		if ((damageTypes & CoreDamageTypes.Ice) == CoreDamageTypes.Ice)
		{
			return;
		}
		this.FlameOn();
		if (base.healthHaver)
		{
			base.healthHaver.OnDamaged -= this.OnDamaged;
		}
	}

	// Token: 0x06005D0D RID: 23821 RVA: 0x0023A350 File Offset: 0x00238550
	private void OnPreDeath(Vector2 obj)
	{
		if (this.eyes)
		{
			this.eyes.gameObject.SetActive(false);
		}
	}

	// Token: 0x06005D0E RID: 23822 RVA: 0x0023A374 File Offset: 0x00238574
	private void FlameOn()
	{
		base.aiActor.ApplyEffect(this.fireEffect, 1f, null);
		base.healthHaver.ApplyDamageModifiers(this.onFireDamageTypeModifiers);
		if (this.overrideMoveSpeed >= 0f)
		{
			base.aiActor.MovementSpeed = TurboModeController.MaybeModifyEnemyMovementSpeed(this.overrideMoveSpeed);
		}
		if (this.overridePauseTime >= 0f)
		{
			for (int i = 0; i < base.behaviorSpeculator.MovementBehaviors.Count; i++)
			{
				if (base.behaviorSpeculator.MovementBehaviors[i] is MoveErraticallyBehavior)
				{
					MoveErraticallyBehavior moveErraticallyBehavior = base.behaviorSpeculator.MovementBehaviors[i] as MoveErraticallyBehavior;
					moveErraticallyBehavior.PointReachedPauseTime = this.overridePauseTime;
					moveErraticallyBehavior.ResetPauseTimer();
					base.aiActor.ClearPath();
				}
			}
		}
		if (!string.IsNullOrEmpty(this.overrideAnimation))
		{
			base.aiAnimator.SetBaseAnim(this.overrideAnimation, false);
			base.aiAnimator.EndAnimation();
		}
		if (this.eyes)
		{
			this.eyes.gameObject.SetActive(true);
			this.eyes.Play(this.eyes.DefaultClip, 0f, this.eyes.DefaultClip.fps, false);
		}
		for (int j = 0; j < base.behaviorSpeculator.AttackBehaviors.Count; j++)
		{
			if (base.behaviorSpeculator.AttackBehaviors[j] is AttackBehaviorGroup)
			{
				this.ProcessAttackGroup(base.behaviorSpeculator.AttackBehaviors[j] as AttackBehaviorGroup);
			}
		}
		base.aiShooter.ToggleGunAndHandRenderers(false, "CoalGuyController");
		base.aiShooter.enabled = false;
		base.behaviorSpeculator.AttackCooldown = 0.66f;
	}

	// Token: 0x06005D0F RID: 23823 RVA: 0x0023A550 File Offset: 0x00238750
	private void ProcessAttackGroup(AttackBehaviorGroup attackGroup)
	{
		for (int i = 0; i < attackGroup.AttackBehaviors.Count; i++)
		{
			AttackBehaviorGroup.AttackGroupItem attackGroupItem = attackGroup.AttackBehaviors[i];
			if (attackGroupItem.Behavior is AttackBehaviorGroup)
			{
				this.ProcessAttackGroup(attackGroupItem.Behavior as AttackBehaviorGroup);
			}
			else if (attackGroupItem.Behavior is ShootGunBehavior)
			{
				attackGroupItem.Probability = 0f;
			}
			else if (attackGroupItem.Behavior is ShootBehavior)
			{
				attackGroupItem.Probability = 1f;
			}
		}
	}

	// Token: 0x040056CB RID: 22219
	[FormerlySerializedAs("fireEffect2")]
	public GameActorFireEffect fireEffect;

	// Token: 0x040056CC RID: 22220
	public tk2dSpriteAnimator eyes;

	// Token: 0x040056CD RID: 22221
	public float overrideMoveSpeed = -1f;

	// Token: 0x040056CE RID: 22222
	public float overridePauseTime = -1f;

	// Token: 0x040056CF RID: 22223
	[CheckDirectionalAnimation(null)]
	public string overrideAnimation;

	// Token: 0x040056D0 RID: 22224
	public List<DamageTypeModifier> onFireDamageTypeModifiers;
}
