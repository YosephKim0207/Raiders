using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000962 RID: 2402
	[Tooltip("Tests if a GameObject is a Child of another GameObject.")]
	[ActionCategory(ActionCategory.Logic)]
	public class GameObjectIsChildOf : FsmStateAction
	{
		// Token: 0x06003468 RID: 13416 RVA: 0x0010FFB8 File Offset: 0x0010E1B8
		public override void Reset()
		{
			this.gameObject = null;
			this.isChildOf = null;
			this.trueEvent = null;
			this.falseEvent = null;
			this.storeResult = null;
		}

		// Token: 0x06003469 RID: 13417 RVA: 0x0010FFE0 File Offset: 0x0010E1E0
		public override void OnEnter()
		{
			this.DoIsChildOf(base.Fsm.GetOwnerDefaultTarget(this.gameObject));
			base.Finish();
		}

		// Token: 0x0600346A RID: 13418 RVA: 0x00110000 File Offset: 0x0010E200
		private void DoIsChildOf(GameObject go)
		{
			if (go == null || this.isChildOf == null)
			{
				return;
			}
			bool flag = go.transform.IsChildOf(this.isChildOf.Value.transform);
			this.storeResult.Value = flag;
			base.Fsm.Event((!flag) ? this.falseEvent : this.trueEvent);
		}

		// Token: 0x04002594 RID: 9620
		[Tooltip("GameObject to test.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002595 RID: 9621
		[Tooltip("Is it a child of this GameObject?")]
		[RequiredField]
		public FsmGameObject isChildOf;

		// Token: 0x04002596 RID: 9622
		[Tooltip("Event to send if GameObject is a child.")]
		public FsmEvent trueEvent;

		// Token: 0x04002597 RID: 9623
		[Tooltip("Event to send if GameObject is NOT a child.")]
		public FsmEvent falseEvent;

		// Token: 0x04002598 RID: 9624
		[Tooltip("Store result in a bool variable")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;
	}
}
