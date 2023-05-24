using System;
using System.Collections;
using UnityEngine;

// Token: 0x020010AF RID: 4271
public class MetalGearRatController : BraveBehaviour
{
	// Token: 0x06005E2F RID: 24111 RVA: 0x00241F44 File Offset: 0x00240144
	public void Start()
	{
		this.m_tailgunPixelCollider = base.specRigidbody.PixelColliders[this.Tailgun.PixelCollider];
		this.m_radomePixelCollider = base.specRigidbody.PixelColliders[this.Radome.PixelCollider];
		base.healthHaver.AddTrackedDamagePixelCollider(this.m_tailgunPixelCollider);
		base.healthHaver.AddTrackedDamagePixelCollider(this.m_radomePixelCollider);
		base.healthHaver.GlobalPixelColliderDamageMultiplier = 0.25f;
	}

	// Token: 0x06005E30 RID: 24112 RVA: 0x00241FC8 File Offset: 0x002401C8
	public void Update()
	{
		float num;
		if (!this.m_isTailgunDestroyed && base.healthHaver.PixelColliderDamage.TryGetValue(this.m_tailgunPixelCollider, out num))
		{
			float num2 = num / base.healthHaver.GetMaxHealth();
			float num3 = 1f - num2 / this.Tailgun.HealthPercentage;
			if (num2 >= this.Tailgun.HealthPercentage && base.behaviorSpeculator && base.behaviorSpeculator.IsInterruptable)
			{
				this.m_isTailgunDestroyed = true;
				base.StartCoroutine(this.DestroyPartCR(this.Tailgun, "destroy_tailgun"));
			}
		}
		float num4;
		if (!this.m_isRadomeDestroyed && base.healthHaver.PixelColliderDamage.TryGetValue(this.m_radomePixelCollider, out num4))
		{
			float num5 = num4 / base.healthHaver.GetMaxHealth();
			float num6 = 1f - num5 / this.Radome.HealthPercentage;
			if (num5 > this.Radome.HealthPercentage && base.behaviorSpeculator && base.behaviorSpeculator.IsInterruptable)
			{
				this.m_isRadomeDestroyed = true;
				base.StartCoroutine(this.DestroyPartCR(this.Radome, "destroy_radome"));
			}
		}
	}

	// Token: 0x06005E31 RID: 24113 RVA: 0x00242128 File Offset: 0x00240328
	private IEnumerator DestroyPartCR(MetalGearRatController.MetalGearPart part, string destroyAnim)
	{
		base.behaviorSpeculator.InterruptAndDisable();
		base.aiAnimator.PlayUntilFinished(destroyAnim, false, null, -1f, false);
		for (int i = 0; i < part.AIAnimator.OtherAnimations.Count; i++)
		{
			if (!(part.AIAnimator.OtherAnimations[i].name == destroyAnim))
			{
				part.AIAnimator.OtherAnimations[i].anim.Type = DirectionalAnimation.DirectionType.Single;
				part.AIAnimator.OtherAnimations[i].anim.Prefix = "blank";
			}
		}
		part.AIAnimator.IdleAnimation.Prefix = "blank";
		base.specRigidbody.PixelColliders[part.DeathPixelCollider].Enabled = true;
		AttackBehaviorGroup group = base.behaviorSpeculator.AttackBehaviors[0] as AttackBehaviorGroup;
		group.AttackBehaviors.Find((AttackBehaviorGroup.AttackGroupItem a) => a.NickName == part.AttackName).Probability = 0f;
		part.AutoAimer.enabled = false;
		if (part.BodyDamageOnDeath > 0f)
		{
			float num = base.healthHaver.GetMaxHealth() * part.BodyDamageOnDeath;
			float num2 = base.healthHaver.GetMaxHealth() * this.MinBodyDamageHealth;
			num = Mathf.Min(num, base.healthHaver.GetCurrentHealth() - num2);
			if (num > 0f)
			{
				base.healthHaver.ApplyDamage(num, Vector2.zero, "body part destruction", CoreDamageTypes.None, DamageCategory.Unstoppable, true, base.specRigidbody.HitboxPixelCollider, true);
			}
		}
		yield return new WaitForSeconds(base.aiAnimator.CurrentClipLength);
		if (this.m_isRadomeDestroyed && this.m_isTailgunDestroyed)
		{
			group.AttackBehaviors.Find((AttackBehaviorGroup.AttackGroupItem a) => a.NickName == this.IconAttackName).Probability = this.IconAttackProbability;
		}
		base.behaviorSpeculator.enabled = true;
		yield break;
	}

	// Token: 0x0400583E RID: 22590
	public MetalGearRatController.MetalGearPart Tailgun;

	// Token: 0x0400583F RID: 22591
	public MetalGearRatController.MetalGearPart Radome;

	// Token: 0x04005840 RID: 22592
	public float MinBodyDamageHealth = 0.15f;

	// Token: 0x04005841 RID: 22593
	public string IconAttackName = "Icon Stomp";

	// Token: 0x04005842 RID: 22594
	public float IconAttackProbability = 4f;

	// Token: 0x04005843 RID: 22595
	private PixelCollider m_tailgunPixelCollider;

	// Token: 0x04005844 RID: 22596
	private bool m_isTailgunDestroyed;

	// Token: 0x04005845 RID: 22597
	private PixelCollider m_radomePixelCollider;

	// Token: 0x04005846 RID: 22598
	private bool m_isRadomeDestroyed;

	// Token: 0x020010B0 RID: 4272
	[Serializable]
	public class MetalGearPart
	{
		// Token: 0x04005847 RID: 22599
		public AIAnimator AIAnimator;

		// Token: 0x04005848 RID: 22600
		public float HealthPercentage = 0.1f;

		// Token: 0x04005849 RID: 22601
		public int PixelCollider;

		// Token: 0x0400584A RID: 22602
		public int DeathPixelCollider;

		// Token: 0x0400584B RID: 22603
		public string AttackName;

		// Token: 0x0400584C RID: 22604
		public AutoAimTarget AutoAimer;

		// Token: 0x0400584D RID: 22605
		public float BodyDamageOnDeath;
	}
}
