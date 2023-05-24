using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B50 RID: 2896
	[Tooltip("Clamps the Magnitude of Vector2 Variable.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2ClampMagnitude : FsmStateAction
	{
		// Token: 0x06003CC8 RID: 15560 RVA: 0x00130F28 File Offset: 0x0012F128
		public override void Reset()
		{
			this.vector2Variable = null;
			this.maxLength = null;
			this.everyFrame = false;
		}

		// Token: 0x06003CC9 RID: 15561 RVA: 0x00130F40 File Offset: 0x0012F140
		public override void OnEnter()
		{
			this.DoVector2ClampMagnitude();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003CCA RID: 15562 RVA: 0x00130F5C File Offset: 0x0012F15C
		public override void OnUpdate()
		{
			this.DoVector2ClampMagnitude();
		}

		// Token: 0x06003CCB RID: 15563 RVA: 0x00130F64 File Offset: 0x0012F164
		private void DoVector2ClampMagnitude()
		{
			this.vector2Variable.Value = Vector2.ClampMagnitude(this.vector2Variable.Value, this.maxLength.Value);
		}

		// Token: 0x04002F0D RID: 12045
		[RequiredField]
		[Tooltip("The Vector2")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 vector2Variable;

		// Token: 0x04002F0E RID: 12046
		[Tooltip("The maximum Magnitude")]
		[RequiredField]
		public FsmFloat maxLength;

		// Token: 0x04002F0F RID: 12047
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
	}
}
