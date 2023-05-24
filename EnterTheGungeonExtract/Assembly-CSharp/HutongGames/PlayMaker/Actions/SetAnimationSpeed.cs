using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AC2 RID: 2754
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Sets the Speed of an Animation. Check Every Frame to update the animation time continuosly, e.g., if you're manipulating a variable that controls animation speed.")]
	public class SetAnimationSpeed : BaseAnimationAction
	{
		// Token: 0x06003A5E RID: 14942 RVA: 0x00128FEC File Offset: 0x001271EC
		public override void Reset()
		{
			this.gameObject = null;
			this.animName = null;
			this.speed = 1f;
			this.everyFrame = false;
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x00129014 File Offset: 0x00127214
		public override void OnEnter()
		{
			this.DoSetAnimationSpeed((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A60 RID: 14944 RVA: 0x00129064 File Offset: 0x00127264
		public override void OnUpdate()
		{
			this.DoSetAnimationSpeed((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
		}

		// Token: 0x06003A61 RID: 14945 RVA: 0x00129098 File Offset: 0x00127298
		private void DoSetAnimationSpeed(GameObject go)
		{
			if (!base.UpdateCache(go))
			{
				return;
			}
			AnimationState animationState = base.animation[this.animName.Value];
			if (animationState == null)
			{
				base.LogWarning("Missing animation: " + this.animName.Value);
				return;
			}
			animationState.speed = this.speed.Value;
		}

		// Token: 0x04002C91 RID: 11409
		[CheckForComponent(typeof(Animation))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002C92 RID: 11410
		[UIHint(UIHint.Animation)]
		[RequiredField]
		public FsmString animName;

		// Token: 0x04002C93 RID: 11411
		public FsmFloat speed = 1f;

		// Token: 0x04002C94 RID: 11412
		public bool everyFrame;
	}
}
