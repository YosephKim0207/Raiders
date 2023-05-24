using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A5A RID: 2650
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Iterate through a list of all colliders that overlap a point in space.The colliders iterated are sorted in order of increasing Z coordinate. No iteration will take place if there are no colliders overlap this point.")]
	public class GetNextOverlapPoint2d : FsmStateAction
	{
		// Token: 0x06003866 RID: 14438 RVA: 0x0012161C File Offset: 0x0011F81C
		public override void Reset()
		{
			this.gameObject = null;
			this.position = new FsmVector2
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
			this.loopEvent = null;
			this.finishedEvent = null;
		}

		// Token: 0x06003867 RID: 14439 RVA: 0x001216A0 File Offset: 0x0011F8A0
		public override void OnEnter()
		{
			if (this.colliders == null)
			{
				this.colliders = this.GetOverlapPointAll();
				this.colliderCount = this.colliders.Length;
				this.collidersCount.Value = this.colliderCount;
			}
			this.DoGetNextCollider();
			base.Finish();
		}

		// Token: 0x06003868 RID: 14440 RVA: 0x001216F0 File Offset: 0x0011F8F0
		private void DoGetNextCollider()
		{
			if (this.nextColliderIndex >= this.colliderCount)
			{
				this.nextColliderIndex = 0;
				base.Fsm.Event(this.finishedEvent);
				return;
			}
			this.storeNextCollider.Value = this.colliders[this.nextColliderIndex].gameObject;
			if (this.nextColliderIndex >= this.colliderCount)
			{
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

		// Token: 0x06003869 RID: 14441 RVA: 0x00121798 File Offset: 0x0011F998
		private Collider2D[] GetOverlapPointAll()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			Vector2 value = this.position.Value;
			if (ownerDefaultTarget != null)
			{
				value.x += ownerDefaultTarget.transform.position.x;
				value.y += ownerDefaultTarget.transform.position.y;
			}
			if (this.minDepth.IsNone && this.maxDepth.IsNone)
			{
				return Physics2D.OverlapPointAll(value, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value));
			}
			float num = ((!this.minDepth.IsNone) ? ((float)this.minDepth.Value) : float.NegativeInfinity);
			float num2 = ((!this.maxDepth.IsNone) ? ((float)this.maxDepth.Value) : float.PositiveInfinity);
			return Physics2D.OverlapPointAll(value, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value), num, num2);
		}

		// Token: 0x04002A7D RID: 10877
		[Tooltip("Point using the gameObject position. \nOr use From Position parameter.")]
		[ActionSection("Setup")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002A7E RID: 10878
		[Tooltip("Point as a world position. \nOr use gameObject parameter. If both define, will add position to the gameObject position")]
		public FsmVector2 position;

		// Token: 0x04002A7F RID: 10879
		[Tooltip("Only include objects with a Z coordinate (depth) greater than this value. leave to none for no effect")]
		public FsmInt minDepth;

		// Token: 0x04002A80 RID: 10880
		[Tooltip("Only include objects with a Z coordinate (depth) less than this value. leave to none")]
		public FsmInt maxDepth;

		// Token: 0x04002A81 RID: 10881
		[ActionSection("Filter")]
		[UIHint(UIHint.Layer)]
		[Tooltip("Pick only from these layers.")]
		public FsmInt[] layerMask;

		// Token: 0x04002A82 RID: 10882
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		// Token: 0x04002A83 RID: 10883
		[ActionSection("Result")]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the number of colliders found for this overlap.")]
		public FsmInt collidersCount;

		// Token: 0x04002A84 RID: 10884
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("Store the next collider in a GameObject variable.")]
		public FsmGameObject storeNextCollider;

		// Token: 0x04002A85 RID: 10885
		[Tooltip("Event to send to get the next collider.")]
		public FsmEvent loopEvent;

		// Token: 0x04002A86 RID: 10886
		[Tooltip("Event to send when there are no more colliders to iterate.")]
		public FsmEvent finishedEvent;

		// Token: 0x04002A87 RID: 10887
		private Collider2D[] colliders;

		// Token: 0x04002A88 RID: 10888
		private int colliderCount;

		// Token: 0x04002A89 RID: 10889
		private int nextColliderIndex;
	}
}
