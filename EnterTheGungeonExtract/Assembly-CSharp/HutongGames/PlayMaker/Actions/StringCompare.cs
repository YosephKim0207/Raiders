using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B2A RID: 2858
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Compares 2 Strings and sends Events based on the result.")]
	public class StringCompare : FsmStateAction
	{
		// Token: 0x06003C27 RID: 15399 RVA: 0x0012ED7C File Offset: 0x0012CF7C
		public override void Reset()
		{
			this.stringVariable = null;
			this.compareTo = string.Empty;
			this.equalEvent = null;
			this.notEqualEvent = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x06003C28 RID: 15400 RVA: 0x0012EDB4 File Offset: 0x0012CFB4
		public override void OnEnter()
		{
			this.DoStringCompare();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C29 RID: 15401 RVA: 0x0012EDD0 File Offset: 0x0012CFD0
		public override void OnUpdate()
		{
			this.DoStringCompare();
		}

		// Token: 0x06003C2A RID: 15402 RVA: 0x0012EDD8 File Offset: 0x0012CFD8
		private void DoStringCompare()
		{
			if (this.stringVariable == null || this.compareTo == null)
			{
				return;
			}
			bool flag = this.stringVariable.Value == this.compareTo.Value;
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

		// Token: 0x04002E54 RID: 11860
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		// Token: 0x04002E55 RID: 11861
		public FsmString compareTo;

		// Token: 0x04002E56 RID: 11862
		public FsmEvent equalEvent;

		// Token: 0x04002E57 RID: 11863
		public FsmEvent notEqualEvent;

		// Token: 0x04002E58 RID: 11864
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the true/false result in a bool variable.")]
		public FsmBool storeResult;

		// Token: 0x04002E59 RID: 11865
		[Tooltip("Repeat every frame. Useful if any of the strings are changing over time.")]
		public bool everyFrame;
	}
}
