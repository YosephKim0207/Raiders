using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B24 RID: 2852
	[Tooltip("Stops all playing Animations on a Game Object. Optionally, specify a single Animation to Stop.")]
	[ActionCategory(ActionCategory.Animation)]
	public class StopAnimation : BaseAnimationAction
	{
		// Token: 0x06003C14 RID: 15380 RVA: 0x0012EA64 File Offset: 0x0012CC64
		public override void Reset()
		{
			this.gameObject = null;
			this.animName = null;
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x0012EA74 File Offset: 0x0012CC74
		public override void OnEnter()
		{
			this.DoStopAnimation();
			base.Finish();
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x0012EA84 File Offset: 0x0012CC84
		private void DoStopAnimation()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (!base.UpdateCache(ownerDefaultTarget))
			{
				return;
			}
			if (FsmString.IsNullOrEmpty(this.animName))
			{
				base.animation.Stop();
			}
			else
			{
				base.animation.Stop(this.animName.Value);
			}
		}

		// Token: 0x04002E45 RID: 11845
		[CheckForComponent(typeof(Animation))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002E46 RID: 11846
		[UIHint(UIHint.Animation)]
		[Tooltip("Leave empty to stop all playing animations.")]
		public FsmString animName;
	}
}
