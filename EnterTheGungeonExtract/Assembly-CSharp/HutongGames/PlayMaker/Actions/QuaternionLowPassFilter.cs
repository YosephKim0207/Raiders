using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A9B RID: 2715
	[Tooltip("Use a low pass filter to reduce the influence of sudden changes in a quaternion Variable.")]
	[ActionCategory(ActionCategory.Quaternion)]
	public class QuaternionLowPassFilter : QuaternionBaseAction
	{
		// Token: 0x060039A0 RID: 14752 RVA: 0x0012626C File Offset: 0x0012446C
		public override void Reset()
		{
			this.quaternionVariable = null;
			this.filteringFactor = 0.1f;
			this.everyFrame = true;
			this.everyFrameOption = QuaternionBaseAction.everyFrameOptions.Update;
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x00126294 File Offset: 0x00124494
		public override void OnEnter()
		{
			this.filteredQuaternion = new Quaternion(this.quaternionVariable.Value.x, this.quaternionVariable.Value.y, this.quaternionVariable.Value.z, this.quaternionVariable.Value.w);
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x0012630C File Offset: 0x0012450C
		public override void OnUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.Update)
			{
				this.DoQuatLowPassFilter();
			}
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x00126320 File Offset: 0x00124520
		public override void OnLateUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.LateUpdate)
			{
				this.DoQuatLowPassFilter();
			}
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x00126334 File Offset: 0x00124534
		public override void OnFixedUpdate()
		{
			if (this.everyFrameOption == QuaternionBaseAction.everyFrameOptions.FixedUpdate)
			{
				this.DoQuatLowPassFilter();
			}
		}

		// Token: 0x060039A5 RID: 14757 RVA: 0x00126348 File Offset: 0x00124548
		private void DoQuatLowPassFilter()
		{
			this.filteredQuaternion.x = this.quaternionVariable.Value.x * this.filteringFactor.Value + this.filteredQuaternion.x * (1f - this.filteringFactor.Value);
			this.filteredQuaternion.y = this.quaternionVariable.Value.y * this.filteringFactor.Value + this.filteredQuaternion.y * (1f - this.filteringFactor.Value);
			this.filteredQuaternion.z = this.quaternionVariable.Value.z * this.filteringFactor.Value + this.filteredQuaternion.z * (1f - this.filteringFactor.Value);
			this.filteredQuaternion.w = this.quaternionVariable.Value.w * this.filteringFactor.Value + this.filteredQuaternion.w * (1f - this.filteringFactor.Value);
			this.quaternionVariable.Value = new Quaternion(this.filteredQuaternion.x, this.filteredQuaternion.y, this.filteredQuaternion.z, this.filteredQuaternion.w);
		}

		// Token: 0x04002BD2 RID: 11218
		[Tooltip("quaternion Variable to filter. Should generally come from some constantly updated input")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmQuaternion quaternionVariable;

		// Token: 0x04002BD3 RID: 11219
		[Tooltip("Determines how much influence new changes have. E.g., 0.1 keeps 10 percent of the unfiltered quaternion and 90 percent of the previously filtered value.")]
		public FsmFloat filteringFactor;

		// Token: 0x04002BD4 RID: 11220
		private Quaternion filteredQuaternion;
	}
}
