using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000933 RID: 2355
	[Tooltip("Unparents all children from the Game Object.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class DetachChildren : FsmStateAction
	{
		// Token: 0x060033A1 RID: 13217 RVA: 0x0010DBB4 File Offset: 0x0010BDB4
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x060033A2 RID: 13218 RVA: 0x0010DBC0 File Offset: 0x0010BDC0
		public override void OnEnter()
		{
			DetachChildren.DoDetachChildren(base.Fsm.GetOwnerDefaultTarget(this.gameObject));
			base.Finish();
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x0010DBE0 File Offset: 0x0010BDE0
		private static void DoDetachChildren(GameObject go)
		{
			if (go != null)
			{
				go.transform.DetachChildren();
			}
		}

		// Token: 0x040024C9 RID: 9417
		[Tooltip("GameObject to unparent children from.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;
	}
}
