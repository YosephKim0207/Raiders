using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008FD RID: 2301
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if any of the given Bool Variables are True.")]
	public class BoolAnyTrue : FsmStateAction
	{
		// Token: 0x060032B6 RID: 12982 RVA: 0x0010A558 File Offset: 0x00108758
		public override void Reset()
		{
			this.boolVariables = null;
			this.sendEvent = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x0010A578 File Offset: 0x00108778
		public override void OnEnter()
		{
			this.DoAnyTrue();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x0010A594 File Offset: 0x00108794
		public override void OnUpdate()
		{
			this.DoAnyTrue();
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x0010A59C File Offset: 0x0010879C
		private void DoAnyTrue()
		{
			if (this.boolVariables.Length == 0)
			{
				return;
			}
			this.storeResult.Value = false;
			for (int i = 0; i < this.boolVariables.Length; i++)
			{
				if (this.boolVariables[i].Value)
				{
					base.Fsm.Event(this.sendEvent);
					this.storeResult.Value = true;
					return;
				}
			}
		}

		// Token: 0x040023DF RID: 9183
		[Tooltip("The Bool variables to check.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool[] boolVariables;

		// Token: 0x040023E0 RID: 9184
		[Tooltip("Event to send if any of the Bool variables are True.")]
		public FsmEvent sendEvent;

		// Token: 0x040023E1 RID: 9185
		[Tooltip("Store the result in a Bool variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		// Token: 0x040023E2 RID: 9186
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
