using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008CA RID: 2250
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Sets the value of a float parameter")]
	public class SetAnimatorFloat : FsmStateActionAnimatorBase
	{
		// Token: 0x060031E7 RID: 12775 RVA: 0x00107624 File Offset: 0x00105824
		public override void Reset()
		{
			base.Reset();
			this.gameObject = null;
			this.parameter = null;
			this.dampTime = new FsmFloat
			{
				UseVariable = true
			};
			this.Value = null;
		}

		// Token: 0x060031E8 RID: 12776 RVA: 0x00107660 File Offset: 0x00105860
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				base.Finish();
				return;
			}
			this._animator = ownerDefaultTarget.GetComponent<Animator>();
			if (this._animator == null)
			{
				base.Finish();
				return;
			}
			this._paramID = Animator.StringToHash(this.parameter.Value);
			this.SetParameter();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x001076E4 File Offset: 0x001058E4
		public override void OnActionUpdate()
		{
			this.SetParameter();
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x001076EC File Offset: 0x001058EC
		private void SetParameter()
		{
			if (this._animator == null)
			{
				return;
			}
			if (this.dampTime.Value > 0f)
			{
				this._animator.SetFloat(this._paramID, this.Value.Value, this.dampTime.Value, Time.deltaTime);
			}
			else
			{
				this._animator.SetFloat(this._paramID, this.Value.Value);
			}
		}

		// Token: 0x04002308 RID: 8968
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002309 RID: 8969
		[UIHint(UIHint.AnimatorFloat)]
		[Tooltip("The animator parameter")]
		[RequiredField]
		public FsmString parameter;

		// Token: 0x0400230A RID: 8970
		[Tooltip("The float value to assign to the animator parameter")]
		public FsmFloat Value;

		// Token: 0x0400230B RID: 8971
		[Tooltip("Optional: The time allowed to parameter to reach the value. Requires everyFrame Checked on")]
		public FsmFloat dampTime;

		// Token: 0x0400230C RID: 8972
		private Animator _animator;

		// Token: 0x0400230D RID: 8973
		private int _paramID;
	}
}
