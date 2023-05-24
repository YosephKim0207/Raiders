using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B16 RID: 2838
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Sets a Game Object's Tag.")]
	public class SetTag : FsmStateAction
	{
		// Token: 0x06003BD2 RID: 15314 RVA: 0x0012D4CC File Offset: 0x0012B6CC
		public override void Reset()
		{
			this.gameObject = null;
			this.tag = "Untagged";
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x0012D4E8 File Offset: 0x0012B6E8
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget != null)
			{
				ownerDefaultTarget.tag = this.tag.Value;
			}
			base.Finish();
		}

		// Token: 0x04002DF0 RID: 11760
		public FsmOwnerDefault gameObject;

		// Token: 0x04002DF1 RID: 11761
		[UIHint(UIHint.Tag)]
		public FsmString tag;
	}
}
