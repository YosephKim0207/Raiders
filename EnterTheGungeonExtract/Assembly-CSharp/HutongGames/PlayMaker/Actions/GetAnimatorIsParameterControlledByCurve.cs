using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008B6 RID: 2230
	[Tooltip("Returns true if a parameter is controlled by an additional curve on an animation")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorIsParameterControlledByCurve : FsmStateAction
	{
		// Token: 0x06003189 RID: 12681 RVA: 0x00106248 File Offset: 0x00104448
		public override void Reset()
		{
			this.gameObject = null;
			this.parameterName = null;
			this.isControlledByCurve = null;
			this.isControlledByCurveEvent = null;
			this.isNotControlledByCurveEvent = null;
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x00106270 File Offset: 0x00104470
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
			this.DoCheckIsParameterControlledByCurve();
			base.Finish();
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x001062D4 File Offset: 0x001044D4
		private void DoCheckIsParameterControlledByCurve()
		{
			if (this._animator == null)
			{
				return;
			}
			bool flag = this._animator.IsParameterControlledByCurve(this.parameterName.Value);
			this.isControlledByCurve.Value = flag;
			if (flag)
			{
				base.Fsm.Event(this.isControlledByCurveEvent);
			}
			else
			{
				base.Fsm.Event(this.isNotControlledByCurveEvent);
			}
		}

		// Token: 0x040022AB RID: 8875
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		[Tooltip("The target. An Animator component is required")]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022AC RID: 8876
		[Tooltip("The parameter's name")]
		public FsmString parameterName;

		// Token: 0x040022AD RID: 8877
		[Tooltip("True if controlled by curve")]
		[ActionSection("Results")]
		[UIHint(UIHint.Variable)]
		public FsmBool isControlledByCurve;

		// Token: 0x040022AE RID: 8878
		[Tooltip("Event send if controlled by curve")]
		public FsmEvent isControlledByCurveEvent;

		// Token: 0x040022AF RID: 8879
		[Tooltip("Event send if not controlled by curve")]
		public FsmEvent isNotControlledByCurveEvent;

		// Token: 0x040022B0 RID: 8880
		private Animator _animator;
	}
}
