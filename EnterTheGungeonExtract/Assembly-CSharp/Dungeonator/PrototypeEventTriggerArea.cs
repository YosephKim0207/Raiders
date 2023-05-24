using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000F5C RID: 3932
	[Serializable]
	public class PrototypeEventTriggerArea
	{
		// Token: 0x060054A9 RID: 21673 RVA: 0x001FCE78 File Offset: 0x001FB078
		public PrototypeEventTriggerArea()
		{
			this.triggerCells = new List<Vector2>();
		}

		// Token: 0x060054AA RID: 21674 RVA: 0x001FCE8C File Offset: 0x001FB08C
		public PrototypeEventTriggerArea(IEnumerable<Vector2> cells)
		{
			this.triggerCells = new List<Vector2>(cells);
		}

		// Token: 0x060054AB RID: 21675 RVA: 0x001FCEA0 File Offset: 0x001FB0A0
		public PrototypeEventTriggerArea(IEnumerable<IntVector2> cells)
		{
			this.triggerCells = new List<Vector2>();
			foreach (IntVector2 intVector in cells)
			{
				this.triggerCells.Add(intVector.ToVector2());
			}
		}

		// Token: 0x060054AC RID: 21676 RVA: 0x001FCF10 File Offset: 0x001FB110
		public PrototypeEventTriggerArea CreateMirror(IntVector2 roomDimensions)
		{
			PrototypeEventTriggerArea prototypeEventTriggerArea = new PrototypeEventTriggerArea();
			for (int i = 0; i < this.triggerCells.Count; i++)
			{
				Vector2 vector = this.triggerCells[i];
				vector.x = (float)roomDimensions.x - (vector.x + 1f);
				prototypeEventTriggerArea.triggerCells.Add(vector);
			}
			return prototypeEventTriggerArea;
		}

		// Token: 0x04004D9E RID: 19870
		[SerializeField]
		public List<Vector2> triggerCells;
	}
}
