using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009A7 RID: 2471
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Gets info on the last Raycast and store in variables.")]
	public class GetRaycastHitInfo : FsmStateAction
	{
		// Token: 0x06003590 RID: 13712 RVA: 0x00113858 File Offset: 0x00111A58
		public override void Reset()
		{
			this.gameObjectHit = null;
			this.point = null;
			this.normal = null;
			this.distance = null;
			this.everyFrame = false;
		}

		// Token: 0x06003591 RID: 13713 RVA: 0x00113880 File Offset: 0x00111A80
		private void StoreRaycastInfo()
		{
			if (base.Fsm.RaycastHitInfo.collider != null)
			{
				this.gameObjectHit.Value = base.Fsm.RaycastHitInfo.collider.gameObject;
				this.point.Value = base.Fsm.RaycastHitInfo.point;
				this.normal.Value = base.Fsm.RaycastHitInfo.normal;
				this.distance.Value = base.Fsm.RaycastHitInfo.distance;
			}
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x0011392C File Offset: 0x00111B2C
		public override void OnEnter()
		{
			this.StoreRaycastInfo();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x00113948 File Offset: 0x00111B48
		public override void OnUpdate()
		{
			this.StoreRaycastInfo();
		}

		// Token: 0x040026DC RID: 9948
		[Tooltip("Get the GameObject hit by the last Raycast and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObjectHit;

		// Token: 0x040026DD RID: 9949
		[UIHint(UIHint.Variable)]
		[Title("Hit Point")]
		[Tooltip("Get the world position of the ray hit point and store it in a variable.")]
		public FsmVector3 point;

		// Token: 0x040026DE RID: 9950
		[Tooltip("Get the normal at the hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 normal;

		// Token: 0x040026DF RID: 9951
		[Tooltip("Get the distance along the ray to the hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat distance;

		// Token: 0x040026E0 RID: 9952
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
