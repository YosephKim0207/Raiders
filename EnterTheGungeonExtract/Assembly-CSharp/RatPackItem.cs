using System;
using UnityEngine;

// Token: 0x020013E4 RID: 5092
public class RatPackItem : PlayerItem
{
	// Token: 0x17001164 RID: 4452
	// (get) Token: 0x0600738E RID: 29582 RVA: 0x002DF968 File Offset: 0x002DDB68
	public int ContainedBullets
	{
		get
		{
			return this.m_containedBullets;
		}
	}

	// Token: 0x0600738F RID: 29583 RVA: 0x002DF970 File Offset: 0x002DDB70
	public override void Pickup(PlayerController player)
	{
		base.Pickup(player);
		player.OnIsRolling += this.HandleRollingFrame;
	}

	// Token: 0x06007390 RID: 29584 RVA: 0x002DF98C File Offset: 0x002DDB8C
	private void HandleRollingFrame(PlayerController src)
	{
		if (this.EatBulletAction == null)
		{
			this.EatBulletAction = (Action<Projectile>)Delegate.Combine(this.EatBulletAction, new Action<Projectile>(this.EatBullet));
		}
		if (src.CurrentRollState == PlayerController.DodgeRollState.InAir)
		{
			RatPackItem.AffectNearbyProjectiles(src.CenterPosition, this.ScoopRadius, this.EatBulletAction);
		}
	}

	// Token: 0x06007391 RID: 29585 RVA: 0x002DF9EC File Offset: 0x002DDBEC
	private static void AffectNearbyProjectiles(Vector2 center, float radius, Action<Projectile> DoEffect)
	{
		for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
		{
			Projectile projectile = StaticReferenceManager.AllProjectiles[i];
			if (projectile && projectile.Owner is AIActor)
			{
				float sqrMagnitude = (projectile.transform.position.XY() - center).sqrMagnitude;
				if (sqrMagnitude < radius)
				{
					DoEffect(projectile);
				}
			}
		}
	}

	// Token: 0x06007392 RID: 29586 RVA: 0x002DFA68 File Offset: 0x002DDC68
	private void EatBullet(Projectile p)
	{
		if (p.Owner is AIActor)
		{
			p.DieInAir(false, true, true, false);
			this.m_containedBullets++;
			this.m_containedBullets = Mathf.Clamp(this.m_containedBullets, 0, this.MaxContainedBullets);
		}
	}

	// Token: 0x06007393 RID: 29587 RVA: 0x002DFAB8 File Offset: 0x002DDCB8
	private void HandleDodgedProjectile(Projectile p)
	{
		this.EatBullet(p);
	}

	// Token: 0x06007394 RID: 29588 RVA: 0x002DFAC4 File Offset: 0x002DDCC4
	public override bool CanBeUsed(PlayerController user)
	{
		return this.m_containedBullets > 0 && base.CanBeUsed(user);
	}

	// Token: 0x06007395 RID: 29589 RVA: 0x002DFADC File Offset: 0x002DDCDC
	protected override void DoEffect(PlayerController user)
	{
		base.DoEffect(user);
		this.Burst.NumberWaves = 1;
		this.Burst.MinToSpawnPerWave = this.ContainedBullets;
		this.Burst.MaxToSpawnPerWave = this.ContainedBullets;
		this.Burst.UseShotgunStyleVelocityModifier = true;
		if (user && user.CurrentGun)
		{
			this.Burst.DoBurst(user, user.CurrentGun.CurrentAngle);
		}
		this.m_containedBullets = 0;
	}

	// Token: 0x06007396 RID: 29590 RVA: 0x002DFB64 File Offset: 0x002DDD64
	protected override void OnPreDrop(PlayerController user)
	{
		user.OnIsRolling -= this.HandleRollingFrame;
		user.OnDodgedProjectile -= this.HandleDodgedProjectile;
		base.OnPreDrop(user);
	}

	// Token: 0x06007397 RID: 29591 RVA: 0x002DFB94 File Offset: 0x002DDD94
	protected override void OnDestroy()
	{
		if (this.LastOwner)
		{
			this.LastOwner.OnIsRolling -= this.HandleRollingFrame;
			this.LastOwner.OnDodgedProjectile -= this.HandleDodgedProjectile;
		}
		base.OnDestroy();
	}

	// Token: 0x0400752A RID: 29994
	public int MaxContainedBullets = 30;

	// Token: 0x0400752B RID: 29995
	public float ScoopRadius = 1f;

	// Token: 0x0400752C RID: 29996
	public DirectedBurstInterface Burst;

	// Token: 0x0400752D RID: 29997
	private int m_containedBullets;

	// Token: 0x0400752E RID: 29998
	private Action<Projectile> EatBulletAction;
}
