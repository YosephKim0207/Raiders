using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E9B RID: 3739
public class GenericRoomTable : ScriptableObject
{
	// Token: 0x06004F47 RID: 20295 RVA: 0x001B7C80 File Offset: 0x001B5E80
	public WeightedRoom SelectByWeight()
	{
		return this.GetCompiledCollection().SelectByWeight();
	}

	// Token: 0x06004F48 RID: 20296 RVA: 0x001B7C90 File Offset: 0x001B5E90
	public WeightedRoom SelectByWeightWithoutDuplicates(List<PrototypeDungeonRoom> extant)
	{
		return this.GetCompiledCollection().SelectByWeightWithoutDuplicates(extant);
	}

	// Token: 0x06004F49 RID: 20297 RVA: 0x001B7CA0 File Offset: 0x001B5EA0
	public List<WeightedRoom> GetCompiledList()
	{
		if (this.m_compiledList != null)
		{
			return this.m_compiledList;
		}
		List<WeightedRoom> list = new List<WeightedRoom>();
		for (int i = 0; i < this.includedRooms.elements.Count; i++)
		{
			list.Add(this.includedRooms.elements[i]);
		}
		for (int j = 0; j < this.includedRoomTables.Count; j++)
		{
			WeightedRoomCollection compiledCollection = this.includedRoomTables[j].GetCompiledCollection();
			for (int k = 0; k < compiledCollection.elements.Count; k++)
			{
				list.Add(compiledCollection.elements[k]);
			}
		}
		if (Application.isPlaying)
		{
			this.m_compiledList = list;
		}
		return list;
	}

	// Token: 0x06004F4A RID: 20298 RVA: 0x001B7D70 File Offset: 0x001B5F70
	protected WeightedRoomCollection GetCompiledCollection()
	{
		WeightedRoomCollection weightedRoomCollection = new WeightedRoomCollection();
		for (int i = 0; i < this.includedRooms.elements.Count; i++)
		{
			weightedRoomCollection.Add(this.includedRooms.elements[i]);
		}
		for (int j = 0; j < this.includedRoomTables.Count; j++)
		{
			WeightedRoomCollection compiledCollection = this.includedRoomTables[j].GetCompiledCollection();
			for (int k = 0; k < compiledCollection.elements.Count; k++)
			{
				weightedRoomCollection.Add(compiledCollection.elements[k]);
			}
		}
		this.m_compiledCollection = weightedRoomCollection;
		return weightedRoomCollection;
	}

	// Token: 0x04004687 RID: 18055
	public WeightedRoomCollection includedRooms;

	// Token: 0x04004688 RID: 18056
	public List<GenericRoomTable> includedRoomTables;

	// Token: 0x04004689 RID: 18057
	[NonSerialized]
	protected List<WeightedRoom> m_compiledList;

	// Token: 0x0400468A RID: 18058
	[NonSerialized]
	protected WeightedRoomCollection m_compiledCollection;
}
