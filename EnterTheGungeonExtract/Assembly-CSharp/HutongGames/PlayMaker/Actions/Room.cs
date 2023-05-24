using System;
using Dungeonator;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CAD RID: 3245
	[Tooltip("Respondes to chest events.")]
	[ActionCategory(".NPCs")]
	public class Room : FsmStateAction
	{
		// Token: 0x06004548 RID: 17736 RVA: 0x001670A4 File Offset: 0x001652A4
		public override void Reset()
		{
			this.seal = false;
			this.unseal = false;
		}

		// Token: 0x06004549 RID: 17737 RVA: 0x001670C0 File Offset: 0x001652C0
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			component.specRigidbody.Initialize();
			RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(component.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor));
			if (this.seal.Value)
			{
				roomFromPosition.npcSealState = RoomHandler.NPCSealState.SealAll;
				if (GameManager.Instance.InTutorial && component.name.Contains("NPC_Tutorial_Knight_001_intro"))
				{
					roomFromPosition.npcSealState = RoomHandler.NPCSealState.SealNext;
				}
				roomFromPosition.SealRoom();
			}
			else if (this.unseal.Value)
			{
				roomFromPosition.npcSealState = RoomHandler.NPCSealState.SealNone;
				if (GameManager.Instance.InTutorial)
				{
					if (component.name.Contains("NPC_Tutorial_Knight_001_intro"))
					{
						roomFromPosition.npcSealState = RoomHandler.NPCSealState.SealNone;
					}
					else
					{
						roomFromPosition.npcSealState = RoomHandler.NPCSealState.SealPrior;
					}
				}
				if (this.unsealAllForceTutorial.Value)
				{
					roomFromPosition.npcSealState = RoomHandler.NPCSealState.SealNone;
				}
				roomFromPosition.UnsealRoom();
			}
			base.Finish();
		}

		// Token: 0x04003766 RID: 14182
		[Tooltip("Seals the room the Owner is in.")]
		public FsmBool seal;

		// Token: 0x04003767 RID: 14183
		[Tooltip("Unseals the room the Owner is in.")]
		public FsmBool unseal;

		// Token: 0x04003768 RID: 14184
		[Tooltip("Ignores SealPrior in Tutorial.")]
		public FsmBool unsealAllForceTutorial;
	}
}
