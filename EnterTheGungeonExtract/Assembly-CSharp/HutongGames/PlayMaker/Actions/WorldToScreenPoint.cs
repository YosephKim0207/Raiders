using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B70 RID: 2928
	[Tooltip("Transforms position from world space into screen space. NOTE: Uses the MainCamera!")]
	[ActionCategory(ActionCategory.Camera)]
	public class WorldToScreenPoint : FsmStateAction
	{
		// Token: 0x06003D48 RID: 15688 RVA: 0x00132A8C File Offset: 0x00130C8C
		public override void Reset()
		{
			this.worldPosition = null;
			this.worldX = new FsmFloat
			{
				UseVariable = true
			};
			this.worldY = new FsmFloat
			{
				UseVariable = true
			};
			this.worldZ = new FsmFloat
			{
				UseVariable = true
			};
			this.storeScreenPoint = null;
			this.storeScreenX = null;
			this.storeScreenY = null;
			this.everyFrame = false;
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x00132AF8 File Offset: 0x00130CF8
		public override void OnEnter()
		{
			this.DoWorldToScreenPoint();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x00132B14 File Offset: 0x00130D14
		public override void OnUpdate()
		{
			this.DoWorldToScreenPoint();
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x00132B1C File Offset: 0x00130D1C
		private void DoWorldToScreenPoint()
		{
			if (Camera.main == null)
			{
				base.LogError("No MainCamera defined!");
				base.Finish();
				return;
			}
			Vector3 vector = Vector3.zero;
			if (!this.worldPosition.IsNone)
			{
				vector = this.worldPosition.Value;
			}
			if (!this.worldX.IsNone)
			{
				vector.x = this.worldX.Value;
			}
			if (!this.worldY.IsNone)
			{
				vector.y = this.worldY.Value;
			}
			if (!this.worldZ.IsNone)
			{
				vector.z = this.worldZ.Value;
			}
			vector = Camera.main.WorldToScreenPoint(vector);
			if (this.normalize.Value)
			{
				vector.x /= (float)Screen.width;
				vector.y /= (float)Screen.height;
			}
			this.storeScreenPoint.Value = vector;
			this.storeScreenX.Value = vector.x;
			this.storeScreenY.Value = vector.y;
		}

		// Token: 0x04002F96 RID: 12182
		[UIHint(UIHint.Variable)]
		[Tooltip("World position to transform into screen coordinates.")]
		public FsmVector3 worldPosition;

		// Token: 0x04002F97 RID: 12183
		[Tooltip("World X position.")]
		public FsmFloat worldX;

		// Token: 0x04002F98 RID: 12184
		[Tooltip("World Y position.")]
		public FsmFloat worldY;

		// Token: 0x04002F99 RID: 12185
		[Tooltip("World Z position.")]
		public FsmFloat worldZ;

		// Token: 0x04002F9A RID: 12186
		[Tooltip("Store the screen position in a Vector3 Variable. Z will equal zero.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeScreenPoint;

		// Token: 0x04002F9B RID: 12187
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the screen X position in a Float Variable.")]
		public FsmFloat storeScreenX;

		// Token: 0x04002F9C RID: 12188
		[Tooltip("Store the screen Y position in a Float Variable.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeScreenY;

		// Token: 0x04002F9D RID: 12189
		[Tooltip("Normalize screen coordinates (0-1). Otherwise coordinates are in pixels.")]
		public FsmBool normalize;

		// Token: 0x04002F9E RID: 12190
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
	}
}
