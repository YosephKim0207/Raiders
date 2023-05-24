using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009A9 RID: 2473
	[Tooltip("Gets the top most parent of the Game Object.\nIf the game object has no parent, returns itself.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetRoot : FsmStateAction
	{
		// Token: 0x0600359A RID: 13722 RVA: 0x00113A48 File Offset: 0x00111C48
		public override void Reset()
		{
			this.gameObject = null;
			this.storeRoot = null;
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x00113A58 File Offset: 0x00111C58
		public override void OnEnter()
		{
			this.DoGetRoot();
			base.Finish();
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x00113A68 File Offset: 0x00111C68
		private void DoGetRoot()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this.storeRoot.Value = ownerDefaultTarget.transform.root.gameObject;
		}

		// Token: 0x040026E7 RID: 9959
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040026E8 RID: 9960
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmGameObject storeRoot;
	}
}
