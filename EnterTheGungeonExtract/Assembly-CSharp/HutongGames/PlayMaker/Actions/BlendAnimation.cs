using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008FA RID: 2298
	[ActionCategory(ActionCategory.Animation)]
	[Tooltip("Blends an Animation towards a Target Weight over a specified Time.\nOptionally sends an Event when finished.")]
	public class BlendAnimation : BaseAnimationAction
	{
		// Token: 0x060032A7 RID: 12967 RVA: 0x0010A178 File Offset: 0x00108378
		public override void Reset()
		{
			this.gameObject = null;
			this.animName = null;
			this.targetWeight = 1f;
			this.time = 0.3f;
			this.finishEvent = null;
		}

		// Token: 0x060032A8 RID: 12968 RVA: 0x0010A1B0 File Offset: 0x001083B0
		public override void OnEnter()
		{
			this.DoBlendAnimation((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
		}

		// Token: 0x060032A9 RID: 12969 RVA: 0x0010A1E4 File Offset: 0x001083E4
		public override void OnUpdate()
		{
			if (DelayedEvent.WasSent(this.delayedFinishEvent))
			{
				base.Finish();
			}
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x0010A1FC File Offset: 0x001083FC
		private void DoBlendAnimation(GameObject go)
		{
			if (go == null)
			{
				return;
			}
			Animation component = go.GetComponent<Animation>();
			if (component == null)
			{
				base.LogWarning("Missing Animation component on GameObject: " + go.name);
				base.Finish();
				return;
			}
			AnimationState animationState = component[this.animName.Value];
			if (animationState == null)
			{
				base.LogWarning("Missing animation: " + this.animName.Value);
				base.Finish();
				return;
			}
			float value = this.time.Value;
			component.Blend(this.animName.Value, this.targetWeight.Value, value);
			if (this.finishEvent != null)
			{
				this.delayedFinishEvent = base.Fsm.DelayedEvent(this.finishEvent, animationState.length);
			}
			else
			{
				base.Finish();
			}
		}

		// Token: 0x040023CC RID: 9164
		[Tooltip("The GameObject to animate.")]
		[CheckForComponent(typeof(Animation))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040023CD RID: 9165
		[UIHint(UIHint.Animation)]
		[Tooltip("The name of the animation to blend.")]
		[RequiredField]
		public FsmString animName;

		// Token: 0x040023CE RID: 9166
		[HasFloatSlider(0f, 1f)]
		[RequiredField]
		[Tooltip("Target weight to blend to.")]
		public FsmFloat targetWeight;

		// Token: 0x040023CF RID: 9167
		[Tooltip("How long should the blend take.")]
		[HasFloatSlider(0f, 5f)]
		[RequiredField]
		public FsmFloat time;

		// Token: 0x040023D0 RID: 9168
		[Tooltip("Event to send when the blend has finished.")]
		public FsmEvent finishEvent;

		// Token: 0x040023D1 RID: 9169
		private DelayedEvent delayedFinishEvent;
	}
}
