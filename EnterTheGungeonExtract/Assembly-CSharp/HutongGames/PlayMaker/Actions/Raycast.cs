using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AA3 RID: 2723
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Casts a Ray against all Colliders in the scene. Use either a Game Object or Vector3 world position as the origin of the ray. Use GetRaycastInfo to get more detailed info.")]
	public class Raycast : FsmStateAction
	{
		// Token: 0x060039C7 RID: 14791 RVA: 0x00126A34 File Offset: 0x00124C34
		public override void Reset()
		{
			this.fromGameObject = null;
			this.fromPosition = new FsmVector3
			{
				UseVariable = true
			};
			this.direction = new FsmVector3
			{
				UseVariable = true
			};
			this.space = Space.Self;
			this.distance = 100f;
			this.hitEvent = null;
			this.storeDidHit = null;
			this.storeHitObject = null;
			this.storeHitPoint = null;
			this.storeHitNormal = null;
			this.storeHitDistance = null;
			this.repeatInterval = 1;
			this.layerMask = new FsmInt[0];
			this.invertMask = false;
			this.debugColor = Color.yellow;
			this.debug = false;
		}

		// Token: 0x060039C8 RID: 14792 RVA: 0x00126AF4 File Offset: 0x00124CF4
		public override void OnEnter()
		{
			this.DoRaycast();
			if (this.repeatInterval.Value == 0)
			{
				base.Finish();
			}
		}

		// Token: 0x060039C9 RID: 14793 RVA: 0x00126B14 File Offset: 0x00124D14
		public override void OnUpdate()
		{
			this.repeat--;
			if (this.repeat == 0)
			{
				this.DoRaycast();
			}
		}

		// Token: 0x060039CA RID: 14794 RVA: 0x00126B38 File Offset: 0x00124D38
		private void DoRaycast()
		{
			this.repeat = this.repeatInterval.Value;
			if (this.distance.Value == 0f)
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.fromGameObject);
			Vector3 vector = ((!(ownerDefaultTarget != null)) ? this.fromPosition.Value : ownerDefaultTarget.transform.position);
			float num = float.PositiveInfinity;
			if (this.distance.Value > 0f)
			{
				num = this.distance.Value;
			}
			Vector3 vector2 = this.direction.Value;
			if (ownerDefaultTarget != null && this.space == Space.Self)
			{
				vector2 = ownerDefaultTarget.transform.TransformDirection(this.direction.Value);
			}
			RaycastHit raycastHit;
			Physics.Raycast(vector, vector2, out raycastHit, num, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value));
			base.Fsm.RaycastHitInfo = raycastHit;
			bool flag = raycastHit.collider != null;
			this.storeDidHit.Value = flag;
			if (flag)
			{
				this.storeHitObject.Value = raycastHit.collider.gameObject;
				this.storeHitPoint.Value = base.Fsm.RaycastHitInfo.point;
				this.storeHitNormal.Value = base.Fsm.RaycastHitInfo.normal;
				this.storeHitDistance.Value = base.Fsm.RaycastHitInfo.distance;
				base.Fsm.Event(this.hitEvent);
			}
			if (this.debug.Value)
			{
				float num2 = Mathf.Min(num, 1000f);
				Debug.DrawLine(vector, vector + vector2 * num2, this.debugColor.Value);
			}
		}

		// Token: 0x04002BF1 RID: 11249
		[Tooltip("Start ray at game object position. \nOr use From Position parameter.")]
		public FsmOwnerDefault fromGameObject;

		// Token: 0x04002BF2 RID: 11250
		[Tooltip("Start ray at a vector3 world position. \nOr use Game Object parameter.")]
		public FsmVector3 fromPosition;

		// Token: 0x04002BF3 RID: 11251
		[Tooltip("A vector3 direction vector")]
		public FsmVector3 direction;

		// Token: 0x04002BF4 RID: 11252
		[Tooltip("Cast the ray in world or local space. Note if no Game Object is specfied, the direction is in world space.")]
		public Space space;

		// Token: 0x04002BF5 RID: 11253
		[Tooltip("The length of the ray. Set to -1 for infinity.")]
		public FsmFloat distance;

		// Token: 0x04002BF6 RID: 11254
		[UIHint(UIHint.Variable)]
		[Tooltip("Event to send if the ray hits an object.")]
		[ActionSection("Result")]
		public FsmEvent hitEvent;

		// Token: 0x04002BF7 RID: 11255
		[UIHint(UIHint.Variable)]
		[Tooltip("Set a bool variable to true if hit something, otherwise false.")]
		public FsmBool storeDidHit;

		// Token: 0x04002BF8 RID: 11256
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the game object hit in a variable.")]
		public FsmGameObject storeHitObject;

		// Token: 0x04002BF9 RID: 11257
		[Tooltip("Get the world position of the ray hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeHitPoint;

		// Token: 0x04002BFA RID: 11258
		[Tooltip("Get the normal at the hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeHitNormal;

		// Token: 0x04002BFB RID: 11259
		[Tooltip("Get the distance along the ray to the hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeHitDistance;

		// Token: 0x04002BFC RID: 11260
		[Tooltip("Set how often to cast a ray. 0 = once, don't repeat; 1 = everyFrame; 2 = every other frame... \nSince raycasts can get expensive use the highest repeat interval you can get away with.")]
		[ActionSection("Filter")]
		public FsmInt repeatInterval;

		// Token: 0x04002BFD RID: 11261
		[UIHint(UIHint.Layer)]
		[Tooltip("Pick only from these layers.")]
		public FsmInt[] layerMask;

		// Token: 0x04002BFE RID: 11262
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		// Token: 0x04002BFF RID: 11263
		[Tooltip("The color to use for the debug line.")]
		[ActionSection("Debug")]
		public FsmColor debugColor;

		// Token: 0x04002C00 RID: 11264
		[Tooltip("Draw a debug line. Note: Check Gizmos in the Game View to see it in game.")]
		public FsmBool debug;

		// Token: 0x04002C01 RID: 11265
		private int repeat;
	}
}
