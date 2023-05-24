using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000960 RID: 2400
	[Tooltip("Tests if a Game Object has a tag.")]
	[ActionCategory(ActionCategory.Logic)]
	public class GameObjectCompareTag : FsmStateAction
	{
		// Token: 0x0600345E RID: 13406 RVA: 0x0010FE24 File Offset: 0x0010E024
		public override void Reset()
		{
			this.gameObject = null;
			this.tag = "Untagged";
			this.trueEvent = null;
			this.falseEvent = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x0600345F RID: 13407 RVA: 0x0010FE5C File Offset: 0x0010E05C
		public override void OnEnter()
		{
			this.DoCompareTag();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003460 RID: 13408 RVA: 0x0010FE78 File Offset: 0x0010E078
		public override void OnUpdate()
		{
			this.DoCompareTag();
		}

		// Token: 0x06003461 RID: 13409 RVA: 0x0010FE80 File Offset: 0x0010E080
		private void DoCompareTag()
		{
			bool flag = false;
			if (this.gameObject.Value != null)
			{
				flag = this.gameObject.Value.CompareTag(this.tag.Value);
			}
			this.storeResult.Value = flag;
			base.Fsm.Event((!flag) ? this.falseEvent : this.trueEvent);
		}

		// Token: 0x04002589 RID: 9609
		[Tooltip("The GameObject to test.")]
		[RequiredField]
		public FsmGameObject gameObject;

		// Token: 0x0400258A RID: 9610
		[Tooltip("The Tag to check for.")]
		[UIHint(UIHint.Tag)]
		[RequiredField]
		public FsmString tag;

		// Token: 0x0400258B RID: 9611
		[Tooltip("Event to send if the GameObject has the Tag.")]
		public FsmEvent trueEvent;

		// Token: 0x0400258C RID: 9612
		[Tooltip("Event to send if the GameObject does not have the Tag.")]
		public FsmEvent falseEvent;

		// Token: 0x0400258D RID: 9613
		[Tooltip("Store the result in a Bool variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		// Token: 0x0400258E RID: 9614
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
