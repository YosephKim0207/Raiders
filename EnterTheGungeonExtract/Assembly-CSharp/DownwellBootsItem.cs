using System;
using System.Collections;
using UnityEngine;

// Token: 0x020013EC RID: 5100
public class DownwellBootsItem : PassiveItem
{
	// Token: 0x060073B2 RID: 29618 RVA: 0x002E0474 File Offset: 0x002DE674
	public override void Pickup(PlayerController player)
	{
		if (this.m_pickedUp)
		{
			return;
		}
		base.Pickup(player);
		this.downwellAfterimage = player.gameObject.AddComponent<AfterImageTrailController>();
		this.downwellAfterimage.spawnShadows = false;
		this.downwellAfterimage.shadowTimeDelay = 0.05f;
		this.downwellAfterimage.shadowLifetime = 0.3f;
		this.downwellAfterimage.minTranslation = 0.05f;
		this.downwellAfterimage.dashColor = Color.red;
		this.downwellAfterimage.OverrideImageShader = ShaderCache.Acquire("Brave/Internal/DownwellAfterImage");
		player.OnRollStarted += this.OnRollStarted;
	}

	// Token: 0x060073B3 RID: 29619 RVA: 0x002E0518 File Offset: 0x002DE718
	private void OnRollStarted(PlayerController sourcePlayer, Vector2 dirVec)
	{
		if (sourcePlayer && sourcePlayer.HasActiveBonusSynergy(CustomSynergyType.DOWNERWELL, false))
		{
			this.m_cooldown = 0f;
		}
		if (this.m_cooldown <= 0f)
		{
			if (sourcePlayer && sourcePlayer.HasActiveBonusSynergy(CustomSynergyType.BLASTBOOTS, false))
			{
				Exploder.Explode(sourcePlayer.CenterPosition + -dirVec.normalized, this.BlastBootsExplosion, dirVec, null, true, CoreDamageTypes.None, false);
			}
			else
			{
				for (int i = 0; i < this.NumProjectilesToFire; i++)
				{
					float num = 0f;
					if (this.NumProjectilesToFire > 1)
					{
						num = this.ProjectileArcAngle / -2f + this.ProjectileArcAngle / (float)(this.NumProjectilesToFire - 1) * (float)i;
					}
					GameObject gameObject = base.bulletBank.CreateProjectileFromBank(sourcePlayer.CenterPosition, BraveMathCollege.Atan2Degrees(dirVec * -1f) + num, "default", null, false, true, false);
					Projectile component = gameObject.GetComponent<Projectile>();
					if (component)
					{
						component.Shooter = sourcePlayer.specRigidbody;
						component.Owner = sourcePlayer;
						component.SpawnedFromNonChallengeItem = true;
						if (component.specRigidbody)
						{
							component.specRigidbody.PrimaryPixelCollider.CollisionLayerIgnoreOverride |= CollisionMask.LayerToMask(CollisionLayer.EnemyBulletBlocker);
						}
					}
					sourcePlayer.DoPostProcessProjectile(component);
				}
			}
			this.m_cooldown = this.FireCooldown;
		}
		sourcePlayer.StartCoroutine(this.HandleAfterImageStop(sourcePlayer));
	}

	// Token: 0x060073B4 RID: 29620 RVA: 0x002E0698 File Offset: 0x002DE898
	private IEnumerator HandleAfterImageStop(PlayerController player)
	{
		this.downwellAfterimage.spawnShadows = true;
		while (player.IsDodgeRolling)
		{
			yield return null;
		}
		if (this.downwellAfterimage)
		{
			this.downwellAfterimage.spawnShadows = false;
		}
		yield break;
	}

	// Token: 0x060073B5 RID: 29621 RVA: 0x002E06BC File Offset: 0x002DE8BC
	protected override void Update()
	{
		base.Update();
		this.m_cooldown -= BraveTime.DeltaTime;
	}

	// Token: 0x060073B6 RID: 29622 RVA: 0x002E06D8 File Offset: 0x002DE8D8
	public override DebrisObject Drop(PlayerController player)
	{
		DebrisObject debrisObject = base.Drop(player);
		debrisObject.GetComponent<DownwellBootsItem>().m_pickedUpThisRun = true;
		player.OnRollStarted -= this.OnRollStarted;
		if (this.downwellAfterimage)
		{
			UnityEngine.Object.Destroy(this.downwellAfterimage);
		}
		this.downwellAfterimage = null;
		return debrisObject;
	}

	// Token: 0x060073B7 RID: 29623 RVA: 0x002E0730 File Offset: 0x002DE930
	protected override void OnDestroy()
	{
		if (this.m_owner != null)
		{
			this.m_owner.OnRollStarted -= this.OnRollStarted;
			if (this.downwellAfterimage)
			{
				UnityEngine.Object.Destroy(this.downwellAfterimage);
			}
			this.downwellAfterimage = null;
		}
		base.OnDestroy();
	}

	// Token: 0x04007544 RID: 30020
	public int NumProjectilesToFire = 5;

	// Token: 0x04007545 RID: 30021
	public float ProjectileArcAngle = 45f;

	// Token: 0x04007546 RID: 30022
	public float FireCooldown = 2f;

	// Token: 0x04007547 RID: 30023
	private float m_cooldown;

	// Token: 0x04007548 RID: 30024
	private AfterImageTrailController downwellAfterimage;

	// Token: 0x04007549 RID: 30025
	[Header("Synergues")]
	public ExplosionData BlastBootsExplosion;

	// Token: 0x0400754A RID: 30026
	private PlayerController m_player;

	// Token: 0x020013ED RID: 5101
	public enum Condition
	{
		// Token: 0x0400754C RID: 30028
		WhileDodgeRolling
	}
}
