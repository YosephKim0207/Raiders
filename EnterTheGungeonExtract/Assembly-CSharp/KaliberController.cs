using System;
using System.Collections;
using UnityEngine;

// Token: 0x020010A6 RID: 4262
public class KaliberController : BraveBehaviour
{
	// Token: 0x06005DF5 RID: 24053 RVA: 0x002408D4 File Offset: 0x0023EAD4
	public void Start()
	{
		this.m_minHealth = (float)Mathf.RoundToInt(base.healthHaver.GetMaxHealth() * 0.666f);
		base.healthHaver.minimumHealth = this.m_minHealth;
	}

	// Token: 0x06005DF6 RID: 24054 RVA: 0x00240904 File Offset: 0x0023EB04
	public void Update()
	{
		if (!this.m_isTransitioning && base.healthHaver.GetCurrentHealth() <= this.m_minHealth + 0.5f)
		{
			base.StartCoroutine(this.DestroyHead());
		}
	}

	// Token: 0x06005DF7 RID: 24055 RVA: 0x0024093C File Offset: 0x0023EB3C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005DF8 RID: 24056 RVA: 0x00240944 File Offset: 0x0023EB44
	private IEnumerator DestroyHead()
	{
		this.m_isTransitioning = true;
		if (base.aiActor.IsFrozen)
		{
			base.aiActor.RemoveEffect("freeze");
		}
		if (base.behaviorSpeculator.IsStunned)
		{
			base.behaviorSpeculator.EndStun();
		}
		base.aiActor.ClearPath();
		base.knockbackDoer.SetImmobile(true, "KaliberController");
		base.behaviorSpeculator.InterruptAndDisable();
		string animName = this.m_headsLeft + 1 + "_die";
		base.aiAnimator.PlayUntilCancelled(animName, false, null, -1f, false);
		base.aiAnimator.PlayVfx("bottom_die", null, null, null);
		base.aiAnimator.IdleAnimation.Prefix = this.m_headsLeft + "_idle";
		base.aiAnimator.OtherAnimations[0].anim.Prefix = this.m_headsLeft + "_attack";
		if (this.m_headsLeft > 1)
		{
			base.specRigidbody.PixelColliders[1].SpecifyBagelFrame = string.Format("kaliber_{0}_idle_001", this.m_headsLeft);
		}
		else
		{
			base.specRigidbody.PixelColliders[1].SpecifyBagelFrame = "kaliber_1_die_001";
		}
		base.specRigidbody.ForceRegenerate(null, null);
		while (base.aiAnimator.IsPlaying(animName))
		{
			yield return null;
		}
		base.aiAnimator.EndAnimation();
		base.knockbackDoer.SetImmobile(false, "KaliberController");
		if (!base.aiActor.IsFrozen)
		{
			base.behaviorSpeculator.enabled = true;
		}
		this.m_headsLeft--;
		if (this.m_headsLeft == 2)
		{
			this.m_minHealth = (float)Mathf.RoundToInt(base.healthHaver.GetMaxHealth() * 0.333f);
			base.healthHaver.minimumHealth = this.m_minHealth;
		}
		else if (this.m_headsLeft == 1)
		{
			this.m_minHealth = 1f;
			base.healthHaver.minimumHealth = this.m_minHealth;
		}
		else if (this.m_headsLeft == 0)
		{
			base.aiActor.ParentRoom.DeregisterEnemy(base.aiActor, false);
			base.aiActor.IgnoreForRoomClear = true;
			base.healthHaver.minimumHealth = 0f;
			base.healthHaver.ApplyDamage(10f, Vector2.zero, "death", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
			base.enabled = false;
		}
		AttackBehaviorGroup attackGroup = (AttackBehaviorGroup)base.behaviorSpeculator.AttackBehaviors.Find((AttackBehaviorBase a) => a is AttackBehaviorGroup);
		int enableIndex = 3 - this.m_headsLeft;
		for (int i = 0; i < attackGroup.AttackBehaviors.Count; i++)
		{
			attackGroup.AttackBehaviors[i].Probability = (float)((i != enableIndex) ? 0 : 1);
		}
		this.m_isTransitioning = false;
		yield break;
	}

	// Token: 0x0400580F RID: 22543
	private int m_headsLeft = 3;

	// Token: 0x04005810 RID: 22544
	private float m_minHealth = 1f;

	// Token: 0x04005811 RID: 22545
	private bool m_isTransitioning;
}
