using System;
using Dungeonator;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CB4 RID: 3252
	[ActionCategory(".NPCs")]
	[Tooltip("Spawns a chest in the NPC's current room.")]
	public class SpawnChest : FsmStateAction
	{
		// Token: 0x06004559 RID: 17753 RVA: 0x001679CC File Offset: 0x00165BCC
		public override void Reset()
		{
			this.type = SpawnChest.Type.RoomReward;
			this.CustomChest = null;
		}

		// Token: 0x0600455A RID: 17754 RVA: 0x001679DC File Offset: 0x00165BDC
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			WeightedGameObjectCollection weightedGameObjectCollection = null;
			if (this.type == SpawnChest.Type.RoomReward)
			{
				weightedGameObjectCollection = null;
			}
			else if (this.type == SpawnChest.Type.Custom)
			{
				WeightedGameObject weightedGameObject = new WeightedGameObject();
				weightedGameObject.SetGameObject(this.CustomChest);
				weightedGameObjectCollection = new WeightedGameObjectCollection();
				weightedGameObjectCollection.Add(weightedGameObject);
			}
			RoomHandler parentRoom = component.ParentRoom;
			if (this.spawnLocation == SpawnChest.SpawnLocation.BestRoomLocation)
			{
				parentRoom.SpawnRoomRewardChest(weightedGameObjectCollection, component.ParentRoom.GetBestRewardLocation(new IntVector2(2, 1), RoomHandler.RewardLocationStyle.CameraCenter, true));
			}
			else if (this.spawnLocation == SpawnChest.SpawnLocation.OffsetFromTalkDoer)
			{
				Vector2 vector = ((!(component.specRigidbody != null)) ? component.sprite.WorldCenter : component.specRigidbody.UnitCenter);
				vector += this.spawnOffset;
				parentRoom.SpawnRoomRewardChest(weightedGameObjectCollection, component.ParentRoom.GetBestRewardLocation(new IntVector2(2, 1), vector, true));
			}
			base.Finish();
		}

		// Token: 0x04003780 RID: 14208
		[Tooltip("Type of chest to spawn.")]
		public SpawnChest.Type type;

		// Token: 0x04003781 RID: 14209
		[Tooltip("Specific chest to spawn.")]
		public GameObject CustomChest;

		// Token: 0x04003782 RID: 14210
		[Tooltip("Where to spawn the item at.")]
		public SpawnChest.SpawnLocation spawnLocation;

		// Token: 0x04003783 RID: 14211
		[Tooltip("Offset from the TalkDoer to spawn the item at.")]
		public Vector2 spawnOffset;

		// Token: 0x02000CB5 RID: 3253
		public enum Type
		{
			// Token: 0x04003785 RID: 14213
			RoomReward,
			// Token: 0x04003786 RID: 14214
			Custom
		}

		// Token: 0x02000CB6 RID: 3254
		public enum SpawnLocation
		{
			// Token: 0x04003788 RID: 14216
			BestRoomLocation,
			// Token: 0x04003789 RID: 14217
			OffsetFromTalkDoer
		}
	}
}
