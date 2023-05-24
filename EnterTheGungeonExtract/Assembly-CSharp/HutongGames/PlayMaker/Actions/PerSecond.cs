using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A50 RID: 2640
	[Tooltip("Multiplies a Float by Time.deltaTime to use in frame-rate independent operations. E.g., 10 becomes 10 units per second.")]
	[ActionCategory(ActionCategory.Time)]
	public class PerSecond : FsmStateAction
	{
		// Token: 0x06003831 RID: 14385 RVA: 0x001203CC File Offset: 0x0011E5CC
		public override void Reset()
		{
			this.floatValue = null;
			this.storeResult = null;
			this.everyFrame = false;
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x001203E4 File Offset: 0x0011E5E4
		public override void OnEnter()
		{
			this.DoPerSecond();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003833 RID: 14387 RVA: 0x00120400 File Offset: 0x0011E600
		public override void OnUpdate()
		{
			this.DoPerSecond();
		}

		// Token: 0x06003834 RID: 14388 RVA: 0x00120408 File Offset: 0x0011E608
		private void DoPerSecond()
		{
			if (this.storeResult == null)
			{
				return;
			}
			this.storeResult.Value = this.floatValue.Value * Time.deltaTime;
		}

		// Token: 0x04002A2D RID: 10797
		[RequiredField]
		public FsmFloat floatValue;

		// Token: 0x04002A2E RID: 10798
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat storeResult;

		// Token: 0x04002A2F RID: 10799
		public bool everyFrame;
	}
}
