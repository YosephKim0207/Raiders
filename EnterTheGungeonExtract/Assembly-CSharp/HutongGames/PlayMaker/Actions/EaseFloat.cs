using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000890 RID: 2192
	[ActionCategory(ActionCategory.AnimateVariables)]
	[Tooltip("Easing Animation - Float")]
	public class EaseFloat : EaseFsmAction
	{
		// Token: 0x060030C7 RID: 12487 RVA: 0x00102980 File Offset: 0x00100B80
		public override void Reset()
		{
			base.Reset();
			this.floatVariable = null;
			this.fromValue = null;
			this.toValue = null;
			this.finishInNextStep = false;
		}

		// Token: 0x060030C8 RID: 12488 RVA: 0x001029A4 File Offset: 0x00100BA4
		public override void OnEnter()
		{
			base.OnEnter();
			this.fromFloats = new float[1];
			this.fromFloats[0] = this.fromValue.Value;
			this.toFloats = new float[1];
			this.toFloats[0] = this.toValue.Value;
			this.resultFloats = new float[1];
			this.finishInNextStep = false;
			this.floatVariable.Value = this.fromValue.Value;
		}

		// Token: 0x060030C9 RID: 12489 RVA: 0x00102A20 File Offset: 0x00100C20
		public override void OnExit()
		{
			base.OnExit();
		}

		// Token: 0x060030CA RID: 12490 RVA: 0x00102A28 File Offset: 0x00100C28
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (!this.floatVariable.IsNone && this.isRunning)
			{
				this.floatVariable.Value = this.resultFloats[0];
			}
			if (this.finishInNextStep)
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
					this.floatVariable.Value = ((!this.reverse.IsNone) ? ((!this.reverse.Value) ? this.toValue.Value : this.fromValue.Value) : this.toValue.Value);
				}
				this.finishInNextStep = true;
			}
		}

		// Token: 0x040021C4 RID: 8644
		[RequiredField]
		public FsmFloat fromValue;

		// Token: 0x040021C5 RID: 8645
		[RequiredField]
		public FsmFloat toValue;

		// Token: 0x040021C6 RID: 8646
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;

		// Token: 0x040021C7 RID: 8647
		private bool finishInNextStep;
	}
}
