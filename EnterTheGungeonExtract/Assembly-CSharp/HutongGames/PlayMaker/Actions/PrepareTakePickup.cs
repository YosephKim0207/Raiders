using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CAB RID: 3243
	public class PrepareTakePickup : FsmStateAction
	{
		// Token: 0x06004543 RID: 17731 RVA: 0x00166D5C File Offset: 0x00164F5C
		public override void OnEnter()
		{
			PickupObject byId = PickupObjectDatabase.GetById(this.TargetPickupIndex);
			FsmString fsmString = base.Fsm.Variables.GetFsmString("npcReplacementString");
			EncounterTrackable component = byId.GetComponent<EncounterTrackable>();
			if (fsmString != null && component != null)
			{
				fsmString.Value = component.journalData.GetPrimaryDisplayName(false);
			}
			base.Finish();
		}

		// Token: 0x04003762 RID: 14178
		public int TargetPickupIndex;
	}
}
