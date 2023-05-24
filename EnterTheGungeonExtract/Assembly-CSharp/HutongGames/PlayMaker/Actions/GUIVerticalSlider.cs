using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009E9 RID: 2537
	[Tooltip("GUI Vertical Slider connected to a Float Variable.")]
	[ActionCategory(ActionCategory.GUI)]
	public class GUIVerticalSlider : GUIAction
	{
		// Token: 0x0600367F RID: 13951 RVA: 0x00116BBC File Offset: 0x00114DBC
		public override void Reset()
		{
			base.Reset();
			this.floatVariable = null;
			this.topValue = 100f;
			this.bottomValue = 0f;
			this.sliderStyle = "verticalslider";
			this.thumbStyle = "verticalsliderthumb";
			this.width = 0.1f;
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x00116C28 File Offset: 0x00114E28
		public override void OnGUI()
		{
			base.OnGUI();
			if (this.floatVariable != null)
			{
				this.floatVariable.Value = GUI.VerticalSlider(this.rect, this.floatVariable.Value, this.topValue.Value, this.bottomValue.Value, (!(this.sliderStyle.Value != string.Empty)) ? "verticalslider" : this.sliderStyle.Value, (!(this.thumbStyle.Value != string.Empty)) ? "verticalsliderthumb" : this.thumbStyle.Value);
			}
		}

		// Token: 0x040027D9 RID: 10201
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;

		// Token: 0x040027DA RID: 10202
		[RequiredField]
		public FsmFloat topValue;

		// Token: 0x040027DB RID: 10203
		[RequiredField]
		public FsmFloat bottomValue;

		// Token: 0x040027DC RID: 10204
		public FsmString sliderStyle;

		// Token: 0x040027DD RID: 10205
		public FsmString thumbStyle;
	}
}
