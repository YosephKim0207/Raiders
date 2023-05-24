using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200111C RID: 4380
public class CartTurretController : BraveBehaviour
{
	// Token: 0x17000E38 RID: 3640
	// (get) Token: 0x060060A5 RID: 24741 RVA: 0x00253258 File Offset: 0x00251458
	public bool Inactive
	{
		get
		{
			return !this.m_active;
		}
	}

	// Token: 0x060060A6 RID: 24742 RVA: 0x00253264 File Offset: 0x00251464
	private void Start()
	{
		this.m_parentRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
		this.m_parentRoom.Entered += this.Activate;
	}

	// Token: 0x060060A7 RID: 24743 RVA: 0x002532B4 File Offset: 0x002514B4
	private void Activate(PlayerController p)
	{
		if (this.m_active)
		{
			return;
		}
		if (this.m_parentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
		{
			this.m_active = true;
			this.m_awakeTimer = this.AwakeTimer;
			RoomHandler parentRoom = this.m_parentRoom;
			parentRoom.OnEnemiesCleared = (Action)Delegate.Combine(parentRoom.OnEnemiesCleared, new Action(this.HandleEnemiesCleared));
		}
	}

	// Token: 0x060060A8 RID: 24744 RVA: 0x00253318 File Offset: 0x00251518
	private void HandleEnemiesCleared()
	{
		this.m_active = false;
		RoomHandler parentRoom = this.m_parentRoom;
		parentRoom.OnEnemiesCleared = (Action)Delegate.Remove(parentRoom.OnEnemiesCleared, new Action(this.HandleEnemiesCleared));
	}

	// Token: 0x060060A9 RID: 24745 RVA: 0x00253348 File Offset: 0x00251548
	private void Update()
	{
		if (this.m_active)
		{
			this.m_awakeTimer -= BraveTime.DeltaTime;
			if (this.m_awakeTimer > 0f)
			{
				return;
			}
			this.m_acquisitionTimer -= BraveTime.DeltaTime;
			if (this.m_targetPlayer != null && this.m_targetPlayer.healthHaver.IsDead)
			{
				this.m_targetPlayer = null;
			}
			if (this.m_targetPlayer == null || this.m_acquisitionTimer <= 0f)
			{
				this.AcquireTarget();
			}
			if (this.m_targetPlayer != null)
			{
				this.FaceTarget();
				this.Fire();
			}
		}
	}

	// Token: 0x060060AA RID: 24746 RVA: 0x00253408 File Offset: 0x00251608
	private void Fire()
	{
		this.m_fireTimer -= BraveTime.DeltaTime;
		if (this.m_fireTimer <= 0f)
		{
			float num = (this.m_targetPlayer.CenterPosition - base.transform.position.XY()).ToAngle();
			if (float.IsNaN(num) || float.IsInfinity(num))
			{
				return;
			}
			this.FireBullet(this.BarrelTransform, num, "default");
			this.m_fireTimer = this.TimeBetweenShots;
		}
	}

	// Token: 0x060060AB RID: 24747 RVA: 0x00253494 File Offset: 0x00251694
	private void FaceTarget()
	{
		Vector2 vector = this.m_targetPlayer.CenterPosition - base.transform.position.XY();
		float num = BraveMathCollege.Atan2Degrees(vector);
		if (!float.IsNaN(num) && !float.IsInfinity(num))
		{
			base.transform.rotation = Quaternion.Euler(0f, 0f, num);
		}
	}

	// Token: 0x060060AC RID: 24748 RVA: 0x002534FC File Offset: 0x002516FC
	private void AcquireTarget()
	{
		this.m_targetPlayer = GameManager.Instance.GetActivePlayerClosestToPoint(base.transform.position.XY(), false);
		this.m_acquisitionTimer = this.ReacquisitonTimer;
	}

	// Token: 0x060060AD RID: 24749 RVA: 0x0025352C File Offset: 0x0025172C
	private void FireBullet(Transform shootPoint, float dir, string bulletType)
	{
		GameObject gameObject = base.bulletBank.CreateProjectileFromBank(shootPoint.position, dir, bulletType, null, false, true, false);
		Projectile component = gameObject.GetComponent<Projectile>();
		component.Shooter = base.specRigidbody;
		component.specRigidbody.RegisterSpecificCollisionException(base.specRigidbody);
		component.OwnerName = StringTableManager.GetEnemiesString("#TRAP", -1);
	}

	// Token: 0x060060AE RID: 24750 RVA: 0x0025358C File Offset: 0x0025178C
	private Vector2 FindPredictedTargetPosition(string bulletName)
	{
		AIBulletBank.Entry bulletEntry = this.GetBulletEntry(bulletName);
		float num;
		if (bulletEntry.OverrideProjectile)
		{
			num = bulletEntry.ProjectileData.speed;
		}
		else
		{
			num = bulletEntry.BulletObject.GetComponent<Projectile>().baseData.speed;
		}
		if (num < 0f)
		{
			num = float.MaxValue;
		}
		Vector2 unitCenter = this.m_targetPlayer.specRigidbody.GetUnitCenter(ColliderType.HitBox);
		Vector2 averageVelocity = this.m_targetPlayer.AverageVelocity;
		Vector2 unitCenter2 = this.m_targetPlayer.specRigidbody.UnitCenter;
		return BraveMathCollege.GetPredictedPosition(unitCenter, averageVelocity, unitCenter2, num);
	}

	// Token: 0x060060AF RID: 24751 RVA: 0x00253624 File Offset: 0x00251824
	public AIBulletBank.Entry GetBulletEntry(string bulletName)
	{
		if (string.IsNullOrEmpty(bulletName))
		{
			return null;
		}
		AIBulletBank.Entry entry = base.bulletBank.Bullets.Find((AIBulletBank.Entry b) => b.Name == bulletName);
		if (entry == null)
		{
			Debug.LogError(string.Format("Unknown bullet type {0} on {1}", base.transform.name, bulletName), base.gameObject);
			return null;
		}
		return entry;
	}

	// Token: 0x04005B4C RID: 23372
	public float AwakeTimer = 3f;

	// Token: 0x04005B4D RID: 23373
	public float TimeBetweenShots = 0.2f;

	// Token: 0x04005B4E RID: 23374
	public float ReacquisitonTimer = 1f;

	// Token: 0x04005B4F RID: 23375
	public float TrackingPercentage;

	// Token: 0x04005B50 RID: 23376
	public Transform BarrelTransform;

	// Token: 0x04005B51 RID: 23377
	private bool m_active;

	// Token: 0x04005B52 RID: 23378
	private RoomHandler m_parentRoom;

	// Token: 0x04005B53 RID: 23379
	private PlayerController m_targetPlayer;

	// Token: 0x04005B54 RID: 23380
	private float m_awakeTimer;

	// Token: 0x04005B55 RID: 23381
	private float m_acquisitionTimer;

	// Token: 0x04005B56 RID: 23382
	private float m_fireTimer;
}
