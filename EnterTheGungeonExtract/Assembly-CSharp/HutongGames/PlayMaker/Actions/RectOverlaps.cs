using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AA6 RID: 2726
	[Tooltip("Tests if 2 Rects overlap.")]
	[ActionCategory(ActionCategory.Rect)]
	public class RectOverlaps : FsmStateAction
	{
		// Token: 0x060039D6 RID: 14806 RVA: 0x001271A4 File Offset: 0x001253A4
		public override void Reset()
		{
			this.rect1 = new FsmRect
			{
				UseVariable = true
			};
			this.rect2 = new FsmRect
			{
				UseVariable = true
			};
			this.storeResult = null;
			this.trueEvent = null;
			this.falseEvent = null;
			this.everyFrame = false;
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x001271F8 File Offset: 0x001253F8
		public override void OnEnter()
		{
			this.DoRectOverlap();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060039D8 RID: 14808 RVA: 0x00127214 File Offset: 0x00125414
		public override void OnUpdate()
		{
			this.DoRectOverlap();
		}

		// Token: 0x060039D9 RID: 14809 RVA: 0x0012721C File Offset: 0x0012541C
		private void DoRectOverlap()
		{
			if (this.rect1.IsNone || this.rect2.IsNone)
			{
				return;
			}
			bool flag = RectOverlaps.Intersect(this.rect1.Value, this.rect2.Value);
			this.storeResult.Value = flag;
			base.Fsm.Event((!flag) ? this.falseEvent : this.trueEvent);
		}

		// Token: 0x060039DA RID: 14810 RVA: 0x00127294 File Offset: 0x00125494
		public static bool Intersect(Rect a, Rect b)
		{
			RectOverlaps.FlipNegative(ref a);
			RectOverlaps.FlipNegative(ref b);
			bool flag = a.xMin < b.xMax;
			bool flag2 = a.xMax > b.xMin;
			bool flag3 = a.yMin < b.yMax;
			bool flag4 = a.yMax > b.yMin;
			return flag && flag2 && flag3 && flag4;
		}

		// Token: 0x060039DB RID: 14811 RVA: 0x0012730C File Offset: 0x0012550C
		public static void FlipNegative(ref Rect r)
		{
			if (r.width < 0f)
			{
				r.x -= (r.width *= -1f);
			}
			if (r.height < 0f)
			{
				r.y -= (r.height *= -1f);
			}
		}

		// Token: 0x04002C1C RID: 11292
		[RequiredField]
		[Tooltip("First Rectangle.")]
		public FsmRect rect1;

		// Token: 0x04002C1D RID: 11293
		[RequiredField]
		[Tooltip("Second Rectangle.")]
		public FsmRect rect2;

		// Token: 0x04002C1E RID: 11294
		[Tooltip("Event to send if the Rects overlap.")]
		public FsmEvent trueEvent;

		// Token: 0x04002C1F RID: 11295
		[Tooltip("Event to send if the Rects do not overlap.")]
		public FsmEvent falseEvent;

		// Token: 0x04002C20 RID: 11296
		[Tooltip("Store the result in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		// Token: 0x04002C21 RID: 11297
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
