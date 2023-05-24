using System;
using System.Collections.Generic;
using AK.Wwise;
using UnityEngine;

// Token: 0x020018FC RID: 6396
[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
[AddComponentMenu("Wwise/AkRoom")]
public class AkRoom : MonoBehaviour
{
	// Token: 0x17001703 RID: 5891
	// (get) Token: 0x06009DA2 RID: 40354 RVA: 0x003F0A58 File Offset: 0x003EEC58
	public static bool IsSpatialAudioEnabled
	{
		get
		{
			return AkSpatialAudioListener.TheSpatialAudioListener != null && AkRoom.RoomCount > 0;
		}
	}

	// Token: 0x06009DA3 RID: 40355 RVA: 0x003F0A78 File Offset: 0x003EEC78
	public ulong GetID()
	{
		return (ulong)((long)base.GetInstanceID());
	}

	// Token: 0x06009DA4 RID: 40356 RVA: 0x003F0A84 File Offset: 0x003EEC84
	private void OnEnable()
	{
		AkRoomParams akRoomParams = new AkRoomParams();
		akRoomParams.Up.X = base.transform.up.x;
		akRoomParams.Up.Y = base.transform.up.y;
		akRoomParams.Up.Z = base.transform.up.z;
		akRoomParams.Front.X = base.transform.forward.x;
		akRoomParams.Front.Y = base.transform.forward.y;
		akRoomParams.Front.Z = base.transform.forward.z;
		akRoomParams.ReverbAuxBus = (uint)this.reverbAuxBus.ID;
		akRoomParams.ReverbLevel = this.reverbLevel;
		akRoomParams.WallOcclusion = this.wallOcclusion;
		AkRoom.RoomCount++;
		AkSoundEngine.SetRoom(this.GetID(), akRoomParams, base.name);
	}

	// Token: 0x06009DA5 RID: 40357 RVA: 0x003F0B98 File Offset: 0x003EED98
	private void OnDisable()
	{
		AkRoom.RoomCount--;
		AkSoundEngine.RemoveRoom(this.GetID());
	}

	// Token: 0x06009DA6 RID: 40358 RVA: 0x003F0BB4 File Offset: 0x003EEDB4
	private void OnTriggerEnter(Collider in_other)
	{
		AkSpatialAudioBase[] componentsInChildren = in_other.GetComponentsInChildren<AkSpatialAudioBase>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].enabled)
			{
				componentsInChildren[i].EnteredRoom(this);
			}
		}
	}

	// Token: 0x06009DA7 RID: 40359 RVA: 0x003F0BF4 File Offset: 0x003EEDF4
	private void OnTriggerExit(Collider in_other)
	{
		AkSpatialAudioBase[] componentsInChildren = in_other.GetComponentsInChildren<AkSpatialAudioBase>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].enabled)
			{
				componentsInChildren[i].ExitedRoom(this);
			}
		}
	}

	// Token: 0x04009F24 RID: 40740
	public static ulong INVALID_ROOM_ID = ulong.MaxValue;

	// Token: 0x04009F25 RID: 40741
	private static int RoomCount;

	// Token: 0x04009F26 RID: 40742
	[Tooltip("Higher number has a higher priority")]
	public int priority;

	// Token: 0x04009F27 RID: 40743
	public AuxBus reverbAuxBus;

	// Token: 0x04009F28 RID: 40744
	[Range(0f, 1f)]
	public float reverbLevel = 1f;

	// Token: 0x04009F29 RID: 40745
	[Range(0f, 1f)]
	public float wallOcclusion = 1f;

	// Token: 0x020018FD RID: 6397
	public class PriorityList
	{
		// Token: 0x06009DAA RID: 40362 RVA: 0x003F0C54 File Offset: 0x003EEE54
		public ulong GetHighestPriorityRoomID()
		{
			AkRoom highestPriorityRoom = this.GetHighestPriorityRoom();
			return (!(highestPriorityRoom == null)) ? highestPriorityRoom.GetID() : AkRoom.INVALID_ROOM_ID;
		}

		// Token: 0x06009DAB RID: 40363 RVA: 0x003F0C84 File Offset: 0x003EEE84
		public AkRoom GetHighestPriorityRoom()
		{
			if (this.rooms.Count == 0)
			{
				return null;
			}
			return this.rooms[0];
		}

		// Token: 0x06009DAC RID: 40364 RVA: 0x003F0CA4 File Offset: 0x003EEEA4
		public void Add(AkRoom room)
		{
			int num = this.BinarySearch(room);
			if (num < 0)
			{
				this.rooms.Insert(~num, room);
			}
		}

		// Token: 0x06009DAD RID: 40365 RVA: 0x003F0CD0 File Offset: 0x003EEED0
		public void Remove(AkRoom room)
		{
			this.rooms.Remove(room);
		}

		// Token: 0x06009DAE RID: 40366 RVA: 0x003F0CE0 File Offset: 0x003EEEE0
		public bool Contains(AkRoom room)
		{
			return this.BinarySearch(room) >= 0;
		}

		// Token: 0x06009DAF RID: 40367 RVA: 0x003F0CF0 File Offset: 0x003EEEF0
		public int BinarySearch(AkRoom room)
		{
			return this.rooms.BinarySearch(room, AkRoom.PriorityList.s_compareByPriority);
		}

		// Token: 0x04009F2A RID: 40746
		private static readonly AkRoom.PriorityList.CompareByPriority s_compareByPriority = new AkRoom.PriorityList.CompareByPriority();

		// Token: 0x04009F2B RID: 40747
		public List<AkRoom> rooms = new List<AkRoom>();

		// Token: 0x020018FE RID: 6398
		private class CompareByPriority : IComparer<AkRoom>
		{
			// Token: 0x06009DB2 RID: 40370 RVA: 0x003F0D18 File Offset: 0x003EEF18
			public virtual int Compare(AkRoom a, AkRoom b)
			{
				int num = a.priority.CompareTo(b.priority);
				if (num == 0 && a != b)
				{
					return 1;
				}
				return -num;
			}
		}
	}
}
