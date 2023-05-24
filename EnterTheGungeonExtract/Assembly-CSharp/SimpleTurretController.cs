using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02001214 RID: 4628
public class SimpleTurretController : DungeonPlaceableBehaviour
{
	// Token: 0x17000F4F RID: 3919
	// (get) Token: 0x06006783 RID: 26499 RVA: 0x00288150 File Offset: 0x00286350
	public bool Inactive
	{
		get
		{
			return !this.m_active;
		}
	}

	// Token: 0x06006784 RID: 26500 RVA: 0x0028815C File Offset: 0x0028635C
	private void Start()
	{
		if (!base.specRigidbody)
		{
			base.specRigidbody = base.GetComponentInChildren<SpeculativeRigidbody>();
		}
		this.m_bank = base.bulletBank;
		if (!base.bulletBank)
		{
			this.m_bank = base.GetComponentInChildren<AIBulletBank>();
		}
		this.m_parentRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
		if (!this.ControlledByPlaymaker)
		{
			this.m_parentRoom.Entered += this.Activate;
		}
	}

	// Token: 0x06006785 RID: 26501 RVA: 0x002881FC File Offset: 0x002863FC
	public void DeactivateManual()
	{
		this.m_active = false;
	}

	// Token: 0x06006786 RID: 26502 RVA: 0x00288208 File Offset: 0x00286408
	public void ActivateManual()
	{
		if (this.m_active)
		{
			return;
		}
		this.m_active = true;
		this.m_awakeTimer = this.AwakeTimer;
	}

	// Token: 0x06006787 RID: 26503 RVA: 0x0028822C File Offset: 0x0028642C
	private void Activate(PlayerController p)
	{
		this.ActivateManual();
	}

	// Token: 0x06006788 RID: 26504 RVA: 0x00288234 File Offset: 0x00286434
	private void Update()
	{
		if (this.m_active)
		{
			if (!this.ControlledByPlaymaker && !GameManager.Instance.IsAnyPlayerInRoom(this.m_parentRoom))
			{
				this.m_active = false;
				return;
			}
			this.m_awakeTimer -= BraveTime.DeltaTime;
			if (this.m_awakeTimer > 0f)
			{
				return;
			}
			this.Fire();
		}
	}

	// Token: 0x06006789 RID: 26505 RVA: 0x002882A0 File Offset: 0x002864A0
	private void Fire()
	{
		this.m_fireTimer -= BraveTime.DeltaTime;
		if (this.m_fireTimer <= 0f)
		{
			this.FireBullet(this.BarrelTransform, this.ShootDirection, "default");
			this.m_fireTimer = this.TimeBetweenShots;
		}
	}

	// Token: 0x0600678A RID: 26506 RVA: 0x002882F4 File Offset: 0x002864F4
	private void FireBullet(Transform shootPoint, Vector2 dirVec, string bulletType)
	{
		GameObject gameObject = this.m_bank.CreateProjectileFromBank(shootPoint.position, BraveMathCollege.Atan2Degrees(dirVec.normalized), bulletType, null, false, true, false);
		Projectile component = gameObject.GetComponent<Projectile>();
		component.Shooter = base.specRigidbody;
		component.specRigidbody.RegisterSpecificCollisionException(base.specRigidbody);
	}

	// Token: 0x0600678B RID: 26507 RVA: 0x00288350 File Offset: 0x00286550
	public AIBulletBank.Entry GetBulletEntry(string bulletName)
	{
		if (string.IsNullOrEmpty(bulletName))
		{
			return null;
		}
		AIBulletBank.Entry entry = this.m_bank.Bullets.Find((AIBulletBank.Entry b) => b.Name == bulletName);
		if (entry == null)
		{
			Debug.LogError(string.Format("Unknown bullet type {0} on {1}", base.transform.name, bulletName), base.gameObject);
			return null;
		}
		return entry;
	}

	// Token: 0x04006363 RID: 25443
	public bool ControlledByPlaymaker = true;

	// Token: 0x04006364 RID: 25444
	public float AwakeTimer = 3f;

	// Token: 0x04006365 RID: 25445
	public float TimeBetweenShots = 0.2f;

	// Token: 0x04006366 RID: 25446
	public Vector2 ShootDirection;

	// Token: 0x04006367 RID: 25447
	public Transform BarrelTransform;

	// Token: 0x04006368 RID: 25448
	private bool m_active;

	// Token: 0x04006369 RID: 25449
	private RoomHandler m_parentRoom;

	// Token: 0x0400636A RID: 25450
	private AIBulletBank m_bank;

	// Token: 0x0400636B RID: 25451
	private float m_awakeTimer;

	// Token: 0x0400636C RID: 25452
	private float m_fireTimer;
}
