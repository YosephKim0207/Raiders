using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200154E RID: 5454
public class GenericLootTable : ScriptableObject
{
	// Token: 0x06007CE1 RID: 31969 RVA: 0x00325C18 File Offset: 0x00323E18
	public bool RawContains(GameObject g)
	{
		for (int i = 0; i < this.defaultItemDrops.elements.Count; i++)
		{
			if (this.defaultItemDrops.elements[i].gameObject == g)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06007CE2 RID: 31970 RVA: 0x00325C6C File Offset: 0x00323E6C
	public GameObject SelectByWeight(bool useSeedRandom = false)
	{
		return this.GetCompiledCollection(true).SelectByWeight();
	}

	// Token: 0x06007CE3 RID: 31971 RVA: 0x00325C7C File Offset: 0x00323E7C
	public GameObject SelectByWeightWithoutDuplicates(List<GameObject> extant, bool useSeedRandom = false)
	{
		return this.GetCompiledCollection(true).SelectByWeightWithoutDuplicates(extant, useSeedRandom);
	}

	// Token: 0x06007CE4 RID: 31972 RVA: 0x00325C8C File Offset: 0x00323E8C
	public GameObject SelectByWeightWithoutDuplicatesFullPrereqs(List<GameObject> extant, bool allowSpice = true, bool useSeedRandom = false)
	{
		return this.GetCompiledCollection(allowSpice).SelectByWeightWithoutDuplicatesFullPrereqs(extant, null, useSeedRandom);
	}

	// Token: 0x06007CE5 RID: 31973 RVA: 0x00325CA0 File Offset: 0x00323EA0
	public GameObject SubshopSelectByWeightWithoutDuplicatesFullPrereqs(List<GameObject> extant, Func<GameObject, float, float> weightModifier, int minElements, bool useSeedRandom = false)
	{
		return this.GetCompiledCollection(true).SubshopStyleSelectByWeightWithoutDuplicatesFullPrereqs(extant, weightModifier, minElements, useSeedRandom);
	}

	// Token: 0x06007CE6 RID: 31974 RVA: 0x00325CB4 File Offset: 0x00323EB4
	public GameObject SelectByWeightWithoutDuplicatesFullPrereqs(List<GameObject> extant, Func<GameObject, float, float> weightModifier, bool useSeedRandom = false)
	{
		return this.GetCompiledCollection(true).SelectByWeightWithoutDuplicatesFullPrereqs(extant, weightModifier, useSeedRandom);
	}

	// Token: 0x06007CE7 RID: 31975 RVA: 0x00325CC8 File Offset: 0x00323EC8
	public List<WeightedGameObject> GetCompiledRawItems()
	{
		WeightedGameObjectCollection compiledCollection = this.GetCompiledCollection(true);
		return compiledCollection.elements;
	}

	// Token: 0x06007CE8 RID: 31976 RVA: 0x00325CE4 File Offset: 0x00323EE4
	protected WeightedGameObjectCollection GetCompiledCollection(bool allowSpice = true)
	{
		int num = 0;
		if (allowSpice && Application.isPlaying && GameManager.Instance.PrimaryPlayer != null)
		{
			num = GameManager.Instance.PrimaryPlayer.spiceCount;
			if (GameManager.Instance.SecondaryPlayer != null)
			{
				num += GameManager.Instance.SecondaryPlayer.spiceCount;
			}
		}
		if (this.includedLootTables.Count == 0 && num == 0)
		{
			return this.defaultItemDrops;
		}
		WeightedGameObjectCollection weightedGameObjectCollection = new WeightedGameObjectCollection();
		for (int i = 0; i < this.defaultItemDrops.elements.Count; i++)
		{
			weightedGameObjectCollection.Add(this.defaultItemDrops.elements[i]);
		}
		int j = 0;
		while (j < this.includedLootTables.Count)
		{
			if (this.includedLootTables[j].tablePrerequisites.Length <= 0)
			{
				goto IL_136;
			}
			bool flag = false;
			for (int k = 0; k < this.includedLootTables[j].tablePrerequisites.Length; k++)
			{
				if (!this.includedLootTables[j].tablePrerequisites[k].CheckConditionsFulfilled())
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				goto IL_136;
			}
			IL_17F:
			j++;
			continue;
			IL_136:
			WeightedGameObjectCollection compiledCollection = this.includedLootTables[j].GetCompiledCollection(true);
			for (int l = 0; l < compiledCollection.elements.Count; l++)
			{
				weightedGameObjectCollection.Add(compiledCollection.elements[l]);
			}
			goto IL_17F;
		}
		if (allowSpice && num > 0)
		{
			float totalWeight = weightedGameObjectCollection.GetTotalWeight();
			float num2 = SpiceItem.GetSpiceWeight(num) * totalWeight;
			GameObject gameObject = PickupObjectDatabase.GetById(GlobalItemIds.Spice).gameObject;
			WeightedGameObject weightedGameObject = new WeightedGameObject();
			weightedGameObject.SetGameObject(gameObject);
			weightedGameObject.weight = num2;
			weightedGameObject.additionalPrerequisites = new DungeonPrerequisite[0];
			weightedGameObjectCollection.Add(weightedGameObject);
		}
		return weightedGameObjectCollection;
	}

	// Token: 0x04007FEE RID: 32750
	public WeightedGameObjectCollection defaultItemDrops;

	// Token: 0x04007FEF RID: 32751
	public List<GenericLootTable> includedLootTables;

	// Token: 0x04007FF0 RID: 32752
	public DungeonPrerequisite[] tablePrerequisites;

	// Token: 0x04007FF1 RID: 32753
	private WeightedGameObjectCollection m_compiledCollection;
}
