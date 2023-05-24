using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AAD RID: 2733
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("Rotates the GUI around a pivot point. By default only effects GUI rendered by this FSM, check Apply Globally to effect all GUI controls.")]
	public class RotateGUI : FsmStateAction
	{
		// Token: 0x060039F4 RID: 14836 RVA: 0x00127764 File Offset: 0x00125964
		public override void Reset()
		{
			this.angle = 0f;
			this.pivotX = 0.5f;
			this.pivotY = 0.5f;
			this.normalized = true;
			this.applyGlobally = false;
		}

		// Token: 0x060039F5 RID: 14837 RVA: 0x001277A4 File Offset: 0x001259A4
		public override void OnGUI()
		{
			if (this.applied)
			{
				return;
			}
			Vector2 vector = new Vector2(this.pivotX.Value, this.pivotY.Value);
			if (this.normalized)
			{
				vector.x *= (float)Screen.width;
				vector.y *= (float)Screen.height;
			}
			GUIUtility.RotateAroundPivot(this.angle.Value, vector);
			if (this.applyGlobally)
			{
				PlayMakerGUI.GUIMatrix = GUI.matrix;
				this.applied = true;
			}
		}

		// Token: 0x060039F6 RID: 14838 RVA: 0x0012783C File Offset: 0x00125A3C
		public override void OnUpdate()
		{
			this.applied = false;
		}

		// Token: 0x04002C31 RID: 11313
		[RequiredField]
		public FsmFloat angle;

		// Token: 0x04002C32 RID: 11314
		[RequiredField]
		public FsmFloat pivotX;

		// Token: 0x04002C33 RID: 11315
		[RequiredField]
		public FsmFloat pivotY;

		// Token: 0x04002C34 RID: 11316
		public bool normalized;

		// Token: 0x04002C35 RID: 11317
		public bool applyGlobally;

		// Token: 0x04002C36 RID: 11318
		private bool applied;
	}
}
