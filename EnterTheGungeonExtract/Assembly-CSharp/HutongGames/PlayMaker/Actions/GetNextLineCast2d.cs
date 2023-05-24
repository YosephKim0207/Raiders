using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A57 RID: 2647
	[Tooltip("Iterate through a list of all colliders detected by a LineCastThe colliders iterated are sorted in order of increasing Z coordinate. No iteration will take place if there are no colliders within the area.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class GetNextLineCast2d : FsmStateAction
	{
		// Token: 0x06003857 RID: 14423 RVA: 0x00120C18 File Offset: 0x0011EE18
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
			this.minDepth = new FsmInt
			{
				UseVariable = true
			};
			this.maxDepth = new FsmInt
			{
				UseVariable = true
			};
			this.layerMask = new FsmInt[0];
			this.invertMask = false;
			this.collidersCount = null;
			this.storeNextCollider = null;
			this.storeNextHitPoint = null;
			this.storeNextHitNormal = null;
			this.storeNextHitDistance = null;
			this.loopEvent = null;
			this.finishedEvent = null;
		}

		// Token: 0x06003858 RID: 14424 RVA: 0x00120CCC File Offset: 0x0011EECC
		public override void OnEnter()
		{
			if (this.hits == null)
			{
				this.hits = this.GetLineCastAll();
				this.colliderCount = this.hits.Length;
				this.collidersCount.Value = this.colliderCount;
			}
			this.DoGetNextCollider();
			base.Finish();
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x00120D1C File Offset: 0x0011EF1C
		private void DoGetNextCollider()
		{
			if (this.nextColliderIndex >= this.colliderCount)
			{
				this.hits = new RaycastHit2D[0];
				this.nextColliderIndex = 0;
				base.Fsm.Event(this.finishedEvent);
				return;
			}
			Fsm.RecordLastRaycastHit2DInfo(base.Fsm, this.hits[this.nextColliderIndex]);
			this.storeNextCollider.Value = this.hits[this.nextColliderIndex].collider.gameObject;
			this.storeNextHitPoint.Value = this.hits[this.nextColliderIndex].point;
			this.storeNextHitNormal.Value = this.hits[this.nextColliderIndex].normal;
			this.storeNextHitDistance.Value = this.hits[this.nextColliderIndex].fraction;
			if (this.nextColliderIndex >= this.colliderCount)
			{
				this.hits = new RaycastHit2D[0];
				this.nextColliderIndex = 0;
				base.Fsm.Event(this.finishedEvent);
				return;
			}
			this.nextColliderIndex++;
			if (this.loopEvent != null)
			{
				base.Fsm.Event(this.loopEvent);
			}
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x00120E6C File Offset: 0x0011F06C
		private RaycastHit2D[] GetLineCastAll()
		{
			Vector2 value = this.fromPosition.Value;
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.fromGameObject);
			if (ownerDefaultTarget != null)
			{
				value.x += ownerDefaultTarget.transform.position.x;
				value.y += ownerDefaultTarget.transform.position.y;
			}
			Vector2 value2 = this.toPosition.Value;
			GameObject value3 = this.toGameObject.Value;
			if (value3 != null)
			{
				value2.x += value3.transform.position.x;
				value2.y += value3.transform.position.y;
			}
			if (this.minDepth.IsNone && this.maxDepth.IsNone)
			{
				return Physics2D.LinecastAll(value, value2, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value));
			}
			float num = ((!this.minDepth.IsNone) ? ((float)this.minDepth.Value) : float.NegativeInfinity);
			float num2 = ((!this.maxDepth.IsNone) ? ((float)this.maxDepth.Value) : float.PositiveInfinity);
			return Physics2D.LinecastAll(value, value2, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value), num, num2);
		}

		// Token: 0x04002A4E RID: 10830
		[Tooltip("Start ray at game object position. \nOr use From Position parameter.")]
		[ActionSection("Setup")]
		public FsmOwnerDefault fromGameObject;

		// Token: 0x04002A4F RID: 10831
		[Tooltip("Start ray at a vector2 world position. \nOr use fromGameObject parameter. If both define, will add fromPosition to the fromGameObject position")]
		public FsmVector2 fromPosition;

		// Token: 0x04002A50 RID: 10832
		[Tooltip("End ray at game object position. \nOr use From Position parameter.")]
		public FsmGameObject toGameObject;

		// Token: 0x04002A51 RID: 10833
		[Tooltip("End ray at a vector2 world position. \nOr use fromGameObject parameter. If both define, will add toPosition to the ToGameObject position")]
		public FsmVector2 toPosition;

		// Token: 0x04002A52 RID: 10834
		[Tooltip("Only include objects with a Z coordinate (depth) greater than this value. leave to none for no effect")]
		public FsmInt minDepth;

		// Token: 0x04002A53 RID: 10835
		[Tooltip("Only include objects with a Z coordinate (depth) less than this value. leave to none")]
		public FsmInt maxDepth;

		// Token: 0x04002A54 RID: 10836
		[Tooltip("Pick only from these layers.")]
		[UIHint(UIHint.Layer)]
		[ActionSection("Filter")]
		public FsmInt[] layerMask;

		// Token: 0x04002A55 RID: 10837
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		// Token: 0x04002A56 RID: 10838
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the number of colliders found for this overlap.")]
		[ActionSection("Result")]
		public FsmInt collidersCount;

		// Token: 0x04002A57 RID: 10839
		[Tooltip("Store the next collider in a GameObject variable.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeNextCollider;

		// Token: 0x04002A58 RID: 10840
		[Tooltip("Get the 2d position of the next ray hit point and store it in a variable.")]
		public FsmVector2 storeNextHitPoint;

		// Token: 0x04002A59 RID: 10841
		[Tooltip("Get the 2d normal at the next hit point and store it in a variable.")]
		public FsmVector2 storeNextHitNormal;

		// Token: 0x04002A5A RID: 10842
		[Tooltip("Get the distance along the ray to the next hit point and store it in a variable.")]
		public FsmFloat storeNextHitDistance;

		// Token: 0x04002A5B RID: 10843
		[Tooltip("Event to send to get the next collider.")]
		public FsmEvent loopEvent;

		// Token: 0x04002A5C RID: 10844
		[Tooltip("Event to send when there are no more colliders to iterate.")]
		public FsmEvent finishedEvent;

		// Token: 0x04002A5D RID: 10845
		private RaycastHit2D[] hits;

		// Token: 0x04002A5E RID: 10846
		private int colliderCount;

		// Token: 0x04002A5F RID: 10847
		private int nextColliderIndex;
	}
}
