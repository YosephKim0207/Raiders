using System;
using Dungeonator;
using UnityEngine;

// Token: 0x0200100B RID: 4107
public class DemonWallController : BraveBehaviour
{
	// Token: 0x17000D00 RID: 3328
	// (get) Token: 0x060059E1 RID: 23009 RVA: 0x00225478 File Offset: 0x00223678
	public bool IsCameraLocked
	{
		get
		{
			return this.m_state == DemonWallController.State.LockCamera;
		}
	}

	// Token: 0x060059E2 RID: 23010 RVA: 0x00225484 File Offset: 0x00223684
	public void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Combine(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
		base.aiActor.ManualKnockbackHandling = true;
		base.aiActor.ParentRoom.Entered += this.PlayerEnteredRoom;
		base.aiActor.healthHaver.OnPreDeath += this.OnPreDeath;
		CameraController mainCameraController = GameManager.Instance.MainCameraController;
		Vector2 vector = new Vector2(0f, base.specRigidbody.HitboxPixelCollider.UnitDimensions.y - mainCameraController.Camera.orthographicSize + 0.5f);
		this.CameraPos = base.specRigidbody.UnitCenter + vector;
		this.m_room = base.aiActor.ParentRoom;
	}

	// Token: 0x060059E3 RID: 23011 RVA: 0x00225568 File Offset: 0x00223768
	public void Update()
	{
		if (this.m_state == DemonWallController.State.Intro)
		{
			if (base.specRigidbody.CollideWithOthers)
			{
				this.m_state = DemonWallController.State.LockCamera;
				CameraController mainCameraController = GameManager.Instance.MainCameraController;
				mainCameraController.SetManualControl(true, true);
				mainCameraController.OverridePosition = this.CameraPos;
				Vector2 unitBottomCenter = base.specRigidbody.UnitBottomCenter;
				this.m_leftId = DeadlyDeadlyGoopManager.RegisterUngoopableCircle(unitBottomCenter + new Vector2(-1.5f, 1.5f), 1.5f);
				this.m_rightId = DeadlyDeadlyGoopManager.RegisterUngoopableCircle(unitBottomCenter + new Vector2(1.5f, 1.5f), 1.5f);
			}
		}
		else if (this.m_state == DemonWallController.State.LockCamera)
		{
			Vector2 unitBottomCenter2 = base.specRigidbody.UnitBottomCenter;
			DeadlyDeadlyGoopManager.UpdateUngoopableCircle(this.m_leftId, unitBottomCenter2 + new Vector2(-1.5f, 1.5f), 1.5f);
			DeadlyDeadlyGoopManager.UpdateUngoopableCircle(this.m_rightId, unitBottomCenter2 + new Vector2(1.5f, 1.5f), 1.5f);
			this.MarkInaccessible(true);
		}
		this.m_cachedCameraMinY = PhysicsEngine.UnitToPixel(BraveUtility.ViewportToWorldpoint(new Vector2(0.5f, 0f), ViewportType.Camera).y);
	}

	// Token: 0x060059E4 RID: 23012 RVA: 0x002256A8 File Offset: 0x002238A8
	protected override void OnDestroy()
	{
		this.RestrictMotion(false);
		this.ModifyCamera(false);
		this.MarkInaccessible(false);
		if (base.specRigidbody)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnCollision = (Action<CollisionData>)Delegate.Remove(specRigidbody.OnCollision, new Action<CollisionData>(this.OnCollision));
		}
		if (base.aiActor && base.aiActor.ParentRoom != null)
		{
			base.aiActor.ParentRoom.Entered -= this.PlayerEnteredRoom;
		}
		if (base.aiActor && base.aiActor.healthHaver)
		{
			base.aiActor.healthHaver.OnPreDeath -= this.OnPreDeath;
		}
		base.OnDestroy();
	}

	// Token: 0x17000D01 RID: 3329
	// (get) Token: 0x060059E5 RID: 23013 RVA: 0x00225784 File Offset: 0x00223984
	// (set) Token: 0x060059E6 RID: 23014 RVA: 0x0022578C File Offset: 0x0022398C
	public Vector2 CameraPos { get; set; }

	// Token: 0x060059E7 RID: 23015 RVA: 0x00225798 File Offset: 0x00223998
	private void OnCollision(CollisionData rigidbodyCollision)
	{
		if (rigidbodyCollision.collisionType == CollisionData.CollisionType.Rigidbody && !base.aiActor.IsFrozen)
		{
			PlayerController component = rigidbodyCollision.OtherRigidbody.GetComponent<PlayerController>();
			MajorBreakable majorBreakable = rigidbodyCollision.OtherRigidbody.majorBreakable;
			AIActor aiActor = rigidbodyCollision.OtherRigidbody.aiActor;
			if (!base.healthHaver.IsDead && component != null)
			{
				Vector2 vector = -Vector2.up;
				IntVector2 intVector = component.specRigidbody.UnitBottomCenter.ToIntVector2(VectorConversions.Floor);
				if (GameManager.Instance.Dungeon.data.isTopWall(intVector.x, intVector.y))
				{
					component.healthHaver.ApplyDamage(1000f, vector, base.aiActor.GetActorName(), CoreDamageTypes.None, DamageCategory.Collision, true, null, false);
				}
				component.healthHaver.ApplyDamage(base.aiActor.CollisionDamage, vector, base.aiActor.GetActorName(), CoreDamageTypes.None, DamageCategory.Collision, false, null, false);
				component.knockbackDoer.ApplySourcedKnockback(vector, base.aiActor.CollisionKnockbackStrength, base.gameObject, true);
				if (base.knockbackDoer)
				{
					base.knockbackDoer.ApplySourcedKnockback(-vector, component.collisionKnockbackStrength, base.gameObject, false);
				}
				base.aiActor.CollisionVFX.SpawnAtPosition(rigidbodyCollision.Contact, 0f, null, new Vector2?(Vector2.zero), new Vector2?(Vector2.zero), new float?(2f), false, null, null, false);
				component.specRigidbody.RegisterGhostCollisionException(base.specRigidbody);
			}
			if (aiActor && aiActor.CompanionOwner)
			{
				Debug.LogError("knocking back companion");
				aiActor.knockbackDoer.ApplySourcedKnockback(Vector2.down, 50f, base.gameObject, true);
			}
			if (majorBreakable)
			{
				majorBreakable.ApplyDamage(1000f, Vector2.down, true, false, true);
			}
		}
	}

	// Token: 0x060059E8 RID: 23016 RVA: 0x00225990 File Offset: 0x00223B90
	private void PlayerEnteredRoom(PlayerController playerController)
	{
		this.RestrictMotion(true);
	}

	// Token: 0x060059E9 RID: 23017 RVA: 0x0022599C File Offset: 0x00223B9C
	private void OnPreDeath(Vector2 finalDirection)
	{
		this.RestrictMotion(false);
		this.MarkInaccessible(false);
		base.aiActor.ParentRoom.Entered -= this.PlayerEnteredRoom;
		if (this.m_state == DemonWallController.State.LockCamera)
		{
			this.m_state = DemonWallController.State.Dead;
		}
	}

	// Token: 0x060059EA RID: 23018 RVA: 0x002259DC File Offset: 0x00223BDC
	private void PlayerMovementRestrictor(SpeculativeRigidbody playerSpecRigidbody, IntVector2 prevPixelOffset, IntVector2 pixelOffset, ref bool validLocation)
	{
		if (!validLocation)
		{
			return;
		}
		int maxY = playerSpecRigidbody.PixelColliders[1].MaxY;
		int minY = base.specRigidbody.PixelColliders[1].MinY;
		if (maxY + pixelOffset.y >= minY && pixelOffset.y > prevPixelOffset.y)
		{
			validLocation = false;
			return;
		}
		IntVector2 intVector = pixelOffset - prevPixelOffset;
		CellArea area = base.aiActor.ParentRoom.area;
		if (intVector.x < 0)
		{
			int num = playerSpecRigidbody.PixelColliders[0].MinX + pixelOffset.x;
			int num2 = area.basePosition.x * 16;
			if (num < num2)
			{
				validLocation = false;
				return;
			}
		}
		else if (intVector.x > 0)
		{
			int num3 = playerSpecRigidbody.PixelColliders[0].MaxX + pixelOffset.x;
			int num4 = (area.basePosition.x + area.dimensions.x) * 16 - 1;
			if (num3 > num4)
			{
				validLocation = false;
				return;
			}
		}
		if (intVector.y < 0)
		{
			int num5 = playerSpecRigidbody.PixelColliders[0].MinY + pixelOffset.y;
			if (num5 < this.m_cachedCameraMinY)
			{
				validLocation = false;
			}
		}
	}

	// Token: 0x060059EB RID: 23019 RVA: 0x00225B34 File Offset: 0x00223D34
	private void RestrictMotion(bool value)
	{
		if (this.m_isMotionRestricted == value)
		{
			return;
		}
		if (value)
		{
			if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel || GameManager.IsReturningToBreach)
			{
				return;
			}
			for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
			{
				SpeculativeRigidbody specRigidbody = GameManager.Instance.AllPlayers[i].specRigidbody;
				specRigidbody.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Combine(specRigidbody.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.PlayerMovementRestrictor));
			}
		}
		else
		{
			if (!GameManager.HasInstance || GameManager.IsReturningToBreach)
			{
				return;
			}
			for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
			{
				PlayerController playerController = GameManager.Instance.AllPlayers[j];
				if (playerController)
				{
					SpeculativeRigidbody specRigidbody2 = playerController.specRigidbody;
					specRigidbody2.MovementRestrictor = (SpeculativeRigidbody.MovementRestrictorDelegate)Delegate.Remove(specRigidbody2.MovementRestrictor, new SpeculativeRigidbody.MovementRestrictorDelegate(this.PlayerMovementRestrictor));
				}
			}
		}
		this.m_isMotionRestricted = value;
	}

	// Token: 0x060059EC RID: 23020 RVA: 0x00225C44 File Offset: 0x00223E44
	public void ModifyCamera(bool value)
	{
		if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel || GameManager.IsReturningToBreach)
		{
			return;
		}
		if (value)
		{
			GameManager.Instance.MainCameraController.SetManualControl(true, false);
		}
		else
		{
			GameManager.Instance.MainCameraController.SetManualControl(false, true);
		}
	}

	// Token: 0x060059ED RID: 23021 RVA: 0x00225CA4 File Offset: 0x00223EA4
	private void MarkInaccessible(bool inaccessible)
	{
		if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel || GameManager.IsReturningToBreach)
		{
			return;
		}
		if (!base.aiActor || base.aiActor.ParentRoom == null)
		{
			return;
		}
		DungeonData data = GameManager.Instance.Dungeon.data;
		IntVector2 basePosition = this.m_room.area.basePosition;
		IntVector2 intVector = this.m_room.area.basePosition + base.aiActor.ParentRoom.area.dimensions - IntVector2.One;
		if (inaccessible && base.specRigidbody)
		{
			basePosition.y = (int)base.specRigidbody.UnitBottomCenter.y - 3;
		}
		for (int i = basePosition.x; i <= intVector.x; i++)
		{
			for (int j = basePosition.y; j <= intVector.y; j++)
			{
				if (data.CheckInBoundsAndValid(i, j))
				{
					data[i, j].IsPlayerInaccessible = inaccessible;
				}
			}
		}
	}

	// Token: 0x04005350 RID: 21328
	private DemonWallController.State m_state;

	// Token: 0x04005351 RID: 21329
	private bool m_isMotionRestricted;

	// Token: 0x04005352 RID: 21330
	private int m_cachedCameraMinY;

	// Token: 0x04005353 RID: 21331
	private RoomHandler m_room;

	// Token: 0x04005354 RID: 21332
	private int m_leftId;

	// Token: 0x04005355 RID: 21333
	private int m_rightId;

	// Token: 0x0200100C RID: 4108
	private enum State
	{
		// Token: 0x04005357 RID: 21335
		Intro,
		// Token: 0x04005358 RID: 21336
		LockCamera,
		// Token: 0x04005359 RID: 21337
		Dead
	}
}
