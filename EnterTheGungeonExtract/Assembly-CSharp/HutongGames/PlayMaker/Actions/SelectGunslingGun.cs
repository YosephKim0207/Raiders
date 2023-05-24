using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CAE RID: 3246
	[ActionCategory(".NPCs")]
	public class SelectGunslingGun : BraveFsmStateAction
	{
		// Token: 0x0600454B RID: 17739 RVA: 0x001671CC File Offset: 0x001653CC
		public override void Reset()
		{
			this.lootTable = null;
		}

		// Token: 0x0600454C RID: 17740 RVA: 0x001671D8 File Offset: 0x001653D8
		public override void OnEnter()
		{
			if (this.SelectedObject == null)
			{
				this.SelectedObject = this.lootTable.SelectByWeightWithoutDuplicatesFullPrereqs(null, false, false);
			}
			if (this.SelectedObject == null)
			{
				this.SelectedObject = this.lootTable.defaultItemDrops.elements[UnityEngine.Random.Range(0, this.lootTable.defaultItemDrops.elements.Count)].gameObject;
			}
			EncounterTrackable component = this.SelectedObject.GetComponent<EncounterTrackable>();
			if (component != null)
			{
				base.SetReplacementString(component.journalData.GetPrimaryDisplayName(false));
			}
			base.Finish();
		}

		// Token: 0x04003769 RID: 14185
		[Tooltip("Loot table to choose an item from.")]
		public GenericLootTable lootTable;

		// Token: 0x0400376A RID: 14186
		[NonSerialized]
		public GameObject SelectedObject;
	}
}
