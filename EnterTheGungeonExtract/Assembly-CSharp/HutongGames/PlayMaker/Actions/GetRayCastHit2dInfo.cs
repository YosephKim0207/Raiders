using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A5C RID: 2652
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Gets info on the last 2d Raycast or LineCast and store in variables.")]
	public class GetRayCastHit2dInfo : FsmStateAction
	{
		// Token: 0x06003870 RID: 14448 RVA: 0x00121D50 File Offset: 0x0011FF50
		public override void Reset()
		{
			this.gameObjectHit = null;
			this.point = null;
			this.normal = null;
			this.distance = null;
			this.everyFrame = false;
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x00121D78 File Offset: 0x0011FF78
		public override void OnEnter()
		{
			this.StoreRaycastInfo();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x00121D94 File Offset: 0x0011FF94
		public override void OnUpdate()
		{
			this.StoreRaycastInfo();
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x00121D9C File Offset: 0x0011FF9C
		private void StoreRaycastInfo()
		{
			RaycastHit2D lastRaycastHit2DInfo = Fsm.GetLastRaycastHit2DInfo(base.Fsm);
			if (lastRaycastHit2DInfo.collider != null)
			{
				this.gameObjectHit.Value = lastRaycastHit2DInfo.collider.gameObject;
				this.point.Value = lastRaycastHit2DInfo.point;
				this.normal.Value = lastRaycastHit2DInfo.normal;
				this.distance.Value = lastRaycastHit2DInfo.fraction;
			}
		}

		// Token: 0x04002A9E RID: 10910
		[Tooltip("Get the GameObject hit by the last Raycast and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObjectHit;

		// Token: 0x04002A9F RID: 10911
		[Title("Hit Point")]
		[Tooltip("Get the world position of the ray hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 point;

		// Token: 0x04002AA0 RID: 10912
		[Tooltip("Get the normal at the hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 normal;

		// Token: 0x04002AA1 RID: 10913
		[Tooltip("Get the distance along the ray to the hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat distance;

		// Token: 0x04002AA2 RID: 10914
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
