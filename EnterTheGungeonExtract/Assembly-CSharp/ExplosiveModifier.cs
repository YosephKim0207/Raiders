using System;
using UnityEngine;

// Token: 0x02001646 RID: 5702
public class ExplosiveModifier : BraveBehaviour
{
	// Token: 0x06008526 RID: 34086 RVA: 0x0036E59C File Offset: 0x0036C79C
	public void Explode(Vector2 sourceNormal, bool ignoreDamageCaps = false, CollisionData cd = null)
	{
		if (base.projectile && base.projectile.Owner)
		{
			if (base.projectile.Owner is PlayerController)
			{
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					PlayerController playerController = GameManager.Instance.AllPlayers[i];
					if (playerController && playerController.specRigidbody)
					{
						this.explosionData.ignoreList.Add(playerController.specRigidbody);
					}
				}
			}
			else
			{
				this.explosionData.ignoreList.Add(base.projectile.Owner.specRigidbody);
			}
		}
		Vector3 vector = ((cd == null) ? base.specRigidbody.UnitCenter.ToVector3ZUp(0f) : cd.Contact.ToVector3ZUp(0f));
		if (this.doExplosion)
		{
			CoreDamageTypes coreDamageTypes = CoreDamageTypes.None;
			if (this.explosionData.doDamage && this.explosionData.damageRadius < 10f && base.projectile)
			{
				if (base.projectile.AppliesFreeze)
				{
					coreDamageTypes |= CoreDamageTypes.Ice;
				}
				if (base.projectile.AppliesFire)
				{
					coreDamageTypes |= CoreDamageTypes.Fire;
				}
				if (base.projectile.AppliesPoison)
				{
					coreDamageTypes |= CoreDamageTypes.Poison;
				}
				if (base.projectile.statusEffectsToApply != null)
				{
					for (int j = 0; j < base.projectile.statusEffectsToApply.Count; j++)
					{
						GameActorEffect gameActorEffect = base.projectile.statusEffectsToApply[j];
						if (gameActorEffect is GameActorFreezeEffect)
						{
							coreDamageTypes |= CoreDamageTypes.Ice;
						}
						else if (gameActorEffect is GameActorFireEffect)
						{
							coreDamageTypes |= CoreDamageTypes.Fire;
						}
						else if (gameActorEffect is GameActorHealthEffect)
						{
							coreDamageTypes |= CoreDamageTypes.Poison;
						}
					}
				}
			}
			Exploder.Explode(vector, this.explosionData, sourceNormal, null, this.IgnoreQueues, coreDamageTypes, ignoreDamageCaps);
		}
		if (this.doDistortionWave)
		{
			Exploder.DoDistortionWave(vector, this.distortionIntensity, this.distortionRadius, this.maxDistortionRadius, this.distortionDuration);
		}
	}

	// Token: 0x06008527 RID: 34087 RVA: 0x0036E7DC File Offset: 0x0036C9DC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400892E RID: 35118
	public bool doExplosion = true;

	// Token: 0x0400892F RID: 35119
	[SerializeField]
	public ExplosionData explosionData;

	// Token: 0x04008930 RID: 35120
	public bool doDistortionWave;

	// Token: 0x04008931 RID: 35121
	[ShowInInspectorIf("doDistortionWave", true)]
	public float distortionIntensity = 1f;

	// Token: 0x04008932 RID: 35122
	[ShowInInspectorIf("doDistortionWave", true)]
	public float distortionRadius = 1f;

	// Token: 0x04008933 RID: 35123
	[ShowInInspectorIf("doDistortionWave", true)]
	public float maxDistortionRadius = 10f;

	// Token: 0x04008934 RID: 35124
	[ShowInInspectorIf("doDistortionWave", true)]
	public float distortionDuration = 0.5f;

	// Token: 0x04008935 RID: 35125
	public bool IgnoreQueues;
}
