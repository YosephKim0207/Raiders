using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000FBB RID: 4027
[RequireComponent(typeof(GenericIntroDoer))]
public class AgunimIntroDoer : SpecificIntroDoer
{
	// Token: 0x060057AE RID: 22446 RVA: 0x00217744 File Offset: 0x00215944
	public void Start()
	{
		base.aiActor.ParentRoom.Entered += this.PlayerEnteredRoom;
		base.aiActor.healthHaver.OnPreDeath += this.OnPreDeath;
	}

	// Token: 0x060057AF RID: 22447 RVA: 0x00217780 File Offset: 0x00215980
	public void Update()
	{
		this.m_cachedCameraMinY = PhysicsEngine.UnitToPixel(BraveUtility.ViewportToWorldpoint(new Vector2(0.5f, 0f), ViewportType.Camera).y);
	}

	// Token: 0x060057B0 RID: 22448 RVA: 0x002177B8 File Offset: 0x002159B8
	protected override void OnDestroy()
	{
		this.RestrictMotion(false);
		this.ModifyCamera(false);
		base.OnDestroy();
	}

	// Token: 0x17000C88 RID: 3208
	// (get) Token: 0x060057B1 RID: 22449 RVA: 0x002177D0 File Offset: 0x002159D0
	public override Vector2? OverrideOutroPosition
	{
		get
		{
			if (!this)
			{
				this.ModifyCamera(false);
			}
			else
			{
				this.ModifyCamera(true);
			}
			return null;
		}
	}

	// Token: 0x060057B2 RID: 22450 RVA: 0x00217804 File Offset: 0x00215A04
	public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
	{
		RoomHandler parentRoom = base.aiActor.ParentRoom;
		if (parentRoom != null)
		{
			List<TorchController> componentsInRoom = parentRoom.GetComponentsInRoom<TorchController>();
			for (int i = 0; i < componentsInRoom.Count; i++)
			{
				TorchController torchController = componentsInRoom[i];
				if (torchController && torchController.specRigidbody)
				{
					torchController.specRigidbody.CollideWithOthers = false;
				}
			}
		}
	}

	// Token: 0x060057B3 RID: 22451 RVA: 0x00217870 File Offset: 0x00215A70
	private void PlayerMovementRestrictor(SpeculativeRigidbody playerSpecRigidbody, IntVector2 prevPixelOffset, IntVector2 pixelOffset, ref bool validLocation)
	{
		if (!validLocation)
		{
			return;
		}
		if ((pixelOffset - prevPixelOffset).y < 0)
		{
			int num = playerSpecRigidbody.PixelColliders[0].MinY + pixelOffset.y;
			if (num < this.m_cachedCameraMinY + 20)
			{
				validLocation = false;
			}
		}
	}

	// Token: 0x060057B4 RID: 22452 RVA: 0x002178C8 File Offset: 0x00215AC8
	private void PlayerEnteredRoom(PlayerController playerController)
	{
		this.RestrictMotion(true);
		BulletPastRoomController[] array = UnityEngine.Object.FindObjectsOfType<BulletPastRoomController>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].RoomIdentifier == BulletPastRoomController.BulletRoomCategory.ROOM_C)
			{
				base.StartCoroutine(array[i].HandleAgunimIntro(base.transform));
				break;
			}
		}
	}

	// Token: 0x060057B5 RID: 22453 RVA: 0x00217920 File Offset: 0x00215B20
	private void OnPreDeath(Vector2 finalDirection)
	{
		this.RestrictMotion(false);
	}

	// Token: 0x060057B6 RID: 22454 RVA: 0x0021792C File Offset: 0x00215B2C
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

	// Token: 0x060057B7 RID: 22455 RVA: 0x00217A3C File Offset: 0x00215C3C
	private void ModifyCamera(bool value)
	{
		if (!GameManager.HasInstance || GameManager.Instance.IsLoadingLevel || GameManager.IsReturningToBreach)
		{
			return;
		}
		if (value)
		{
			CameraController mainCameraController = GameManager.Instance.MainCameraController;
			mainCameraController.LockToRoom = true;
			mainCameraController.PreventAimLook = true;
			mainCameraController.AddFocusPoint(this.cameraPoint.gameObject);
		}
		else
		{
			CameraController mainCameraController2 = GameManager.Instance.MainCameraController;
			mainCameraController2.LockToRoom = false;
			mainCameraController2.PreventAimLook = false;
			mainCameraController2.RemoveFocusPoint(this.cameraPoint.gameObject);
		}
	}

	// Token: 0x040050B7 RID: 20663
	public GameObject cameraPoint;

	// Token: 0x040050B8 RID: 20664
	private int m_cachedCameraMinY;

	// Token: 0x040050B9 RID: 20665
	private bool m_isMotionRestricted;
}
