using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B08 RID: 2824
	[Tooltip("Sets a Game Object's Name.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class SetName : FsmStateAction
	{
		// Token: 0x06003B92 RID: 15250 RVA: 0x0012C814 File Offset: 0x0012AA14
		public override void Reset()
		{
			this.gameObject = null;
			this.name = null;
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x0012C824 File Offset: 0x0012AA24
		public override void OnEnter()
		{
			this.DoSetLayer();
			base.Finish();
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x0012C834 File Offset: 0x0012AA34
		private void DoSetLayer()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			ownerDefaultTarget.name = this.name.Value;
		}

		// Token: 0x04002DB5 RID: 11701
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002DB6 RID: 11702
		[RequiredField]
		public FsmString name;
	}
}
