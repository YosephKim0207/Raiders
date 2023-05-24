using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AF8 RID: 2808
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Sets a Game Object's Layer.")]
	public class SetLayer : FsmStateAction
	{
		// Token: 0x06003B4E RID: 15182 RVA: 0x0012BBCC File Offset: 0x00129DCC
		public override void Reset()
		{
			this.gameObject = null;
			this.layer = 0;
		}

		// Token: 0x06003B4F RID: 15183 RVA: 0x0012BBDC File Offset: 0x00129DDC
		public override void OnEnter()
		{
			this.DoSetLayer();
			base.Finish();
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x0012BBEC File Offset: 0x00129DEC
		private void DoSetLayer()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			ownerDefaultTarget.layer = this.layer;
		}

		// Token: 0x04002D82 RID: 11650
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002D83 RID: 11651
		[UIHint(UIHint.Layer)]
		public int layer;
	}
}
