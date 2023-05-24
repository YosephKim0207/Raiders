using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AB0 RID: 2736
	[Tooltip("Gets the value of a curve at a given time and stores it in a Float Variable. NOTE: This can be used for more than just animation! It's a general way to transform an input number into an output number using a curve (e.g., linear input -> bell curve).")]
	[ActionCategory(ActionCategory.Math)]
	public class SampleCurve : FsmStateAction
	{
		// Token: 0x06003A15 RID: 14869 RVA: 0x00127C8C File Offset: 0x00125E8C
		public override void Reset()
		{
			this.curve = null;
			this.sampleAt = null;
			this.storeValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x00127CAC File Offset: 0x00125EAC
		public override void OnEnter()
		{
			this.DoSampleCurve();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x00127CC8 File Offset: 0x00125EC8
		public override void OnUpdate()
		{
			this.DoSampleCurve();
		}

		// Token: 0x06003A18 RID: 14872 RVA: 0x00127CD0 File Offset: 0x00125ED0
		private void DoSampleCurve()
		{
			if (this.curve == null || this.curve.curve == null || this.storeValue == null)
			{
				return;
			}
			this.storeValue.Value = this.curve.curve.Evaluate(this.sampleAt.Value);
		}

		// Token: 0x04002C3B RID: 11323
		[RequiredField]
		public FsmAnimationCurve curve;

		// Token: 0x04002C3C RID: 11324
		[RequiredField]
		public FsmFloat sampleAt;

		// Token: 0x04002C3D RID: 11325
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat storeValue;

		// Token: 0x04002C3E RID: 11326
		public bool everyFrame;
	}
}
