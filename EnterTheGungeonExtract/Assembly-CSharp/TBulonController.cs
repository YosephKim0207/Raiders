using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020010CA RID: 4298
public class TBulonController : BraveBehaviour
{
	// Token: 0x06005EAB RID: 24235 RVA: 0x00245C10 File Offset: 0x00243E10
	public void Start()
	{
		base.healthHaver.minimumHealth = 1f;
		base.healthHaver.OnDamaged += this.OnDamaged;
		this.m_goopDoer = base.GetComponent<GoopDoer>();
	}

	// Token: 0x06005EAC RID: 24236 RVA: 0x00245C48 File Offset: 0x00243E48
	public void Update()
	{
		if (!base.aiActor || !base.healthHaver || base.healthHaver.IsDead)
		{
			return;
		}
		if (this.m_state != TBulonController.State.Normal)
		{
			if (this.m_state == TBulonController.State.Transforming)
			{
				base.sprite.ForceUpdateMaterial();
				if (!base.aiAnimator.IsPlaying(this.transformAnim))
				{
					base.aiAnimator.PlayUntilFinished(this.enrageAnim, true, null, -1f, false);
					base.behaviorSpeculator.enabled = true;
					if (this.overrideMoveSpeed >= 0f)
					{
						base.aiActor.MovementSpeed = TurboModeController.MaybeModifyEnemyMovementSpeed(this.overrideMoveSpeed);
					}
					if (this.overrideWeight >= 0f)
					{
						base.knockbackDoer.weight = this.overrideWeight;
					}
					this.m_goopDoer.enabled = true;
					this.m_startGoopRadius = this.m_goopDoer.defaultGoopRadius;
					this.m_state = TBulonController.State.Enraged;
				}
			}
			else if (this.m_state == TBulonController.State.Enraged)
			{
				if (!base.aiAnimator.IsPlaying(this.enrageAnim))
				{
					base.healthHaver.ManualDeathHandling = true;
					base.aiActor.ForceDeath(Vector2.zero, false);
					UnityEngine.Object.Destroy(base.gameObject);
				}
				else
				{
					this.m_goopDoer.defaultGoopRadius = Mathf.Lerp(this.m_startGoopRadius, 0.2f, base.aiAnimator.CurrentClipProgress);
				}
			}
		}
	}

	// Token: 0x06005EAD RID: 24237 RVA: 0x00245DD0 File Offset: 0x00243FD0
	protected override void OnDestroy()
	{
		if (base.healthHaver)
		{
			base.healthHaver.OnDamaged -= this.OnDamaged;
		}
		base.OnDestroy();
	}

	// Token: 0x06005EAE RID: 24238 RVA: 0x00245E00 File Offset: 0x00244000
	private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		if (this.m_state == TBulonController.State.Normal && resultValue == 1f)
		{
			base.aiAnimator.PlayUntilFinished(this.transformAnim, true, null, -1f, false);
			base.healthHaver.ApplyDamageModifiers(this.onFireDamageTypeModifiers);
			base.healthHaver.SetHealthMaximum(this.newHealth, null, false);
			base.healthHaver.ForceSetCurrentHealth(this.newHealth);
			base.healthHaver.minimumHealth = 0f;
			base.behaviorSpeculator.InterruptAndDisable();
			base.aiActor.ClearPath();
			base.aiAnimator.OtherAnimations.Find((AIAnimator.NamedDirectionalAnimation a) => a.name == "pitfall").anim.Prefix = "pitfall_hot";
			this.m_state = TBulonController.State.Transforming;
		}
	}

	// Token: 0x040058D9 RID: 22745
	public float newHealth = 50f;

	// Token: 0x040058DA RID: 22746
	[CheckDirectionalAnimation(null)]
	public string transformAnim;

	// Token: 0x040058DB RID: 22747
	[CheckDirectionalAnimation(null)]
	public string enrageAnim;

	// Token: 0x040058DC RID: 22748
	public float overrideMoveSpeed = -1f;

	// Token: 0x040058DD RID: 22749
	public float overrideWeight = -1f;

	// Token: 0x040058DE RID: 22750
	public List<DamageTypeModifier> onFireDamageTypeModifiers;

	// Token: 0x040058DF RID: 22751
	private TBulonController.State m_state;

	// Token: 0x040058E0 RID: 22752
	private GoopDoer m_goopDoer;

	// Token: 0x040058E1 RID: 22753
	private float m_startGoopRadius;

	// Token: 0x020010CB RID: 4299
	private enum State
	{
		// Token: 0x040058E4 RID: 22756
		Normal,
		// Token: 0x040058E5 RID: 22757
		Transforming,
		// Token: 0x040058E6 RID: 22758
		Enraged
	}
}
