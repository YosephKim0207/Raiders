using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A6F RID: 2671
	[Tooltip("Controls whether 2D physics affects the Game Object.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class SetIsKinematic2d : ComponentAction<Rigidbody2D>
	{
		// Token: 0x060038CD RID: 14541 RVA: 0x00123850 File Offset: 0x00121A50
		public override void Reset()
		{
			this.gameObject = null;
			this.isKinematic = false;
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x00123868 File Offset: 0x00121A68
		public override void OnEnter()
		{
			this.DoSetIsKinematic();
			base.Finish();
		}

		// Token: 0x060038CF RID: 14543 RVA: 0x00123878 File Offset: 0x00121A78
		private void DoSetIsKinematic()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			base.rigidbody2d.isKinematic = this.isKinematic.Value;
		}

		// Token: 0x04002B25 RID: 11045
		[Tooltip("The GameObject with the Rigidbody2D attached")]
		[CheckForComponent(typeof(Rigidbody2D))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002B26 RID: 11046
		[Tooltip("The isKinematic value")]
		[RequiredField]
		public FsmBool isKinematic;
	}
}
