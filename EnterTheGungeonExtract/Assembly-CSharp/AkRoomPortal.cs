using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020018FF RID: 6399
[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider))]
[AddComponentMenu("Wwise/AkRoomPortal")]
public class AkRoomPortal : AkUnityEventHandler
{
	// Token: 0x06009DB4 RID: 40372 RVA: 0x003F0DA8 File Offset: 0x003EEFA8
	public ulong GetID()
	{
		return (ulong)((long)base.GetInstanceID());
	}

	// Token: 0x06009DB5 RID: 40373 RVA: 0x003F0DB4 File Offset: 0x003EEFB4
	protected override void Awake()
	{
		BoxCollider component = base.GetComponent<BoxCollider>();
		component.isTrigger = true;
		this.portalTransform.Set(component.bounds.center.x, component.bounds.center.y, component.bounds.center.z, base.transform.forward.x, base.transform.forward.y, base.transform.forward.z, base.transform.up.x, base.transform.up.y, base.transform.up.z);
		this.extent.X = component.size.x * base.transform.localScale.x / 2f;
		this.extent.Y = component.size.y * base.transform.localScale.y / 2f;
		this.extent.Z = component.size.z * base.transform.localScale.z / 2f;
		this.frontRoomID = ((!(this.rooms[1] == null)) ? this.rooms[1].GetID() : AkRoom.INVALID_ROOM_ID);
		this.backRoomID = ((!(this.rooms[0] == null)) ? this.rooms[0].GetID() : AkRoom.INVALID_ROOM_ID);
		base.RegisterTriggers(this.closePortalTriggerList, new AkTriggerBase.Trigger(this.ClosePortal));
		base.Awake();
		if (this.closePortalTriggerList.Contains(1151176110))
		{
			this.ClosePortal(null);
		}
	}

	// Token: 0x06009DB6 RID: 40374 RVA: 0x003F0FD0 File Offset: 0x003EF1D0
	protected override void Start()
	{
		base.Start();
		if (this.closePortalTriggerList.Contains(1281810935))
		{
			this.ClosePortal(null);
		}
	}

	// Token: 0x06009DB7 RID: 40375 RVA: 0x003F0FF4 File Offset: 0x003EF1F4
	public override void HandleEvent(GameObject in_gameObject)
	{
		this.Open();
	}

	// Token: 0x06009DB8 RID: 40376 RVA: 0x003F0FFC File Offset: 0x003EF1FC
	public void ClosePortal(GameObject in_gameObject)
	{
		this.Close();
	}

	// Token: 0x06009DB9 RID: 40377 RVA: 0x003F1004 File Offset: 0x003EF204
	protected override void OnDestroy()
	{
		base.OnDestroy();
		base.UnregisterTriggers(this.closePortalTriggerList, new AkTriggerBase.Trigger(this.ClosePortal));
		if (this.closePortalTriggerList.Contains(-358577003))
		{
			this.ClosePortal(null);
		}
	}

	// Token: 0x06009DBA RID: 40378 RVA: 0x003F1040 File Offset: 0x003EF240
	public void Open()
	{
		this.ActivatePortal(true);
	}

	// Token: 0x06009DBB RID: 40379 RVA: 0x003F104C File Offset: 0x003EF24C
	public void Close()
	{
		this.ActivatePortal(false);
	}

	// Token: 0x06009DBC RID: 40380 RVA: 0x003F1058 File Offset: 0x003EF258
	private void ActivatePortal(bool active)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.frontRoomID != this.backRoomID)
		{
			AkSoundEngine.SetRoomPortal(this.GetID(), this.portalTransform, this.extent, active, this.frontRoomID, this.backRoomID);
		}
		else
		{
			Debug.LogError(base.name + " is not placed/oriented correctly");
		}
	}

	// Token: 0x06009DBD RID: 40381 RVA: 0x003F10C4 File Offset: 0x003EF2C4
	public void FindOverlappingRooms(AkRoom.PriorityList[] roomList)
	{
		BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
		if (component == null)
		{
			return;
		}
		Vector3 vector = new Vector3(component.size.x * base.transform.localScale.x / 2f, component.size.y * base.transform.localScale.y / 2f, component.size.z * base.transform.localScale.z / 4f);
		this.FillRoomList(Vector3.forward * -0.25f, vector, roomList[0]);
		this.FillRoomList(Vector3.forward * 0.25f, vector, roomList[1]);
	}

	// Token: 0x06009DBE RID: 40382 RVA: 0x003F11A0 File Offset: 0x003EF3A0
	private void FillRoomList(Vector3 center, Vector3 halfExtents, AkRoom.PriorityList list)
	{
		list.rooms.Clear();
		center = base.transform.TransformPoint(center);
		Collider[] array = Physics.OverlapBox(center, halfExtents, base.transform.rotation, -1, QueryTriggerInteraction.Collide);
		foreach (Collider collider in array)
		{
			AkRoom component = collider.gameObject.GetComponent<AkRoom>();
			if (component != null && !list.Contains(component))
			{
				list.Add(component);
			}
		}
	}

	// Token: 0x06009DBF RID: 40383 RVA: 0x003F1224 File Offset: 0x003EF424
	public void SetFrontRoom(AkRoom room)
	{
		this.rooms[1] = room;
		this.frontRoomID = ((!(this.rooms[1] == null)) ? this.rooms[1].GetID() : AkRoom.INVALID_ROOM_ID);
	}

	// Token: 0x06009DC0 RID: 40384 RVA: 0x003F1260 File Offset: 0x003EF460
	public void SetBackRoom(AkRoom room)
	{
		this.rooms[0] = room;
		this.backRoomID = ((!(this.rooms[0] == null)) ? this.rooms[0].GetID() : AkRoom.INVALID_ROOM_ID);
	}

	// Token: 0x06009DC1 RID: 40385 RVA: 0x003F129C File Offset: 0x003EF49C
	public void UpdateOverlappingRooms()
	{
		AkRoom.PriorityList[] array = new AkRoom.PriorityList[]
		{
			new AkRoom.PriorityList(),
			new AkRoom.PriorityList()
		};
		this.FindOverlappingRooms(array);
		for (int i = 0; i < 2; i++)
		{
			if (!array[i].Contains(this.rooms[i]))
			{
				this.rooms[i] = array[i].GetHighestPriorityRoom();
			}
		}
		this.frontRoomID = ((!(this.rooms[1] == null)) ? this.rooms[1].GetID() : AkRoom.INVALID_ROOM_ID);
		this.backRoomID = ((!(this.rooms[0] == null)) ? this.rooms[0].GetID() : AkRoom.INVALID_ROOM_ID);
	}

	// Token: 0x04009F2C RID: 40748
	public const int MAX_ROOMS_PER_PORTAL = 2;

	// Token: 0x04009F2D RID: 40749
	private readonly AkVector extent = new AkVector();

	// Token: 0x04009F2E RID: 40750
	private readonly AkTransform portalTransform = new AkTransform();

	// Token: 0x04009F2F RID: 40751
	private ulong backRoomID = AkRoom.INVALID_ROOM_ID;

	// Token: 0x04009F30 RID: 40752
	public List<int> closePortalTriggerList = new List<int>();

	// Token: 0x04009F31 RID: 40753
	private ulong frontRoomID = AkRoom.INVALID_ROOM_ID;

	// Token: 0x04009F32 RID: 40754
	public AkRoom[] rooms = new AkRoom[2];
}
