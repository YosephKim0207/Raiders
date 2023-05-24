using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AF6 RID: 2806
	[Tooltip("Controls whether physics affects the Game Object.")]
	[ActionCategory(ActionCategory.Physics)]
	public class SetIsKinematic : ComponentAction<Rigidbody>
	{
		// Token: 0x06003B47 RID: 15175 RVA: 0x0012BAC8 File Offset: 0x00129CC8
		public override void Reset()
		{
			this.gameObject = null;
			this.isKinematic = false;
		}

		// Token: 0x06003B48 RID: 15176 RVA: 0x0012BAE0 File Offset: 0x00129CE0
		public override void OnEnter()
		{
			this.DoSetIsKinematic();
			base.Finish();
		}

		// Token: 0x06003B49 RID: 15177 RVA: 0x0012BAF0 File Offset: 0x00129CF0
		private void DoSetIsKinematic()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.rigidbody.isKinematic = this.isKinematic.Value;
			}
		}

		// Token: 0x04002D7E RID: 11646
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D7F RID: 11647
		[RequiredField]
		public FsmBool isKinematic;
	}
}
