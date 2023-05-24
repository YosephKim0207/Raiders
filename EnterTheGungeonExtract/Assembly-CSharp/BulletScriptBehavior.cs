using System;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x0200033A RID: 826
public class BulletScriptBehavior : BraveBehaviour
{
	// Token: 0x06000CC6 RID: 3270 RVA: 0x0003D5D0 File Offset: 0x0003B7D0
	public void Initialize(Bullet newBullet)
	{
		this.bullet = newBullet;
		this.m_firstFrame = -1;
		base.enabled = true;
		if (base.projectile)
		{
			this.bullet.AutoRotation = base.projectile.shouldRotate;
			base.projectile.braveBulletScript = this;
		}
		this.Update();
		this.m_firstFrame = Time.frameCount;
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x0003D638 File Offset: 0x0003B838
	public void Update()
	{
		if (this.m_firstFrame == Time.frameCount)
		{
			return;
		}
		if (this.bullet != null)
		{
			this.bullet.FrameUpdate();
			if (this.bullet == null)
			{
				return;
			}
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			if (this.bullet.DisableMotion)
			{
				if (specRigidbody)
				{
					specRigidbody.Velocity = Vector2.zero;
				}
			}
			else if (specRigidbody)
			{
				float deltaTime = BraveTime.DeltaTime;
				Vector2 predictedPosition = this.bullet.PredictedPosition;
				Vector2 unitPosition = specRigidbody.Position.UnitPosition;
				specRigidbody.Velocity.x = (predictedPosition.x - unitPosition.x) / deltaTime;
				specRigidbody.Velocity.y = (predictedPosition.y - unitPosition.y) / deltaTime;
			}
			else
			{
				base.transform.position = this.bullet.PredictedPosition;
			}
			if (this.bullet.AutoRotation)
			{
				base.transform.rotation = Quaternion.identity;
				base.transform.Rotate(0f, 0f, this.bullet.Direction);
			}
		}
	}

	// Token: 0x06000CC8 RID: 3272 RVA: 0x0003D770 File Offset: 0x0003B970
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06000CC9 RID: 3273 RVA: 0x0003D778 File Offset: 0x0003B978
	public void RemoveBullet()
	{
		if (this.bullet != null)
		{
			this.bullet.OnForceRemoved();
			this.bullet.BulletManager.RemoveBullet(this.bullet);
		}
	}

	// Token: 0x06000CCA RID: 3274 RVA: 0x0003D7A8 File Offset: 0x0003B9A8
	public virtual void HandleBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool allowProjectileSpawns)
	{
		if (this.bullet != null)
		{
			if (destroyType != Bullet.DestroyType.DieInAir)
			{
				this.bullet.Position = this.bullet.Projectile.specRigidbody.UnitCenter;
			}
			this.bullet.HandleBulletDestruction(destroyType, hitRigidbody, allowProjectileSpawns);
		}
	}

	// Token: 0x06000CCB RID: 3275 RVA: 0x0003D7F4 File Offset: 0x0003B9F4
	public void OnDespawned()
	{
		this.bullet = null;
	}

	// Token: 0x04000D7A RID: 3450
	public Bullet bullet;

	// Token: 0x04000D7B RID: 3451
	private int m_firstFrame = -1;
}
