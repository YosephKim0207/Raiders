using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000903 RID: 2307
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Sends Events based on the value of a Boolean Variable.")]
	public class BoolTest : FsmStateAction
	{
		// Token: 0x060032CC RID: 13004 RVA: 0x0010A8AC File Offset: 0x00108AAC
		public override void Reset()
		{
			this.boolVariable = null;
			this.isTrue = null;
			this.isFalse = null;
			this.everyFrame = false;
		}

		// Token: 0x060032CD RID: 13005 RVA: 0x0010A8CC File Offset: 0x00108ACC
		public override void OnEnter()
		{
			base.Fsm.Event((!this.boolVariable.Value) ? this.isFalse : this.isTrue);
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060032CE RID: 13006 RVA: 0x0010A90C File Offset: 0x00108B0C
		public override void OnUpdate()
		{
			base.Fsm.Event((!this.boolVariable.Value) ? this.isFalse : this.isTrue);
		}

		// Token: 0x040023F6 RID: 9206
		[Tooltip("The Bool variable to test.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool boolVariable;

		// Token: 0x040023F7 RID: 9207
		[Tooltip("Event to send if the Bool variable is True.")]
		public FsmEvent isTrue;

		// Token: 0x040023F8 RID: 9208
		[Tooltip("Event to send if the Bool variable is False.")]
		public FsmEvent isFalse;

		// Token: 0x040023F9 RID: 9209
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
