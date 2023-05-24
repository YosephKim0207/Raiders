using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A0F RID: 2575
	[Tooltip("Changes a GameObject's scale over time.")]
	[ActionCategory("iTween")]
	public class iTweenScaleTo : iTweenFsmAction
	{
		// Token: 0x06003731 RID: 14129 RVA: 0x0011C258 File Offset: 0x0011A458
		public override void Reset()
		{
			base.Reset();
			this.id = new FsmString
			{
				UseVariable = true
			};
			this.transformScale = new FsmGameObject
			{
				UseVariable = true
			};
			this.vectorScale = new FsmVector3
			{
				UseVariable = true
			};
			this.time = 1f;
			this.delay = 0f;
			this.loopType = iTween.LoopType.none;
			this.speed = new FsmFloat
			{
				UseVariable = true
			};
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x0011C2E4 File Offset: 0x0011A4E4
		public override void OnEnter()
		{
			base.OnEnteriTween(this.gameObject);
			if (this.loopType != iTween.LoopType.none)
			{
				base.IsLoop(true);
			}
			this.DoiTween();
		}

		// Token: 0x06003733 RID: 14131 RVA: 0x0011C30C File Offset: 0x0011A50C
		public override void OnExit()
		{
			base.OnExitiTween(this.gameObject);
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x0011C31C File Offset: 0x0011A51C
		private void DoiTween()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Vector3 vector = ((!this.vectorScale.IsNone) ? this.vectorScale.Value : Vector3.zero);
			if (!this.transformScale.IsNone && this.transformScale.Value)
			{
				vector = this.transformScale.Value.transform.localScale + vector;
			}
			this.itweenType = "scale";
			iTween.ScaleTo(ownerDefaultTarget, iTween.Hash(new object[]
			{
				"scale",
				vector,
				"name",
				(!this.id.IsNone) ? this.id.Value : string.Empty,
				(!this.speed.IsNone) ? "speed" : "time",
				(!this.speed.IsNone) ? this.speed.Value : ((!this.time.IsNone) ? this.time.Value : 1f),
				"delay",
				(!this.delay.IsNone) ? this.delay.Value : 0f,
				"easetype",
				this.easeType,
				"looptype",
				this.loopType,
				"oncomplete",
				"iTweenOnComplete",
				"oncompleteparams",
				this.itweenID,
				"onstart",
				"iTweenOnStart",
				"onstartparams",
				this.itweenID,
				"ignoretimescale",
				!this.realTime.IsNone && this.realTime.Value
			}));
		}

		// Token: 0x040028FB RID: 10491
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040028FC RID: 10492
		[Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
		public FsmString id;

		// Token: 0x040028FD RID: 10493
		[Tooltip("Scale To a transform scale.")]
		public FsmGameObject transformScale;

		// Token: 0x040028FE RID: 10494
		[Tooltip("A scale vector the GameObject will animate To.")]
		public FsmVector3 vectorScale;

		// Token: 0x040028FF RID: 10495
		[Tooltip("The time in seconds the animation will take to complete.")]
		public FsmFloat time;

		// Token: 0x04002900 RID: 10496
		[Tooltip("The time in seconds the animation will wait before beginning.")]
		public FsmFloat delay;

		// Token: 0x04002901 RID: 10497
		[Tooltip("Can be used instead of time to allow animation based on speed. When you define speed the time variable is ignored.")]
		public FsmFloat speed;

		// Token: 0x04002902 RID: 10498
		[Tooltip("The shape of the easing curve applied to the animation.")]
		public iTween.EaseType easeType = iTween.EaseType.linear;

		// Token: 0x04002903 RID: 10499
		[Tooltip("The type of loop to apply once the animation has completed.")]
		public iTween.LoopType loopType;
	}
}
