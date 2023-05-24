using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A59 RID: 2649
	[Tooltip("Iterate through a list of all colliders that fall within a circular area.The colliders iterated are sorted in order of increasing Z coordinate. No iteration will take place if there are no colliders within the area.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class GetNextOverlapCircle2d : FsmStateAction
	{
		// Token: 0x06003861 RID: 14433 RVA: 0x00121344 File Offset: 0x0011F544
		public override void Reset()
		{
			this.fromGameObject = null;
			this.fromPosition = new FsmVector2
			{
				UseVariable = true
			};
			this.radius = 10f;
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

		// Token: 0x06003862 RID: 14434 RVA: 0x001213D8 File Offset: 0x0011F5D8
		public override void OnEnter()
		{
			if (this.colliders == null)
			{
				this.colliders = this.GetOverlapCircleAll();
				this.colliderCount = this.colliders.Length;
				this.collidersCount.Value = this.colliderCount;
			}
			this.DoGetNextCollider();
			base.Finish();
		}

		// Token: 0x06003863 RID: 14435 RVA: 0x00121428 File Offset: 0x0011F628
		private void DoGetNextCollider()
		{
			if (this.nextColliderIndex >= this.colliderCount)
			{
				this.nextColliderIndex = 0;
				this.colliders = null;
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

		// Token: 0x06003864 RID: 14436 RVA: 0x001214D8 File Offset: 0x0011F6D8
		private Collider2D[] GetOverlapCircleAll()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.fromGameObject);
			Vector2 value = this.fromPosition.Value;
			if (ownerDefaultTarget != null)
			{
				value.x += ownerDefaultTarget.transform.position.x;
				value.y += ownerDefaultTarget.transform.position.y;
			}
			if (this.minDepth.IsNone && this.maxDepth.IsNone)
			{
				return Physics2D.OverlapCircleAll(value, this.radius.Value, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value));
			}
			float num = ((!this.minDepth.IsNone) ? ((float)this.minDepth.Value) : float.NegativeInfinity);
			float num2 = ((!this.maxDepth.IsNone) ? ((float)this.maxDepth.Value) : float.PositiveInfinity);
			return Physics2D.OverlapCircleAll(value, this.radius.Value, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value), num, num2);
		}

		// Token: 0x04002A6F RID: 10863
		[Tooltip("Center of the circle area. \nOr use From Position parameter.")]
		[ActionSection("Setup")]
		public FsmOwnerDefault fromGameObject;

		// Token: 0x04002A70 RID: 10864
		[Tooltip("CEnter of the circle area as a world position. \nOr use fromGameObject parameter. If both define, will add fromPosition to the fromGameObject position")]
		public FsmVector2 fromPosition;

		// Token: 0x04002A71 RID: 10865
		[Tooltip("The circle radius")]
		public FsmFloat radius;

		// Token: 0x04002A72 RID: 10866
		[Tooltip("Only include objects with a Z coordinate (depth) greater than this value. leave to none for no effect")]
		public FsmInt minDepth;

		// Token: 0x04002A73 RID: 10867
		[Tooltip("Only include objects with a Z coordinate (depth) less than this value. leave to none")]
		public FsmInt maxDepth;

		// Token: 0x04002A74 RID: 10868
		[Tooltip("Pick only from these layers.")]
		[UIHint(UIHint.Layer)]
		[ActionSection("Filter")]
		public FsmInt[] layerMask;

		// Token: 0x04002A75 RID: 10869
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		// Token: 0x04002A76 RID: 10870
		[ActionSection("Result")]
		[Tooltip("Store the number of colliders found for this overlap.")]
		[UIHint(UIHint.Variable)]
		public FsmInt collidersCount;

		// Token: 0x04002A77 RID: 10871
		[RequiredField]
		[Tooltip("Store the next collider in a GameObject variable.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeNextCollider;

		// Token: 0x04002A78 RID: 10872
		[Tooltip("Event to send to get the next collider.")]
		public FsmEvent loopEvent;

		// Token: 0x04002A79 RID: 10873
		[Tooltip("Event to send when there are no more colliders to iterate.")]
		public FsmEvent finishedEvent;

		// Token: 0x04002A7A RID: 10874
		private Collider2D[] colliders;

		// Token: 0x04002A7B RID: 10875
		private int colliderCount;

		// Token: 0x04002A7C RID: 10876
		private int nextColliderIndex;
	}
}
