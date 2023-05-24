using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brave.BulletScript
{
	// Token: 0x02000344 RID: 836
	public class Bullet
	{
		// Token: 0x06000CF6 RID: 3318 RVA: 0x0003E0B4 File Offset: 0x0003C2B4
		public Bullet(string bankName = null, bool suppressVfx = false, bool firstBulletOfAttack = false, bool forceBlackBullet = false)
		{
			this.BankName = bankName;
			this.SuppressVfx = suppressVfx;
			this.FirstBulletOfAttack = firstBulletOfAttack;
			this.ForceBlackBullet = forceBlackBullet;
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x0003E0F0 File Offset: 0x0003C2F0
		// (set) Token: 0x06000CF8 RID: 3320 RVA: 0x0003E0F8 File Offset: 0x0003C2F8
		public Transform RootTransform { get; set; }

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x0003E104 File Offset: 0x0003C304
		// (set) Token: 0x06000CFA RID: 3322 RVA: 0x0003E10C File Offset: 0x0003C30C
		public Vector2 Position { get; set; }

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x0003E118 File Offset: 0x0003C318
		public Vector2 PredictedPosition
		{
			get
			{
				return new Vector2(this.Position.x + this.m_timer * this.Velocity.x, this.Position.y + this.m_timer * this.Velocity.y);
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000CFC RID: 3324 RVA: 0x0003E16C File Offset: 0x0003C36C
		public AIBulletBank BulletBank
		{
			get
			{
				return this.BulletManager as AIBulletBank;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x0003E17C File Offset: 0x0003C37C
		// (set) Token: 0x06000CFE RID: 3326 RVA: 0x0003E184 File Offset: 0x0003C384
		public bool ManualControl { get; set; }

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x0003E190 File Offset: 0x0003C390
		// (set) Token: 0x06000D00 RID: 3328 RVA: 0x0003E198 File Offset: 0x0003C398
		public bool DisableMotion { get; set; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x0003E1A4 File Offset: 0x0003C3A4
		// (set) Token: 0x06000D02 RID: 3330 RVA: 0x0003E1AC File Offset: 0x0003C3AC
		public bool Destroyed { get; set; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x0003E1B8 File Offset: 0x0003C3B8
		// (set) Token: 0x06000D04 RID: 3332 RVA: 0x0003E1C0 File Offset: 0x0003C3C0
		public bool SuppressVfx { get; set; }

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x0003E1CC File Offset: 0x0003C3CC
		// (set) Token: 0x06000D06 RID: 3334 RVA: 0x0003E1D4 File Offset: 0x0003C3D4
		public bool FirstBulletOfAttack { get; set; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x0003E1E0 File Offset: 0x0003C3E0
		// (set) Token: 0x06000D08 RID: 3336 RVA: 0x0003E1E8 File Offset: 0x0003C3E8
		public bool ForceBlackBullet { get; set; }

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x0003E1F4 File Offset: 0x0003C3F4
		// (set) Token: 0x06000D0A RID: 3338 RVA: 0x0003E1FC File Offset: 0x0003C3FC
		public bool EndOnBlank { get; set; }

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x0003E208 File Offset: 0x0003C408
		// (set) Token: 0x06000D0C RID: 3340 RVA: 0x0003E210 File Offset: 0x0003C410
		public int Tick { get; set; }

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x0003E21C File Offset: 0x0003C41C
		public float AimDirection
		{
			get
			{
				return (this.BulletManager.PlayerPosition() - this.Position).ToAngle();
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x0003E23C File Offset: 0x0003C43C
		public bool IsOwnerAlive
		{
			get
			{
				AIActor aiactor = null;
				if (this.BulletBank)
				{
					aiactor = this.BulletBank.aiActor;
				}
				if (!aiactor && this.Projectile)
				{
					aiactor = this.Projectile.Owner as AIActor;
				}
				return aiactor && aiactor.healthHaver && aiactor.healthHaver.IsAlive;
			}
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0003E2BC File Offset: 0x0003C4BC
		public virtual void Initialize()
		{
			IEnumerator enumerator = this.Top();
			if (enumerator != null)
			{
				this.m_tasks.Add(new Bullet.Task(enumerator));
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x0003E2E8 File Offset: 0x0003C4E8
		private float LocalDeltaTime
		{
			get
			{
				if (this.Projectile)
				{
					return this.Projectile.LocalDeltaTime;
				}
				if (this.BulletBank && this.BulletBank.aiActor)
				{
					return this.BulletBank.aiActor.LocalDeltaTime;
				}
				return BraveTime.DeltaTime;
			}
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0003E34C File Offset: 0x0003C54C
		public void FrameUpdate()
		{
			this.m_timer += this.LocalDeltaTime * this.TimeScale * Projectile.EnemyBulletSpeedMultiplier;
			while (this.m_timer > 0.016666668f)
			{
				this.m_timer -= 0.016666668f;
				this.DoTick();
			}
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0003E3A8 File Offset: 0x0003C5A8
		public void DoTick()
		{
			for (int i = 0; i < this.m_tasks.Count; i++)
			{
				if (this.m_tasks[i] != null)
				{
					bool flag;
					this.m_tasks[i].Tick(out flag);
					if (flag && i < this.m_tasks.Count)
					{
						this.m_tasks[i] = null;
					}
				}
			}
			this.Tick++;
			if (!this.ManualControl)
			{
				this.UpdateVelocity();
				this.UpdatePosition();
			}
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0003E444 File Offset: 0x0003C644
		public void HandleBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool allowProjectileSpawns)
		{
			this.Destroyed = true;
			bool flag = !allowProjectileSpawns;
			flag |= destroyType == Bullet.DestroyType.HitRigidbody && hitRigidbody && hitRigidbody.GetComponent<PlayerOrbital>();
			this.OnBulletDestruction(destroyType, hitRigidbody, flag);
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0003E48C File Offset: 0x0003C68C
		public virtual void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
		{
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x0003E490 File Offset: 0x0003C690
		public bool IsEnded
		{
			get
			{
				for (int i = 0; i < this.m_tasks.Count; i++)
				{
					if (this.m_tasks[i] != null)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0003E4D0 File Offset: 0x0003C6D0
		public void ForceEnd()
		{
			this.OnForceEnded();
			this.m_tasks.Clear();
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0003E4E4 File Offset: 0x0003C6E4
		public virtual void OnForceEnded()
		{
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0003E4E8 File Offset: 0x0003C6E8
		public virtual void OnForceRemoved()
		{
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0003E4EC File Offset: 0x0003C6EC
		public float GetAimDirection(string transform)
		{
			Vector2 vector = this.BulletManager.TransformOffset(this.Position, transform);
			Vector2 vector2 = this.BulletManager.PlayerPosition();
			return (vector2 - vector).ToAngle();
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0003E524 File Offset: 0x0003C724
		public float GetAimDirection(float leadAmount, float speed)
		{
			return this.GetAimDirection(this.Position, leadAmount, speed);
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0003E534 File Offset: 0x0003C734
		public float GetAimDirection(string transform, float leadAmount, float speed)
		{
			return this.GetAimDirection(this.BulletManager.TransformOffset(this.Position, transform), leadAmount, speed);
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0003E550 File Offset: 0x0003C750
		public float GetAimDirection(Vector2 position, float leadAmount, float speed)
		{
			Vector2 vector = this.BulletManager.PlayerPosition();
			Vector2 predictedPosition = BraveMathCollege.GetPredictedPosition(vector, this.BulletManager.PlayerVelocity(), position, speed);
			vector = new Vector2(vector.x + (predictedPosition.x - vector.x) * leadAmount, vector.y + (predictedPosition.y - vector.y) * leadAmount);
			return (vector - position).ToAngle();
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0003E5C4 File Offset: 0x0003C7C4
		public Vector2 GetPredictedTargetPosition(float leadAmount, float speed)
		{
			Vector2 position = this.Position;
			Vector2 vector = this.BulletManager.PlayerPosition();
			Vector2 predictedPosition = BraveMathCollege.GetPredictedPosition(vector, this.BulletManager.PlayerVelocity(), position, speed);
			vector = new Vector2(vector.x + (predictedPosition.x - vector.x) * leadAmount, vector.y + (predictedPosition.y - vector.y) * leadAmount);
			return vector;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0003E634 File Offset: 0x0003C834
		public Vector2 GetPredictedTargetPositionExact(float leadAmount, float speed)
		{
			this.BulletBank.SuppressPlayerVelocityAveraging = true;
			Vector2 position = this.Position;
			Vector2 vector = this.BulletManager.PlayerPosition();
			Vector2 predictedPosition = BraveMathCollege.GetPredictedPosition(vector, this.BulletManager.PlayerVelocity(), position, speed);
			vector = new Vector2(vector.x + (predictedPosition.x - vector.x) * leadAmount, vector.y + (predictedPosition.y - vector.y) * leadAmount);
			this.BulletBank.SuppressPlayerVelocityAveraging = false;
			return vector;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0003E6BC File Offset: 0x0003C8BC
		public void PostWwiseEvent(string AudioEvent, string SwitchName = null)
		{
			if (this.BulletBank)
			{
				this.BulletBank.PostWwiseEvent(AudioEvent, SwitchName);
			}
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0003E6DC File Offset: 0x0003C8DC
		public void Fire(Bullet bullet = null)
		{
			this.Fire(null, null, null, bullet);
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0003E6E8 File Offset: 0x0003C8E8
		public void Fire(Offset offset = null, Bullet bullet = null)
		{
			this.Fire(offset, null, null, bullet);
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0003E6F4 File Offset: 0x0003C8F4
		public void Fire(Offset offset = null, Speed speed = null, Bullet bullet = null)
		{
			this.Fire(offset, null, speed, bullet);
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0003E700 File Offset: 0x0003C900
		public void Fire(Offset offset = null, Direction direction = null, Bullet bullet = null)
		{
			this.Fire(offset, direction, null, bullet);
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0003E70C File Offset: 0x0003C90C
		public void Fire(Direction direction = null, Bullet bullet = null)
		{
			this.Fire(null, direction, null, bullet);
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0003E718 File Offset: 0x0003C918
		public void Fire(Direction direction = null, Speed speed = null, Bullet bullet = null)
		{
			this.Fire(null, direction, speed, bullet);
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0003E724 File Offset: 0x0003C924
		public void Fire(Offset offset = null, Direction direction = null, Speed speed = null, Bullet bullet = null)
		{
			if (bullet == null)
			{
				bullet = new Bullet(null, false, false, false);
			}
			if (!this.m_hasFiredBullet)
			{
				bullet.FirstBulletOfAttack = true;
			}
			bullet.BulletManager = this.BulletManager;
			if (this is Script)
			{
				bullet.RootTransform = this.RootTransform;
			}
			bullet.Position = this.Position;
			bullet.Direction = this.Direction;
			bullet.Speed = this.Speed;
			bullet.m_timer = this.m_timer - this.LocalDeltaTime;
			bullet.EndOnBlank = this.EndOnBlank;
			float? num = null;
			if (offset != null)
			{
				num = offset.GetDirection(this);
				if (!string.IsNullOrEmpty(offset.transform))
				{
					bullet.SpawnTransform = offset.transform;
					Transform transform = this.BulletBank.GetTransform(offset.transform);
					if (transform)
					{
						bullet.RootTransform = transform;
					}
				}
			}
			bullet.Position = ((offset == null) ? this.Position : offset.GetPosition(this));
			bullet.Direction = ((direction == null) ? 0f : direction.GetDirection(this, num));
			bullet.Speed = ((speed == null) ? 0f : speed.GetSpeed(this));
			this.BulletManager.BulletSpawnedHandler(bullet);
			if (this.Projectile && this.Projectile.IsBlackBullet && bullet.Projectile)
			{
				bullet.Projectile.ForceBlackBullet = true;
				bullet.Projectile.BecomeBlackBullet();
			}
			this.m_hasFiredBullet = true;
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x0003E8D8 File Offset: 0x0003CAD8
		protected void ChangeSpeed(Speed speed, int term = 1)
		{
			if (term <= 1)
			{
				this.Speed = speed.GetSpeed(this);
			}
			else
			{
				this.m_tasks.Add(new Bullet.Task(this.ChangeSpeedTask(speed, term)));
			}
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0003E90C File Offset: 0x0003CB0C
		protected void ChangeDirection(Direction direction, int term = 1)
		{
			if (term <= 1)
			{
				this.Direction = direction.GetDirection(this, null);
			}
			else
			{
				this.m_tasks.Add(new Bullet.Task(this.ChangeDirectionTask(direction, term)));
			}
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0003E954 File Offset: 0x0003CB54
		protected void StartTask(IEnumerator enumerator)
		{
			this.m_tasks.Add(new Bullet.Task(enumerator));
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0003E968 File Offset: 0x0003CB68
		protected int Wait(int frames)
		{
			return frames;
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0003E96C File Offset: 0x0003CB6C
		protected int Wait(float frames)
		{
			return Mathf.CeilToInt(frames);
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0003E974 File Offset: 0x0003CB74
		public void Vanish(bool suppressInAirEffects = false)
		{
			this.Destroyed = true;
			this.BulletManager.DestroyBullet(this, suppressInAirEffects);
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0003E98C File Offset: 0x0003CB8C
		protected virtual IEnumerator Top()
		{
			return null;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0003E990 File Offset: 0x0003CB90
		protected void UpdateVelocity()
		{
			float num = this.Direction * 0.017453292f;
			this.Velocity.x = Mathf.Cos(num) * this.Speed;
			this.Velocity.y = Mathf.Sin(num) * this.Speed;
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0003E9DC File Offset: 0x0003CBDC
		protected void UpdatePosition()
		{
			Vector2 position = this.Position;
			position.x += this.Velocity.x / 60f;
			position.y += this.Velocity.y / 60f;
			this.Position = position;
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0003EA38 File Offset: 0x0003CC38
		protected float RandomAngle()
		{
			return (float)UnityEngine.Random.Range(0, 360);
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0003EA48 File Offset: 0x0003CC48
		protected float SubdivideRange(float startValue, float endValue, int numDivisions, int i, bool offset = false)
		{
			return Mathf.Lerp(startValue, endValue, ((float)i + ((!offset) ? 0f : 0.5f)) / (float)(numDivisions - 1));
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x0003EA70 File Offset: 0x0003CC70
		protected float SubdivideArc(float startAngle, float sweepAngle, int numBullets, int i, bool offset = false)
		{
			return startAngle + Mathf.Lerp(0f, sweepAngle, ((float)i + ((!offset) ? 0f : 0.5f)) / (float)(numBullets - 1));
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0003EAB0 File Offset: 0x0003CCB0
		protected float SubdivideCircle(float startAngle, int numBullets, int i, float direction = 1f, bool offset = false)
		{
			return startAngle + direction * Mathf.Lerp(0f, 360f, ((float)i + ((!offset) ? 0f : 0.5f)) / (float)numBullets);
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0003EAF4 File Offset: 0x0003CCF4
		protected bool IsPointInTile(Vector2 point)
		{
			if (!GameManager.Instance.Dungeon.data.CheckInBoundsAndValid((int)point.x, (int)point.y))
			{
				return true;
			}
			int num = CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.BulletBlocker);
			SpeculativeRigidbody speculativeRigidbody;
			return PhysicsEngine.Instance.Pointcast(point, out speculativeRigidbody, true, false, num, new CollisionLayer?(CollisionLayer.Projectile), false, new SpeculativeRigidbody[0]);
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0003EB54 File Offset: 0x0003CD54
		private IEnumerator ChangeSpeedTask(Speed speed, int term)
		{
			float delta;
			if (speed.type == SpeedType.Sequence)
			{
				delta = speed.speed;
			}
			else
			{
				delta = (speed.GetSpeed(this) - this.Speed) / (float)term;
			}
			for (int i = 0; i < term; i++)
			{
				this.Speed += delta;
				yield return this.Wait(1);
			}
			yield break;
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0003EB80 File Offset: 0x0003CD80
		private IEnumerator ChangeDirectionTask(Direction direction, int term)
		{
			float delta;
			if (direction.type == DirectionType.Sequence)
			{
				delta = direction.direction;
			}
			else
			{
				delta = BraveMathCollege.ClampAngle180(direction.GetDirection(this, null) - this.Direction) / (float)term;
			}
			if (direction.maxFrameDelta >= 0f)
			{
				delta = Mathf.Clamp(delta, -direction.maxFrameDelta, direction.maxFrameDelta);
			}
			for (int i = 0; i < term; i++)
			{
				this.Direction += delta;
				yield return this.Wait(1);
			}
			yield break;
		}

		// Token: 0x04000DA0 RID: 3488
		public string BankName;

		// Token: 0x04000DA2 RID: 3490
		public string SpawnTransform;

		// Token: 0x04000DA3 RID: 3491
		public float Direction;

		// Token: 0x04000DA4 RID: 3492
		public float Speed;

		// Token: 0x04000DA5 RID: 3493
		public Vector2 Velocity;

		// Token: 0x04000DA6 RID: 3494
		public bool AutoRotation;

		// Token: 0x04000DA8 RID: 3496
		public float TimeScale = 1f;

		// Token: 0x04000DA9 RID: 3497
		public IBulletManager BulletManager;

		// Token: 0x04000DAA RID: 3498
		public GameObject Parent;

		// Token: 0x04000DAB RID: 3499
		public Projectile Projectile;

		// Token: 0x04000DAC RID: 3500
		public bool DontDestroyGameObject;

		// Token: 0x04000DB5 RID: 3509
		protected readonly List<Bullet.ITask> m_tasks = new List<Bullet.ITask>();

		// Token: 0x04000DB6 RID: 3510
		private float m_timer;

		// Token: 0x04000DB7 RID: 3511
		private bool m_hasFiredBullet;

		// Token: 0x04000DB8 RID: 3512
		private const float c_idealFrameTime = 0.016666668f;

		// Token: 0x02000345 RID: 837
		public enum DestroyType
		{
			// Token: 0x04000DBA RID: 3514
			DieInAir,
			// Token: 0x04000DBB RID: 3515
			HitRigidbody,
			// Token: 0x04000DBC RID: 3516
			HitTile
		}

		// Token: 0x02000346 RID: 838
		protected interface ITask
		{
			// Token: 0x06000D37 RID: 3383
			void Tick(out bool isFinished);
		}

		// Token: 0x02000347 RID: 839
		protected class Task : Bullet.ITask
		{
			// Token: 0x06000D38 RID: 3384 RVA: 0x0003EBAC File Offset: 0x0003CDAC
			public Task(IEnumerator enumerator)
			{
				this.m_currentEnum = enumerator;
			}

			// Token: 0x06000D39 RID: 3385 RVA: 0x0003EBBC File Offset: 0x0003CDBC
			public void Tick(out bool isFinished)
			{
				if (this.m_wait > 0)
				{
					this.m_wait--;
					isFinished = false;
					return;
				}
				if (!this.m_currentEnum.MoveNext())
				{
					if (this.m_enumStack != null && this.m_enumStack.Count != 1)
					{
						this.m_enumStack.RemoveAt(this.m_enumStack.Count - 1);
						this.m_currentEnum = this.m_enumStack[this.m_enumStack.Count - 1];
						this.Tick(out isFinished);
						return;
					}
					isFinished = true;
				}
				else
				{
					isFinished = false;
					object obj = this.m_currentEnum.Current;
					if (obj is int)
					{
						this.m_wait = (int)obj - 1;
						return;
					}
					if (obj == null)
					{
						return;
					}
					if (obj is IEnumerator)
					{
						if (this.m_enumStack == null)
						{
							this.m_enumStack = new List<IEnumerator>();
							this.m_enumStack.Add(this.m_currentEnum);
						}
						this.m_enumStack.Add(obj as IEnumerator);
						this.m_currentEnum = obj as IEnumerator;
						this.Tick(out isFinished);
						return;
					}
					Debug.LogError("Unknown return type from BulletScript: " + obj);
				}
			}

			// Token: 0x04000DBD RID: 3517
			private int m_wait;

			// Token: 0x04000DBE RID: 3518
			private IEnumerator m_currentEnum;

			// Token: 0x04000DBF RID: 3519
			private List<IEnumerator> m_enumStack;
		}

		// Token: 0x02000348 RID: 840
		protected class TaskLite : Bullet.ITask
		{
			// Token: 0x06000D3A RID: 3386 RVA: 0x0003ECF8 File Offset: 0x0003CEF8
			public TaskLite(BulletLite bullet)
			{
				this.m_currentBullet = bullet;
			}

			// Token: 0x06000D3B RID: 3387 RVA: 0x0003ED08 File Offset: 0x0003CF08
			public void Tick(out bool isFinished)
			{
				if (this.m_wait > 0)
				{
					this.m_wait--;
					isFinished = false;
					return;
				}
				if (this.m_currentBullet.Tick == 0)
				{
					this.m_currentBullet.Start();
				}
				int num = this.m_currentBullet.Update(ref this.m_state);
				if (num == -1)
				{
					isFinished = true;
				}
				else
				{
					isFinished = false;
					this.m_wait = num - 1;
				}
			}

			// Token: 0x04000DC0 RID: 3520
			private int m_wait;

			// Token: 0x04000DC1 RID: 3521
			private BulletLite m_currentBullet;

			// Token: 0x04000DC2 RID: 3522
			private int m_state;
		}
	}
}
