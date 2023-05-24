using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000E6D RID: 3693
[Serializable]
public class DungeonFlowNode
{
	// Token: 0x06004EA1 RID: 20129 RVA: 0x001B3128 File Offset: 0x001B1328
	public DungeonFlowNode(DungeonFlow parentFlow)
	{
		this.flow = parentFlow;
		this.childNodeGuids = new List<string>();
		this.guidAsString = Guid.NewGuid().ToString();
	}

	// Token: 0x17000B1D RID: 2845
	// (get) Token: 0x06004EA2 RID: 20130 RVA: 0x001B31A0 File Offset: 0x001B13A0
	public bool UsesGlobalBossData
	{
		get
		{
			return GameManager.Instance.CurrentGameMode != GameManager.GameMode.BOSSRUSH && GameManager.Instance.CurrentGameMode != GameManager.GameMode.SUPERBOSSRUSH && this.overrideExactRoom == null && this.roomCategory == PrototypeDungeonRoom.RoomCategory.BOSS;
		}
	}

	// Token: 0x06004EA3 RID: 20131 RVA: 0x001B31EC File Offset: 0x001B13EC
	public static bool operator ==(DungeonFlowNode a, DungeonFlowNode b)
	{
		return object.ReferenceEquals(a, b) || (a != null && b != null && a.guidAsString == b.guidAsString);
	}

	// Token: 0x06004EA4 RID: 20132 RVA: 0x001B321C File Offset: 0x001B141C
	public static bool operator !=(DungeonFlowNode a, DungeonFlowNode b)
	{
		return !(a == b);
	}

	// Token: 0x06004EA5 RID: 20133 RVA: 0x001B3228 File Offset: 0x001B1428
	protected bool Equals(DungeonFlowNode other)
	{
		return string.Equals(this.guidAsString, other.guidAsString);
	}

	// Token: 0x06004EA6 RID: 20134 RVA: 0x001B323C File Offset: 0x001B143C
	public override bool Equals(object obj)
	{
		return !object.ReferenceEquals(null, obj) && (object.ReferenceEquals(this, obj) || (obj.GetType() == base.GetType() && this.Equals((DungeonFlowNode)obj)));
	}

	// Token: 0x06004EA7 RID: 20135 RVA: 0x001B327C File Offset: 0x001B147C
	public override int GetHashCode()
	{
		return (this.guidAsString == null) ? 0 : this.guidAsString.GetHashCode();
	}

	// Token: 0x06004EA8 RID: 20136 RVA: 0x001B329C File Offset: 0x001B149C
	public int GetAverageNumberNodes()
	{
		if (this.nodeExpands)
		{
			return Mathf.Max(Mathf.FloorToInt((float)(this.minChainLength + this.maxChainLength) / 2f), 1);
		}
		if (this.nodeType == DungeonFlowNode.ControlNodeType.SELECTOR)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x06004EA9 RID: 20137 RVA: 0x001B32D8 File Offset: 0x001B14D8
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

	// Token: 0x06004EAA RID: 20138 RVA: 0x001B3368 File Offset: 0x001B1568
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

	// Token: 0x06004EAB RID: 20139 RVA: 0x001B3474 File Offset: 0x001B1674
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

	// Token: 0x06004EAC RID: 20140 RVA: 0x001B3580 File Offset: 0x001B1780
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

	// Token: 0x040044FD RID: 17661
	public bool isSubchainStandin;

	// Token: 0x040044FE RID: 17662
	public DungeonFlowNode.ControlNodeType nodeType;

	// Token: 0x040044FF RID: 17663
	public PrototypeDungeonRoom.RoomCategory roomCategory;

	// Token: 0x04004500 RID: 17664
	public float percentChance = 1f;

	// Token: 0x04004501 RID: 17665
	public DungeonFlowNode.NodePriority priority;

	// Token: 0x04004502 RID: 17666
	public PrototypeDungeonRoom overrideExactRoom;

	// Token: 0x04004503 RID: 17667
	public GenericRoomTable overrideRoomTable;

	// Token: 0x04004504 RID: 17668
	public bool capSubchain;

	// Token: 0x04004505 RID: 17669
	public string subchainIdentifier;

	// Token: 0x04004506 RID: 17670
	public bool limitedCopiesOfSubchain;

	// Token: 0x04004507 RID: 17671
	public int maxCopiesOfSubchain = 1;

	// Token: 0x04004508 RID: 17672
	public List<string> subchainIdentifiers;

	// Token: 0x04004509 RID: 17673
	public bool receivesCaps;

	// Token: 0x0400450A RID: 17674
	public bool isWarpWingEntrance;

	// Token: 0x0400450B RID: 17675
	public bool handlesOwnWarping;

	// Token: 0x0400450C RID: 17676
	public DungeonFlowNode.ForcedDoorType forcedDoorType;

	// Token: 0x0400450D RID: 17677
	public DungeonFlowNode.ForcedDoorType loopForcedDoorType;

	// Token: 0x0400450E RID: 17678
	public bool nodeExpands;

	// Token: 0x0400450F RID: 17679
	public string initialChainPrototype = "n";

	// Token: 0x04004510 RID: 17680
	public List<ChainRule> chainRules;

	// Token: 0x04004511 RID: 17681
	public int minChainLength = 3;

	// Token: 0x04004512 RID: 17682
	public int maxChainLength = 8;

	// Token: 0x04004513 RID: 17683
	public int minChildrenToBuild = 1;

	// Token: 0x04004514 RID: 17684
	public int maxChildrenToBuild = 1;

	// Token: 0x04004515 RID: 17685
	public bool canBuildDuplicateChildren;

	// Token: 0x04004516 RID: 17686
	public string parentNodeGuid;

	// Token: 0x04004517 RID: 17687
	public List<string> childNodeGuids;

	// Token: 0x04004518 RID: 17688
	public string loopTargetNodeGuid;

	// Token: 0x04004519 RID: 17689
	public bool loopTargetIsOneWay;

	// Token: 0x0400451A RID: 17690
	[HideInInspector]
	public string guidAsString;

	// Token: 0x0400451B RID: 17691
	public DungeonFlow flow;

	// Token: 0x02000E6E RID: 3694
	public enum ControlNodeType
	{
		// Token: 0x0400451D RID: 17693
		ROOM,
		// Token: 0x0400451E RID: 17694
		SUBCHAIN,
		// Token: 0x0400451F RID: 17695
		SELECTOR
	}

	// Token: 0x02000E6F RID: 3695
	public enum NodePriority
	{
		// Token: 0x04004521 RID: 17697
		MANDATORY,
		// Token: 0x04004522 RID: 17698
		OPTIONAL
	}

	// Token: 0x02000E70 RID: 3696
	public enum ForcedDoorType
	{
		// Token: 0x04004524 RID: 17700
		NONE,
		// Token: 0x04004525 RID: 17701
		LOCKED,
		// Token: 0x04004526 RID: 17702
		ONE_WAY
	}
}
