using System;
using System.Collections;
using Brave.BulletScript;
using UnityEngine;

// Token: 0x02000326 RID: 806
public class WizardPurpleHomingShots1 : Script
{
	// Token: 0x06000C75 RID: 3189 RVA: 0x0003BF74 File Offset: 0x0003A174
	protected override IEnumerator Top()
	{
		base.EndOnBlank = true;
		BraveUtility.RandomizeArray<int>(this.m_bullets, 0, -1);
		yield return base.Wait(15);
		HealthHaver healthHaver = base.BulletBank.healthHaver;
		float startingHealth = healthHaver.GetCurrentHealth();
		for (int i = 0; i < 3; i++)
		{
			healthHaver.aiAnimator.PlayVfx("fire", null, null, null);
			yield return base.Wait(28);
			Bullet newBullet;
			switch (this.m_bullets[i])
			{
			case 0:
				newBullet = new WizardPurpleHomingShots1.BardBullet();
				break;
			case 1:
				newBullet = new WizardPurpleHomingShots1.KnightBullet();
				break;
			case 2:
				newBullet = new WizardPurpleHomingShots1.MageBullet();
				break;
			default:
				newBullet = new WizardPurpleHomingShots1.RogueBullet();
				break;
			}
			base.Fire(new Direction(90f, DirectionType.Absolute, -1f), new Speed(1f, SpeedType.Absolute), newBullet);
			for (int j = 0; j < 45; j++)
			{
				if (!healthHaver || healthHaver.IsDead || healthHaver.GetCurrentHealth() < startingHealth || WizardPurpleHomingShots1.HasLostTarget(this))
				{
					base.ForceEnd();
					yield break;
				}
				yield return base.Wait(1);
			}
		}
		yield break;
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x0003BF90 File Offset: 0x0003A190
	protected static bool HasLostTarget(Bullet bullet)
	{
		AIActor aiActor = bullet.BulletBank.aiActor;
		return aiActor && !aiActor.TargetRigidbody && aiActor.CanTargetEnemies && !aiActor.CanTargetPlayers;
	}

	// Token: 0x04000D20 RID: 3360
	private const int NumBullets = 3;

	// Token: 0x04000D21 RID: 3361
	private const int Delay = 45;

	// Token: 0x04000D22 RID: 3362
	private const int AirTime = 300;

	// Token: 0x04000D23 RID: 3363
	private int[] m_bullets = new int[] { 0, 1, 2, 3 };

	// Token: 0x02000327 RID: 807
	public class StoryBullet : Bullet
	{
		// Token: 0x06000C77 RID: 3191 RVA: 0x0003BFE0 File Offset: 0x0003A1E0
		public StoryBullet(string name, float horizontalOffset)
			: base(name, false, false, false)
		{
			this.horizontalOffset = horizontalOffset;
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x0003BFF4 File Offset: 0x0003A1F4
		public float OffsetAimDirection
		{
			get
			{
				Vector2 vector = this.BulletManager.PlayerPosition();
				Vector2 position = base.Position;
				Vector2 vector2 = vector - position;
				Vector2 vector3 = ((vector2.magnitude >= Mathf.Abs(this.horizontalOffset) * 2f) ? (vector2.Rotate(90f).normalized * this.horizontalOffset) : Vector2.zero);
				return (vector + vector3 - position).ToAngle();
			}
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x0003C078 File Offset: 0x0003A278
		public bool HasLostTarget()
		{
			return WizardPurpleHomingShots1.HasLostTarget(this);
		}

		// Token: 0x04000D24 RID: 3364
		public float horizontalOffset;
	}

	// Token: 0x02000328 RID: 808
	public class KnightBullet : WizardPurpleHomingShots1.StoryBullet
	{
		// Token: 0x06000C7A RID: 3194 RVA: 0x0003C080 File Offset: 0x0003A280
		public KnightBullet()
			: base("knight", 1.5f)
		{
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x0003C094 File Offset: 0x0003A294
		protected override IEnumerator Top()
		{
			yield return base.Wait(60);
			this.Speed = 7f;
			this.Direction = base.OffsetAimDirection;
			for (int i = 0; i < 300; i++)
			{
				if (base.HasLostTarget())
				{
					base.Vanish(false);
					yield break;
				}
				base.ChangeDirection(new Direction(base.OffsetAimDirection, DirectionType.Absolute, 3f), 1);
				if (this.Projectile)
				{
					this.Projectile.Direction = BraveMathCollege.DegreesToVector(this.Direction, 1f);
				}
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}
	}

	// Token: 0x0200032A RID: 810
	public class MageBullet : WizardPurpleHomingShots1.StoryBullet
	{
		// Token: 0x06000C82 RID: 3202 RVA: 0x0003C24C File Offset: 0x0003A44C
		public MageBullet()
			: base("mage", 0.5f)
		{
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x0003C260 File Offset: 0x0003A460
		protected override IEnumerator Top()
		{
			tk2dSpriteAnimator spriteAnimator = this.Projectile.spriteAnimator;
			yield return base.Wait(60);
			this.Speed = 2f;
			this.Direction = base.OffsetAimDirection;
			int shotsFired = 0;
			int cooldown = 60;
			for (int i = 0; i < 300; i++)
			{
				if (base.HasLostTarget())
				{
					base.Vanish(false);
					yield break;
				}
				if (!spriteAnimator.Playing)
				{
					spriteAnimator.Play(spriteAnimator.DefaultClip);
				}
				if (shotsFired < 3)
				{
					cooldown--;
					if (cooldown == 12)
					{
						spriteAnimator.Play("enemy_projectile_mage_fire");
					}
					if (cooldown <= 0)
					{
						float num = BraveMathCollege.ClampAngle360(this.Direction);
						int num2 = ((num <= 90f || num >= 270f) ? 1 : (-1));
						base.Fire(new Offset(PhysicsEngine.PixelToUnit(new IntVector2(num2 * 5, 12)), 0f, string.Empty, DirectionType.Absolute), new Direction(base.GetAimDirection(1f, 10f), DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), new Bullet("mage_fireball", false, false, false));
						shotsFired++;
						cooldown = 60;
					}
				}
				else if (shotsFired == 3 && cooldown > 0)
				{
					cooldown--;
					if (cooldown == 0)
					{
						this.Speed = 7f;
					}
				}
				base.ChangeDirection(new Direction(base.OffsetAimDirection, DirectionType.Absolute, 3f), 1);
				if (this.Projectile)
				{
					this.Projectile.Direction = BraveMathCollege.DegreesToVector(this.Direction, 1f);
				}
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x04000D2A RID: 3370
		private const int ShotCooldown = 60;

		// Token: 0x04000D2B RID: 3371
		private const int NumBullets = 3;
	}

	// Token: 0x0200032C RID: 812
	public class BardBullet : WizardPurpleHomingShots1.StoryBullet
	{
		// Token: 0x06000C8A RID: 3210 RVA: 0x0003C594 File Offset: 0x0003A794
		public BardBullet()
			: base("bard", -1f)
		{
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0003C5A8 File Offset: 0x0003A7A8
		protected override IEnumerator Top()
		{
			this.m_bounceMod = this.Projectile.GetComponent<BounceProjModifier>();
			this.m_bounceMod.OnBounce += this.OnBounce;
			yield return base.Wait(60);
			this.Speed = 7f;
			this.Direction = base.OffsetAimDirection;
			for (int i = 0; i < 300; i++)
			{
				if (base.HasLostTarget())
				{
					base.Vanish(false);
					yield break;
				}
				int turnSpeed = 3;
				if (this.m_noTurnTimer > 0)
				{
					turnSpeed = (int)Mathf.Lerp(0f, 3f, 1f - (float)this.m_noTurnTimer / 60f);
					this.m_noTurnTimer--;
				}
				float offsetAimDirection = base.OffsetAimDirection;
				float num = (float)turnSpeed;
				base.ChangeDirection(new Direction(offsetAimDirection, DirectionType.Absolute, num), 1);
				if (this.Projectile)
				{
					this.Projectile.Direction = BraveMathCollege.DegreesToVector(this.Direction, 1f);
				}
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0003C5C4 File Offset: 0x0003A7C4
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (this.m_bounceMod)
			{
				this.m_bounceMod.OnBounce -= this.OnBounce;
			}
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0003C5F0 File Offset: 0x0003A7F0
		private void OnBounce()
		{
			this.m_noTurnTimer = 60;
		}

		// Token: 0x04000D34 RID: 3380
		private const int NoTurnTime = 60;

		// Token: 0x04000D35 RID: 3381
		private BounceProjModifier m_bounceMod;

		// Token: 0x04000D36 RID: 3382
		private int m_noTurnTimer;
	}

	// Token: 0x0200032E RID: 814
	public class RogueBullet : WizardPurpleHomingShots1.StoryBullet
	{
		// Token: 0x06000C94 RID: 3220 RVA: 0x0003C834 File Offset: 0x0003AA34
		public RogueBullet()
			: base("rogue", -2f)
		{
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x0003C848 File Offset: 0x0003AA48
		protected override IEnumerator Top()
		{
			this.m_teleportMod = this.Projectile.GetComponent<TeleportProjModifier>();
			this.m_teleportMod.enabled = false;
			this.m_teleportMod.OnTeleport += this.OnTeleport;
			yield return base.Wait(60);
			this.Speed = 7f;
			this.Direction = base.OffsetAimDirection;
			this.m_teleportMod.enabled = true;
			int lifetime = 300;
			for (int i = 0; i < lifetime; i++)
			{
				if (base.HasLostTarget())
				{
					base.Vanish(false);
					yield break;
				}
				if (this.m_clampLifetime)
				{
					if (lifetime > i + 80)
					{
						lifetime = i + 80;
					}
					this.m_clampLifetime = false;
				}
				base.ChangeDirection(new Direction(base.OffsetAimDirection, DirectionType.Absolute, 3f), 1);
				if (this.Projectile)
				{
					this.Projectile.Direction = BraveMathCollege.DegreesToVector(this.Direction, 1f);
				}
				yield return base.Wait(1);
			}
			base.Vanish(false);
			yield break;
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x0003C864 File Offset: 0x0003AA64
		public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
			if (this.m_teleportMod)
			{
				this.m_teleportMod.OnTeleport -= this.OnTeleport;
			}
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0003C890 File Offset: 0x0003AA90
		private void OnTeleport()
		{
			this.m_clampLifetime = true;
		}

		// Token: 0x04000D3D RID: 3389
		private const int ClampedLifetime = 80;

		// Token: 0x04000D3E RID: 3390
		private TeleportProjModifier m_teleportMod;

		// Token: 0x04000D3F RID: 3391
		private bool m_clampLifetime;
	}
}
