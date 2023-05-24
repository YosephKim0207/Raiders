using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001645 RID: 5701
[Serializable]
public class ExplosionData
{
	// Token: 0x06008523 RID: 34083 RVA: 0x0036E3E0 File Offset: 0x0036C5E0
	public void CopyFrom(ExplosionData source)
	{
		this.doDamage = source.doDamage;
		this.forceUseThisRadius = source.forceUseThisRadius;
		this.damageRadius = source.damageRadius;
		this.damageToPlayer = source.damageToPlayer;
		this.damage = source.damage;
		this.breakSecretWalls = source.breakSecretWalls;
		this.secretWallsRadius = source.secretWallsRadius;
		this.doDestroyProjectiles = source.doDestroyProjectiles;
		this.doForce = source.doForce;
		this.pushRadius = source.pushRadius;
		this.force = source.force;
		this.debrisForce = source.debrisForce;
		this.explosionDelay = source.explosionDelay;
		this.effect = source.effect;
		this.doScreenShake = source.doScreenShake;
		this.ss = source.ss;
		this.doStickyFriction = source.doStickyFriction;
		this.doExplosionRing = source.doExplosionRing;
		this.isFreezeExplosion = source.isFreezeExplosion;
		this.freezeRadius = source.freezeRadius;
		this.freezeEffect = source.freezeEffect;
		this.playDefaultSFX = source.playDefaultSFX;
		this.IsChandelierExplosion = source.IsChandelierExplosion;
		this.ignoreList = new List<SpeculativeRigidbody>();
	}

	// Token: 0x06008524 RID: 34084 RVA: 0x0036E50C File Offset: 0x0036C70C
	public float GetDefinedDamageRadius()
	{
		if (this.forceUseThisRadius)
		{
			return this.damageRadius;
		}
		if (this.effect)
		{
			ExplosionRadiusDefiner component = this.effect.GetComponent<ExplosionRadiusDefiner>();
			if (component)
			{
				return component.Radius;
			}
		}
		return this.damageRadius;
	}

	// Token: 0x0400890F RID: 35087
	public bool useDefaultExplosion;

	// Token: 0x04008910 RID: 35088
	public bool doDamage = true;

	// Token: 0x04008911 RID: 35089
	public bool forceUseThisRadius;

	// Token: 0x04008912 RID: 35090
	[ShowInInspectorIf("doDamage", true)]
	public float damageRadius = 4.5f;

	// Token: 0x04008913 RID: 35091
	[ShowInInspectorIf("doDamage", true)]
	public float damageToPlayer = 0.5f;

	// Token: 0x04008914 RID: 35092
	[ShowInInspectorIf("doDamage", true)]
	public float damage = 25f;

	// Token: 0x04008915 RID: 35093
	public bool breakSecretWalls;

	// Token: 0x04008916 RID: 35094
	[ShowInInspectorIf("breakSecretWalls", true)]
	public float secretWallsRadius = 4.5f;

	// Token: 0x04008917 RID: 35095
	public bool forcePreventSecretWallDamage;

	// Token: 0x04008918 RID: 35096
	public bool doDestroyProjectiles = true;

	// Token: 0x04008919 RID: 35097
	public bool doForce = true;

	// Token: 0x0400891A RID: 35098
	[ShowInInspectorIf("doForce", true)]
	public float pushRadius = 6f;

	// Token: 0x0400891B RID: 35099
	[ShowInInspectorIf("doForce", true)]
	public float force = 100f;

	// Token: 0x0400891C RID: 35100
	[ShowInInspectorIf("doForce", true)]
	public float debrisForce = 50f;

	// Token: 0x0400891D RID: 35101
	[ShowInInspectorIf("doForce", true)]
	public bool preventPlayerForce;

	// Token: 0x0400891E RID: 35102
	public float explosionDelay = 0.1f;

	// Token: 0x0400891F RID: 35103
	public bool usesComprehensiveDelay;

	// Token: 0x04008920 RID: 35104
	[ShowInInspectorIf("usesComprehensiveDelay", false)]
	public float comprehensiveDelay;

	// Token: 0x04008921 RID: 35105
	public GameObject effect;

	// Token: 0x04008922 RID: 35106
	public bool doScreenShake = true;

	// Token: 0x04008923 RID: 35107
	[ShowInInspectorIf("doScreenShake", true)]
	public ScreenShakeSettings ss;

	// Token: 0x04008924 RID: 35108
	public bool doStickyFriction = true;

	// Token: 0x04008925 RID: 35109
	public bool doExplosionRing = true;

	// Token: 0x04008926 RID: 35110
	public bool isFreezeExplosion;

	// Token: 0x04008927 RID: 35111
	[ShowInInspectorIf("isFreezeExplosion", false)]
	public float freezeRadius = 5f;

	// Token: 0x04008928 RID: 35112
	public GameActorFreezeEffect freezeEffect;

	// Token: 0x04008929 RID: 35113
	public bool playDefaultSFX = true;

	// Token: 0x0400892A RID: 35114
	public bool IsChandelierExplosion;

	// Token: 0x0400892B RID: 35115
	public bool rotateEffectToNormal;

	// Token: 0x0400892C RID: 35116
	[HideInInspector]
	public List<SpeculativeRigidbody> ignoreList;

	// Token: 0x0400892D RID: 35117
	[HideInInspector]
	public GameObject overrideRangeIndicatorEffect;
}
