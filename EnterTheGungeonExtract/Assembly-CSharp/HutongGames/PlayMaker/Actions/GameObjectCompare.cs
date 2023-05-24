using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200095F RID: 2399
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Compares 2 Game Objects and sends Events based on the result.")]
	public class GameObjectCompare : FsmStateAction
	{
		// Token: 0x06003459 RID: 13401 RVA: 0x0010FD48 File Offset: 0x0010DF48
		public override void Reset()
		{
			this.gameObjectVariable = null;
			this.compareTo = null;
			this.equalEvent = null;
			this.notEqualEvent = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x0600345A RID: 13402 RVA: 0x0010FD74 File Offset: 0x0010DF74
		public override void OnEnter()
		{
			this.DoGameObjectCompare();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600345B RID: 13403 RVA: 0x0010FD90 File Offset: 0x0010DF90
		public override void OnUpdate()
		{
			this.DoGameObjectCompare();
		}

		// Token: 0x0600345C RID: 13404 RVA: 0x0010FD98 File Offset: 0x0010DF98
		private void DoGameObjectCompare()
		{
			bool flag = base.Fsm.GetOwnerDefaultTarget(this.gameObjectVariable) == this.compareTo.Value;
			this.storeResult.Value = flag;
			if (flag && this.equalEvent != null)
			{
				base.Fsm.Event(this.equalEvent);
			}
			else if (!flag && this.notEqualEvent != null)
			{
				base.Fsm.Event(this.notEqualEvent);
			}
		}

		// Token: 0x04002583 RID: 9603
		[Title("Game Object")]
		[UIHint(UIHint.Variable)]
		[Tooltip("A Game Object variable to compare.")]
		[RequiredField]
		public FsmOwnerDefault gameObjectVariable;

		// Token: 0x04002584 RID: 9604
		[Tooltip("Compare the variable with this Game Object")]
		[RequiredField]
		public FsmGameObject compareTo;

		// Token: 0x04002585 RID: 9605
		[Tooltip("Send this event if Game Objects are equal")]
		public FsmEvent equalEvent;

		// Token: 0x04002586 RID: 9606
		[Tooltip("Send this event if Game Objects are not equal")]
		public FsmEvent notEqualEvent;

		// Token: 0x04002587 RID: 9607
		[Tooltip("Store the result of the check in a Bool Variable. (True if equal, false if not equal).")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		// Token: 0x04002588 RID: 9608
		[Tooltip("Repeat every frame. Useful if you're waiting for a true or false result.")]
		public bool everyFrame;
	}
}
