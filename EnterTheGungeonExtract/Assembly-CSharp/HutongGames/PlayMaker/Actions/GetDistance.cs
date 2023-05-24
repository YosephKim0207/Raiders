using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000978 RID: 2424
	[Tooltip("Measures the Distance betweens 2 Game Objects and stores the result in a Float Variable.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetDistance : FsmStateAction
	{
		// Token: 0x060034C0 RID: 13504 RVA: 0x001112F4 File Offset: 0x0010F4F4
		public override void Reset()
		{
			this.gameObject = null;
			this.target = null;
			this.storeResult = null;
			this.everyFrame = true;
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x00111314 File Offset: 0x0010F514
		public override void OnEnter()
		{
			this.DoGetDistance();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x00111330 File Offset: 0x0010F530
		public override void OnUpdate()
		{
			this.DoGetDistance();
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x00111338 File Offset: 0x0010F538
		private void DoGetDistance()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null || this.target.Value == null || this.storeResult == null)
			{
				return;
			}
			this.storeResult.Value = Vector3.Distance(ownerDefaultTarget.transform.position, this.target.Value.transform.position);
		}

		// Token: 0x040025FB RID: 9723
		[Tooltip("Measure distance from this GameObject.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040025FC RID: 9724
		[Tooltip("Target GameObject.")]
		[RequiredField]
		public FsmGameObject target;

		// Token: 0x040025FD RID: 9725
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("Store the distance in a float variable.")]
		public FsmFloat storeResult;

		// Token: 0x040025FE RID: 9726
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
