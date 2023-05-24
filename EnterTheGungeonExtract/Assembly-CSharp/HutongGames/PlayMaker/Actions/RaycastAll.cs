using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AA4 RID: 2724
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Casts a Ray against all Colliders in the scene. Use either a GameObject or Vector3 world position as the origin of the ray. Use GetRaycastAllInfo to get more detailed info.")]
	public class RaycastAll : FsmStateAction
	{
		// Token: 0x060039CC RID: 14796 RVA: 0x00126D24 File Offset: 0x00124F24
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
			this.storeHitObjects = null;
			this.storeHitPoint = null;
			this.storeHitNormal = null;
			this.storeHitDistance = null;
			this.repeatInterval = 1;
			this.layerMask = new FsmInt[0];
			this.invertMask = false;
			this.debugColor = Color.yellow;
			this.debug = false;
		}

		// Token: 0x060039CD RID: 14797 RVA: 0x00126DE4 File Offset: 0x00124FE4
		public override void OnEnter()
		{
			this.DoRaycast();
			if (this.repeatInterval.Value == 0)
			{
				base.Finish();
			}
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x00126E04 File Offset: 0x00125004
		public override void OnUpdate()
		{
			this.repeat--;
			if (this.repeat == 0)
			{
				this.DoRaycast();
			}
		}

		// Token: 0x060039CF RID: 14799 RVA: 0x00126E28 File Offset: 0x00125028
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
			RaycastAll.RaycastAllHitInfo = Physics.RaycastAll(vector, vector2, num, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value));
			bool flag = RaycastAll.RaycastAllHitInfo.Length > 0;
			this.storeDidHit.Value = flag;
			if (flag)
			{
				GameObject[] array = new GameObject[RaycastAll.RaycastAllHitInfo.Length];
				for (int i = 0; i < RaycastAll.RaycastAllHitInfo.Length; i++)
				{
					RaycastHit raycastHit = RaycastAll.RaycastAllHitInfo[i];
					array[i] = raycastHit.collider.gameObject;
				}
				this.storeHitObjects.Values = array;
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

		// Token: 0x04002C02 RID: 11266
		public static RaycastHit[] RaycastAllHitInfo;

		// Token: 0x04002C03 RID: 11267
		[Tooltip("Start ray at game object position. \nOr use From Position parameter.")]
		public FsmOwnerDefault fromGameObject;

		// Token: 0x04002C04 RID: 11268
		[Tooltip("Start ray at a vector3 world position. \nOr use Game Object parameter.")]
		public FsmVector3 fromPosition;

		// Token: 0x04002C05 RID: 11269
		[Tooltip("A vector3 direction vector")]
		public FsmVector3 direction;

		// Token: 0x04002C06 RID: 11270
		[Tooltip("Cast the ray in world or local space. Note if no Game Object is specfied, the direction is in world space.")]
		public Space space;

		// Token: 0x04002C07 RID: 11271
		[Tooltip("The length of the ray. Set to -1 for infinity.")]
		public FsmFloat distance;

		// Token: 0x04002C08 RID: 11272
		[Tooltip("Event to send if the ray hits an object.")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Result")]
		public FsmEvent hitEvent;

		// Token: 0x04002C09 RID: 11273
		[UIHint(UIHint.Variable)]
		[Tooltip("Set a bool variable to true if hit something, otherwise false.")]
		public FsmBool storeDidHit;

		// Token: 0x04002C0A RID: 11274
		[ArrayEditor(VariableType.GameObject, "", 0, 0, 65536)]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the GameObjects hit in an array variable.")]
		public FsmArray storeHitObjects;

		// Token: 0x04002C0B RID: 11275
		[Tooltip("Get the world position of the ray hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeHitPoint;

		// Token: 0x04002C0C RID: 11276
		[Tooltip("Get the normal at the hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeHitNormal;

		// Token: 0x04002C0D RID: 11277
		[Tooltip("Get the distance along the ray to the hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeHitDistance;

		// Token: 0x04002C0E RID: 11278
		[ActionSection("Filter")]
		[Tooltip("Set how often to cast a ray. 0 = once, don't repeat; 1 = everyFrame; 2 = every other frame... \nSince raycasts can get expensive use the highest repeat interval you can get away with.")]
		public FsmInt repeatInterval;

		// Token: 0x04002C0F RID: 11279
		[UIHint(UIHint.Layer)]
		[Tooltip("Pick only from these layers.")]
		public FsmInt[] layerMask;

		// Token: 0x04002C10 RID: 11280
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		// Token: 0x04002C11 RID: 11281
		[Tooltip("The color to use for the debug line.")]
		[ActionSection("Debug")]
		public FsmColor debugColor;

		// Token: 0x04002C12 RID: 11282
		[Tooltip("Draw a debug line. Note: Check Gizmos in the Game View to see it in game.")]
		public FsmBool debug;

		// Token: 0x04002C13 RID: 11283
		private int repeat;
	}
}
