using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AAB RID: 2731
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Rewinds the named animation.")]
	public class RewindAnimation : BaseAnimationAction
	{
		// Token: 0x060039E8 RID: 14824 RVA: 0x001274A0 File Offset: 0x001256A0
		public override void Reset()
		{
			this.gameObject = null;
			this.animName = null;
		}

		// Token: 0x060039E9 RID: 14825 RVA: 0x001274B0 File Offset: 0x001256B0
		public override void OnEnter()
		{
			this.DoRewindAnimation();
			base.Finish();
		}

		// Token: 0x060039EA RID: 14826 RVA: 0x001274C0 File Offset: 0x001256C0
		private void DoRewindAnimation()
		{
			if (string.IsNullOrEmpty(this.animName.Value))
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (base.UpdateCache(ownerDefaultTarget))
			{
				base.animation.Rewind(this.animName.Value);
			}
		}

		// Token: 0x04002C25 RID: 11301
		[CheckForComponent(typeof(Animation))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002C26 RID: 11302
		[UIHint(UIHint.Animation)]
		public FsmString animName;
	}
}
