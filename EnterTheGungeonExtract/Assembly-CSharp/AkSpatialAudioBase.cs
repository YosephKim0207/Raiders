using System;
using UnityEngine;

// Token: 0x02001905 RID: 6405
public abstract class AkSpatialAudioBase : MonoBehaviour
{
	// Token: 0x06009DE7 RID: 40423 RVA: 0x003F1B54 File Offset: 0x003EFD54
	protected void SetGameObjectInHighestPriorityRoom()
	{
		ulong highestPriorityRoomID = this.roomPriorityList.GetHighestPriorityRoomID();
		AkSoundEngine.SetGameObjectInRoom(base.gameObject, highestPriorityRoomID);
	}

	// Token: 0x06009DE8 RID: 40424 RVA: 0x003F1B7C File Offset: 0x003EFD7C
	public void EnteredRoom(AkRoom room)
	{
		this.roomPriorityList.Add(room);
		this.SetGameObjectInHighestPriorityRoom();
	}

	// Token: 0x06009DE9 RID: 40425 RVA: 0x003F1B90 File Offset: 0x003EFD90
	public void ExitedRoom(AkRoom room)
	{
		this.roomPriorityList.Remove(room);
		this.SetGameObjectInHighestPriorityRoom();
	}

	// Token: 0x06009DEA RID: 40426 RVA: 0x003F1BA4 File Offset: 0x003EFDA4
	public void SetGameObjectInRoom()
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position, 0f);
		foreach (Collider collider in array)
		{
			AkRoom component = collider.gameObject.GetComponent<AkRoom>();
			if (component != null)
			{
				this.roomPriorityList.Add(component);
			}
		}
		this.SetGameObjectInHighestPriorityRoom();
	}

	// Token: 0x04009F50 RID: 40784
	private readonly AkRoom.PriorityList roomPriorityList = new AkRoom.PriorityList();
}
