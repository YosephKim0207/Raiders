using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000ACD RID: 2765
	[Tooltip("Sets the RGBA channels of a Color Variable. To leave any channel unchanged, set variable to 'None'.")]
	[ActionCategory(ActionCategory.Color)]
	public class SetColorRGBA : FsmStateAction
	{
		// Token: 0x06003A90 RID: 14992 RVA: 0x001297D8 File Offset: 0x001279D8
		public override void Reset()
		{
			this.colorVariable = null;
			this.red = 0f;
			this.green = 0f;
			this.blue = 0f;
			this.alpha = 1f;
			this.everyFrame = false;
		}

		// Token: 0x06003A91 RID: 14993 RVA: 0x00129834 File Offset: 0x00127A34
		public override void OnEnter()
		{
			this.DoSetColorRGBA();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A92 RID: 14994 RVA: 0x00129850 File Offset: 0x00127A50
		public override void OnUpdate()
		{
			this.DoSetColorRGBA();
		}

		// Token: 0x06003A93 RID: 14995 RVA: 0x00129858 File Offset: 0x00127A58
		private void DoSetColorRGBA()
		{
			if (this.colorVariable == null)
			{
				return;
			}
			Color value = this.colorVariable.Value;
			if (!this.red.IsNone)
			{
				value.r = this.red.Value;
			}
			if (!this.green.IsNone)
			{
				value.g = this.green.Value;
			}
			if (!this.blue.IsNone)
			{
				value.b = this.blue.Value;
			}
			if (!this.alpha.IsNone)
			{
				value.a = this.alpha.Value;
			}
			this.colorVariable.Value = value;
		}

		// Token: 0x04002CB5 RID: 11445
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmColor colorVariable;

		// Token: 0x04002CB6 RID: 11446
		[HasFloatSlider(0f, 1f)]
		public FsmFloat red;

		// Token: 0x04002CB7 RID: 11447
		[HasFloatSlider(0f, 1f)]
		public FsmFloat green;

		// Token: 0x04002CB8 RID: 11448
		[HasFloatSlider(0f, 1f)]
		public FsmFloat blue;

		// Token: 0x04002CB9 RID: 11449
		[HasFloatSlider(0f, 1f)]
		public FsmFloat alpha;

		// Token: 0x04002CBA RID: 11450
		public bool everyFrame;
	}
}
