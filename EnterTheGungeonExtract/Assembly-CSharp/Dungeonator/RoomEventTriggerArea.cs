using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000F12 RID: 3858
	public class RoomEventTriggerArea
	{
		// Token: 0x06005258 RID: 21080 RVA: 0x001D929C File Offset: 0x001D749C
		public RoomEventTriggerArea()
		{
			this.triggerCells = new HashSet<IntVector2>();
			this.events = new List<IEventTriggerable>();
		}

		// Token: 0x06005259 RID: 21081 RVA: 0x001D92BC File Offset: 0x001D74BC
		public RoomEventTriggerArea(PrototypeEventTriggerArea prototype, IntVector2 basePosition)
		{
			this.triggerCells = new HashSet<IntVector2>();
			this.events = new List<IEventTriggerable>();
			for (int i = 0; i < prototype.triggerCells.Count; i++)
			{
				IntVector2 intVector = prototype.triggerCells[i].ToIntVector2(VectorConversions.Round) + basePosition;
				CellData cellData = GameManager.Instance.Dungeon.data[intVector];
				cellData.cellVisualData.containsObjectSpaceStamp = true;
				this.triggerCells.Add(intVector);
				if (i == 0)
				{
					this.initialPosition = intVector;
				}
			}
		}

		// Token: 0x0600525A RID: 21082 RVA: 0x001D9358 File Offset: 0x001D7558
		public void Trigger(int eventIndex)
		{
			for (int i = 0; i < this.events.Count; i++)
			{
				this.events[i].Trigger(eventIndex);
			}
		}

		// Token: 0x0600525B RID: 21083 RVA: 0x001D9394 File Offset: 0x001D7594
		public void AddGameObject(GameObject g)
		{
			IEventTriggerable eventTriggerable = g.GetComponentInChildren(typeof(IEventTriggerable)) as IEventTriggerable;
			if (eventTriggerable != null)
			{
				this.events.Add(eventTriggerable);
				if (eventTriggerable is HangingObjectController)
				{
					for (int i = 0; i < 2; i++)
					{
						for (int j = 0; j < 3; j++)
						{
							IntVector2 intVector = this.initialPosition + new IntVector2(i, j);
							GameManager.Instance.Dungeon.data[intVector].cellVisualData.containsWallSpaceStamp = true;
							GameManager.Instance.Dungeon.data[intVector].cellVisualData.containsObjectSpaceStamp = true;
						}
					}
				}
			}
		}

		// Token: 0x04004ADF RID: 19167
		public HashSet<IntVector2> triggerCells;

		// Token: 0x04004AE0 RID: 19168
		public IntVector2 initialPosition;

		// Token: 0x04004AE1 RID: 19169
		public List<IEventTriggerable> events;

		// Token: 0x04004AE2 RID: 19170
		[NonSerialized]
		public GameObject tempDataObject;
	}
}
