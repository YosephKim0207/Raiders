using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000945 RID: 2373
	[Tooltip("Finds the Child of a GameObject by Name.\nNote, you can specify a path to the child, e.g., LeftShoulder/Arm/Hand/Finger. If you need to specify a tag, use GetChild.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class FindChild : FsmStateAction
	{
		// Token: 0x060033EA RID: 13290 RVA: 0x0010E910 File Offset: 0x0010CB10
		public override void Reset()
		{
			this.gameObject = null;
			this.childName = string.Empty;
			this.storeResult = null;
		}

		// Token: 0x060033EB RID: 13291 RVA: 0x0010E930 File Offset: 0x0010CB30
		public override void OnEnter()
		{
			this.DoFindChild();
			base.Finish();
		}

		// Token: 0x060033EC RID: 13292 RVA: 0x0010E940 File Offset: 0x0010CB40
		private void DoFindChild()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Transform transform = ownerDefaultTarget.transform.Find(this.childName.Value);
			this.storeResult.Value = ((!(transform != null)) ? null : transform.gameObject);
		}

		// Token: 0x0400250C RID: 9484
		[Tooltip("The GameObject to search.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400250D RID: 9485
		[Tooltip("The name of the child. Note, you can specify a path to the child, e.g., LeftShoulder/Arm/Hand/Finger")]
		[RequiredField]
		public FsmString childName;

		// Token: 0x0400250E RID: 9486
		[Tooltip("Store the child in a GameObject variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmGameObject storeResult;
	}
}
