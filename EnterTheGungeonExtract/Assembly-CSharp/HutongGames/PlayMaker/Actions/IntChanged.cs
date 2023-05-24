using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009ED RID: 2541
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if the value of an integer variable changed. Use this to send an event on change, or store a bool that can be used in other operations.")]
	public class IntChanged : FsmStateAction
	{
		// Token: 0x0600368F RID: 13967 RVA: 0x00116F18 File Offset: 0x00115118
		public override void Reset()
		{
			this.intVariable = null;
			this.changedEvent = null;
			this.storeResult = null;
		}

		// Token: 0x06003690 RID: 13968 RVA: 0x00116F30 File Offset: 0x00115130
		public override void OnEnter()
		{
			if (this.intVariable.IsNone)
			{
				base.Finish();
				return;
			}
			this.previousValue = this.intVariable.Value;
		}

		// Token: 0x06003691 RID: 13969 RVA: 0x00116F5C File Offset: 0x0011515C
		public override void OnUpdate()
		{
			this.storeResult.Value = false;
			if (this.intVariable.Value != this.previousValue)
			{
				this.previousValue = this.intVariable.Value;
				this.storeResult.Value = true;
				base.Fsm.Event(this.changedEvent);
			}
		}

		// Token: 0x040027E9 RID: 10217
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt intVariable;

		// Token: 0x040027EA RID: 10218
		public FsmEvent changedEvent;

		// Token: 0x040027EB RID: 10219
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		// Token: 0x040027EC RID: 10220
		private int previousValue;
	}
}
