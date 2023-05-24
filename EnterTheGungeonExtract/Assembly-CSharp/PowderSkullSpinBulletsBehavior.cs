using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D34 RID: 3380
public class PowderSkullSpinBulletsBehavior : BehaviorBase
{
	// Token: 0x06004761 RID: 18273 RVA: 0x00175E0C File Offset: 0x0017400C
	public override void Start()
	{
		base.Start();
		this.m_bulletBank = this.m_gameObject.GetComponent<AIBulletBank>();
	}

	// Token: 0x06004762 RID: 18274 RVA: 0x00175E28 File Offset: 0x00174028
	public override void Upkeep()
	{
		base.Upkeep();
		base.DecrementTimer(ref this.m_regenTimer, false);
	}

	// Token: 0x06004763 RID: 18275 RVA: 0x00175E40 File Offset: 0x00174040
	public override BehaviorResult Update()
	{
		base.Update();
		BehaviorResult behaviorResult = base.Update();
		if (behaviorResult != BehaviorResult.Continue)
		{
			return behaviorResult;
		}
		float num = float.MaxValue;
		if (this.m_aiActor && this.m_aiActor.TargetRigidbody)
		{
			num = (this.m_aiActor.TargetRigidbody.GetUnitCenter(ColliderType.HitBox) - this.m_aiActor.specRigidbody.GetUnitCenter(ColliderType.HitBox)).magnitude;
		}
		for (int i = 0; i < this.NumBullets; i++)
		{
			float num2 = Mathf.Lerp(this.BulletMinRadius, this.BulletMaxRadius, (float)i / ((float)this.NumBullets - 1f));
			if (num2 * 2f > num)
			{
				this.m_projectiles.Add(new PowderSkullSpinBulletsBehavior.ProjectileContainer
				{
					projectile = null,
					angle = 0f,
					distFromCenter = num2
				});
				this.m_projectiles.Add(new PowderSkullSpinBulletsBehavior.ProjectileContainer
				{
					projectile = null,
					angle = 180f,
					distFromCenter = num2
				});
			}
			else
			{
				GameObject gameObject = this.m_bulletBank.CreateProjectileFromBank(this.GetBulletPosition(0f, num2), 90f, this.OverrideBulletName, null, false, true, false);
				Projectile projectile = gameObject.GetComponent<Projectile>();
				projectile.specRigidbody.Velocity = Vector2.zero;
				projectile.ManualControl = true;
				if (this.BulletsIgnoreTiles)
				{
					projectile.specRigidbody.CollideWithTileMap = false;
				}
				this.m_projectiles.Add(new PowderSkullSpinBulletsBehavior.ProjectileContainer
				{
					projectile = projectile,
					angle = 0f,
					distFromCenter = num2
				});
				gameObject = this.m_bulletBank.CreateProjectileFromBank(this.GetBulletPosition(180f, num2), -90f, this.OverrideBulletName, null, false, true, false);
				projectile = gameObject.GetComponent<Projectile>();
				projectile.specRigidbody.Velocity = Vector2.zero;
				projectile.ManualControl = true;
				if (this.BulletsIgnoreTiles)
				{
					projectile.specRigidbody.CollideWithTileMap = false;
				}
				this.m_projectiles.Add(new PowderSkullSpinBulletsBehavior.ProjectileContainer
				{
					projectile = projectile,
					angle = 180f,
					distFromCenter = num2
				});
			}
		}
		this.m_updateEveryFrame = true;
		return BehaviorResult.RunContinuousInClass;
	}

	// Token: 0x06004764 RID: 18276 RVA: 0x001760A0 File Offset: 0x001742A0
	public override ContinuousBehaviorResult ContinuousUpdate()
	{
		if (this.m_aiActor)
		{
			bool flag = this.m_aiActor.CanTargetEnemies && !this.m_aiActor.CanTargetPlayers;
			if (this.m_cachedCharm != flag)
			{
				for (int i = 0; i < this.m_projectiles.Count; i++)
				{
					if (this.m_projectiles[i] != null && this.m_projectiles[i].projectile && this.m_projectiles[i].projectile.gameObject.activeSelf)
					{
						this.m_projectiles[i].projectile.DieInAir(false, false, true, false);
						this.m_projectiles[i].projectile = null;
					}
				}
				this.m_cachedCharm = flag;
			}
		}
		for (int j = 0; j < this.m_projectiles.Count; j++)
		{
			if (!this.m_projectiles[j].projectile || !this.m_projectiles[j].projectile.gameObject.activeSelf)
			{
				this.m_projectiles[j].projectile = null;
			}
		}
		for (int k = 0; k < this.m_projectiles.Count; k++)
		{
			float num = this.m_projectiles[k].angle + this.m_deltaTime * (float)this.BulletCircleSpeed;
			this.m_projectiles[k].angle = num;
			Projectile projectile = this.m_projectiles[k].projectile;
			if (projectile)
			{
				Vector2 bulletPosition = this.GetBulletPosition(num, this.m_projectiles[k].distFromCenter);
				projectile.specRigidbody.Velocity = (bulletPosition - projectile.transform.position) / BraveTime.DeltaTime;
				if (projectile.shouldRotate)
				{
					projectile.transform.rotation = Quaternion.Euler(0f, 0f, 180f + (Quaternion.Euler(0f, 0f, 90f) * (this.ShootPoint.transform.position.XY() - bulletPosition)).XY().ToAngle());
				}
				projectile.ResetDistance();
			}
			else if (this.m_regenTimer <= 0f)
			{
				Vector2 bulletPosition2 = this.GetBulletPosition(this.m_projectiles[k].angle, this.m_projectiles[k].distFromCenter);
				if (GameManager.Instance.Dungeon.CellExists(bulletPosition2) && !GameManager.Instance.Dungeon.data.isWall((int)bulletPosition2.x, (int)bulletPosition2.y))
				{
					GameObject gameObject = this.m_bulletBank.CreateProjectileFromBank(bulletPosition2, 0f, this.OverrideBulletName, null, false, true, false);
					projectile = gameObject.GetComponent<Projectile>();
					projectile.specRigidbody.Velocity = Vector2.zero;
					projectile.ManualControl = true;
					if (this.BulletsIgnoreTiles)
					{
						projectile.specRigidbody.CollideWithTileMap = false;
					}
					this.m_projectiles[k].projectile = projectile;
					this.m_regenTimer = this.RegenTimer;
				}
			}
		}
		for (int l = 0; l < this.m_projectiles.Count; l++)
		{
			if (this.m_projectiles[l] != null && this.m_projectiles[l].projectile)
			{
				bool flag2 = this.m_aiActor && this.m_aiActor.CanTargetEnemies;
				this.m_projectiles[l].projectile.collidesWithEnemies = this.m_projectiles[l].projectile.collidesWithEnemies || flag2;
			}
		}
		return ContinuousBehaviorResult.Continue;
	}

	// Token: 0x06004765 RID: 18277 RVA: 0x001764BC File Offset: 0x001746BC
	public override void EndContinuousUpdate()
	{
		base.EndContinuousUpdate();
		this.DestroyProjectiles();
		this.m_updateEveryFrame = false;
	}

	// Token: 0x06004766 RID: 18278 RVA: 0x001764D4 File Offset: 0x001746D4
	public override void OnActorPreDeath()
	{
		base.OnActorPreDeath();
		this.DestroyProjectiles();
	}

	// Token: 0x06004767 RID: 18279 RVA: 0x001764E4 File Offset: 0x001746E4
	public override void Destroy()
	{
		base.Destroy();
	}

	// Token: 0x06004768 RID: 18280 RVA: 0x001764EC File Offset: 0x001746EC
	private Vector2 GetBulletPosition(float angle, float distFromCenter)
	{
		return this.ShootPoint.transform.position.XY() + BraveMathCollege.DegreesToVector(angle, distFromCenter);
	}

	// Token: 0x06004769 RID: 18281 RVA: 0x00176510 File Offset: 0x00174710
	private void DestroyProjectiles()
	{
		for (int i = 0; i < this.m_projectiles.Count; i++)
		{
			Projectile projectile = this.m_projectiles[i].projectile;
			if (projectile != null)
			{
				projectile.DieInAir(false, true, true, false);
			}
		}
		this.m_projectiles.Clear();
	}

	// Token: 0x04003A59 RID: 14937
	public string OverrideBulletName;

	// Token: 0x04003A5A RID: 14938
	public GameObject ShootPoint;

	// Token: 0x04003A5B RID: 14939
	public int NumBullets;

	// Token: 0x04003A5C RID: 14940
	public float BulletMinRadius;

	// Token: 0x04003A5D RID: 14941
	public float BulletMaxRadius;

	// Token: 0x04003A5E RID: 14942
	public int BulletCircleSpeed;

	// Token: 0x04003A5F RID: 14943
	public bool BulletsIgnoreTiles;

	// Token: 0x04003A60 RID: 14944
	public float RegenTimer;

	// Token: 0x04003A61 RID: 14945
	private readonly List<PowderSkullSpinBulletsBehavior.ProjectileContainer> m_projectiles = new List<PowderSkullSpinBulletsBehavior.ProjectileContainer>();

	// Token: 0x04003A62 RID: 14946
	private AIBulletBank m_bulletBank;

	// Token: 0x04003A63 RID: 14947
	private bool m_cachedCharm;

	// Token: 0x04003A64 RID: 14948
	private float m_regenTimer;

	// Token: 0x02000D35 RID: 3381
	private class ProjectileContainer
	{
		// Token: 0x04003A65 RID: 14949
		public Projectile projectile;

		// Token: 0x04003A66 RID: 14950
		public float angle;

		// Token: 0x04003A67 RID: 14951
		public float distFromCenter;
	}
}
