using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A11 RID: 2577
	[Tooltip("Randomly shakes a GameObject's position by a diminishing amount over time.")]
	[ActionCategory("iTween")]
	public class iTweenShakePosition : iTweenFsmAction
	{
		// Token: 0x0600373C RID: 14140 RVA: 0x0011C804 File Offset: 0x0011AA04
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
			this.axis = iTweenFsmAction.AxisRestriction.none;
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x0011C874 File Offset: 0x0011AA74
		public override void OnEnter()
		{
			base.OnEnteriTween(this.gameObject);
			if (this.loopType != iTween.LoopType.none)
			{
				base.IsLoop(true);
			}
			this.DoiTween();
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x0011C89C File Offset: 0x0011AA9C
		public override void OnExit()
		{
			base.OnExitiTween(this.gameObject);
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x0011C8AC File Offset: 0x0011AAAC
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
			iTween.ShakePosition(ownerDefaultTarget, iTween.Hash(new object[]
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
				this.space,
				"axis",
				(this.axis != iTweenFsmAction.AxisRestriction.none) ? Enum.GetName(typeof(iTweenFsmAction.AxisRestriction), this.axis) : string.Empty
			}));
		}

		// Token: 0x0400290A RID: 10506
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x0400290B RID: 10507
		[Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
		public FsmString id;

		// Token: 0x0400290C RID: 10508
		[Tooltip("A vector shake range.")]
		[RequiredField]
		public FsmVector3 vector;

		// Token: 0x0400290D RID: 10509
		[Tooltip("The time in seconds the animation will take to complete.")]
		public FsmFloat time;

		// Token: 0x0400290E RID: 10510
		[Tooltip("The time in seconds the animation will wait before beginning.")]
		public FsmFloat delay;

		// Token: 0x0400290F RID: 10511
		[Tooltip("The type of loop to apply once the animation has completed.")]
		public iTween.LoopType loopType;

		// Token: 0x04002910 RID: 10512
		public Space space;

		// Token: 0x04002911 RID: 10513
		[Tooltip("Restricts rotation to the supplied axis only. Just put there strinc like 'x' or 'xz'")]
		public iTweenFsmAction.AxisRestriction axis;
	}
}
