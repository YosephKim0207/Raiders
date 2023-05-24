using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A66 RID: 2662
	[Tooltip("Perform a Mouse Pick on a 2d scene and stores the results. Use Ray Distance to set how close the camera must be to pick the 2d object.")]
	[ActionCategory(ActionCategory.Input)]
	public class MousePick2d : FsmStateAction
	{
		// Token: 0x060038A1 RID: 14497 RVA: 0x0012298C File Offset: 0x00120B8C
		public override void Reset()
		{
			this.storeDidPickObject = null;
			this.storeGameObject = null;
			this.storePoint = null;
			this.layerMask = new FsmInt[0];
			this.invertMask = false;
			this.everyFrame = false;
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x001229C4 File Offset: 0x00120BC4
		public override void OnEnter()
		{
			this.DoMousePick2d();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x001229E0 File Offset: 0x00120BE0
		public override void OnUpdate()
		{
			this.DoMousePick2d();
		}

		// Token: 0x060038A4 RID: 14500 RVA: 0x001229E8 File Offset: 0x00120BE8
		private void DoMousePick2d()
		{
			RaycastHit2D rayIntersection = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), float.PositiveInfinity, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value));
			bool flag = rayIntersection.collider != null;
			this.storeDidPickObject.Value = flag;
			if (flag)
			{
				this.storeGameObject.Value = rayIntersection.collider.gameObject;
				this.storePoint.Value = rayIntersection.point;
			}
			else
			{
				this.storeGameObject.Value = null;
				this.storePoint.Value = Vector3.zero;
			}
		}

		// Token: 0x04002AE1 RID: 10977
		[Tooltip("Store if a GameObject was picked in a Bool variable. True if a GameObject was picked, otherwise false.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeDidPickObject;

		// Token: 0x04002AE2 RID: 10978
		[Tooltip("Store the picked GameObject in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeGameObject;

		// Token: 0x04002AE3 RID: 10979
		[Tooltip("Store the picked point in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 storePoint;

		// Token: 0x04002AE4 RID: 10980
		[Tooltip("Pick only from these layers.")]
		[UIHint(UIHint.Layer)]
		public FsmInt[] layerMask;

		// Token: 0x04002AE5 RID: 10981
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		// Token: 0x04002AE6 RID: 10982
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
