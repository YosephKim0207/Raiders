using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A05 RID: 2565
	[Tooltip("Applies a jolt of force to a GameObject's scale and wobbles it back to its initial scale.")]
	[ActionCategory("iTween")]
	public class iTweenPunchScale : iTweenFsmAction
	{
		// Token: 0x060036FF RID: 14079 RVA: 0x0011A66C File Offset: 0x0011886C
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
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x0011A6D0 File Offset: 0x001188D0
		public override void OnEnter()
		{
			base.OnEnteriTween(this.gameObject);
			if (this.loopType != iTween.LoopType.none)
			{
				base.IsLoop(true);
			}
			this.DoiTween();
		}

		// Token: 0x06003701 RID: 14081 RVA: 0x0011A6F8 File Offset: 0x001188F8
		public override void OnExit()
		{
			base.OnExitiTween(this.gameObject);
		}

		// Token: 0x06003702 RID: 14082 RVA: 0x0011A708 File Offset: 0x00118908
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
			this.itweenType = "punch";
			iTween.PunchScale(ownerDefaultTarget, iTween.Hash(new object[]
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
				!this.realTime.IsNone && this.realTime.Value
			}));
		}

		// Token: 0x040028AB RID: 10411
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040028AC RID: 10412
		[Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
		public FsmString id;

		// Token: 0x040028AD RID: 10413
		[Tooltip("A vector punch range.")]
		[RequiredField]
		public FsmVector3 vector;

		// Token: 0x040028AE RID: 10414
		[Tooltip("The time in seconds the animation will take to complete.")]
		public FsmFloat time;

		// Token: 0x040028AF RID: 10415
		[Tooltip("The time in seconds the animation will wait before beginning.")]
		public FsmFloat delay;

		// Token: 0x040028B0 RID: 10416
		[Tooltip("The type of loop to apply once the animation has completed.")]
		public iTween.LoopType loopType;
	}
}
