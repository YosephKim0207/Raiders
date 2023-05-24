using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x0200124A RID: 4682
public class WarpWingPortalController : BraveBehaviour, IPlayerInteractable
{
	// Token: 0x060068EC RID: 26860 RVA: 0x0029154C File Offset: 0x0028F74C
	private IEnumerator Start()
	{
		yield return null;
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.RESOURCEFUL_RAT)
		{
			this.HandleResourcefulRatFlowSetup();
		}
		base.sprite.UpdateZDepth();
		if (this.UsesTriggerZone)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleTriggerEntered));
		}
		yield break;
	}

	// Token: 0x060068ED RID: 26861 RVA: 0x00291568 File Offset: 0x0028F768
	private void Update()
	{
		this.m_justUsed = false;
	}

	// Token: 0x060068EE RID: 26862 RVA: 0x00291574 File Offset: 0x0028F774
	private void HandleTriggerEntered(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (this.m_justUsed)
		{
			return;
		}
		PlayerController component = specRigidbody.GetComponent<PlayerController>();
		if (component != null)
		{
			this.DoTeleport(component);
		}
	}

	// Token: 0x060068EF RID: 26863 RVA: 0x002915A8 File Offset: 0x0028F7A8
	private void HandleResourcefulRatFlowSetup()
	{
		DungeonData.Direction[] resourcefulRatSolution = GameManager.GetResourcefulRatSolution();
		if (this.parentRoom.area.PrototypeRoomName == "ResourcefulRat_ChainRoom_01")
		{
			if (resourcefulRatSolution[0] == this.parentExit.referencedExit.exitDirection)
			{
				this.pairedPortal = this.GetDirectionalPortalFromRoom(2, DungeonData.Direction.WEST);
			}
			else
			{
				this.AttachResourcefulRatFailRoom();
				this.pairedPortal = this.GetRatFirstRoomEntrancePortal();
			}
		}
		else if (this.parentRoom.area.PrototypeRoomName == "ResourcefulRat_ChainRoom_02")
		{
			if (resourcefulRatSolution[1] == this.parentExit.referencedExit.exitDirection)
			{
				this.pairedPortal = this.GetDirectionalPortalFromRoom(3, DungeonData.Direction.WEST);
			}
			else
			{
				this.AttachResourcefulRatFailRoom();
				this.pairedPortal = this.GetRatFirstRoomEntrancePortal();
			}
		}
		else if (this.parentRoom.area.PrototypeRoomName == "ResourcefulRat_ChainRoom_03")
		{
			if (resourcefulRatSolution[2] == this.parentExit.referencedExit.exitDirection)
			{
				this.pairedPortal = this.GetDirectionalPortalFromRoom(4, DungeonData.Direction.WEST);
			}
			else
			{
				this.AttachResourcefulRatFailRoom();
				this.pairedPortal = this.GetRatFirstRoomEntrancePortal();
			}
		}
		else if (this.parentRoom.area.PrototypeRoomName == "ResourcefulRat_ChainRoom_04")
		{
			if (resourcefulRatSolution[3] == this.parentExit.referencedExit.exitDirection)
			{
				this.pairedPortal = this.GetDirectionalPortalFromRoom(5, DungeonData.Direction.WEST);
			}
			else
			{
				this.AttachResourcefulRatFailRoom();
				this.pairedPortal = this.GetRatFirstRoomEntrancePortal();
			}
		}
		else if (this.parentRoom.area.PrototypeRoomName == "ResourcefulRat_ChainRoom_05")
		{
			if (resourcefulRatSolution[4] == this.parentExit.referencedExit.exitDirection)
			{
				this.pairedPortal = this.GetDirectionalPortalFromRoom(6, DungeonData.Direction.WEST);
			}
			else
			{
				this.AttachResourcefulRatFailRoom();
				this.pairedPortal = this.GetRatFirstRoomEntrancePortal();
			}
		}
		else if (this.parentRoom.area.PrototypeRoomName == "ResourcefulRat_ChainRoom_06")
		{
			if (resourcefulRatSolution[5] == this.parentExit.referencedExit.exitDirection)
			{
				this.pairedPortal = this.GetDirectionalPortalFromRoom(7, DungeonData.Direction.WEST);
			}
			else
			{
				this.AttachResourcefulRatFailRoom();
				this.pairedPortal = this.GetRatFirstRoomEntrancePortal();
			}
		}
		else if (this.parentRoom.area.PrototypeRoomName == "ResourcefulRat_FailRoom")
		{
			this.AttachResourcefulRatFailRoom();
			WarpWingPortalController ratFirstRoomEntrancePortal = this.GetRatFirstRoomEntrancePortal();
			if (ratFirstRoomEntrancePortal != null)
			{
				this.pairedPortal = ratFirstRoomEntrancePortal;
			}
		}
	}

	// Token: 0x060068F0 RID: 26864 RVA: 0x00291840 File Offset: 0x0028FA40
	private WarpWingPortalController GetDirectionalPortalFromRoom(int roomIndex, DungeonData.Direction dir)
	{
		WarpWingPortalController warpWingPortalController = null;
		for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
		{
			string text = "ResourcefulRat_ChainRoom_0" + roomIndex.ToString();
			if (roomIndex > 6)
			{
				text = "Boss Foyer";
			}
			if (GameManager.Instance.Dungeon.data.rooms[i].area.PrototypeRoomName == text)
			{
				RoomHandler roomHandler = GameManager.Instance.Dungeon.data.rooms[i];
				for (int j = 0; j < roomHandler.area.instanceUsedExits.Count; j++)
				{
					if (roomHandler.area.instanceUsedExits[j].exitDirection == dir)
					{
						warpWingPortalController = roomHandler.area.exitToLocalDataMap[roomHandler.area.instanceUsedExits[j]].warpWingPortal;
					}
				}
			}
		}
		return warpWingPortalController;
	}

	// Token: 0x060068F1 RID: 26865 RVA: 0x00291954 File Offset: 0x0028FB54
	private WarpWingPortalController GetRatFirstRoomEntrancePortal()
	{
		return this.GetDirectionalPortalFromRoom(2, DungeonData.Direction.WEST);
	}

	// Token: 0x060068F2 RID: 26866 RVA: 0x00291960 File Offset: 0x0028FB60
	private void AttachResourcefulRatFailRoom()
	{
		for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
		{
			if (GameManager.Instance.Dungeon.data.rooms[i].area.PrototypeRoomName == "ResourcefulRat_FailExit")
			{
				RoomHandler roomHandler = GameManager.Instance.Dungeon.data.rooms[i];
				for (int j = 0; j < roomHandler.area.instanceUsedExits.Count; j++)
				{
					WarpWingPortalController warpWingPortal = roomHandler.area.exitToLocalDataMap[roomHandler.area.instanceUsedExits[j]].warpWingPortal;
					if (warpWingPortal != null)
					{
						warpWingPortal.pairedPortal = warpWingPortal;
						this.failPortal = warpWingPortal;
						this.FailChance = 0.25f;
						break;
					}
				}
			}
		}
	}

	// Token: 0x060068F3 RID: 26867 RVA: 0x00291A58 File Offset: 0x0028FC58
	public void OnEnteredRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white, 0.1f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x060068F4 RID: 26868 RVA: 0x00291A8C File Offset: 0x0028FC8C
	public void OnExitRange(PlayerController interactor)
	{
		if (!this)
		{
			return;
		}
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, false);
		base.sprite.UpdateZDepth();
	}

	// Token: 0x060068F5 RID: 26869 RVA: 0x00291AB4 File Offset: 0x0028FCB4
	public float GetDistanceToPoint(Vector2 point)
	{
		if (this.UsesTriggerZone)
		{
			return 1000f;
		}
		if (this.pairedPortal == this || this.pairedPortal == null)
		{
			return 1000f;
		}
		Bounds bounds = base.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x060068F6 RID: 26870 RVA: 0x00291BCC File Offset: 0x0028FDCC
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x060068F7 RID: 26871 RVA: 0x00291BD4 File Offset: 0x0028FDD4
	private IEnumerator HandleDelayedAnimationTrigger(tk2dSpriteAnimator target)
	{
		yield return new WaitForSeconds(0.6f);
		target.Play("resourceful_rat_teleport_in");
		yield break;
	}

	// Token: 0x060068F8 RID: 26872 RVA: 0x00291BF0 File Offset: 0x0028FDF0
	public void Interact(PlayerController player)
	{
		this.DoTeleport(player);
	}

	// Token: 0x060068F9 RID: 26873 RVA: 0x00291BFC File Offset: 0x0028FDFC
	private IEnumerator MarkUsed(WarpWingPortalController targetPortal)
	{
		float elapsed = 0f;
		while (elapsed < 2f)
		{
			elapsed += BraveTime.DeltaTime;
			targetPortal.m_justUsed = true;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060068FA RID: 26874 RVA: 0x00291C18 File Offset: 0x0028FE18
	private void DoTeleport(PlayerController player)
	{
		if (this.pairedPortal == this || this.pairedPortal == null)
		{
			return;
		}
		if (this.failPortal != null && UnityEngine.Random.value < this.FailChance)
		{
			base.spriteAnimator.Play("resourceful_rat_teleport_out");
			base.StartCoroutine(this.HandleDelayedAnimationTrigger(this.failPortal.spriteAnimator));
			base.StartCoroutine(this.MarkUsed(this));
			base.StartCoroutine(this.MarkUsed(this.pairedPortal));
			player.TeleportToPoint(this.failPortal.sprite.WorldCenter, true);
		}
		else
		{
			base.spriteAnimator.Play("resourceful_rat_teleport_out");
			base.StartCoroutine(this.HandleDelayedAnimationTrigger(this.pairedPortal.spriteAnimator));
			base.StartCoroutine(this.MarkUsed(this));
			base.StartCoroutine(this.MarkUsed(this.pairedPortal));
			player.TeleportToPoint(this.pairedPortal.sprite.WorldCenter, true);
		}
	}

	// Token: 0x060068FB RID: 26875 RVA: 0x00291D30 File Offset: 0x0028FF30
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x060068FC RID: 26876 RVA: 0x00291D3C File Offset: 0x0028FF3C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400654D RID: 25933
	public bool UsesTriggerZone;

	// Token: 0x0400654E RID: 25934
	[NonSerialized]
	public WarpWingPortalController pairedPortal;

	// Token: 0x0400654F RID: 25935
	[NonSerialized]
	public RoomHandler parentRoom;

	// Token: 0x04006550 RID: 25936
	[NonSerialized]
	public RuntimeRoomExitData parentExit;

	// Token: 0x04006551 RID: 25937
	[NonSerialized]
	private float FailChance;

	// Token: 0x04006552 RID: 25938
	[NonSerialized]
	public WarpWingPortalController failPortal;

	// Token: 0x04006553 RID: 25939
	private bool m_justUsed;
}
