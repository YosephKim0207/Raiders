using System;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x02000579 RID: 1401
	public static class fiRectUtility
	{
		// Token: 0x06002107 RID: 8455 RVA: 0x00091A30 File Offset: 0x0008FC30
		public static Rect IndentedRect(Rect source)
		{
			return new Rect(source.x + 15f, source.y + 2f, source.width - 15f, source.height - 2f);
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x00091A6C File Offset: 0x0008FC6C
		public static Rect MoveDown(Rect rect, float amount)
		{
			rect.y += amount;
			rect.height -= amount;
			return rect;
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x00091A90 File Offset: 0x0008FC90
		public static void SplitLeftHorizontalExact(Rect rect, float leftWidth, float margin, out Rect left, out Rect right)
		{
			left = rect;
			right = rect;
			left.width = leftWidth;
			right.x += leftWidth + margin;
			right.width -= leftWidth + margin;
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x00091ACC File Offset: 0x0008FCCC
		public static void SplitRightHorizontalExact(Rect rect, float rightWidth, float margin, out Rect left, out Rect right)
		{
			left = new Rect(rect);
			left.width -= rightWidth + margin;
			right = new Rect(rect);
			right.x += left.width + margin;
			right.width = rightWidth;
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x00091B0C File Offset: 0x0008FD0C
		public static void SplitHorizontalPercentage(Rect rect, float percentage, float margin, out Rect left, out Rect right)
		{
			left = new Rect(rect);
			left.width *= percentage;
			right = new Rect(rect);
			right.x += left.width + margin;
			right.width -= left.width + margin;
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x00091B64 File Offset: 0x0008FD64
		public static void SplitHorizontalMiddleExact(Rect rect, float middleWidth, float margin, out Rect left, out Rect middle, out Rect right)
		{
			left = new Rect(rect);
			left.width = (rect.width - 2f * margin - middleWidth) / 2f;
			middle = new Rect(rect);
			middle.x += left.width + margin;
			middle.width = middleWidth;
			right = new Rect(rect);
			right.x += left.width + margin + middleWidth + margin;
			right.width = (rect.width - 2f * margin - middleWidth) / 2f;
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x00091BFC File Offset: 0x0008FDFC
		public static void SplitHorizontalFlexibleMiddle(Rect rect, float leftWidth, float rightWidth, out Rect left, out Rect middle, out Rect right)
		{
			left = new Rect(rect);
			left.width = leftWidth;
			middle = new Rect(rect);
			middle.x += left.width;
			middle.width = rect.width - leftWidth - rightWidth;
			right = new Rect(rect);
			right.x += left.width + middle.width;
			right.width = rightWidth;
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x00091C74 File Offset: 0x0008FE74
		public static void CenterRect(Rect toCenter, float height, out Rect centered)
		{
			float num = toCenter.height - height;
			centered = new Rect(toCenter);
			centered.y += num / 2f;
			centered.height = height;
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x00091CB0 File Offset: 0x0008FEB0
		public static void Margin(Rect container, float horizontalMargin, float verticalMargin, out Rect smaller)
		{
			smaller = container;
			smaller.x += horizontalMargin;
			smaller.width -= horizontalMargin * 2f;
			smaller.y += verticalMargin;
			smaller.height -= verticalMargin * 2f;
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x00091D08 File Offset: 0x0008FF08
		public static void SplitVerticalPercentage(Rect rect, float percentage, float margin, out Rect top, out Rect bottom)
		{
			top = new Rect(rect);
			top.height *= percentage;
			bottom = new Rect(rect);
			bottom.y += top.height + margin;
			bottom.height -= top.height + margin;
		}

		// Token: 0x040017DD RID: 6109
		public const float IndentHorizontal = 15f;

		// Token: 0x040017DE RID: 6110
		public const float IndentVertical = 2f;
	}
}
