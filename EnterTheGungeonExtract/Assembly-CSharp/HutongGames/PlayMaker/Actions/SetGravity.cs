using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AE8 RID: 2792
	[Tooltip("Sets the gravity vector, or individual axis.")]
	[ActionCategory(ActionCategory.Physics)]
	public class SetGravity : FsmStateAction
	{
		// Token: 0x06003B10 RID: 15120 RVA: 0x0012B470 File Offset: 0x00129670
		public override void Reset()
		{
			this.vector = null;
			this.x = new FsmFloat
			{
				UseVariable = true
			};
			this.y = new FsmFloat
			{
				UseVariable = true
			};
			this.z = new FsmFloat
			{
				UseVariable = true
			};
			this.everyFrame = false;
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x0012B4C8 File Offset: 0x001296C8
		public override void OnEnter()
		{
			this.DoSetGravity();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x0012B4E4 File Offset: 0x001296E4
		public override void OnUpdate()
		{
			this.DoSetGravity();
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x0012B4EC File Offset: 0x001296EC
		private void DoSetGravity()
		{
			Vector3 value = this.vector.Value;
			if (!this.x.IsNone)
			{
				value.x = this.x.Value;
			}
			if (!this.y.IsNone)
			{
				value.y = this.y.Value;
			}
			if (!this.z.IsNone)
			{
				value.z = this.z.Value;
			}
			Physics.gravity = value;
		}

		// Token: 0x04002D5B RID: 11611
		public FsmVector3 vector;

		// Token: 0x04002D5C RID: 11612
		public FsmFloat x;

		// Token: 0x04002D5D RID: 11613
		public FsmFloat y;

		// Token: 0x04002D5E RID: 11614
		public FsmFloat z;

		// Token: 0x04002D5F RID: 11615
		public bool everyFrame;
	}
}
