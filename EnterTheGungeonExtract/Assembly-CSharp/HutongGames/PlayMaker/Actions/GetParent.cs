using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200099F RID: 2463
	[Tooltip("Gets the Parent of a Game Object.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetParent : FsmStateAction
	{
		// Token: 0x0600356F RID: 13679 RVA: 0x00113334 File Offset: 0x00111534
		public override void Reset()
		{
			this.gameObject = null;
			this.storeResult = null;
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x00113344 File Offset: 0x00111544
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget != null)
			{
				this.storeResult.Value = ((!(ownerDefaultTarget.transform.parent == null)) ? ownerDefaultTarget.transform.parent.gameObject : null);
			}
			else
			{
				this.storeResult.Value = null;
			}
			base.Finish();
		}

		// Token: 0x040026C5 RID: 9925
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040026C6 RID: 9926
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeResult;
	}
}
