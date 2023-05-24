using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009A4 RID: 2468
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Gets a Random Child of a Game Object.")]
	public class GetRandomChild : FsmStateAction
	{
		// Token: 0x06003582 RID: 13698 RVA: 0x001135AC File Offset: 0x001117AC
		public override void Reset()
		{
			this.gameObject = null;
			this.storeResult = null;
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x001135BC File Offset: 0x001117BC
		public override void OnEnter()
		{
			this.DoGetRandomChild();
			base.Finish();
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x001135CC File Offset: 0x001117CC
		private void DoGetRandomChild()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			int childCount = ownerDefaultTarget.transform.childCount;
			if (childCount == 0)
			{
				return;
			}
			this.storeResult.Value = ownerDefaultTarget.transform.GetChild(UnityEngine.Random.Range(0, childCount)).gameObject;
		}

		// Token: 0x040026D2 RID: 9938
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040026D3 RID: 9939
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmGameObject storeResult;
	}
}
