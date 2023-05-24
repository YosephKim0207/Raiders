using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008FC RID: 2300
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if all the given Bool Variables are True.")]
	public class BoolAllTrue : FsmStateAction
	{
		// Token: 0x060032B1 RID: 12977 RVA: 0x0010A498 File Offset: 0x00108698
		public override void Reset()
		{
			this.boolVariables = null;
			this.sendEvent = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x0010A4B8 File Offset: 0x001086B8
		public override void OnEnter()
		{
			this.DoAllTrue();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060032B3 RID: 12979 RVA: 0x0010A4D4 File Offset: 0x001086D4
		public override void OnUpdate()
		{
			this.DoAllTrue();
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x0010A4DC File Offset: 0x001086DC
		private void DoAllTrue()
		{
			if (this.boolVariables.Length == 0)
			{
				return;
			}
			bool flag = true;
			for (int i = 0; i < this.boolVariables.Length; i++)
			{
				if (!this.boolVariables[i].Value)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				base.Fsm.Event(this.sendEvent);
			}
			this.storeResult.Value = flag;
		}

		// Token: 0x040023DB RID: 9179
		[Tooltip("The Bool variables to check.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool[] boolVariables;

		// Token: 0x040023DC RID: 9180
		[Tooltip("Event to send if all the Bool variables are True.")]
		public FsmEvent sendEvent;

		// Token: 0x040023DD RID: 9181
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a Bool variable.")]
		public FsmBool storeResult;

		// Token: 0x040023DE RID: 9182
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
