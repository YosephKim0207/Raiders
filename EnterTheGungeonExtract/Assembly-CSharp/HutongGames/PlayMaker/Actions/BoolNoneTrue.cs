using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000900 RID: 2304
	[Tooltip("Tests if all the Bool Variables are False.\nSend an event or store the result.")]
	[ActionCategory(ActionCategory.Logic)]
	public class BoolNoneTrue : FsmStateAction
	{
		// Token: 0x060032C2 RID: 12994 RVA: 0x0010A6E4 File Offset: 0x001088E4
		public override void Reset()
		{
			this.boolVariables = null;
			this.sendEvent = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x060032C3 RID: 12995 RVA: 0x0010A704 File Offset: 0x00108904
		public override void OnEnter()
		{
			this.DoNoneTrue();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x0010A720 File Offset: 0x00108920
		public override void OnUpdate()
		{
			this.DoNoneTrue();
		}

		// Token: 0x060032C5 RID: 12997 RVA: 0x0010A728 File Offset: 0x00108928
		private void DoNoneTrue()
		{
			if (this.boolVariables.Length == 0)
			{
				return;
			}
			bool flag = true;
			for (int i = 0; i < this.boolVariables.Length; i++)
			{
				if (this.boolVariables[i].Value)
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

		// Token: 0x040023E8 RID: 9192
		[Tooltip("The Bool variables to check.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool[] boolVariables;

		// Token: 0x040023E9 RID: 9193
		[Tooltip("Event to send if none of the Bool variables are True.")]
		public FsmEvent sendEvent;

		// Token: 0x040023EA RID: 9194
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a Bool variable.")]
		public FsmBool storeResult;

		// Token: 0x040023EB RID: 9195
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;
	}
}
