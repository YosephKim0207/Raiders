using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009EE RID: 2542
	[Tooltip("Clamp the value of an Integer Variable to a Min/Max range.")]
	[ActionCategory(ActionCategory.Math)]
	public class IntClamp : FsmStateAction
	{
		// Token: 0x06003693 RID: 13971 RVA: 0x00116FC4 File Offset: 0x001151C4
		public override void Reset()
		{
			this.intVariable = null;
			this.minValue = null;
			this.maxValue = null;
			this.everyFrame = false;
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x00116FE4 File Offset: 0x001151E4
		public override void OnEnter()
		{
			this.DoClamp();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x00117000 File Offset: 0x00115200
		public override void OnUpdate()
		{
			this.DoClamp();
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x00117008 File Offset: 0x00115208
		private void DoClamp()
		{
			this.intVariable.Value = Mathf.Clamp(this.intVariable.Value, this.minValue.Value, this.maxValue.Value);
		}

		// Token: 0x040027ED RID: 10221
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt intVariable;

		// Token: 0x040027EE RID: 10222
		[RequiredField]
		public FsmInt minValue;

		// Token: 0x040027EF RID: 10223
		[RequiredField]
		public FsmInt maxValue;

		// Token: 0x040027F0 RID: 10224
		public bool everyFrame;
	}
}
