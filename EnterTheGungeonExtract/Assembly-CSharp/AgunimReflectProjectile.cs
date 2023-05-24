using System;
using UnityEngine;

// Token: 0x02000FBC RID: 4028
[RequireComponent(typeof(Projectile))]
public class AgunimReflectProjectile : BraveBehaviour
{
	// Token: 0x060057B9 RID: 22457 RVA: 0x00217AD4 File Offset: 0x00215CD4
	public void Awake()
	{
		base.projectile.OnReflected += this.OnReflected;
		SpeculativeRigidbody specRigidbody = base.projectile.specRigidbody;
		specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreRigidbodyCollision));
		SpeculativeRigidbody specRigidbody2 = base.projectile.specRigidbody;
		specRigidbody2.OnPreTileCollision = (SpeculativeRigidbody.OnPreTileCollisionDelegate)Delegate.Combine(specRigidbody2.OnPreTileCollision, new SpeculativeRigidbody.OnPreTileCollisionDelegate(this.OnPreTileCollision));
	}

	// Token: 0x060057BA RID: 22458 RVA: 0x00217B50 File Offset: 0x00215D50
	public void OnSpawned()
	{
		this.m_playerReflects = 0;
	}

	// Token: 0x060057BB RID: 22459 RVA: 0x00217B5C File Offset: 0x00215D5C
	public void OnDespawned()
	{
		base.projectile.spriteAnimator.OverrideTimeScale = -1f;
	}

	// Token: 0x060057BC RID: 22460 RVA: 0x00217B74 File Offset: 0x00215D74
	protected override void OnDestroy()
	{
		if (base.projectile)
		{
			base.projectile.OnReflected -= this.OnReflected;
			if (base.projectile.specRigidbody)
			{
				SpeculativeRigidbody specRigidbody = base.projectile.specRigidbody;
				specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreRigidbodyCollision));
				SpeculativeRigidbody specRigidbody2 = base.projectile.specRigidbody;
				specRigidbody2.OnPreTileCollision = (SpeculativeRigidbody.OnPreTileCollisionDelegate)Delegate.Remove(specRigidbody2.OnPreTileCollision, new SpeculativeRigidbody.OnPreTileCollisionDelegate(this.OnPreTileCollision));
			}
		}
		base.OnDestroy();
	}

	// Token: 0x060057BD RID: 22461 RVA: 0x00217C1C File Offset: 0x00215E1C
	private void OnReflected(Projectile p)
	{
		if (p.Owner is PlayerController)
		{
			if (p.spriteAnimator.OverrideTimeScale < 0f)
			{
				p.spriteAnimator.OverrideTimeScale = 1f;
			}
			p.Speed += this.SpeedIncreases[this.m_playerReflects];
			p.spriteAnimator.OverrideTimeScale *= this.AnimSpeedMultipliers[this.m_playerReflects];
			this.PlayerReflectVfx.SpawnAtPosition(base.transform.position, 0f, null, null, null, null, false, null, null, false);
			StickyFrictionManager.Instance.RegisterCustomStickyFriction(this.PlayerReflectFriction[this.m_playerReflects], 0f, false, true);
			this.m_playerReflects++;
			AkSoundEngine.PostEvent("Play_BOSS_agunim_deflect_01", base.gameObject);
		}
	}

	// Token: 0x060057BE RID: 22462 RVA: 0x00217D10 File Offset: 0x00215F10
	private void OnPreRigidbodyCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
	{
		if (base.projectile.Owner is PlayerController && otherRigidbody.healthHaver && otherRigidbody.healthHaver.IsBoss)
		{
			int num = this.NumBounces[3 - Mathf.RoundToInt(otherRigidbody.healthHaver.GetCurrentHealth())];
			if (this.m_playerReflects < num)
			{
				AkSoundEngine.PostEvent("Play_BOSS_agunim_deflect_01", base.gameObject);
				Projectile projectile = base.projectile;
				bool flag = true;
				AIActor aiActor = otherRigidbody.aiActor;
				float num2 = 2f;
				float num3 = this.BossReflectSpreads[this.m_playerReflects - 1];
				PassiveReflectItem.ReflectBullet(projectile, flag, aiActor, num2, 1f, 1f, num3);
				StickyFrictionManager.Instance.RegisterCustomStickyFriction(this.BossReflectFriction[this.m_playerReflects - 1], 0f, false, true);
				AIActor aiActor2 = otherRigidbody.aiActor;
				if (aiActor2)
				{
					aiActor2.aiAnimator.PlayUntilFinished("deflect", true, null, -1f, false);
					AIAnimator aiAnimator = aiActor2.aiAnimator;
					string text = "deflect";
					Vector2? vector = new Vector2?((aiActor2.specRigidbody.GetUnitCenter(ColliderType.HitBox) + base.projectile.specRigidbody.UnitCenter) / 2f);
					aiAnimator.PlayVfx(text, null, null, vector);
				}
				PhysicsEngine.SkipCollision = true;
				PhysicsEngine.PostSliceVelocity = new Vector2?(base.projectile.specRigidbody.Velocity);
			}
			else
			{
				AIActor aiActor3 = otherRigidbody.aiActor;
				if (aiActor3)
				{
					aiActor3.aiAnimator.PlayUntilFinished("big_hit", true, null, -1f, false);
					AIAnimator aiAnimator2 = aiActor3.aiAnimator;
					string text = "big_hit";
					Vector2? vector = new Vector2?((aiActor3.specRigidbody.GetUnitCenter(ColliderType.HitBox) + base.projectile.specRigidbody.UnitCenter) / 2f);
					aiAnimator2.PlayVfx(text, null, null, vector);
				}
			}
		}
	}

	// Token: 0x060057BF RID: 22463 RVA: 0x00217F24 File Offset: 0x00216124
	private void OnPreTileCollision(SpeculativeRigidbody myrigidbody, PixelCollider mypixelcollider, PhysicsEngine.Tile tile, PixelCollider tilepixelcollider)
	{
		if (tile == null)
		{
			return;
		}
		if (GameManager.Instance.Dungeon.data.isFaceWallHigher(tile.X, tile.Y) || GameManager.Instance.Dungeon.data.isFaceWallLower(tile.X, tile.Y))
		{
			PhysicsEngine.SkipCollision = true;
		}
	}

	// Token: 0x040050BA RID: 20666
	public int[] NumBounces;

	// Token: 0x040050BB RID: 20667
	public float[] SpeedIncreases;

	// Token: 0x040050BC RID: 20668
	public float[] AnimSpeedMultipliers;

	// Token: 0x040050BD RID: 20669
	public float[] BossReflectSpreads;

	// Token: 0x040050BE RID: 20670
	public float[] PlayerReflectFriction;

	// Token: 0x040050BF RID: 20671
	public float[] BossReflectFriction;

	// Token: 0x040050C0 RID: 20672
	public VFXPool PlayerReflectVfx;

	// Token: 0x040050C1 RID: 20673
	private int m_playerReflects;
}
