using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000971 RID: 2417
	[ActionCategory(ActionCategory.Color)]
	[Tooltip("Get the RGBA channels of a Color Variable and store them in Float Variables.")]
	public class GetColorRGBA : FsmStateAction
	{
		// Token: 0x060034A3 RID: 13475 RVA: 0x00110C8C File Offset: 0x0010EE8C
		public override void Reset()
		{
			this.color = null;
			this.storeRed = null;
			this.storeGreen = null;
			this.storeBlue = null;
			this.storeAlpha = null;
			this.everyFrame = false;
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x00110CB8 File Offset: 0x0010EEB8
		public override void OnEnter()
		{
			this.DoGetColorRGBA();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x00110CD4 File Offset: 0x0010EED4
		public override void OnUpdate()
		{
			this.DoGetColorRGBA();
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x00110CDC File Offset: 0x0010EEDC
		private void DoGetColorRGBA()
		{
			if (this.color.IsNone)
			{
				return;
			}
			this.storeRed.Value = this.color.Value.r;
			this.storeGreen.Value = this.color.Value.g;
			this.storeBlue.Value = this.color.Value.b;
			this.storeAlpha.Value = this.color.Value.a;
		}

		// Token: 0x040025D4 RID: 9684
		[Tooltip("The Color variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmColor color;

		// Token: 0x040025D5 RID: 9685
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the red channel in a float variable.")]
		public FsmFloat storeRed;

		// Token: 0x040025D6 RID: 9686
		[Tooltip("Store the green channel in a float variable.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeGreen;

		// Token: 0x040025D7 RID: 9687
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the blue channel in a float variable.")]
		public FsmFloat storeBlue;

		// Token: 0x040025D8 RID: 9688
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the alpha channel in a float variable.")]
		public FsmFloat storeAlpha;

		// Token: 0x040025D9 RID: 9689
		[Tooltip("Repeat every frame. Useful if the color variable is changing.")]
		public bool everyFrame;
	}
}
