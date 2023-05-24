using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B32 RID: 2866
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Sends events when a GUI Texture or GUI Text is touched. Optionally filter by a fingerID.")]
	public class TouchGUIEvent : FsmStateAction
	{
		// Token: 0x06003C46 RID: 15430 RVA: 0x0012F564 File Offset: 0x0012D764
		public override void Reset()
		{
			this.gameObject = null;
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
			this.normalizeHitPoint = false;
			this.storeOffset = null;
			this.relativeTo = TouchGUIEvent.OffsetOptions.Center;
			this.normalizeOffset = true;
			this.everyFrame = true;
		}

		// Token: 0x06003C47 RID: 15431 RVA: 0x0012F5EC File Offset: 0x0012D7EC
		public override void OnEnter()
		{
			this.DoTouchGUIEvent();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C48 RID: 15432 RVA: 0x0012F608 File Offset: 0x0012D808
		public override void OnUpdate()
		{
			this.DoTouchGUIEvent();
		}

		// Token: 0x06003C49 RID: 15433 RVA: 0x0012F610 File Offset: 0x0012D810
		private void DoTouchGUIEvent()
		{
			if (Input.touchCount > 0)
			{
				GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
				if (ownerDefaultTarget == null)
				{
					return;
				}
				this.guiElement = ownerDefaultTarget.GetComponent<GUITexture>() ?? ownerDefaultTarget.GetComponent<GUIText>();
				if (this.guiElement == null)
				{
					return;
				}
				foreach (Touch touch in Input.touches)
				{
					this.DoTouch(touch);
				}
			}
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x0012F6A0 File Offset: 0x0012D8A0
		private void DoTouch(Touch touch)
		{
			if (this.fingerId.IsNone || touch.fingerId == this.fingerId.Value)
			{
				Vector3 vector = touch.position;
				if (this.guiElement.HitTest(vector))
				{
					if (touch.phase == TouchPhase.Began)
					{
						this.touchStartPos = vector;
					}
					this.storeFingerId.Value = touch.fingerId;
					if (this.normalizeHitPoint.Value)
					{
						vector.x /= (float)Screen.width;
						vector.y /= (float)Screen.height;
					}
					this.storeHitPoint.Value = vector;
					this.DoTouchOffset(vector);
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
				else
				{
					base.Fsm.Event(this.notTouching);
				}
			}
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x0012F7FC File Offset: 0x0012D9FC
		private void DoTouchOffset(Vector3 touchPos)
		{
			if (this.storeOffset.IsNone)
			{
				return;
			}
			Rect screenRect = this.guiElement.GetScreenRect();
			Vector3 vector = default(Vector3);
			TouchGUIEvent.OffsetOptions offsetOptions = this.relativeTo;
			if (offsetOptions != TouchGUIEvent.OffsetOptions.TopLeft)
			{
				if (offsetOptions != TouchGUIEvent.OffsetOptions.Center)
				{
					if (offsetOptions == TouchGUIEvent.OffsetOptions.TouchStart)
					{
						vector = touchPos - this.touchStartPos;
					}
				}
				else
				{
					Vector3 vector2 = new Vector3(screenRect.x + screenRect.width * 0.5f, screenRect.y + screenRect.height * 0.5f, 0f);
					vector = touchPos - vector2;
				}
			}
			else
			{
				vector.x = touchPos.x - screenRect.x;
				vector.y = touchPos.y - screenRect.y;
			}
			if (this.normalizeOffset.Value)
			{
				vector.x /= screenRect.width;
				vector.y /= screenRect.height;
			}
			this.storeOffset.Value = vector;
		}

		// Token: 0x04002E81 RID: 11905
		[RequiredField]
		[CheckForComponent(typeof(GUIElement))]
		[Tooltip("The Game Object that owns the GUI Texture or GUI Text.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002E82 RID: 11906
		[Tooltip("Only detect touches that match this fingerID, or set to None.")]
		public FsmInt fingerId;

		// Token: 0x04002E83 RID: 11907
		[Tooltip("Event to send on touch began.")]
		[ActionSection("Events")]
		public FsmEvent touchBegan;

		// Token: 0x04002E84 RID: 11908
		[Tooltip("Event to send on touch moved.")]
		public FsmEvent touchMoved;

		// Token: 0x04002E85 RID: 11909
		[Tooltip("Event to send on stationary touch.")]
		public FsmEvent touchStationary;

		// Token: 0x04002E86 RID: 11910
		[Tooltip("Event to send on touch ended.")]
		public FsmEvent touchEnded;

		// Token: 0x04002E87 RID: 11911
		[Tooltip("Event to send on touch cancel.")]
		public FsmEvent touchCanceled;

		// Token: 0x04002E88 RID: 11912
		[Tooltip("Event to send if not touching (finger down but not over the GUI element)")]
		public FsmEvent notTouching;

		// Token: 0x04002E89 RID: 11913
		[ActionSection("Store Results")]
		[Tooltip("Store the fingerId of the touch.")]
		[UIHint(UIHint.Variable)]
		public FsmInt storeFingerId;

		// Token: 0x04002E8A RID: 11914
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the screen position where the GUI element was touched.")]
		public FsmVector3 storeHitPoint;

		// Token: 0x04002E8B RID: 11915
		[Tooltip("Normalize the hit point screen coordinates (0-1).")]
		public FsmBool normalizeHitPoint;

		// Token: 0x04002E8C RID: 11916
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the offset position of the hit.")]
		public FsmVector3 storeOffset;

		// Token: 0x04002E8D RID: 11917
		[Tooltip("How to measure the offset.")]
		public TouchGUIEvent.OffsetOptions relativeTo;

		// Token: 0x04002E8E RID: 11918
		[Tooltip("Normalize the offset.")]
		public FsmBool normalizeOffset;

		// Token: 0x04002E8F RID: 11919
		[Tooltip("Repeate every frame.")]
		[ActionSection("")]
		public bool everyFrame;

		// Token: 0x04002E90 RID: 11920
		private Vector3 touchStartPos;

		// Token: 0x04002E91 RID: 11921
		private GUIElement guiElement;

		// Token: 0x02000B33 RID: 2867
		public enum OffsetOptions
		{
			// Token: 0x04002E93 RID: 11923
			TopLeft,
			// Token: 0x04002E94 RID: 11924
			Center,
			// Token: 0x04002E95 RID: 11925
			TouchStart
		}
	}
}
