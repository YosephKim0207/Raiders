using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A1D RID: 2589
	[Tooltip("Perform a Mouse Pick on the scene from the Main Camera and stores the results. Use Ray Distance to set how close the camera must be to pick the object.")]
	[ActionCategory(ActionCategory.Input)]
	public class MousePick : FsmStateAction
	{
		// Token: 0x06003770 RID: 14192 RVA: 0x0011DBBC File Offset: 0x0011BDBC
		public override void Reset()
		{
			this.rayDistance = 100f;
			this.storeDidPickObject = null;
			this.storeGameObject = null;
			this.storePoint = null;
			this.storeNormal = null;
			this.storeDistance = null;
			this.layerMask = new FsmInt[0];
			this.invertMask = false;
			this.everyFrame = false;
		}

		// Token: 0x06003771 RID: 14193 RVA: 0x0011DC1C File Offset: 0x0011BE1C
		public override void OnEnter()
		{
			this.DoMousePick();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x0011DC38 File Offset: 0x0011BE38
		public override void OnUpdate()
		{
			this.DoMousePick();
		}

		// Token: 0x06003773 RID: 14195 RVA: 0x0011DC40 File Offset: 0x0011BE40
		private void DoMousePick()
		{
			RaycastHit raycastHit = ActionHelpers.MousePick(this.rayDistance.Value, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value));
			bool flag = raycastHit.collider != null;
			this.storeDidPickObject.Value = flag;
			if (flag)
			{
				this.storeGameObject.Value = raycastHit.collider.gameObject;
				this.storeDistance.Value = raycastHit.distance;
				this.storePoint.Value = raycastHit.point;
				this.storeNormal.Value = raycastHit.normal;
			}
			else
			{
				this.storeGameObject.Value = null;
				this.storeDistance.Value = float.PositiveInfinity;
				this.storePoint.Value = Vector3.zero;
				this.storeNormal.Value = Vector3.zero;
			}
		}

		// Token: 0x0400295A RID: 10586
		[Tooltip("Set the length of the ray to cast from the Main Camera.")]
		[RequiredField]
		public FsmFloat rayDistance = 100f;

		// Token: 0x0400295B RID: 10587
		[Tooltip("Set Bool variable true if an object was picked, false if not.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeDidPickObject;

		// Token: 0x0400295C RID: 10588
		[Tooltip("Store the picked GameObject.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeGameObject;

		// Token: 0x0400295D RID: 10589
		[Tooltip("Store the point of contact.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storePoint;

		// Token: 0x0400295E RID: 10590
		[Tooltip("Store the normal at the point of contact.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeNormal;

		// Token: 0x0400295F RID: 10591
		[Tooltip("Store the distance to the point of contact.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeDistance;

		// Token: 0x04002960 RID: 10592
		[Tooltip("Pick only from these layers.")]
		[UIHint(UIHint.Layer)]
		public FsmInt[] layerMask;

		// Token: 0x04002961 RID: 10593
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		// Token: 0x04002962 RID: 10594
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
