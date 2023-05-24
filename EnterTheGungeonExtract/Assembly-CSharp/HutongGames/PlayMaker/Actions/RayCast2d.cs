using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A68 RID: 2664
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Casts a Ray against all Colliders in the scene. A raycast is conceptually like a laser beam that is fired from a point in space along a particular direction. Any object making contact with the beam can be detected and reported. Use GetRaycastHit2dInfo to get more detailed info.")]
	public class RayCast2d : FsmStateAction
	{
		// Token: 0x060038AC RID: 14508 RVA: 0x00122C64 File Offset: 0x00120E64
		public override void Reset()
		{
			this.fromGameObject = null;
			this.fromPosition = new FsmVector2
			{
				UseVariable = true
			};
			this.direction = new FsmVector2
			{
				UseVariable = true
			};
			this.space = Space.Self;
			this.minDepth = new FsmInt
			{
				UseVariable = true
			};
			this.maxDepth = new FsmInt
			{
				UseVariable = true
			};
			this.distance = 100f;
			this.hitEvent = null;
			this.storeDidHit = null;
			this.storeHitObject = null;
			this.storeHitPoint = null;
			this.storeHitNormal = null;
			this.storeHitDistance = null;
			this.storeHitFraction = null;
			this.repeatInterval = 1;
			this.layerMask = new FsmInt[0];
			this.invertMask = false;
			this.debugColor = Color.yellow;
			this.debug = false;
		}

		// Token: 0x060038AD RID: 14509 RVA: 0x00122D50 File Offset: 0x00120F50
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.fromGameObject);
			if (ownerDefaultTarget != null)
			{
				this._transform = ownerDefaultTarget.transform;
			}
			this.DoRaycast();
			if (this.repeatInterval.Value == 0)
			{
				base.Finish();
			}
		}

		// Token: 0x060038AE RID: 14510 RVA: 0x00122DA4 File Offset: 0x00120FA4
		public override void OnUpdate()
		{
			this.repeat--;
			if (this.repeat == 0)
			{
				this.DoRaycast();
			}
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x00122DC8 File Offset: 0x00120FC8
		private void DoRaycast()
		{
			this.repeat = this.repeatInterval.Value;
			if (Math.Abs(this.distance.Value) < Mathf.Epsilon)
			{
				return;
			}
			Vector2 value = this.fromPosition.Value;
			if (this._transform != null)
			{
				value.x += this._transform.position.x;
				value.y += this._transform.position.y;
			}
			float num = float.PositiveInfinity;
			if (this.distance.Value > 0f)
			{
				num = this.distance.Value;
			}
			Vector2 normalized = this.direction.Value.normalized;
			if (this._transform != null && this.space == Space.Self)
			{
				Vector3 vector = this._transform.TransformDirection(new Vector3(this.direction.Value.x, this.direction.Value.y, 0f));
				normalized.x = vector.x;
				normalized.y = vector.y;
			}
			RaycastHit2D raycastHit2D;
			if (this.minDepth.IsNone && this.maxDepth.IsNone)
			{
				raycastHit2D = Physics2D.Raycast(value, normalized, num, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value));
			}
			else
			{
				float num2 = ((!this.minDepth.IsNone) ? ((float)this.minDepth.Value) : float.NegativeInfinity);
				float num3 = ((!this.maxDepth.IsNone) ? ((float)this.maxDepth.Value) : float.PositiveInfinity);
				raycastHit2D = Physics2D.Raycast(value, normalized, num, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value), num2, num3);
			}
			Fsm.RecordLastRaycastHit2DInfo(base.Fsm, raycastHit2D);
			bool flag = raycastHit2D.collider != null;
			this.storeDidHit.Value = flag;
			if (flag)
			{
				this.storeHitObject.Value = raycastHit2D.collider.gameObject;
				this.storeHitPoint.Value = raycastHit2D.point;
				this.storeHitNormal.Value = raycastHit2D.normal;
				this.storeHitDistance.Value = raycastHit2D.distance;
				this.storeHitFraction.Value = raycastHit2D.fraction;
				base.Fsm.Event(this.hitEvent);
			}
			if (this.debug.Value)
			{
				float num4 = Mathf.Min(num, 1000f);
				Vector3 vector2 = new Vector3(value.x, value.y, 0f);
				Vector3 vector3 = new Vector3(normalized.x, normalized.y, 0f);
				Vector3 vector4 = vector2 + vector3 * num4;
				Debug.DrawLine(vector2, vector4, this.debugColor.Value);
			}
		}

		// Token: 0x04002AEF RID: 10991
		[Tooltip("Start ray at game object position. \nOr use From Position parameter.")]
		[ActionSection("Setup")]
		public FsmOwnerDefault fromGameObject;

		// Token: 0x04002AF0 RID: 10992
		[Tooltip("Start ray at a vector2 world position. \nOr use Game Object parameter.")]
		public FsmVector2 fromPosition;

		// Token: 0x04002AF1 RID: 10993
		[Tooltip("A vector2 direction vector")]
		public FsmVector2 direction;

		// Token: 0x04002AF2 RID: 10994
		[Tooltip("Cast the ray in world or local space. Note if no Game Object is specified, the direction is in world space.")]
		public Space space;

		// Token: 0x04002AF3 RID: 10995
		[Tooltip("The length of the ray. Set to -1 for infinity.")]
		public FsmFloat distance;

		// Token: 0x04002AF4 RID: 10996
		[Tooltip("Only include objects with a Z coordinate (depth) greater than this value. leave to none for no effect")]
		public FsmInt minDepth;

		// Token: 0x04002AF5 RID: 10997
		[Tooltip("Only include objects with a Z coordinate (depth) less than this value. leave to none")]
		public FsmInt maxDepth;

		// Token: 0x04002AF6 RID: 10998
		[UIHint(UIHint.Variable)]
		[Tooltip("Event to send if the ray hits an object.")]
		[ActionSection("Result")]
		public FsmEvent hitEvent;

		// Token: 0x04002AF7 RID: 10999
		[UIHint(UIHint.Variable)]
		[Tooltip("Set a bool variable to true if hit something, otherwise false.")]
		public FsmBool storeDidHit;

		// Token: 0x04002AF8 RID: 11000
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the game object hit in a variable.")]
		public FsmGameObject storeHitObject;

		// Token: 0x04002AF9 RID: 11001
		[Tooltip("Get the 2d position of the ray hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 storeHitPoint;

		// Token: 0x04002AFA RID: 11002
		[Tooltip("Get the 2d normal at the hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 storeHitNormal;

		// Token: 0x04002AFB RID: 11003
		[Tooltip("Get the distance along the ray to the hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeHitDistance;

		// Token: 0x04002AFC RID: 11004
		[Tooltip("Get the fraction along the ray to the hit point and store it in a variable. If the ray's direction vector is normalised then this value is simply the distance between the origin and the hit point. If the direction is not normalised then this distance is expressed as a 'fraction' (which could be greater than 1) of the vector's magnitude.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeHitFraction;

		// Token: 0x04002AFD RID: 11005
		[Tooltip("Set how often to cast a ray. 0 = once, don't repeat; 1 = everyFrame; 2 = every other frame... \nSince raycasts can get expensive use the highest repeat interval you can get away with.")]
		[ActionSection("Filter")]
		public FsmInt repeatInterval;

		// Token: 0x04002AFE RID: 11006
		[Tooltip("Pick only from these layers.")]
		[UIHint(UIHint.Layer)]
		public FsmInt[] layerMask;

		// Token: 0x04002AFF RID: 11007
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		// Token: 0x04002B00 RID: 11008
		[Tooltip("The color to use for the debug line.")]
		[ActionSection("Debug")]
		public FsmColor debugColor;

		// Token: 0x04002B01 RID: 11009
		[Tooltip("Draw a debug line. Note: Check Gizmos in the Game View to see it in game.")]
		public FsmBool debug;

		// Token: 0x04002B02 RID: 11010
		private Transform _transform;

		// Token: 0x04002B03 RID: 11011
		private int repeat;
	}
}
