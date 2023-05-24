using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AB4 RID: 2740
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Transforms position from screen space into world space. NOTE: Uses the MainCamera!")]
	public class ScreenToWorldPoint : FsmStateAction
	{
		// Token: 0x06003A28 RID: 14888 RVA: 0x001281B0 File Offset: 0x001263B0
		public override void Reset()
		{
			this.screenVector = null;
			this.screenX = new FsmFloat
			{
				UseVariable = true
			};
			this.screenY = new FsmFloat
			{
				UseVariable = true
			};
			this.screenZ = 1f;
			this.normalized = false;
			this.storeWorldVector = null;
			this.storeWorldX = null;
			this.storeWorldY = null;
			this.storeWorldZ = null;
			this.everyFrame = false;
		}

		// Token: 0x06003A29 RID: 14889 RVA: 0x0012822C File Offset: 0x0012642C
		public override void OnEnter()
		{
			this.DoScreenToWorldPoint();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A2A RID: 14890 RVA: 0x00128248 File Offset: 0x00126448
		public override void OnUpdate()
		{
			this.DoScreenToWorldPoint();
		}

		// Token: 0x06003A2B RID: 14891 RVA: 0x00128250 File Offset: 0x00126450
		private void DoScreenToWorldPoint()
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
			if (!this.screenZ.IsNone)
			{
				vector.z = this.screenZ.Value;
			}
			if (this.normalized.Value)
			{
				vector.x *= (float)Screen.width;
				vector.y *= (float)Screen.height;
			}
			vector = Camera.main.ScreenToWorldPoint(vector);
			this.storeWorldVector.Value = vector;
			this.storeWorldX.Value = vector.x;
			this.storeWorldY.Value = vector.y;
			this.storeWorldZ.Value = vector.z;
		}

		// Token: 0x04002C56 RID: 11350
		[UIHint(UIHint.Variable)]
		[Tooltip("Screen position as a vector.")]
		public FsmVector3 screenVector;

		// Token: 0x04002C57 RID: 11351
		[Tooltip("Screen X position in pixels or normalized. See Normalized.")]
		public FsmFloat screenX;

		// Token: 0x04002C58 RID: 11352
		[Tooltip("Screen X position in pixels or normalized. See Normalized.")]
		public FsmFloat screenY;

		// Token: 0x04002C59 RID: 11353
		[Tooltip("Distance into the screen in world units.")]
		public FsmFloat screenZ;

		// Token: 0x04002C5A RID: 11354
		[Tooltip("If true, X/Y coordinates are considered normalized (0-1), otherwise they are expected to be in pixels")]
		public FsmBool normalized;

		// Token: 0x04002C5B RID: 11355
		[Tooltip("Store the world position in a vector3 variable.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeWorldVector;

		// Token: 0x04002C5C RID: 11356
		[Tooltip("Store the world X position in a float variable.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeWorldX;

		// Token: 0x04002C5D RID: 11357
		[Tooltip("Store the world Y position in a float variable.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeWorldY;

		// Token: 0x04002C5E RID: 11358
		[Tooltip("Store the world Z position in a float variable.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeWorldZ;

		// Token: 0x04002C5F RID: 11359
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
	}
}
