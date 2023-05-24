using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001850 RID: 6224
[Serializable]
public class WeightedGameObjectCollection
{
	// Token: 0x06009332 RID: 37682 RVA: 0x003E20D0 File Offset: 0x003E02D0
	public WeightedGameObjectCollection()
	{
		this.elements = new List<WeightedGameObject>();
	}

	// Token: 0x06009333 RID: 37683 RVA: 0x003E20E4 File Offset: 0x003E02E4
	public void Add(WeightedGameObject w)
	{
		this.elements.Add(w);
	}

	// Token: 0x06009334 RID: 37684 RVA: 0x003E20F4 File Offset: 0x003E02F4
	public float GetTotalWeight()
	{
		float num = 0f;
		for (int i = 0; i < this.elements.Count; i++)
		{
			WeightedGameObject weightedGameObject = this.elements[i];
			bool flag = true;
			for (int j = 0; j < weightedGameObject.additionalPrerequisites.Length; j++)
			{
				if (!weightedGameObject.additionalPrerequisites[j].CheckConditionsFulfilled())
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				num += weightedGameObject.weight;
			}
		}
		return num;
	}

	// Token: 0x06009335 RID: 37685 RVA: 0x003E217C File Offset: 0x003E037C
	public GameObject SelectByWeight()
	{
		int num = -1;
		return this.SelectByWeight(out num, false);
	}

	// Token: 0x06009336 RID: 37686 RVA: 0x003E2194 File Offset: 0x003E0394
	public GameObject SelectByWeight(out int outIndex, bool useSeedRandom = false)
	{
		outIndex = -1;
		List<WeightedGameObject> list = new List<WeightedGameObject>();
		float num = 0f;
		for (int i = 0; i < this.elements.Count; i++)
		{
			WeightedGameObject weightedGameObject = this.elements[i];
			bool flag = true;
			if (weightedGameObject.additionalPrerequisites != null)
			{
				for (int j = 0; j < weightedGameObject.additionalPrerequisites.Length; j++)
				{
					if (!weightedGameObject.additionalPrerequisites[j].CheckConditionsFulfilled())
					{
						flag = false;
						break;
					}
				}
			}
			if (weightedGameObject.gameObject != null)
			{
				PickupObject component = weightedGameObject.gameObject.GetComponent<PickupObject>();
				if (component != null && !component.PrerequisitesMet())
				{
					flag = false;
				}
			}
			if (flag)
			{
				list.Add(weightedGameObject);
				num += weightedGameObject.weight;
			}
		}
		float num2 = ((!useSeedRandom) ? UnityEngine.Random.value : BraveRandom.GenerationRandomValue()) * num;
		float num3 = 0f;
		for (int k = 0; k < list.Count; k++)
		{
			num3 += list[k].weight;
			if (num3 > num2)
			{
				outIndex = this.elements.IndexOf(list[k]);
				return list[k].gameObject;
			}
		}
		outIndex = this.elements.IndexOf(list[list.Count - 1]);
		return list[list.Count - 1].gameObject;
	}

	// Token: 0x06009337 RID: 37687 RVA: 0x003E2318 File Offset: 0x003E0518
	public GameObject SubshopStyleSelectByWeightWithoutDuplicatesFullPrereqs(List<GameObject> extant, Func<GameObject, float, float> weightModifier, int minElements, bool useSeedRandom = false)
	{
		List<WeightedGameObject> list = new List<WeightedGameObject>();
		float num = 0f;
		bool flag = useSeedRandom;
		int num2 = 0;
		while (num2 < 2 && list.Count < minElements)
		{
			num2++;
			for (int i = 0; i < this.elements.Count; i++)
			{
				WeightedGameObject weightedGameObject = this.elements[i];
				if (!(weightedGameObject.gameObject == null))
				{
					if (extant == null || !extant.Contains(weightedGameObject.gameObject) || weightedGameObject.forceDuplicatesPossible)
					{
						PickupObject component = weightedGameObject.gameObject.GetComponent<PickupObject>();
						bool flag2 = true;
						if (component.quality == PickupObject.ItemQuality.SPECIAL)
						{
							flag2 = false;
							if (component is AncientPrimerItem || component is ArcaneGunpowderItem || component is AstralSlugItem || component is ObsidianShellItem)
							{
								flag2 = true;
							}
						}
						if (!(component != null) || (component.PrerequisitesMet() && flag2))
						{
							EncounterTrackable component2 = weightedGameObject.gameObject.GetComponent<EncounterTrackable>();
							if (!(component2 != null) || flag || GameStatsManager.Instance.QueryEncounterableDifferentiator(component2) <= 0 || weightedGameObject.forceDuplicatesPossible)
							{
								bool flag3 = true;
								for (int j = 0; j < weightedGameObject.additionalPrerequisites.Length; j++)
								{
									if (!weightedGameObject.additionalPrerequisites[j].CheckConditionsFulfilled())
									{
										flag3 = false;
										break;
									}
								}
								if (flag3)
								{
									float num3 = ((weightModifier == null) ? weightedGameObject.weight : weightModifier(weightedGameObject.gameObject, weightedGameObject.weight));
									list.Add(weightedGameObject);
									num += num3;
								}
							}
						}
					}
				}
			}
			flag = true;
		}
		float num4 = ((!useSeedRandom) ? UnityEngine.Random.value : BraveRandom.GenerationRandomValue()) * num;
		float num5 = 0f;
		for (int k = 0; k < list.Count; k++)
		{
			float num6 = ((weightModifier == null) ? list[k].weight : weightModifier(list[k].gameObject, list[k].weight));
			num5 += num6;
			if (num5 > num4)
			{
				return list[k].gameObject;
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list[list.Count - 1].gameObject;
	}

	// Token: 0x06009338 RID: 37688 RVA: 0x003E25B0 File Offset: 0x003E07B0
	public GameObject SelectByWeightWithoutDuplicatesFullPrereqs(List<GameObject> extant, Func<GameObject, float, float> weightModifier, bool useSeedRandom = false)
	{
		List<WeightedGameObject> list = new List<WeightedGameObject>();
		float num = 0f;
		for (int i = 0; i < this.elements.Count; i++)
		{
			WeightedGameObject weightedGameObject = this.elements[i];
			if (weightedGameObject.gameObject == null)
			{
				list.Add(weightedGameObject);
				num += weightedGameObject.weight;
			}
			else if (extant == null || !extant.Contains(weightedGameObject.gameObject) || weightedGameObject.forceDuplicatesPossible)
			{
				PickupObject component = weightedGameObject.gameObject.GetComponent<PickupObject>();
				bool flag = true;
				if (component.quality == PickupObject.ItemQuality.SPECIAL)
				{
					flag = false;
					bool flag2 = component is SpecialKeyItem && (component as SpecialKeyItem).keyType == SpecialKeyItem.SpecialKeyType.RESOURCEFUL_RAT_LAIR;
					if (component is AncientPrimerItem || component is ArcaneGunpowderItem || component is AstralSlugItem || component is ObsidianShellItem || flag2)
					{
						flag = true;
					}
				}
				if (!(component != null) || (component.PrerequisitesMet() && flag))
				{
					EncounterTrackable component2 = weightedGameObject.gameObject.GetComponent<EncounterTrackable>();
					if (useSeedRandom || !(component2 != null) || GameStatsManager.Instance.QueryEncounterableDifferentiator(component2) <= 0 || weightedGameObject.forceDuplicatesPossible)
					{
						bool flag3 = true;
						for (int j = 0; j < weightedGameObject.additionalPrerequisites.Length; j++)
						{
							if (!weightedGameObject.additionalPrerequisites[j].CheckConditionsFulfilled())
							{
								flag3 = false;
								break;
							}
						}
						if (flag3)
						{
							float num2 = ((weightModifier == null) ? weightedGameObject.weight : weightModifier(weightedGameObject.gameObject, weightedGameObject.weight));
							list.Add(weightedGameObject);
							num += num2;
						}
					}
				}
			}
		}
		float num3 = ((!useSeedRandom) ? UnityEngine.Random.value : BraveRandom.GenerationRandomValue()) * num;
		float num4 = 0f;
		for (int k = 0; k < list.Count; k++)
		{
			float num5 = ((weightModifier == null) ? list[k].weight : weightModifier(list[k].gameObject, list[k].weight));
			num4 += num5;
			if (num4 > num3)
			{
				return list[k].gameObject;
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list[list.Count - 1].gameObject;
	}

	// Token: 0x06009339 RID: 37689 RVA: 0x003E2848 File Offset: 0x003E0A48
	public GameObject SelectByWeightWithoutDuplicates(List<GameObject> extant, bool useSeedRandom = false)
	{
		if (extant.Count == this.elements.Count)
		{
			return null;
		}
		List<WeightedGameObject> list = new List<WeightedGameObject>();
		float num = 0f;
		for (int i = 0; i < this.elements.Count; i++)
		{
			WeightedGameObject weightedGameObject = this.elements[i];
			if (!extant.Contains(weightedGameObject.gameObject))
			{
				bool flag = true;
				for (int j = 0; j < weightedGameObject.additionalPrerequisites.Length; j++)
				{
					if (!weightedGameObject.additionalPrerequisites[j].CheckConditionsFulfilled())
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(weightedGameObject);
					num += weightedGameObject.weight;
				}
			}
		}
		float num2 = ((!useSeedRandom) ? UnityEngine.Random.value : BraveRandom.GenerationRandomValue()) * num;
		float num3 = 0f;
		for (int k = 0; k < list.Count; k++)
		{
			num3 += list[k].weight;
			if (num3 > num2)
			{
				return list[k].gameObject;
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list[list.Count - 1].gameObject;
	}

	// Token: 0x04009ABF RID: 39615
	public List<WeightedGameObject> elements;
}
