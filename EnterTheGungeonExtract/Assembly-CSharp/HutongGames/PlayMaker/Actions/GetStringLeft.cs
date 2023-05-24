using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009AF RID: 2479
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Gets the Left n characters from a String Variable.")]
	public class GetStringLeft : FsmStateAction
	{
		// Token: 0x060035B3 RID: 13747 RVA: 0x00113E2C File Offset: 0x0011202C
		public override void Reset()
		{
			this.stringVariable = null;
			this.charCount = 0;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x00113E50 File Offset: 0x00112050
		public override void OnEnter()
		{
			this.DoGetStringLeft();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x00113E6C File Offset: 0x0011206C
		public override void OnUpdate()
		{
			this.DoGetStringLeft();
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x00113E74 File Offset: 0x00112074
		private void DoGetStringLeft()
		{
			if (this.stringVariable.IsNone)
			{
				return;
			}
			if (this.storeResult.IsNone)
			{
				return;
			}
			this.storeResult.Value = this.stringVariable.Value.Substring(0, Mathf.Clamp(this.charCount.Value, 0, this.stringVariable.Value.Length));
		}

		// Token: 0x040026FD RID: 9981
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString stringVariable;

		// Token: 0x040026FE RID: 9982
		[Tooltip("Number of characters to get.")]
		public FsmInt charCount;

		// Token: 0x040026FF RID: 9983
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString storeResult;

		// Token: 0x04002700 RID: 9984
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
