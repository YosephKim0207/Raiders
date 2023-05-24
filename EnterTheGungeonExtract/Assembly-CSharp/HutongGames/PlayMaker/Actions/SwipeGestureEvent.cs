using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B2E RID: 2862
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Sends an event when a swipe is detected.")]
	public class SwipeGestureEvent : FsmStateAction
	{
		// Token: 0x06003C3B RID: 15419 RVA: 0x0012F108 File Offset: 0x0012D308
		public override void Reset()
		{
			this.minSwipeDistance = 0.1f;
			this.swipeLeftEvent = null;
			this.swipeRightEvent = null;
			this.swipeUpEvent = null;
			this.swipeDownEvent = null;
		}

		// Token: 0x06003C3C RID: 15420 RVA: 0x0012F138 File Offset: 0x0012D338
		public override void OnEnter()
		{
			this.screenDiagonalSize = Mathf.Sqrt((float)(Screen.width * Screen.width + Screen.height * Screen.height));
			this.minSwipeDistancePixels = this.minSwipeDistance.Value * this.screenDiagonalSize;
		}

		// Token: 0x06003C3D RID: 15421 RVA: 0x0012F178 File Offset: 0x0012D378
		public override void OnUpdate()
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.touches[0];
				switch (touch.phase)
				{
				case TouchPhase.Began:
					this.touchStarted = true;
					this.touchStartPos = touch.position;
					break;
				case TouchPhase.Ended:
					if (this.touchStarted)
					{
						this.TestForSwipeGesture(touch);
						this.touchStarted = false;
					}
					break;
				case TouchPhase.Canceled:
					this.touchStarted = false;
					break;
				}
			}
		}

		// Token: 0x06003C3E RID: 15422 RVA: 0x0012F218 File Offset: 0x0012D418
		private void TestForSwipeGesture(Touch touch)
		{
			Vector2 position = touch.position;
			float num = Vector2.Distance(position, this.touchStartPos);
			if (num > this.minSwipeDistancePixels)
			{
				float num2 = position.y - this.touchStartPos.y;
				float num3 = position.x - this.touchStartPos.x;
				float num4 = 57.29578f * Mathf.Atan2(num3, num2);
				num4 = (360f + num4 - 45f) % 360f;
				Debug.Log(num4);
				if (num4 < 90f)
				{
					base.Fsm.Event(this.swipeRightEvent);
				}
				else if (num4 < 180f)
				{
					base.Fsm.Event(this.swipeDownEvent);
				}
				else if (num4 < 270f)
				{
					base.Fsm.Event(this.swipeLeftEvent);
				}
				else
				{
					base.Fsm.Event(this.swipeUpEvent);
				}
			}
		}

		// Token: 0x04002E69 RID: 11881
		[Tooltip("How far a touch has to travel to be considered a swipe. Uses normalized distance (e.g. 1 = 1 screen diagonal distance). Should generally be a very small number.")]
		public FsmFloat minSwipeDistance;

		// Token: 0x04002E6A RID: 11882
		[Tooltip("Event to send when swipe left detected.")]
		public FsmEvent swipeLeftEvent;

		// Token: 0x04002E6B RID: 11883
		[Tooltip("Event to send when swipe right detected.")]
		public FsmEvent swipeRightEvent;

		// Token: 0x04002E6C RID: 11884
		[Tooltip("Event to send when swipe up detected.")]
		public FsmEvent swipeUpEvent;

		// Token: 0x04002E6D RID: 11885
		[Tooltip("Event to send when swipe down detected.")]
		public FsmEvent swipeDownEvent;

		// Token: 0x04002E6E RID: 11886
		private float screenDiagonalSize;

		// Token: 0x04002E6F RID: 11887
		private float minSwipeDistancePixels;

		// Token: 0x04002E70 RID: 11888
		private bool touchStarted;

		// Token: 0x04002E71 RID: 11889
		private Vector2 touchStartPos;
	}
}
