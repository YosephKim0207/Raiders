using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009A2 RID: 2466
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Gets the name of the previously active state and stores it in a String Variable.")]
	public class GetPreviousStateName : FsmStateAction
	{
		// Token: 0x0600357B RID: 13691 RVA: 0x001134FC File Offset: 0x001116FC
		public override void Reset()
		{
			this.storeName = null;
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x00113508 File Offset: 0x00111708
		public override void OnEnter()
		{
			this.storeName.Value = ((base.Fsm.PreviousActiveState != null) ? base.Fsm.PreviousActiveState.Name : null);
			base.Finish();
		}

		// Token: 0x040026CF RID: 9935
		[UIHint(UIHint.Variable)]
		public FsmString storeName;
	}
}
