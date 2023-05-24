using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200093B RID: 2363
	[ActionCategory(ActionCategory.Debug)]
	[Tooltip("Draws a state label for this FSM in the Game View. The label is drawn on the game object that owns the FSM. Use this to override the global setting in the PlayMaker Debug menu.")]
	public class DrawStateLabel : FsmStateAction
	{
		// Token: 0x060033BC RID: 13244 RVA: 0x0010DEEC File Offset: 0x0010C0EC
		public override void Reset()
		{
			this.showLabel = true;
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x0010DEFC File Offset: 0x0010C0FC
		public override void OnEnter()
		{
			base.Fsm.ShowStateLabel = this.showLabel.Value;
			base.Finish();
		}

		// Token: 0x040024DA RID: 9434
		[RequiredField]
		[Tooltip("Set to True to show State labels, or Fals to hide them.")]
		public FsmBool showLabel;
	}
}
