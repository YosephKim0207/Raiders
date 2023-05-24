using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AB1 RID: 2737
	[Tooltip("Scales the GUI around a pivot point. By default only effects GUI rendered by this FSM, check Apply Globally to effect all GUI controls.")]
	[ActionCategory(ActionCategory.GUI)]
	public class ScaleGUI : FsmStateAction
	{
		// Token: 0x06003A1A RID: 14874 RVA: 0x00127D34 File Offset: 0x00125F34
		public override void Reset()
		{
			this.scaleX = 1f;
			this.scaleY = 1f;
			this.pivotX = 0.5f;
			this.pivotY = 0.5f;
			this.normalized = true;
			this.applyGlobally = false;
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x00127D90 File Offset: 0x00125F90
		public override void OnGUI()
		{
			if (this.applied)
			{
				return;
			}
			Vector2 vector = new Vector2(this.scaleX.Value, this.scaleY.Value);
			if (object.Equals(vector.x, 0))
			{
				vector.x = 0.0001f;
			}
			if (object.Equals(vector.y, 0))
			{
				vector.x = 0.0001f;
			}
			Vector2 vector2 = new Vector2(this.pivotX.Value, this.pivotY.Value);
			if (this.normalized)
			{
				vector2.x *= (float)Screen.width;
				vector2.y *= (float)Screen.height;
			}
			GUIUtility.ScaleAroundPivot(vector, vector2);
			if (this.applyGlobally)
			{
				PlayMakerGUI.GUIMatrix = GUI.matrix;
				this.applied = true;
			}
		}

		// Token: 0x06003A1C RID: 14876 RVA: 0x00127E8C File Offset: 0x0012608C
		public override void OnUpdate()
		{
			this.applied = false;
		}

		// Token: 0x04002C3F RID: 11327
		[RequiredField]
		public FsmFloat scaleX;

		// Token: 0x04002C40 RID: 11328
		[RequiredField]
		public FsmFloat scaleY;

		// Token: 0x04002C41 RID: 11329
		[RequiredField]
		public FsmFloat pivotX;

		// Token: 0x04002C42 RID: 11330
		[RequiredField]
		public FsmFloat pivotY;

		// Token: 0x04002C43 RID: 11331
		[Tooltip("Pivot point uses normalized coordinates. E.g. 0.5 is the center of the screen.")]
		public bool normalized;

		// Token: 0x04002C44 RID: 11332
		public bool applyGlobally;

		// Token: 0x04002C45 RID: 11333
		private bool applied;
	}
}
