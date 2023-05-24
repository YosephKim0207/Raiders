using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A12 RID: 2578
	[Tooltip("Randomly shakes a GameObject's rotation by a diminishing amount over time.")]
	[ActionCategory("iTween")]
	public class iTweenShakeRotation : iTweenFsmAction
	{
		// Token: 0x06003741 RID: 14145 RVA: 0x0011CAC4 File Offset: 0x0011ACC4
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
			this.space = Space.World;
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x0011CB30 File Offset: 0x0011AD30
		public override void OnEnter()
		{
			base.OnEnteriTween(this.gameObject);
			if (this.loopType != iTween.LoopType.none)
			{
				base.IsLoop(true);
			}
			this.DoiTween();
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x0011CB58 File Offset: 0x0011AD58
		public override void OnExit()
		{
			base.OnExitiTween(this.gameObject);
		}

		// Token: 0x06003744 RID: 14148 RVA: 0x0011CB68 File Offset: 0x0011AD68
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
			this.itweenType = "shake";
			iTween.ShakeRotation(ownerDefaultTarget, iTween.Hash(new object[]
			{
				"amount",
				vector,
				"name",
				(!this.id.IsNone) ? this.id.Value : string.Empty,
				"time",
				(!this.time.IsNone) ? this.time.Value : 1f,
				"delay",
				(!this.delay.IsNone) ? this.delay.Value : 0f,
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
				"space",
				this.space
			}));
		}

		// Token: 0x04002912 RID: 10514
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002913 RID: 10515
		[Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
		public FsmString id;

		// Token: 0x04002914 RID: 10516
		[Tooltip("A vector shake range.")]
		[RequiredField]
		public FsmVector3 vector;

		// Token: 0x04002915 RID: 10517
		[Tooltip("The time in seconds the animation will take to complete.")]
		public FsmFloat time;

		// Token: 0x04002916 RID: 10518
		[Tooltip("The time in seconds the animation will wait before beginning.")]
		public FsmFloat delay;

		// Token: 0x04002917 RID: 10519
		[Tooltip("The type of loop to apply once the animation has completed.")]
		public iTween.LoopType loopType;

		// Token: 0x04002918 RID: 10520
		public Space space;
	}
}
