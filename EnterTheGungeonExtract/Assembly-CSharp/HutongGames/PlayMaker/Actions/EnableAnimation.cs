using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200093D RID: 2365
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Enables/Disables an Animation on a GameObject.\nAnimation time is paused while disabled. Animation must also have a non zero weight to play.")]
	public class EnableAnimation : BaseAnimationAction
	{
		// Token: 0x060033C2 RID: 13250 RVA: 0x0010E148 File Offset: 0x0010C348
		public override void Reset()
		{
			this.gameObject = null;
			this.animName = null;
			this.enable = true;
			this.resetOnExit = false;
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x0010E170 File Offset: 0x0010C370
		public override void OnEnter()
		{
			this.DoEnableAnimation(base.Fsm.GetOwnerDefaultTarget(this.gameObject));
			base.Finish();
		}

		// Token: 0x060033C4 RID: 13252 RVA: 0x0010E190 File Offset: 0x0010C390
		private void DoEnableAnimation(GameObject go)
		{
			if (base.UpdateCache(go))
			{
				this.anim = base.animation[this.animName.Value];
				if (this.anim != null)
				{
					this.anim.enabled = this.enable.Value;
				}
			}
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x0010E1EC File Offset: 0x0010C3EC
		public override void OnExit()
		{
			if (this.resetOnExit.Value && this.anim != null)
			{
				this.anim.enabled = !this.enable.Value;
			}
		}

		// Token: 0x040024E6 RID: 9446
		[Tooltip("The GameObject playing the animation.")]
		[CheckForComponent(typeof(Animation))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040024E7 RID: 9447
		[Tooltip("The name of the animation to enable/disable.")]
		[UIHint(UIHint.Animation)]
		[RequiredField]
		public FsmString animName;

		// Token: 0x040024E8 RID: 9448
		[Tooltip("Set to True to enable, False to disable.")]
		[RequiredField]
		public FsmBool enable;

		// Token: 0x040024E9 RID: 9449
		[Tooltip("Reset the initial enabled state when exiting the state.")]
		public FsmBool resetOnExit;

		// Token: 0x040024EA RID: 9450
		private AnimationState anim;
	}
}
