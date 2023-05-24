using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009C7 RID: 2503
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("GUI Horizontal Slider connected to a Float Variable.")]
	public class GUIHorizontalSlider : GUIAction
	{
		// Token: 0x06003616 RID: 13846 RVA: 0x0011515C File Offset: 0x0011335C
		public override void Reset()
		{
			base.Reset();
			this.floatVariable = null;
			this.leftValue = 0f;
			this.rightValue = 100f;
			this.sliderStyle = "horizontalslider";
			this.thumbStyle = "horizontalsliderthumb";
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x001151B8 File Offset: 0x001133B8
		public override void OnGUI()
		{
			base.OnGUI();
			if (this.floatVariable != null)
			{
				this.floatVariable.Value = GUI.HorizontalSlider(this.rect, this.floatVariable.Value, this.leftValue.Value, this.rightValue.Value, (!(this.sliderStyle.Value != string.Empty)) ? "horizontalslider" : this.sliderStyle.Value, (!(this.thumbStyle.Value != string.Empty)) ? "horizontalsliderthumb" : this.thumbStyle.Value);
			}
		}

		// Token: 0x04002764 RID: 10084
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		// Token: 0x04002765 RID: 10085
		[RequiredField]
		public FsmFloat leftValue;

		// Token: 0x04002766 RID: 10086
		[RequiredField]
		public FsmFloat rightValue;

		// Token: 0x04002767 RID: 10087
		public FsmString sliderStyle;

		// Token: 0x04002768 RID: 10088
		public FsmString thumbStyle;
	}
}
