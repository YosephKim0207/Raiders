using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AC3 RID: 2755
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Sets the current Time of an Animation, Normalize time means 0 (start) to 1 (end); useful if you don't care about the exact time. Check Every Frame to update the time continuosly.")]
	public class SetAnimationTime : BaseAnimationAction
	{
		// Token: 0x06003A63 RID: 14947 RVA: 0x0012910C File Offset: 0x0012730C
		public override void Reset()
		{
			this.gameObject = null;
			this.animName = null;
			this.time = null;
			this.normalized = false;
			this.everyFrame = false;
		}

		// Token: 0x06003A64 RID: 14948 RVA: 0x00129134 File Offset: 0x00127334
		public override void OnEnter()
		{
			this.DoSetAnimationTime((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A65 RID: 14949 RVA: 0x00129184 File Offset: 0x00127384
		public override void OnUpdate()
		{
			this.DoSetAnimationTime((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
		}

		// Token: 0x06003A66 RID: 14950 RVA: 0x001291B8 File Offset: 0x001273B8
		private void DoSetAnimationTime(GameObject go)
		{
			if (!base.UpdateCache(go))
			{
				return;
			}
			base.animation.Play(this.animName.Value);
			AnimationState animationState = base.animation[this.animName.Value];
			if (animationState == null)
			{
				base.LogWarning("Missing animation: " + this.animName.Value);
				return;
			}
			if (this.normalized)
			{
				animationState.normalizedTime = this.time.Value;
			}
			else
			{
				animationState.time = this.time.Value;
			}
			if (this.everyFrame)
			{
				animationState.speed = 0f;
			}
		}

		// Token: 0x04002C95 RID: 11413
		[CheckForComponent(typeof(Animation))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002C96 RID: 11414
		[UIHint(UIHint.Animation)]
		[RequiredField]
		public FsmString animName;

		// Token: 0x04002C97 RID: 11415
		public FsmFloat time;

		// Token: 0x04002C98 RID: 11416
		public bool normalized;

		// Token: 0x04002C99 RID: 11417
		public bool everyFrame;
	}
}
