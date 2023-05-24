using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200096F RID: 2415
	[Tooltip("Gets the Child of a GameObject by Index.\nE.g., O to get the first child. HINT: Use this with an integer variable to iterate through children.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetChildNum : FsmStateAction
	{
		// Token: 0x0600349B RID: 13467 RVA: 0x00110AA4 File Offset: 0x0010ECA4
		public override void Reset()
		{
			this.gameObject = null;
			this.childIndex = 0;
			this.store = null;
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x00110AC0 File Offset: 0x0010ECC0
		public override void OnEnter()
		{
			this.store.Value = this.DoGetChildNum(base.Fsm.GetOwnerDefaultTarget(this.gameObject));
			base.Finish();
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x00110AEC File Offset: 0x0010ECEC
		private GameObject DoGetChildNum(GameObject go)
		{
			return (!(go == null)) ? go.transform.GetChild(this.childIndex.Value % go.transform.childCount).gameObject : null;
		}

		// Token: 0x040025CB RID: 9675
		[RequiredField]
		[Tooltip("The GameObject to search.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x040025CC RID: 9676
		[RequiredField]
		[Tooltip("The index of the child to find.")]
		public FsmInt childIndex;

		// Token: 0x040025CD RID: 9677
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the child in a GameObject variable.")]
		[RequiredField]
		public FsmGameObject store;
	}
}
