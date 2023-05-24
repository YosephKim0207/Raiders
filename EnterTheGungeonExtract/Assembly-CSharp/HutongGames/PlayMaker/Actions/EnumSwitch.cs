using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000943 RID: 2371
	[Tooltip("Sends an Event based on the value of an Enum Variable.")]
	[ActionCategory(ActionCategory.Logic)]
	public class EnumSwitch : FsmStateAction
	{
		// Token: 0x060033DE RID: 13278 RVA: 0x0010E70C File Offset: 0x0010C90C
		public override void Reset()
		{
			this.enumVariable = null;
			this.compareTo = new FsmEnum[0];
			this.sendEvent = new FsmEvent[0];
			this.everyFrame = false;
		}

		// Token: 0x060033DF RID: 13279 RVA: 0x0010E734 File Offset: 0x0010C934
		public override void OnEnter()
		{
			this.DoEnumSwitch();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060033E0 RID: 13280 RVA: 0x0010E750 File Offset: 0x0010C950
		public override void OnUpdate()
		{
			this.DoEnumSwitch();
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x0010E758 File Offset: 0x0010C958
		private void DoEnumSwitch()
		{
			if (this.enumVariable.IsNone)
			{
				return;
			}
			for (int i = 0; i < this.compareTo.Length; i++)
			{
				if (object.Equals(this.enumVariable.Value, this.compareTo[i].Value))
				{
					base.Fsm.Event(this.sendEvent[i]);
					return;
				}
			}
		}

		// Token: 0x040024FF RID: 9471
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmEnum enumVariable;

		// Token: 0x04002500 RID: 9472
		[CompoundArray("Enum Switches", "Compare Enum Values", "Send")]
		[MatchFieldType("enumVariable")]
		public FsmEnum[] compareTo;

		// Token: 0x04002501 RID: 9473
		public FsmEvent[] sendEvent;

		// Token: 0x04002502 RID: 9474
		public bool everyFrame;
	}
}
