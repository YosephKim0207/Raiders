using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A0D RID: 2573
	[ActionCategory("iTween")]
	[Tooltip("Multiplies a GameObject's scale over time.")]
	public class iTweenScaleBy : iTweenFsmAction
	{
		// Token: 0x06003727 RID: 14119 RVA: 0x0011BC6C File Offset: 0x00119E6C
		public override void Reset()
		{
			base.Reset();
			this.id = new FsmString
			{
				UseVariable = true
			};
			this.time = 1f;
			this.delay = 0f;
			this.loopType = iTween.LoopType.none;
			this.vector = new FsmVector3
			{
				UseVariable = true
			};
			this.speed = new FsmFloat
			{
				UseVariable = true
			};
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x0011BCE4 File Offset: 0x00119EE4
		public override void OnEnter()
		{
			base.OnEnteriTween(this.gameObject);
			if (this.loopType != iTween.LoopType.none)
			{
				base.IsLoop(true);
			}
			this.DoiTween();
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x0011BD0C File Offset: 0x00119F0C
		public override void OnExit()
		{
			base.OnExitiTween(this.gameObject);
		}

		// Token: 0x0600372A RID: 14122 RVA: 0x0011BD1C File Offset: 0x00119F1C
		private void DoiTween()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Vector3 vector = Vector3.zero;
			if (!this.vector.IsNone)
			{
				vector = this.vector.Value;
			}
			this.itweenType = "scale";
			iTween.ScaleBy(ownerDefaultTarget, iTween.Hash(new object[]
			{
				"amount",
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

		// Token: 0x040028EA RID: 10474
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040028EB RID: 10475
		[Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
		public FsmString id;

		// Token: 0x040028EC RID: 10476
		[Tooltip("A vector that will multiply current GameObjects scale.")]
		[RequiredField]
		public FsmVector3 vector;

		// Token: 0x040028ED RID: 10477
		[Tooltip("The time in seconds the animation will take to complete.")]
		public FsmFloat time;

		// Token: 0x040028EE RID: 10478
		[Tooltip("The time in seconds the animation will wait before beginning.")]
		public FsmFloat delay;

		// Token: 0x040028EF RID: 10479
		[Tooltip("Can be used instead of time to allow animation based on speed. When you define speed the time variable is ignored.")]
		public FsmFloat speed;

		// Token: 0x040028F0 RID: 10480
		[Tooltip("The shape of the easing curve applied to the animation.")]
		public iTween.EaseType easeType = iTween.EaseType.linear;

		// Token: 0x040028F1 RID: 10481
		[Tooltip("The type of loop to apply once the animation has completed.")]
		public iTween.LoopType loopType;
	}
}
