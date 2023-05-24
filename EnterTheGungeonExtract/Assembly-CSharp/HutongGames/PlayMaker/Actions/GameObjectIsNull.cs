using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000963 RID: 2403
	[Tooltip("Tests if a GameObject Variable has a null value. E.g., If the FindGameObject action failed to find an object.")]
	[ActionCategory(ActionCategory.Logic)]
	public class GameObjectIsNull : FsmStateAction
	{
		// Token: 0x0600346C RID: 13420 RVA: 0x00110078 File Offset: 0x0010E278
		public override void Reset()
		{
			this.gameObject = null;
			this.isNull = null;
			this.isNotNull = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x0600346D RID: 13421 RVA: 0x001100A0 File Offset: 0x0010E2A0
		public override void OnEnter()
		{
			this.DoIsGameObjectNull();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600346E RID: 13422 RVA: 0x001100BC File Offset: 0x0010E2BC
		public override void OnUpdate()
		{
			this.DoIsGameObjectNull();
		}

		// Token: 0x0600346F RID: 13423 RVA: 0x001100C4 File Offset: 0x0010E2C4
		private void DoIsGameObjectNull()
		{
			bool flag = this.gameObject.Value == null;
			if (this.storeResult != null)
			{
				this.storeResult.Value = flag;
			}
			base.Fsm.Event((!flag) ? this.isNotNull : this.isNull);
		}

		// Token: 0x04002599 RID: 9625
		[Tooltip("The GameObject variable to test.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmGameObject gameObject;

		// Token: 0x0400259A RID: 9626
		[Tooltip("Event to send if the GamObject is null.")]
		public FsmEvent isNull;

		// Token: 0x0400259B RID: 9627
		[Tooltip("Event to send if the GamObject is NOT null.")]
		public FsmEvent isNotNull;

		// Token: 0x0400259C RID: 9628
		[Tooltip("Store the result in a bool variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		// Token: 0x0400259D RID: 9629
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
