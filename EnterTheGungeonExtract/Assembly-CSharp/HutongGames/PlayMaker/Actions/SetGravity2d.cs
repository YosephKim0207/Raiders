using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A6B RID: 2667
	[Tooltip("Sets the gravity vector, or individual axis.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class SetGravity2d : FsmStateAction
	{
		// Token: 0x060038BA RID: 14522 RVA: 0x001233E0 File Offset: 0x001215E0
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
			this.everyFrame = false;
		}

		// Token: 0x060038BB RID: 14523 RVA: 0x00123424 File Offset: 0x00121624
		public override void OnEnter()
		{
			this.DoSetGravity();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060038BC RID: 14524 RVA: 0x00123440 File Offset: 0x00121640
		public override void OnUpdate()
		{
			this.DoSetGravity();
		}

		// Token: 0x060038BD RID: 14525 RVA: 0x00123448 File Offset: 0x00121648
		private void DoSetGravity()
		{
			Vector2 value = this.vector.Value;
			if (!this.x.IsNone)
			{
				value.x = this.x.Value;
			}
			if (!this.y.IsNone)
			{
				value.y = this.y.Value;
			}
			Physics2D.gravity = value;
		}

		// Token: 0x04002B11 RID: 11025
		[Tooltip("Gravity as Vector2.")]
		public FsmVector2 vector;

		// Token: 0x04002B12 RID: 11026
		[Tooltip("Override the x value of the gravity")]
		public FsmFloat x;

		// Token: 0x04002B13 RID: 11027
		[Tooltip("Override the y value of the gravity")]
		public FsmFloat y;

		// Token: 0x04002B14 RID: 11028
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
	}
}
