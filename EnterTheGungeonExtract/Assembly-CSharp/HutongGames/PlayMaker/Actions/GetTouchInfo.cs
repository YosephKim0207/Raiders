using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009B9 RID: 2489
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Gets info on a touch event.")]
	public class GetTouchInfo : FsmStateAction
	{
		// Token: 0x060035DD RID: 13789 RVA: 0x00114404 File Offset: 0x00112604
		public override void Reset()
		{
			this.fingerId = new FsmInt
			{
				UseVariable = true
			};
			this.normalize = true;
			this.storePosition = null;
			this.storeDeltaPosition = null;
			this.storeDeltaTime = null;
			this.storeTapCount = null;
			this.everyFrame = true;
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x00114454 File Offset: 0x00112654
		public override void OnEnter()
		{
			this.screenWidth = (float)Screen.width;
			this.screenHeight = (float)Screen.height;
			this.DoGetTouchInfo();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x00114488 File Offset: 0x00112688
		public override void OnUpdate()
		{
			this.DoGetTouchInfo();
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x00114490 File Offset: 0x00112690
		private void DoGetTouchInfo()
		{
			if (Input.touchCount > 0)
			{
				foreach (Touch touch in Input.touches)
				{
					if (this.fingerId.IsNone || touch.fingerId == this.fingerId.Value)
					{
						float num = (this.normalize.Value ? (touch.position.x / this.screenWidth) : touch.position.x);
						float num2 = (this.normalize.Value ? (touch.position.y / this.screenHeight) : touch.position.y);
						if (!this.storePosition.IsNone)
						{
							this.storePosition.Value = new Vector3(num, num2, 0f);
						}
						this.storeX.Value = num;
						this.storeY.Value = num2;
						float num3 = (this.normalize.Value ? (touch.deltaPosition.x / this.screenWidth) : touch.deltaPosition.x);
						float num4 = (this.normalize.Value ? (touch.deltaPosition.y / this.screenHeight) : touch.deltaPosition.y);
						if (!this.storeDeltaPosition.IsNone)
						{
							this.storeDeltaPosition.Value = new Vector3(num3, num4, 0f);
						}
						this.storeDeltaX.Value = num3;
						this.storeDeltaY.Value = num4;
						this.storeDeltaTime.Value = touch.deltaTime;
						this.storeTapCount.Value = touch.tapCount;
					}
				}
			}
		}

		// Token: 0x04002723 RID: 10019
		[Tooltip("Filter by a Finger ID. You can store a Finger ID in other Touch actions, e.g., Touch Event.")]
		public FsmInt fingerId;

		// Token: 0x04002724 RID: 10020
		[Tooltip("If true, all screen coordinates are returned normalized (0-1), otherwise in pixels.")]
		public FsmBool normalize;

		// Token: 0x04002725 RID: 10021
		[UIHint(UIHint.Variable)]
		public FsmVector3 storePosition;

		// Token: 0x04002726 RID: 10022
		[UIHint(UIHint.Variable)]
		public FsmFloat storeX;

		// Token: 0x04002727 RID: 10023
		[UIHint(UIHint.Variable)]
		public FsmFloat storeY;

		// Token: 0x04002728 RID: 10024
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeDeltaPosition;

		// Token: 0x04002729 RID: 10025
		[UIHint(UIHint.Variable)]
		public FsmFloat storeDeltaX;

		// Token: 0x0400272A RID: 10026
		[UIHint(UIHint.Variable)]
		public FsmFloat storeDeltaY;

		// Token: 0x0400272B RID: 10027
		[UIHint(UIHint.Variable)]
		public FsmFloat storeDeltaTime;

		// Token: 0x0400272C RID: 10028
		[UIHint(UIHint.Variable)]
		public FsmInt storeTapCount;

		// Token: 0x0400272D RID: 10029
		public bool everyFrame = true;

		// Token: 0x0400272E RID: 10030
		private float screenWidth;

		// Token: 0x0400272F RID: 10031
		private float screenHeight;
	}
}
