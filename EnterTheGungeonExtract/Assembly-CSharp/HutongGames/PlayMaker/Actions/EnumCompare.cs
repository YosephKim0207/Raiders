using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000942 RID: 2370
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Compares 2 Enum values and sends Events based on the result.")]
	public class EnumCompare : FsmStateAction
	{
		// Token: 0x060033D9 RID: 13273 RVA: 0x0010E618 File Offset: 0x0010C818
		public override void Reset()
		{
			this.enumVariable = null;
			this.compareTo = null;
			this.equalEvent = null;
			this.notEqualEvent = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x0010E644 File Offset: 0x0010C844
		public override void OnEnter()
		{
			this.DoEnumCompare();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x0010E660 File Offset: 0x0010C860
		public override void OnUpdate()
		{
			this.DoEnumCompare();
		}

		// Token: 0x060033DC RID: 13276 RVA: 0x0010E668 File Offset: 0x0010C868
		private void DoEnumCompare()
		{
			if (this.enumVariable == null || this.compareTo == null)
			{
				return;
			}
			bool flag = object.Equals(this.enumVariable.Value, this.compareTo.Value);
			if (this.storeResult != null)
			{
				this.storeResult.Value = flag;
			}
			if (flag && this.equalEvent != null)
			{
				base.Fsm.Event(this.equalEvent);
				return;
			}
			if (!flag && this.notEqualEvent != null)
			{
				base.Fsm.Event(this.notEqualEvent);
			}
		}

		// Token: 0x040024F9 RID: 9465
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmEnum enumVariable;

		// Token: 0x040024FA RID: 9466
		[MatchFieldType("enumVariable")]
		public FsmEnum compareTo;

		// Token: 0x040024FB RID: 9467
		public FsmEvent equalEvent;

		// Token: 0x040024FC RID: 9468
		public FsmEvent notEqualEvent;

		// Token: 0x040024FD RID: 9469
		[Tooltip("Store the true/false result in a bool variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		// Token: 0x040024FE RID: 9470
		[Tooltip("Repeat every frame. Useful if the enum is changing over time.")]
		public bool everyFrame;
	}
}
