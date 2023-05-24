using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000EC8 RID: 3784
public class DungeonChain : ScriptableObject
{
	// Token: 0x0600502F RID: 20527 RVA: 0x001C1F74 File Offset: 0x001C0174
	public IntVector2 GetMandatoryDifficultyRating()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.mandatoryIncludedRooms.Count; i++)
		{
			if (!(this.mandatoryIncludedRooms[i].prototypeRoom == null))
			{
				num += this.mandatoryIncludedRooms[i].prototypeRoom.MinDifficultyRating;
				num2 += this.mandatoryIncludedRooms[i].prototypeRoom.MaxDifficultyRating;
			}
		}
		return new IntVector2(num, num2);
	}

	// Token: 0x06005030 RID: 20528 RVA: 0x001C1FFC File Offset: 0x001C01FC
	public string EvolveChainToCompletion()
	{
		int num = BraveRandom.GenerationRandomRange(this.minChainLength, this.maxChainLength + 1);
		string text = this.initialChainPrototype;
		while (text.Length < num)
		{
			int length = text.Length;
			text = this.EvolveChain(text);
			if (text.Length >= num)
			{
				bool flag = true;
				while (flag)
				{
					flag = false;
					string text2 = text;
					text = this.ApplyMandatoryRule(text);
					if (text2 != text)
					{
						flag = true;
					}
				}
			}
			if (text.Length == length)
			{
				break;
			}
		}
		return text;
	}

	// Token: 0x06005031 RID: 20529 RVA: 0x001C208C File Offset: 0x001C028C
	private string ApplyMandatoryRule(string current)
	{
		List<ChainRule> list = new List<ChainRule>();
		for (int i = 0; i < this.chainRules.Count; i++)
		{
			ChainRule chainRule = this.chainRules[i];
			if (chainRule.mandatory)
			{
				if (current.Contains(chainRule.form))
				{
					list.Add(chainRule);
				}
			}
		}
		if (list.Count == 0)
		{
			return current;
		}
		ChainRule chainRule2 = this.SelectRuleByWeighting(list);
		MatchCollection matchCollection = Regex.Matches(current, chainRule2.form);
		Match match = matchCollection[BraveRandom.GenerationRandomRange(0, matchCollection.Count)];
		string text = ((match.Index == 0) ? string.Empty : current.Substring(0, match.Index));
		string text2 = ((match.Index == current.Length - 1) ? string.Empty : current.Substring(match.Index + chainRule2.form.Length));
		return text + chainRule2.target + text2;
	}

	// Token: 0x06005032 RID: 20530 RVA: 0x001C2198 File Offset: 0x001C0398
	public string EvolveChain(string current)
	{
		List<ChainRule> list = new List<ChainRule>();
		for (int i = 0; i < this.chainRules.Count; i++)
		{
			ChainRule chainRule = this.chainRules[i];
			if (current.Contains(chainRule.form))
			{
				list.Add(chainRule);
			}
		}
		if (list.Count == 0)
		{
			BraveUtility.Log("A DungeonChain has no associated rules. This works if no evolution is desired, but here's a warning just in case...", Color.yellow, BraveUtility.LogVerbosity.VERBOSE);
			return current;
		}
		ChainRule chainRule2 = this.SelectRuleByWeighting(list);
		MatchCollection matchCollection = Regex.Matches(current, chainRule2.form);
		Match match = matchCollection[BraveRandom.GenerationRandomRange(0, matchCollection.Count)];
		string text = ((match.Index == 0) ? string.Empty : current.Substring(0, match.Index));
		string text2 = ((match.Index == current.Length - 1) ? string.Empty : current.Substring(match.Index + chainRule2.form.Length));
		return text + chainRule2.target + text2;
	}

	// Token: 0x06005033 RID: 20531 RVA: 0x001C22A4 File Offset: 0x001C04A4
	private ChainRule SelectRuleByWeighting(List<ChainRule> source)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < source.Count; i++)
		{
			num2 += source[i].weight;
		}
		float num3 = BraveRandom.GenerationRandomValue() * num2;
		for (int j = 0; j < source.Count; j++)
		{
			num += source[j].weight;
			if (num >= num3)
			{
				return source[j];
			}
		}
		return null;
	}

	// Token: 0x04004861 RID: 18529
	public string initialChainPrototype = "n";

	// Token: 0x04004862 RID: 18530
	public List<ChainRule> chainRules;

	// Token: 0x04004863 RID: 18531
	public int minChainLength = 3;

	// Token: 0x04004864 RID: 18532
	public int maxChainLength = 8;

	// Token: 0x04004865 RID: 18533
	public List<ChainRoom> mandatoryIncludedRooms;

	// Token: 0x04004866 RID: 18534
	public List<ChainRoom> possiblyIncludedRooms;

	// Token: 0x04004867 RID: 18535
	public List<ChainRoom> capRooms;
}
