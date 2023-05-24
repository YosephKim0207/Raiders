using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000961 RID: 2401
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Tests if a GameObject has children.")]
	public class GameObjectHasChildren : FsmStateAction
	{
		// Token: 0x06003463 RID: 13411 RVA: 0x0010FEF8 File Offset: 0x0010E0F8
		public override void Reset()
		{
			this.gameObject = null;
			this.trueEvent = null;
			this.falseEvent = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x0010FF20 File Offset: 0x0010E120
		public override void OnEnter()
		{
			this.DoHasChildren();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003465 RID: 13413 RVA: 0x0010FF3C File Offset: 0x0010E13C
		public override void OnUpdate()
		{
			this.DoHasChildren();
		}

		// Token: 0x06003466 RID: 13414 RVA: 0x0010FF44 File Offset: 0x0010E144
		private void DoHasChildren()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			bool flag = ownerDefaultTarget.transform.childCount > 0;
			this.storeResult.Value = flag;
			base.Fsm.Event((!flag) ? this.falseEvent : this.trueEvent);
		}

		// Token: 0x0400258F RID: 9615
		[Tooltip("The GameObject to test.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002590 RID: 9616
		[Tooltip("Event to send if the GameObject has children.")]
		public FsmEvent trueEvent;

		// Token: 0x04002591 RID: 9617
		[Tooltip("Event to send if the GameObject does not have children.")]
		public FsmEvent falseEvent;

		// Token: 0x04002592 RID: 9618
		[Tooltip("Store the result in a bool variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		// Token: 0x04002593 RID: 9619
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
