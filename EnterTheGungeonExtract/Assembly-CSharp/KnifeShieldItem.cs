using System;
using UnityEngine;

// Token: 0x0200142D RID: 5165
public class KnifeShieldItem : PlayerItem
{
	// Token: 0x06007539 RID: 30009 RVA: 0x002EAAA8 File Offset: 0x002E8CA8
	protected override void DoEffect(PlayerController user)
	{
		this.m_extantEffect = this.CreateEffect(user, 1f, 1f);
		if (user.HasActiveBonusSynergy(CustomSynergyType.TWO_BLADES, false))
		{
			this.m_secondaryEffect = this.CreateEffect(user, 1.25f, -1f);
		}
		AkSoundEngine.PostEvent("Play_OBJ_daggershield_start_01", base.gameObject);
	}

	// Token: 0x0600753A RID: 30010 RVA: 0x002EAB08 File Offset: 0x002E8D08
	private KnifeShieldEffect CreateEffect(PlayerController user, float radiusMultiplier = 1f, float rotationSpeedMultiplier = 1f)
	{
		KnifeShieldEffect knifeShieldEffect = new GameObject("knife shield effect")
		{
			transform = 
			{
				position = user.LockedApproximateSpriteCenter,
				parent = user.transform
			}
		}.AddComponent<KnifeShieldEffect>();
		knifeShieldEffect.numKnives = this.numKnives;
		knifeShieldEffect.remainingHealth = this.knifeHealth;
		knifeShieldEffect.knifeDamage = this.knifeDamage;
		knifeShieldEffect.circleRadius = this.circleRadius * radiusMultiplier;
		knifeShieldEffect.rotationDegreesPerSecond = this.rotationDegreesPerSecond * rotationSpeedMultiplier;
		knifeShieldEffect.throwSpeed = this.throwSpeed;
		knifeShieldEffect.throwRange = this.throwRange;
		knifeShieldEffect.throwRadius = this.throwRadius;
		knifeShieldEffect.radiusChangeDistance = this.radiusChangeDistance;
		knifeShieldEffect.deathVFX = this.knifeDeathVFX;
		knifeShieldEffect.Initialize(user, this.knifePrefab);
		return knifeShieldEffect;
	}

	// Token: 0x0600753B RID: 30011 RVA: 0x002EABD4 File Offset: 0x002E8DD4
	public override void Update()
	{
		base.Update();
		if (this.m_extantEffect != null && !this.m_extantEffect.IsActive)
		{
			this.m_extantEffect = null;
		}
		if (this.m_secondaryEffect != null && !this.m_secondaryEffect.IsActive)
		{
			this.m_secondaryEffect = null;
		}
	}

	// Token: 0x0600753C RID: 30012 RVA: 0x002EAC38 File Offset: 0x002E8E38
	protected override void DoOnCooldownEffect(PlayerController user)
	{
		if (this.m_extantEffect != null && this.m_extantEffect.IsActive)
		{
			this.m_extantEffect.ThrowShield();
		}
		if (this.m_secondaryEffect != null && this.m_secondaryEffect.IsActive)
		{
			this.m_secondaryEffect.ThrowShield();
		}
	}

	// Token: 0x0600753D RID: 30013 RVA: 0x002EACA0 File Offset: 0x002E8EA0
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04007714 RID: 30484
	[Header("Knife Properties")]
	public int numKnives = 5;

	// Token: 0x04007715 RID: 30485
	public float knifeHealth = 0.5f;

	// Token: 0x04007716 RID: 30486
	public float knifeDamage = 5f;

	// Token: 0x04007717 RID: 30487
	public float circleRadius = 3f;

	// Token: 0x04007718 RID: 30488
	public float rotationDegreesPerSecond = 360f;

	// Token: 0x04007719 RID: 30489
	[Header("Thrown Properties")]
	public float throwSpeed = 10f;

	// Token: 0x0400771A RID: 30490
	public float throwRange = 25f;

	// Token: 0x0400771B RID: 30491
	public float throwRadius = 3f;

	// Token: 0x0400771C RID: 30492
	public float radiusChangeDistance = 3f;

	// Token: 0x0400771D RID: 30493
	public GameObject knifePrefab;

	// Token: 0x0400771E RID: 30494
	public GameObject knifeDeathVFX;

	// Token: 0x0400771F RID: 30495
	protected KnifeShieldEffect m_extantEffect;

	// Token: 0x04007720 RID: 30496
	protected KnifeShieldEffect m_secondaryEffect;
}
