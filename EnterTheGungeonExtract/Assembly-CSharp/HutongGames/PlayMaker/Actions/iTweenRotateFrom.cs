using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A09 RID: 2569
	[Tooltip("Instantly changes a GameObject's Euler angles in degrees then returns it to it's starting rotation over time.")]
	[ActionCategory("iTween")]
	public class iTweenRotateFrom : iTweenFsmAction
	{
		// Token: 0x06003712 RID: 14098 RVA: 0x0011AF78 File Offset: 0x00119178
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

		// Token: 0x06003713 RID: 14099 RVA: 0x0011B00C File Offset: 0x0011920C
		public override void OnEnter()
		{
			base.OnEnteriTween(this.gameObject);
			if (this.loopType != iTween.LoopType.none)
			{
				base.IsLoop(true);
			}
			this.DoiTween();
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x0011B034 File Offset: 0x00119234
		public override void OnExit()
		{
			base.OnExitiTween(this.gameObject);
		}

		// Token: 0x06003715 RID: 14101 RVA: 0x0011B044 File Offset: 0x00119244
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
			iTween.RotateFrom(ownerDefaultTarget, iTween.Hash(new object[]
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

		// Token: 0x040028C7 RID: 10439
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040028C8 RID: 10440
		[Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
		public FsmString id;

		// Token: 0x040028C9 RID: 10441
		[Tooltip("A rotation from a GameObject.")]
		public FsmGameObject transformRotation;

		// Token: 0x040028CA RID: 10442
		[Tooltip("A rotation vector the GameObject will animate from.")]
		public FsmVector3 vectorRotation;

		// Token: 0x040028CB RID: 10443
		[Tooltip("The time in seconds the animation will take to complete.")]
		public FsmFloat time;

		// Token: 0x040028CC RID: 10444
		[Tooltip("The time in seconds the animation will wait before beginning.")]
		public FsmFloat delay;

		// Token: 0x040028CD RID: 10445
		[Tooltip("Can be used instead of time to allow animation based on speed. When you define speed the time variable is ignored.")]
		public FsmFloat speed;

		// Token: 0x040028CE RID: 10446
		[Tooltip("The shape of the easing curve applied to the animation.")]
		public iTween.EaseType easeType = iTween.EaseType.linear;

		// Token: 0x040028CF RID: 10447
		[Tooltip("The type of loop to apply once the animation has completed.")]
		public iTween.LoopType loopType;

		// Token: 0x040028D0 RID: 10448
		[Tooltip("Whether to animate in local or world space.")]
		public Space space;
	}
}
