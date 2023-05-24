using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009E7 RID: 2535
	[Tooltip("A Vertical Slider linked to a Float Variable.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutVerticalSlider : GUILayoutAction
	{
		// Token: 0x06003679 RID: 13945 RVA: 0x00116ACC File Offset: 0x00114CCC
		public override void Reset()
		{
			base.Reset();
			this.floatVariable = null;
			this.topValue = 100f;
			this.bottomValue = 0f;
			this.changedEvent = null;
		}

		// Token: 0x0600367A RID: 13946 RVA: 0x00116B04 File Offset: 0x00114D04
		public override void OnGUI()
		{
			bool changed = GUI.changed;
			GUI.changed = false;
			if (this.floatVariable != null)
			{
				this.floatVariable.Value = GUILayout.VerticalSlider(this.floatVariable.Value, this.topValue.Value, this.bottomValue.Value, base.LayoutOptions);
			}
			if (GUI.changed)
			{
				base.Fsm.Event(this.changedEvent);
				GUIUtility.ExitGUI();
			}
			else
			{
				GUI.changed = changed;
			}
		}

		// Token: 0x040027D4 RID: 10196
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		// Token: 0x040027D5 RID: 10197
		[RequiredField]
		public FsmFloat topValue;

		// Token: 0x040027D6 RID: 10198
		[RequiredField]
		public FsmFloat bottomValue;

		// Token: 0x040027D7 RID: 10199
		public FsmEvent changedEvent;
	}
}
