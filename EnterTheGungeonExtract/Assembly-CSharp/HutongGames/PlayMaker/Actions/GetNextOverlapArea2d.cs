using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A58 RID: 2648
	[Tooltip("Iterate through a list of all colliders that fall within a rectangular area.The colliders iterated are sorted in order of increasing Z coordinate. No iteration will take place if there are no colliders within the area.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class GetNextOverlapArea2d : FsmStateAction
	{
		// Token: 0x0600385C RID: 14428 RVA: 0x0012100C File Offset: 0x0011F20C
		public override void Reset()
		{
			this.firstCornerGameObject = null;
			this.firstCornerPosition = new FsmVector2
			{
				UseVariable = true
			};
			this.secondCornerGameObject = null;
			this.secondCornerPosition = new FsmVector2
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

		// Token: 0x0600385D RID: 14429 RVA: 0x001210AC File Offset: 0x0011F2AC
		public override void OnEnter()
		{
			if (this.colliders == null)
			{
				this.colliders = this.GetOverlapAreaAll();
				this.colliderCount = this.colliders.Length;
				this.collidersCount.Value = this.colliderCount;
			}
			this.DoGetNextCollider();
			base.Finish();
		}

		// Token: 0x0600385E RID: 14430 RVA: 0x001210FC File Offset: 0x0011F2FC
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

		// Token: 0x0600385F RID: 14431 RVA: 0x001211A4 File Offset: 0x0011F3A4
		private Collider2D[] GetOverlapAreaAll()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.firstCornerGameObject);
			Vector2 value = this.firstCornerPosition.Value;
			if (ownerDefaultTarget != null)
			{
				value.x += ownerDefaultTarget.transform.position.x;
				value.y += ownerDefaultTarget.transform.position.y;
			}
			GameObject value2 = this.secondCornerGameObject.Value;
			Vector2 value3 = this.secondCornerPosition.Value;
			if (value2 != null)
			{
				value3.x += value2.transform.position.x;
				value3.y += value2.transform.position.y;
			}
			if (this.minDepth.IsNone && this.maxDepth.IsNone)
			{
				return Physics2D.OverlapAreaAll(value, value3, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value));
			}
			float num = ((!this.minDepth.IsNone) ? ((float)this.minDepth.Value) : float.NegativeInfinity);
			float num2 = ((!this.maxDepth.IsNone) ? ((float)this.maxDepth.Value) : float.PositiveInfinity);
			return Physics2D.OverlapAreaAll(value, value3, ActionHelpers.LayerArrayToLayerMask(this.layerMask, this.invertMask.Value), num, num2);
		}

		// Token: 0x04002A60 RID: 10848
		[Tooltip("First corner of the rectangle area using the game object position. \nOr use firstCornerPosition parameter.")]
		[ActionSection("Setup")]
		public FsmOwnerDefault firstCornerGameObject;

		// Token: 0x04002A61 RID: 10849
		[Tooltip("First Corner of the rectangle area as a world position. \nOr use FirstCornerGameObject parameter. If both define, will add firstCornerPosition to the FirstCornerGameObject position")]
		public FsmVector2 firstCornerPosition;

		// Token: 0x04002A62 RID: 10850
		[Tooltip("Second corner of the rectangle area using the game object position. \nOr use secondCornerPosition parameter.")]
		public FsmGameObject secondCornerGameObject;

		// Token: 0x04002A63 RID: 10851
		[Tooltip("Second Corner rectangle area as a world position. \nOr use SecondCornerGameObject parameter. If both define, will add secondCornerPosition to the SecondCornerGameObject position")]
		public FsmVector2 secondCornerPosition;

		// Token: 0x04002A64 RID: 10852
		[Tooltip("Only include objects with a Z coordinate (depth) greater than this value. leave to none for no effect")]
		public FsmInt minDepth;

		// Token: 0x04002A65 RID: 10853
		[Tooltip("Only include objects with a Z coordinate (depth) less than this value. leave to none")]
		public FsmInt maxDepth;

		// Token: 0x04002A66 RID: 10854
		[UIHint(UIHint.Layer)]
		[Tooltip("Pick only from these layers.")]
		[ActionSection("Filter")]
		public FsmInt[] layerMask;

		// Token: 0x04002A67 RID: 10855
		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		// Token: 0x04002A68 RID: 10856
		[Tooltip("Store the number of colliders found for this overlap.")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Result")]
		public FsmInt collidersCount;

		// Token: 0x04002A69 RID: 10857
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the next collider in a GameObject variable.")]
		[RequiredField]
		public FsmGameObject storeNextCollider;

		// Token: 0x04002A6A RID: 10858
		[Tooltip("Event to send to get the next collider.")]
		public FsmEvent loopEvent;

		// Token: 0x04002A6B RID: 10859
		[Tooltip("Event to send when there are no more colliders to iterate.")]
		public FsmEvent finishedEvent;

		// Token: 0x04002A6C RID: 10860
		private Collider2D[] colliders;

		// Token: 0x04002A6D RID: 10861
		private int colliderCount;

		// Token: 0x04002A6E RID: 10862
		private int nextColliderIndex;
	}
}
