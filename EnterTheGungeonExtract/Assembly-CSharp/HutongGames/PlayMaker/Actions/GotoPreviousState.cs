using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009C1 RID: 2497
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Immediately return to the previously active state.")]
	public class GotoPreviousState : FsmStateAction
	{
		// Token: 0x06003603 RID: 13827 RVA: 0x00114BC8 File Offset: 0x00112DC8
		public override void Reset()
		{
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x00114BCC File Offset: 0x00112DCC
		public override void OnEnter()
		{
			if (base.Fsm.PreviousActiveState != null)
			{
				base.Log("Goto Previous State: " + base.Fsm.PreviousActiveState.Name);
				base.Fsm.GotoPreviousState();
			}
			base.Finish();
		}
	}
}
