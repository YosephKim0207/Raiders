using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000C65 RID: 3173
	[ActionCategory(".Brave")]
	[Tooltip("Handles updating an AIAnimator.")]
	public class SetAiAnimator : FsmStateAction
	{
		// Token: 0x0600443F RID: 17471 RVA: 0x00160AD0 File Offset: 0x0015ECD0
		public override void Reset()
		{
			this.GameObject = null;
			this.mode = SetAiAnimator.Mode.SetBaseAnim;
			this.baseAnimName = string.Empty;
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x00160AF0 File Offset: 0x0015ECF0
		public override string ErrorCheck()
		{
			string text = string.Empty;
			GameObject gameObject = ((this.GameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.GameObject.GameObject.Value : base.Owner);
			if (gameObject)
			{
				AIAnimator component = gameObject.GetComponent<AIAnimator>();
				if (!component)
				{
					return "Requires an AI Animator.\n";
				}
				if (this.mode == SetAiAnimator.Mode.SetBaseAnim && this.baseAnimName.Value != string.Empty && !component.HasDirectionalAnimation(this.baseAnimName.Value))
				{
					text = text + "Unknown animation " + this.baseAnimName.Value + ".\n";
				}
			}
			else if (!this.GameObject.GameObject.UseVariable)
			{
				return "No object specified";
			}
			return text;
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x00160BCC File Offset: 0x0015EDCC
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.GameObject);
			AIAnimator component = ownerDefaultTarget.GetComponent<AIAnimator>();
			if (this.mode == SetAiAnimator.Mode.SetBaseAnim)
			{
				if (this.baseAnimName.Value == string.Empty)
				{
					component.ClearBaseAnim();
				}
				else
				{
					component.SetBaseAnim(this.baseAnimName.Value, false);
				}
			}
			base.Finish();
		}

		// Token: 0x04003651 RID: 13905
		public FsmOwnerDefault GameObject;

		// Token: 0x04003652 RID: 13906
		public SetAiAnimator.Mode mode;

		// Token: 0x04003653 RID: 13907
		[Tooltip("Name of the new default animation state (Directional Animations only).  Leave blank to return to the default (idle/base).")]
		public FsmString baseAnimName;

		// Token: 0x02000C66 RID: 3174
		public enum Mode
		{
			// Token: 0x04003655 RID: 13909
			SetBaseAnim
		}
	}
}
