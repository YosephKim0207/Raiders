using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A63 RID: 2659
	[Tooltip("Casts a Ray against all Colliders in the scene.A linecast is an imaginary line between two points in world space. Any object making contact with the beam can be detected and reported. This differs from the similar raycast in that raycasting specifies the line using an origin and direction.Use GetRaycastHit2dInfo to get more detailed info.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class LineCast2d : FsmStateAction
	{
		// Token: 0x06003892 RID: 14482 RVA: 0x001222B8 File Offset: 0x001204B8
		public override void Reset()
		{
			this.fromGameObject = null;
			this.fromPosition = new FsmVector2
			{
				UseVariable = true
			};
			this.toGameObject = null;
			this.toPosition = new FsmVector2
			{
				UseVariable = true
			};
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

		// Token: 0x06003893 RID: 14483 RVA: 0x00122368 File Offset: 0x00120568
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.fromGameObject);
			if (ownerDefaultTarget != null)
			{
				this._fromTrans = ownerDefaultTarget.transform;
			}
			GameObject value = this.toGameObject.Value;
			if (value != null)
			{
				this._toTrans = value.transform;
			}
			this.DoRaycast();
			if (this.repeatInterval.Value == 0)
			{
				base.Finish();
			}
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x001223E0 File Offset: 0x001205E0
		public override void OnUpdate()
		{
			this.repeat--;
			if (this.repeat == 0)
			{
				this.DoRaycast();
			}
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x00122404 File Offset: 0x00120604
		private void DoRaycast()
		{
			this.repeat = this.repeatInterval.Value;
			Vector2 value = this.fromPosition.Value;
			if (this._fromTrans != null)
			{
				value.x += this._fromTrans.position.x;
				value.y += this._fromTrans.position.y;
			}
			Vector2 value2 = this.toPosition.Value;
			if (this._toTrans != null)
			{
				value2.x += this._toTrans.position.x;
				value2.y += this._toTrans.position.y;
			}
			RaycastHit2D raycastHit2D;
			if (this.minDepth.IsNone && this.maxDepth.IsNone)
			{
				raycastHit2D = Physics2D.Linecast(value, value2, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value));
			}
			else
			{
				float num = ((!this.minDepth.IsNone) ? ((float)this.minDepth.Value) : float.NegativeInfinity);
				float num2 = ((!this.maxDepth.IsNone) ? ((float)this.maxDepth.Value) : float.PositiveInfinity);
				raycastHit2D = Physics2D.Linecast(value, value2, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value), num, num2);
			}
			Fsm.RecordLastRaycastHit2DInfo(base.Fsm, raycastHit2D);
			bool flag = raycastHit2D.collider != null;
			this.storeDidHit.Value = flag;
			if (flag)
			{
				this.storeHitObject.Value = raycastHit2D.collider.gameObject;
				this.storeHitPoint.Value = raycastHit2D.point;
				this.storeHitNormal.Value = raycastHit2D.normal;
				this.storeHitDistance.Value = raycastHit2D.fraction;
				base.Fsm.Event(this.hitEvent);
			}
			if (this.debug.Value)
			{
				Vector3 vector = new Vector3(value.x, value.y, 0f);
				Vector3 vector2 = new Vector3(value2.x, value2.y, 0f);
				Debug.DrawLine(vector, vector2, this.debugColor.Value);
			}
		}

		// Token: 0x04002ABE RID: 10942
		[Tooltip("Start ray at game object position. \nOr use From Position parameter.")]
		[ActionSection("Setup")]
		public FsmOwnerDefault fromGameObject;

		// Token: 0x04002ABF RID: 10943
		[Tooltip("Start ray at a vector2 world position. \nOr use fromGameObject parameter. If both define, will add fromPosition to the fromGameObject position")]
		public FsmVector2 fromPosition;

		// Token: 0x04002AC0 RID: 10944
		[Tooltip("End ray at game object position. \nOr use From Position parameter.")]
		public FsmGameObject toGameObject;

		// Token: 0x04002AC1 RID: 10945
		[Tooltip("End ray at a vector2 world position. \nOr use fromGameObject parameter. If both define, will add toPosition to the ToGameObject position")]
		public FsmVector2 toPosition;

		// Token: 0x04002AC2 RID: 10946
		[Tooltip("Only include objects with a Z coordinate (depth) greater than this value. leave to none for no effect")]
		public FsmInt minDepth;

		// Token: 0x04002AC3 RID: 10947
		[Tooltip("Only include objects with a Z coordinate (depth) less than this value. leave to none")]
		public FsmInt maxDepth;

		// Token: 0x04002AC4 RID: 10948
		[UIHint(UIHint.Variable)]
		[Tooltip("Event to send if the ray hits an object.")]
		[ActionSection("Result")]
		public FsmEvent hitEvent;

		// Token: 0x04002AC5 RID: 10949
		[UIHint(UIHint.Variable)]
		[Tooltip("Set a bool variable to true if hit something, otherwise false.")]
		public FsmBool storeDidHit;

		// Token: 0x04002AC6 RID: 10950
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the game object hit in a variable.")]
		public FsmGameObject storeHitObject;

		// Token: 0x04002AC7 RID: 10951
		[Tooltip("Get the 2d position of the ray hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 storeHitPoint;

		// Token: 0x04002AC8 RID: 10952
		[Tooltip("Get the 2d normal at the hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 storeHitNormal;

		// Token: 0x04002AC9 RID: 10953
		[Tooltip("Get the distance along the ray to the hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeHitDistance;

		// Token: 0x04002ACA RID: 10954
		[Tooltip("Set how often to cast a ray. 0 = once, don't repeat; 1 = everyFrame; 2 = every other frame... \nSince raycasts can get expensive use the highest repeat interval you can get away with.")]
		[ActionSection("Filter")]
		public FsmInt repeatInterval;

		// Token: 0x04002ACB RID: 10955
		[Tooltip("Pick only from these layers.")]
		[UIHint(UIHint.Layer)]
		public FsmInt[] layerMask;

		// Token: 0x04002ACC RID: 10956
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		// Token: 0x04002ACD RID: 10957
		[Tooltip("The color to use for the debug line.")]
		[ActionSection("Debug")]
		public FsmColor debugColor;

		// Token: 0x04002ACE RID: 10958
		[Tooltip("Draw a debug line. Note: Check Gizmos in the Game View to see it in game.")]
		public FsmBool debug;

		// Token: 0x04002ACF RID: 10959
		private Transform _fromTrans;

		// Token: 0x04002AD0 RID: 10960
		private Transform _toTrans;

		// Token: 0x04002AD1 RID: 10961
		private int repeat;
	}
}
