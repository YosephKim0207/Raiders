using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A69 RID: 2665
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Perform a raycast into the 2d scene using screen coordinates and stores the results. Use Ray Distance to set how close the camera must be to pick the 2d object. NOTE: Uses the MainCamera!")]
	public class ScreenPick2d : FsmStateAction
	{
		// Token: 0x060038B1 RID: 14513 RVA: 0x001230F0 File Offset: 0x001212F0
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
			this.storeDidPickObject = null;
			this.storeGameObject = null;
			this.storePoint = null;
			this.layerMask = new FsmInt[0];
			this.invertMask = false;
			this.everyFrame = false;
		}

		// Token: 0x060038B2 RID: 14514 RVA: 0x0012317C File Offset: 0x0012137C
		public override void OnEnter()
		{
			this.DoScreenPick();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060038B3 RID: 14515 RVA: 0x00123198 File Offset: 0x00121398
		public override void OnUpdate()
		{
			this.DoScreenPick();
		}

		// Token: 0x060038B4 RID: 14516 RVA: 0x001231A0 File Offset: 0x001213A0
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
			RaycastHit2D rayIntersection = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(vector), float.PositiveInfinity, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value));
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

		// Token: 0x04002B04 RID: 11012
		[Tooltip("A Vector3 screen position. Commonly stored by other actions.")]
		public FsmVector3 screenVector;

		// Token: 0x04002B05 RID: 11013
		[Tooltip("X position on screen.")]
		public FsmFloat screenX;

		// Token: 0x04002B06 RID: 11014
		[Tooltip("Y position on screen.")]
		public FsmFloat screenY;

		// Token: 0x04002B07 RID: 11015
		[Tooltip("Are the supplied screen coordinates normalized (0-1), or in pixels.")]
		public FsmBool normalized;

		// Token: 0x04002B08 RID: 11016
		[Tooltip("Store whether the Screen pick did pick a GameObject")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeDidPickObject;

		// Token: 0x04002B09 RID: 11017
		[Tooltip("Store the picked GameObject")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeGameObject;

		// Token: 0x04002B0A RID: 11018
		[Tooltip("Store the picked position in world Space")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storePoint;

		// Token: 0x04002B0B RID: 11019
		[Tooltip("Pick only from these layers.")]
		[UIHint(UIHint.Layer)]
		public FsmInt[] layerMask;

		// Token: 0x04002B0C RID: 11020
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		// Token: 0x04002B0D RID: 11021
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
