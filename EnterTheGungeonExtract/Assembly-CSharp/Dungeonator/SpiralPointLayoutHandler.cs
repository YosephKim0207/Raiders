using System;
using System.Collections.Generic;

namespace Dungeonator
{
	// Token: 0x02000F36 RID: 3894
	public class SpiralPointLayoutHandler
	{
		// Token: 0x0600536E RID: 21358 RVA: 0x001E684C File Offset: 0x001E4A4C
		public SpiralPointLayoutHandler(SemioticLayoutManager c1, SemioticLayoutManager c2, int id)
		{
			this.canvas = c1;
			this.otherCanvas = c2;
			this.currentElementIndex = -1;
		}

		// Token: 0x0600536F RID: 21359 RVA: 0x001E6870 File Offset: 0x001E4A70
		public void ThreadRun()
		{
			while (SpiralPointLayoutHandler.currentResultElementIndex == -1)
			{
				object obj = SpiralPointLayoutHandler.spiralOffsets;
				lock (obj)
				{
					if (SpiralPointLayoutHandler.spiralOffsets.Count > 0)
					{
						this.otherCanvasOffset = SpiralPointLayoutHandler.spiralOffsets.Dequeue();
						this.currentElementIndex = SpiralPointLayoutHandler.nextElementIndex;
						SpiralPointLayoutHandler.nextElementIndex++;
					}
					else
					{
						this.currentElementIndex = -1;
					}
				}
				if (this.currentElementIndex < 0)
				{
					break;
				}
				this.CheckRectangleDecompositionCollisions();
			}
		}

		// Token: 0x06005370 RID: 21360 RVA: 0x001E6914 File Offset: 0x001E4B14
		public void CheckRectangleDecompositionCollisions()
		{
			bool flag = true;
			for (int i = 0; i < this.otherCanvas.RectangleDecomposition.Count; i++)
			{
				Tuple<IntVector2, IntVector2> tuple = this.otherCanvas.RectangleDecomposition[i];
				for (int j = 0; j < this.canvas.RectangleDecomposition.Count; j++)
				{
					Tuple<IntVector2, IntVector2> tuple2 = this.canvas.RectangleDecomposition[j];
					if (IntVector2.AABBOverlap(tuple.First + this.otherCanvasOffset, tuple.Second, tuple2.First, tuple2.Second))
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
			}
			if (flag)
			{
				object obj = SpiralPointLayoutHandler.spiralOffsets;
				lock (obj)
				{
					if (SpiralPointLayoutHandler.currentResultElementIndex == -1 || this.currentElementIndex < SpiralPointLayoutHandler.currentResultElementIndex)
					{
						SpiralPointLayoutHandler.spiralOffsets.Clear();
						SpiralPointLayoutHandler.currentResultElementIndex = this.currentElementIndex;
						SpiralPointLayoutHandler.resultOffset = this.otherCanvasOffset;
					}
				}
			}
		}

		// Token: 0x04004BE6 RID: 19430
		public static Queue<IntVector2> spiralOffsets;

		// Token: 0x04004BE7 RID: 19431
		public static int nextElementIndex;

		// Token: 0x04004BE8 RID: 19432
		public static IntVector2 resultOffset;

		// Token: 0x04004BE9 RID: 19433
		public static int currentResultElementIndex = -1;

		// Token: 0x04004BEA RID: 19434
		private SemioticLayoutManager canvas;

		// Token: 0x04004BEB RID: 19435
		private SemioticLayoutManager otherCanvas;

		// Token: 0x04004BEC RID: 19436
		private IntVector2 otherCanvasOffset;

		// Token: 0x04004BED RID: 19437
		private int currentElementIndex = -1;
	}
}
