using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CA9 RID: 3241
	[Tooltip("Triggers phantom door events.")]
	[ActionCategory(".NPCs")]
	public class PhantomDoor : FsmStateAction
	{
		// Token: 0x0600453E RID: 17726 RVA: 0x00166C60 File Offset: 0x00164E60
		public override void Reset()
		{
			this.seal = false;
		}

		// Token: 0x0600453F RID: 17727 RVA: 0x00166C70 File Offset: 0x00164E70
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			component.specRigidbody.Initialize();
			RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(component.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor));
			if (this.seal.Value)
			{
				DungeonDoorSubsidiaryBlocker closestToPosition = BraveUtility.GetClosestToPosition<DungeonDoorSubsidiaryBlocker>(new List<DungeonDoorSubsidiaryBlocker>(UnityEngine.Object.FindObjectsOfType<DungeonDoorSubsidiaryBlocker>()), roomFromPosition.area.Center, new DungeonDoorSubsidiaryBlocker[0]);
				closestToPosition.Seal();
			}
			base.Finish();
		}

		// Token: 0x04003761 RID: 14177
		[Tooltip("Seals the room the Owner is in.")]
		public FsmBool seal;
	}
}
