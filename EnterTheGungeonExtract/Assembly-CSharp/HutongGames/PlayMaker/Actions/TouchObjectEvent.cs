using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B34 RID: 2868
	[ActionTarget(typeof(GameObject), "gameObject", false)]
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Sends events when an object is touched. Optionally filter by a fingerID. NOTE: Uses the MainCamera!")]
	public class TouchObjectEvent : FsmStateAction
	{
		// Token: 0x06003C4D RID: 15437 RVA: 0x0012F924 File Offset: 0x0012DB24
		public override void Reset()
		{
			this.gameObject = null;
			this.pickDistance = 100f;
			this.fingerId = new FsmInt
			{
				UseVariable = true
			};
			this.touchBegan = null;
			this.touchMoved = null;
			this.touchStationary = null;
			this.touchEnded = null;
			this.touchCanceled = null;
			this.storeFingerId = null;
			this.storeHitPoint = null;
			this.storeHitNormal = null;
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x0012F994 File Offset: 0x0012DB94
		public override void OnUpdate()
		{
			if (Camera.main == null)
			{
				base.LogError("No MainCamera defined!");
				base.Finish();
				return;
			}
			if (Input.touchCount > 0)
			{
				GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
				if (ownerDefaultTarget == null)
				{
					return;
				}
				foreach (Touch touch in Input.touches)
				{
					if (this.fingerId.IsNone || touch.fingerId == this.fingerId.Value)
					{
						Vector2 position = touch.position;
						RaycastHit raycastHit;
						Physics.Raycast(Camera.main.ScreenPointToRay(position), out raycastHit, this.pickDistance.Value);
						base.Fsm.RaycastHitInfo = raycastHit;
						if (raycastHit.transform != null && raycastHit.transform.gameObject == ownerDefaultTarget)
						{
							this.storeFingerId.Value = touch.fingerId;
							this.storeHitPoint.Value = raycastHit.point;
							this.storeHitNormal.Value = raycastHit.normal;
							switch (touch.phase)
							{
							case TouchPhase.Began:
								base.Fsm.Event(this.touchBegan);
								return;
							case TouchPhase.Moved:
								base.Fsm.Event(this.touchMoved);
								return;
							case TouchPhase.Stationary:
								base.Fsm.Event(this.touchStationary);
								return;
							case TouchPhase.Ended:
								base.Fsm.Event(this.touchEnded);
								return;
							case TouchPhase.Canceled:
								base.Fsm.Event(this.touchCanceled);
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x04002E96 RID: 11926
		[CheckForComponent(typeof(Collider))]
		[RequiredField]
		[Tooltip("The Game Object to detect touches on.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002E97 RID: 11927
		[Tooltip("How far from the camera is the Game Object pickable.")]
		[RequiredField]
		public FsmFloat pickDistance;

		// Token: 0x04002E98 RID: 11928
		[Tooltip("Only detect touches that match this fingerID, or set to None.")]
		public FsmInt fingerId;

		// Token: 0x04002E99 RID: 11929
		[Tooltip("Event to send on touch began.")]
		[ActionSection("Events")]
		public FsmEvent touchBegan;

		// Token: 0x04002E9A RID: 11930
		[Tooltip("Event to send on touch moved.")]
		public FsmEvent touchMoved;

		// Token: 0x04002E9B RID: 11931
		[Tooltip("Event to send on stationary touch.")]
		public FsmEvent touchStationary;

		// Token: 0x04002E9C RID: 11932
		[Tooltip("Event to send on touch ended.")]
		public FsmEvent touchEnded;

		// Token: 0x04002E9D RID: 11933
		[Tooltip("Event to send on touch cancel.")]
		public FsmEvent touchCanceled;

		// Token: 0x04002E9E RID: 11934
		[Tooltip("Store the fingerId of the touch.")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Store Results")]
		public FsmInt storeFingerId;

		// Token: 0x04002E9F RID: 11935
		[Tooltip("Store the world position where the object was touched.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeHitPoint;

		// Token: 0x04002EA0 RID: 11936
		[Tooltip("Store the surface normal vector where the object was touched.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeHitNormal;
	}
}
