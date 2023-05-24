using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000884 RID: 2180
	[Tooltip("Animates the value of a Float Variable using an Animation Curve.")]
	[ActionCategory(ActionCategory.AnimateVariables)]
	public class AnimateFloatV2 : AnimateFsmAction
	{
		// Token: 0x06003091 RID: 12433 RVA: 0x000FF914 File Offset: 0x000FDB14
		public override void Reset()
		{
			base.Reset();
			this.floatVariable = new FsmFloat
			{
				UseVariable = true
			};
		}

		// Token: 0x06003092 RID: 12434 RVA: 0x000FF93C File Offset: 0x000FDB3C
		public override void OnEnter()
		{
			base.OnEnter();
			this.finishInNextStep = false;
			this.resultFloats = new float[1];
			this.fromFloats = new float[1];
			this.fromFloats[0] = ((!this.floatVariable.IsNone) ? this.floatVariable.Value : 0f);
			this.calculations = new AnimateFsmAction.Calculation[1];
			this.calculations[0] = this.calculation;
			this.curves = new AnimationCurve[1];
			this.curves[0] = this.animCurve.curve;
			base.Init();
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x000FF9DC File Offset: 0x000FDBDC
		public override void OnExit()
		{
		}

		// Token: 0x06003094 RID: 12436 RVA: 0x000FF9E0 File Offset: 0x000FDBE0
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (!this.floatVariable.IsNone && this.isRunning)
			{
				this.floatVariable.Value = this.resultFloats[0];
			}
			if (this.finishInNextStep && !this.looping)
			{
				base.Finish();
				if (this.finishEvent != null)
				{
					base.Fsm.Event(this.finishEvent);
				}
			}
			if (this.finishAction && !this.finishInNextStep)
			{
				if (!this.floatVariable.IsNone)
				{
					this.floatVariable.Value = this.resultFloats[0];
				}
				this.finishInNextStep = true;
			}
		}

		// Token: 0x0400213F RID: 8511
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;

		// Token: 0x04002140 RID: 8512
		[RequiredField]
		public FsmAnimationCurve animCurve;

		// Token: 0x04002141 RID: 8513
		[Tooltip("Calculation lets you set a type of curve deformation that will be applied to floatVariable")]
		public AnimateFsmAction.Calculation calculation;

		// Token: 0x04002142 RID: 8514
		private bool finishInNextStep;
	}
}
