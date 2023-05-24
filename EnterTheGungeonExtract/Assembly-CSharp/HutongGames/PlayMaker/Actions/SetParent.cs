using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B0A RID: 2826
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Sets the Parent of a Game Object.")]
	public class SetParent : FsmStateAction
	{
		// Token: 0x06003B9A RID: 15258 RVA: 0x0012C8E0 File Offset: 0x0012AAE0
		public override void Reset()
		{
			this.gameObject = null;
			this.parent = null;
			this.resetLocalPosition = null;
			this.resetLocalRotation = null;
		}

		// Token: 0x06003B9B RID: 15259 RVA: 0x0012C900 File Offset: 0x0012AB00
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget != null)
			{
				ownerDefaultTarget.transform.parent = ((!(this.parent.Value == null)) ? this.parent.Value.transform : null);
				if (this.resetLocalPosition.Value)
				{
					ownerDefaultTarget.transform.localPosition = Vector3.zero;
				}
				if (this.resetLocalRotation.Value)
				{
					ownerDefaultTarget.transform.localRotation = Quaternion.identity;
				}
			}
			base.Finish();
		}

		// Token: 0x04002DBA RID: 11706
		[Tooltip("The Game Object to parent.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002DBB RID: 11707
		[Tooltip("The new parent for the Game Object.")]
		public FsmGameObject parent;

		// Token: 0x04002DBC RID: 11708
		[Tooltip("Set the local position to 0,0,0 after parenting.")]
		public FsmBool resetLocalPosition;

		// Token: 0x04002DBD RID: 11709
		[Tooltip("Set the local rotation to 0,0,0 after parenting.")]
		public FsmBool resetLocalRotation;
	}
}
