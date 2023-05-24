using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009B1 RID: 2481
	[Tooltip("Gets the Right n characters from a String.")]
	[ActionCategory(ActionCategory.String)]
	public class GetStringRight : FsmStateAction
	{
		// Token: 0x060035BD RID: 13757 RVA: 0x00113F64 File Offset: 0x00112164
		public override void Reset()
		{
			this.stringVariable = null;
			this.charCount = 0;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x00113F88 File Offset: 0x00112188
		public override void OnEnter()
		{
			this.DoGetStringRight();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x00113FA4 File Offset: 0x001121A4
		public override void OnUpdate()
		{
			this.DoGetStringRight();
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x00113FAC File Offset: 0x001121AC
		private void DoGetStringRight()
		{
			if (this.stringVariable.IsNone)
			{
				return;
			}
			if (this.storeResult.IsNone)
			{
				return;
			}
			string value = this.stringVariable.Value;
			int num = Mathf.Clamp(this.charCount.Value, 0, value.Length);
			this.storeResult.Value = value.Substring(value.Length - num, num);
		}

		// Token: 0x04002704 RID: 9988
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		// Token: 0x04002705 RID: 9989
		[Tooltip("Number of characters to get.")]
		public FsmInt charCount;

		// Token: 0x04002706 RID: 9990
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString storeResult;

		// Token: 0x04002707 RID: 9991
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
