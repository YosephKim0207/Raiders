using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A0A RID: 2570
	[Tooltip("Rotates a GameObject to the supplied Euler angles in degrees over time.")]
	[ActionCategory("iTween")]
	public class iTweenRotateTo : iTweenFsmAction
	{
		// Token: 0x06003717 RID: 14103 RVA: 0x0011B2E8 File Offset: 0x001194E8
		public override void Reset()
		{
			base.Reset();
			this.id = new FsmString
			{
				UseVariable = true
			};
			this.transformRotation = new FsmGameObject
			{
				UseVariable = true
			};
			this.vectorRotation = new FsmVector3
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
			this.space = Space.World;
		}

		// Token: 0x06003718 RID: 14104 RVA: 0x0011B37C File Offset: 0x0011957C
		public override void OnEnter()
		{
			base.OnEnteriTween(this.gameObject);
			if (this.loopType != iTween.LoopType.none)
			{
				base.IsLoop(true);
			}
			this.DoiTween();
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x0011B3A4 File Offset: 0x001195A4
		public override void OnExit()
		{
			base.OnExitiTween(this.gameObject);
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x0011B3B4 File Offset: 0x001195B4
		private void DoiTween()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Vector3 vector = ((!this.vectorRotation.IsNone) ? this.vectorRotation.Value : Vector3.zero);
			if (!this.transformRotation.IsNone && this.transformRotation.Value)
			{
				vector = ((this.space != Space.World) ? (this.transformRotation.Value.transform.localEulerAngles + vector) : (this.transformRotation.Value.transform.eulerAngles + vector));
			}
			this.itweenType = "rotate";
			iTween.RotateTo(ownerDefaultTarget, iTween.Hash(new object[]
			{
				"rotation",
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
				!this.realTime.IsNone && this.realTime.Value,
				"islocal",
				this.space == Space.Self
			}));
		}

		// Token: 0x040028D1 RID: 10449
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040028D2 RID: 10450
		[Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
		public FsmString id;

		// Token: 0x040028D3 RID: 10451
		[Tooltip("Rotate to a transform rotation.")]
		public FsmGameObject transformRotation;

		// Token: 0x040028D4 RID: 10452
		[Tooltip("A rotation the GameObject will animate from.")]
		public FsmVector3 vectorRotation;

		// Token: 0x040028D5 RID: 10453
		[Tooltip("The time in seconds the animation will take to complete.")]
		public FsmFloat time;

		// Token: 0x040028D6 RID: 10454
		[Tooltip("The time in seconds the animation will wait before beginning.")]
		public FsmFloat delay;

		// Token: 0x040028D7 RID: 10455
		[Tooltip("Can be used instead of time to allow animation based on speed. When you define speed the time variable is ignored.")]
		public FsmFloat speed;

		// Token: 0x040028D8 RID: 10456
		[Tooltip("The shape of the easing curve applied to the animation.")]
		public iTween.EaseType easeType = iTween.EaseType.linear;

		// Token: 0x040028D9 RID: 10457
		[Tooltip("The type of loop to apply once the animation has completed.")]
		public iTween.LoopType loopType;

		// Token: 0x040028DA RID: 10458
		[Tooltip("Whether to animate in local or world space.")]
		public Space space;
	}
}
