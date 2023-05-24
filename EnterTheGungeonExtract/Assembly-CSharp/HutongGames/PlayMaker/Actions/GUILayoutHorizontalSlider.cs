using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009DC RID: 2524
	[Tooltip("A Horizontal Slider linked to a Float Variable.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutHorizontalSlider : GUILayoutAction
	{
		// Token: 0x06003654 RID: 13908 RVA: 0x00115FD0 File Offset: 0x001141D0
		public override void Reset()
		{
			base.Reset();
			this.floatVariable = null;
			this.leftValue = 0f;
			this.rightValue = 100f;
			this.changedEvent = null;
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x00116008 File Offset: 0x00114208
		public override void OnGUI()
		{
			bool changed = GUI.changed;
			GUI.changed = false;
			if (this.floatVariable != null)
			{
				this.floatVariable.Value = GUILayout.HorizontalSlider(this.floatVariable.Value, this.leftValue.Value, this.rightValue.Value, base.LayoutOptions);
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

		// Token: 0x040027A5 RID: 10149
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		// Token: 0x040027A6 RID: 10150
		[RequiredField]
		public FsmFloat leftValue;

		// Token: 0x040027A7 RID: 10151
		[RequiredField]
		public FsmFloat rightValue;

		// Token: 0x040027A8 RID: 10152
		public FsmEvent changedEvent;
	}
}
