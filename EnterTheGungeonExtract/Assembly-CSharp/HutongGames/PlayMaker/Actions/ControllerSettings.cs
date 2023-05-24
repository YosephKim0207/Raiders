using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000912 RID: 2322
	[Tooltip("Modify various character controller settings.\n'None' leaves the setting unchanged.")]
	[ActionCategory(ActionCategory.Character)]
	public class ControllerSettings : FsmStateAction
	{
		// Token: 0x06003326 RID: 13094 RVA: 0x0010C490 File Offset: 0x0010A690
		public override void Reset()
		{
			this.gameObject = null;
			this.height = new FsmFloat
			{
				UseVariable = true
			};
			this.radius = new FsmFloat
			{
				UseVariable = true
			};
			this.slopeLimit = new FsmFloat
			{
				UseVariable = true
			};
			this.stepOffset = new FsmFloat
			{
				UseVariable = true
			};
			this.center = new FsmVector3
			{
				UseVariable = true
			};
			this.detectCollisions = new FsmBool
			{
				UseVariable = true
			};
			this.everyFrame = false;
		}

		// Token: 0x06003327 RID: 13095 RVA: 0x0010C524 File Offset: 0x0010A724
		public override void OnEnter()
		{
			this.DoControllerSettings();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x0010C540 File Offset: 0x0010A740
		public override void OnUpdate()
		{
			this.DoControllerSettings();
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x0010C548 File Offset: 0x0010A748
		private void DoControllerSettings()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (ownerDefaultTarget != this.previousGo)
			{
				this.controller = ownerDefaultTarget.GetComponent<CharacterController>();
				this.previousGo = ownerDefaultTarget;
			}
			if (this.controller != null)
			{
				if (!this.height.IsNone)
				{
					this.controller.height = this.height.Value;
				}
				if (!this.radius.IsNone)
				{
					this.controller.radius = this.radius.Value;
				}
				if (!this.slopeLimit.IsNone)
				{
					this.controller.slopeLimit = this.slopeLimit.Value;
				}
				if (!this.stepOffset.IsNone)
				{
					this.controller.stepOffset = this.stepOffset.Value;
				}
				if (!this.center.IsNone)
				{
					this.controller.center = this.center.Value;
				}
				if (!this.detectCollisions.IsNone)
				{
					this.controller.detectCollisions = this.detectCollisions.Value;
				}
			}
		}

		// Token: 0x0400244F RID: 9295
		[Tooltip("The GameObject that owns the CharacterController.")]
		[CheckForComponent(typeof(CharacterController))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002450 RID: 9296
		[Tooltip("The height of the character's capsule.")]
		public FsmFloat height;

		// Token: 0x04002451 RID: 9297
		[Tooltip("The radius of the character's capsule.")]
		public FsmFloat radius;

		// Token: 0x04002452 RID: 9298
		[Tooltip("The character controllers slope limit in degrees.")]
		public FsmFloat slopeLimit;

		// Token: 0x04002453 RID: 9299
		[Tooltip("The character controllers step offset in meters.")]
		public FsmFloat stepOffset;

		// Token: 0x04002454 RID: 9300
		[Tooltip("The center of the character's capsule relative to the transform's position")]
		public FsmVector3 center;

		// Token: 0x04002455 RID: 9301
		[Tooltip("Should other rigidbodies or character controllers collide with this character controller (By default always enabled).")]
		public FsmBool detectCollisions;

		// Token: 0x04002456 RID: 9302
		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		// Token: 0x04002457 RID: 9303
		private GameObject previousGo;

		// Token: 0x04002458 RID: 9304
		private CharacterController controller;
	}
}
