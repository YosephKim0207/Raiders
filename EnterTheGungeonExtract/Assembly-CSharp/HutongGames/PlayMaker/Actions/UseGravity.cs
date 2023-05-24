using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B46 RID: 2886
	[Tooltip("Sets whether a Game Object's Rigidy Body is affected by Gravity.")]
	[ActionCategory(ActionCategory.Physics)]
	public class UseGravity : ComponentAction<Rigidbody>
	{
		// Token: 0x06003C9D RID: 15517 RVA: 0x001308D4 File Offset: 0x0012EAD4
		public override void Reset()
		{
			this.gameObject = null;
			this.useGravity = true;
		}

		// Token: 0x06003C9E RID: 15518 RVA: 0x001308EC File Offset: 0x0012EAEC
		public override void OnEnter()
		{
			this.DoUseGravity();
			base.Finish();
		}

		// Token: 0x06003C9F RID: 15519 RVA: 0x001308FC File Offset: 0x0012EAFC
		private void DoUseGravity()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.rigidbody.useGravity = this.useGravity.Value;
			}
		}

		// Token: 0x04002EED RID: 12013
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002EEE RID: 12014
		[RequiredField]
		public FsmBool useGravity;
	}
}
