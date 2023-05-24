using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A4E RID: 2638
	[Tooltip("Compare 2 Object Variables and send events based on the result.")]
	[ActionCategory(ActionCategory.Logic)]
	public class ObjectCompare : FsmStateAction
	{
		// Token: 0x06003829 RID: 14377 RVA: 0x001202D0 File Offset: 0x0011E4D0
		public override void Reset()
		{
			this.objectVariable = null;
			this.compareTo = null;
			this.storeResult = null;
			this.equalEvent = null;
			this.notEqualEvent = null;
			this.everyFrame = false;
		}

		// Token: 0x0600382A RID: 14378 RVA: 0x001202FC File Offset: 0x0011E4FC
		public override void OnEnter()
		{
			this.DoObjectCompare();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600382B RID: 14379 RVA: 0x00120318 File Offset: 0x0011E518
		public override void OnUpdate()
		{
			this.DoObjectCompare();
		}

		// Token: 0x0600382C RID: 14380 RVA: 0x00120320 File Offset: 0x0011E520
		private void DoObjectCompare()
		{
			bool flag = this.objectVariable.Value == this.compareTo.Value;
			this.storeResult.Value = flag;
			base.Fsm.Event((!flag) ? this.notEqualEvent : this.equalEvent);
		}

		// Token: 0x04002A26 RID: 10790
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmObject objectVariable;

		// Token: 0x04002A27 RID: 10791
		[RequiredField]
		public FsmObject compareTo;

		// Token: 0x04002A28 RID: 10792
		[Tooltip("Event to send if the 2 object values are equal.")]
		public FsmEvent equalEvent;

		// Token: 0x04002A29 RID: 10793
		[Tooltip("Event to send if the 2 object values are not equal.")]
		public FsmEvent notEqualEvent;

		// Token: 0x04002A2A RID: 10794
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a variable.")]
		public FsmBool storeResult;

		// Token: 0x04002A2B RID: 10795
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
