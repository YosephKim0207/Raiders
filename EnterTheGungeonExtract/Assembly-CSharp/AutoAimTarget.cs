using System;
using Dungeonator;
using UnityEngine;

// Token: 0x02000D08 RID: 3336
public class AutoAimTarget : BraveBehaviour, IAutoAimTarget
{
	// Token: 0x0600465E RID: 18014 RVA: 0x0016D480 File Offset: 0x0016B680
	public void Start()
	{
		Vector2 aimCenter = this.AimCenter;
		this.parentRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(aimCenter.ToIntVector2(VectorConversions.Floor));
		this.parentRoom.RegisterAutoAimTarget(this);
	}

	// Token: 0x0600465F RID: 18015 RVA: 0x0016D4C4 File Offset: 0x0016B6C4
	protected override void OnDestroy()
	{
		if (this.parentRoom != null)
		{
			this.parentRoom.DeregisterAutoAimTarget(this);
		}
		base.OnDestroy();
	}

	// Token: 0x17000A4F RID: 2639
	// (get) Token: 0x06004660 RID: 18016 RVA: 0x0016D4E4 File Offset: 0x0016B6E4
	public bool IsValid
	{
		get
		{
			if (!this)
			{
				return false;
			}
			if (base.specRigidbody && !this.ForceUseTransform)
			{
				return base.specRigidbody.enabled && base.specRigidbody.GetPixelCollider(ColliderType.HitBox) != null;
			}
			return base.enabled && base.gameObject.activeSelf;
		}
	}

	// Token: 0x17000A50 RID: 2640
	// (get) Token: 0x06004661 RID: 18017 RVA: 0x0016D55C File Offset: 0x0016B75C
	public Vector2 AimCenter
	{
		get
		{
			if (base.specRigidbody && !this.ForceUseTransform)
			{
				return base.specRigidbody.GetUnitCenter(ColliderType.HitBox);
			}
			return base.transform.position.XY();
		}
	}

	// Token: 0x17000A51 RID: 2641
	// (get) Token: 0x06004662 RID: 18018 RVA: 0x0016D598 File Offset: 0x0016B798
	public Vector2 Velocity
	{
		get
		{
			if (base.specRigidbody)
			{
				return base.specRigidbody.Velocity;
			}
			return Vector2.zero;
		}
	}

	// Token: 0x17000A52 RID: 2642
	// (get) Token: 0x06004663 RID: 18019 RVA: 0x0016D5BC File Offset: 0x0016B7BC
	public bool IgnoreForSuperDuperAutoAim
	{
		get
		{
			return this.IgnoreForSuperAutoAim;
		}
	}

	// Token: 0x17000A53 RID: 2643
	// (get) Token: 0x06004664 RID: 18020 RVA: 0x0016D5C4 File Offset: 0x0016B7C4
	public float MinDistForSuperDuperAutoAim
	{
		get
		{
			return this.MinDistForSuperAutoAim;
		}
	}

	// Token: 0x040038D5 RID: 14549
	public bool ForceUseTransform;

	// Token: 0x040038D6 RID: 14550
	public bool IgnoreForSuperAutoAim;

	// Token: 0x040038D7 RID: 14551
	public float MinDistForSuperAutoAim;

	// Token: 0x040038D8 RID: 14552
	private RoomHandler parentRoom;
}
