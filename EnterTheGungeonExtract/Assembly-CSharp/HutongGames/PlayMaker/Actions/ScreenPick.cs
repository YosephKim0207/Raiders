using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AB3 RID: 2739
	[Tooltip("Perform a raycast into the scene using screen coordinates and stores the results. Use Ray Distance to set how close the camera must be to pick the object. NOTE: Uses the MainCamera!")]
	[ActionCategory(ActionCategory.Input)]
	public class ScreenPick : FsmStateAction
	{
		// Token: 0x06003A23 RID: 14883 RVA: 0x00127F28 File Offset: 0x00126128
		public override void Reset()
		{
			this.screenVector = new FsmVector3
			{
				UseVariable = true
			};
			this.screenX = new FsmFloat
			{
				UseVariable = true
			};
			this.screenY = new FsmFloat
			{
				UseVariable = true
			};
			this.normalized = false;
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

		// Token: 0x06003A24 RID: 14884 RVA: 0x00127FD0 File Offset: 0x001261D0
		public override void OnEnter()
		{
			this.DoScreenPick();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x00127FEC File Offset: 0x001261EC
		public override void OnUpdate()
		{
			this.DoScreenPick();
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x00127FF4 File Offset: 0x001261F4
		private void DoScreenPick()
		{
			if (Camera.main == null)
			{
				base.LogError("No MainCamera defined!");
				base.Finish();
				return;
			}
			Vector3 vector = Vector3.zero;
			if (!this.screenVector.IsNone)
			{
				vector = this.screenVector.Value;
			}
			if (!this.screenX.IsNone)
			{
				vector.x = this.screenX.Value;
			}
			if (!this.screenY.IsNone)
			{
				vector.y = this.screenY.Value;
			}
			if (this.normalized.Value)
			{
				vector.x *= (float)Screen.width;
				vector.y *= (float)Screen.height;
			}
			Ray ray = Camera.main.ScreenPointToRay(vector);
			RaycastHit raycastHit;
			Physics.Raycast(ray, out raycastHit, this.rayDistance.Value, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value));
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
				this.storeDistance = float.PositiveInfinity;
				this.storePoint.Value = Vector3.zero;
				this.storeNormal.Value = Vector3.zero;
			}
		}

		// Token: 0x04002C49 RID: 11337
		[Tooltip("A Vector3 screen position. Commonly stored by other actions.")]
		public FsmVector3 screenVector;

		// Token: 0x04002C4A RID: 11338
		[Tooltip("X position on screen.")]
		public FsmFloat screenX;

		// Token: 0x04002C4B RID: 11339
		[Tooltip("Y position on screen.")]
		public FsmFloat screenY;

		// Token: 0x04002C4C RID: 11340
		[Tooltip("Are the supplied screen coordinates normalized (0-1), or in pixels.")]
		public FsmBool normalized;

		// Token: 0x04002C4D RID: 11341
		[RequiredField]
		public FsmFloat rayDistance = 100f;

		// Token: 0x04002C4E RID: 11342
		[UIHint(UIHint.Variable)]
		public FsmBool storeDidPickObject;

		// Token: 0x04002C4F RID: 11343
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeGameObject;

		// Token: 0x04002C50 RID: 11344
		[UIHint(UIHint.Variable)]
		public FsmVector3 storePoint;

		// Token: 0x04002C51 RID: 11345
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeNormal;

		// Token: 0x04002C52 RID: 11346
		[UIHint(UIHint.Variable)]
		public FsmFloat storeDistance;

		// Token: 0x04002C53 RID: 11347
		[UIHint(UIHint.Layer)]
		[Tooltip("Pick only from these layers.")]
		public FsmInt[] layerMask;

		// Token: 0x04002C54 RID: 11348
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		// Token: 0x04002C55 RID: 11349
		public bool everyFrame;
	}
}
