using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B2D RID: 2861
	[Tooltip("Sends an Event based on the value of a String Variable.")]
	[ActionCategory(ActionCategory.Logic)]
	public class StringSwitch : FsmStateAction
	{
		// Token: 0x06003C36 RID: 15414 RVA: 0x0012F044 File Offset: 0x0012D244
		public override void Reset()
		{
			this.stringVariable = null;
			this.compareTo = new FsmString[1];
			this.sendEvent = new FsmEvent[1];
			this.everyFrame = false;
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x0012F06C File Offset: 0x0012D26C
		public override void OnEnter()
		{
			this.DoStringSwitch();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C38 RID: 15416 RVA: 0x0012F088 File Offset: 0x0012D288
		public override void OnUpdate()
		{
			this.DoStringSwitch();
		}

		// Token: 0x06003C39 RID: 15417 RVA: 0x0012F090 File Offset: 0x0012D290
		private void DoStringSwitch()
		{
			if (this.stringVariable.IsNone)
			{
				return;
			}
			for (int i = 0; i < this.compareTo.Length; i++)
			{
				if (this.stringVariable.Value == this.compareTo[i].Value)
				{
					base.Fsm.Event(this.sendEvent[i]);
					return;
				}
			}
		}

		// Token: 0x04002E65 RID: 11877
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString stringVariable;

		// Token: 0x04002E66 RID: 11878
		[CompoundArray("String Switches", "Compare String", "Send Event")]
		public FsmString[] compareTo;

		// Token: 0x04002E67 RID: 11879
		public FsmEvent[] sendEvent;

		// Token: 0x04002E68 RID: 11880
		public bool everyFrame;
	}
}
