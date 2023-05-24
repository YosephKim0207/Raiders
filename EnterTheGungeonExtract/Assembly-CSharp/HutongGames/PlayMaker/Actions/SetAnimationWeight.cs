using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AC4 RID: 2756
	[Tooltip("Sets the Blend Weight of an Animation. Check Every Frame to update the weight continuosly, e.g., if you're manipulating a variable that controls the weight.")]
	[ActionCategory(ActionCategory.Animation)]
	public class SetAnimationWeight : BaseAnimationAction
	{
		// Token: 0x06003A68 RID: 14952 RVA: 0x00129288 File Offset: 0x00127488
		public override void Reset()
		{
			this.gameObject = null;
			this.animName = null;
			this.weight = 1f;
			this.everyFrame = false;
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x001292B0 File Offset: 0x001274B0
		public override void OnEnter()
		{
			this.DoSetAnimationWeight((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x00129300 File Offset: 0x00127500
		public override void OnUpdate()
		{
			this.DoSetAnimationWeight((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x00129334 File Offset: 0x00127534
		private void DoSetAnimationWeight(GameObject go)
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
			animationState.weight = this.weight.Value;
		}

		// Token: 0x04002C9A RID: 11418
		[CheckForComponent(typeof(Animation))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002C9B RID: 11419
		[UIHint(UIHint.Animation)]
		[RequiredField]
		public FsmString animName;

		// Token: 0x04002C9C RID: 11420
		public FsmFloat weight = 1f;

		// Token: 0x04002C9D RID: 11421
		public bool everyFrame;
	}
}
