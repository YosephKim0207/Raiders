using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C83 RID: 3203
	[ActionCategory(ActionCategory.Events)]
	public class ActivateBossRushElevator : FsmStateAction
	{
		// Token: 0x060044AD RID: 17581 RVA: 0x00162FC4 File Offset: 0x001611C4
		public override void Reset()
		{
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x00162FC8 File Offset: 0x001611C8
		public override void OnEnter()
		{
			ShortcutElevatorController shortcutElevatorController = UnityEngine.Object.FindObjectOfType<ShortcutElevatorController>();
			shortcutElevatorController.SetBossRushPaymentValid();
			base.Finish();
		}
	}
}
