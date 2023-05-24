using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CB7 RID: 3255
	[ActionCategory(".NPCs")]
	[Tooltip("Spawns enemies in the NPC's current room.")]
	public class SpawnEnemies : FsmStateAction
	{
		// Token: 0x0600455C RID: 17756 RVA: 0x00167ADC File Offset: 0x00165CDC
		public override void Reset()
		{
			this.type = SpawnEnemies.Type.Reinforcement;
			this.roomEventTrigger = RoomEventTriggerCondition.NPC_TRIGGER_A;
		}

		// Token: 0x0600455D RID: 17757 RVA: 0x00167AF0 File Offset: 0x00165CF0
		public override void OnEnter()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			if (this.type == SpawnEnemies.Type.Reinforcement)
			{
				component.ParentRoom.TriggerReinforcementLayersOnEvent(this.roomEventTrigger, this.InstantReinforcement);
				component.ParentRoom.SealRoom();
			}
			base.Finish();
		}

		// Token: 0x0400378A RID: 14218
		[Tooltip("Type of enemy spawn.")]
		public SpawnEnemies.Type type;

		// Token: 0x0400378B RID: 14219
		public RoomEventTriggerCondition roomEventTrigger;

		// Token: 0x0400378C RID: 14220
		public bool InstantReinforcement;

		// Token: 0x02000CB8 RID: 3256
		public enum Type
		{
			// Token: 0x0400378E RID: 14222
			Reinforcement
		}
	}
}
