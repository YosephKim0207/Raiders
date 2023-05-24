using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009F2 RID: 2546
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Sends an Event based on the value of an Integer Variable.")]
	public class IntSwitch : FsmStateAction
	{
		// Token: 0x060036A3 RID: 13987 RVA: 0x0011729C File Offset: 0x0011549C
		public override void Reset()
		{
			this.intVariable = null;
			this.compareTo = new FsmInt[1];
			this.sendEvent = new FsmEvent[1];
			this.everyFrame = false;
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x001172C4 File Offset: 0x001154C4
		public override void OnEnter()
		{
			this.DoIntSwitch();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060036A5 RID: 13989 RVA: 0x001172E0 File Offset: 0x001154E0
		public override void OnUpdate()
		{
			this.DoIntSwitch();
		}

		// Token: 0x060036A6 RID: 13990 RVA: 0x001172E8 File Offset: 0x001154E8
		private void DoIntSwitch()
		{
			if (this.intVariable.IsNone)
			{
				return;
			}
			for (int i = 0; i < this.compareTo.Length; i++)
			{
				if (this.intVariable.Value == this.compareTo[i].Value)
				{
					base.Fsm.Event(this.sendEvent[i]);
					return;
				}
			}
		}

		// Token: 0x04002803 RID: 10243
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt intVariable;

		// Token: 0x04002804 RID: 10244
		[CompoundArray("Int Switches", "Compare Int", "Send Event")]
		public FsmInt[] compareTo;

		// Token: 0x04002805 RID: 10245
		public FsmEvent[] sendEvent;

		// Token: 0x04002806 RID: 10246
		public bool everyFrame;
	}
}
