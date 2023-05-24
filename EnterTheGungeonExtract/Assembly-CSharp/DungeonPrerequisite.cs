using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E78 RID: 3704
[Serializable]
public class DungeonPrerequisite
{
	// Token: 0x06004EC9 RID: 20169 RVA: 0x001B3E94 File Offset: 0x001B2094
	private bool UsesOperation()
	{
		return this.prerequisiteType == DungeonPrerequisite.PrerequisiteType.COMPARISON || this.prerequisiteType == DungeonPrerequisite.PrerequisiteType.MAXIMUM_COMPARISON || this.prerequisiteType == DungeonPrerequisite.PrerequisiteType.NUMBER_PASTS_COMPLETED || this.prerequisiteType == DungeonPrerequisite.PrerequisiteType.ENCOUNTER || this.prerequisiteType == DungeonPrerequisite.PrerequisiteType.ENCOUNTER;
	}

	// Token: 0x06004ECA RID: 20170 RVA: 0x001B3ED4 File Offset: 0x001B20D4
	private bool IsComparison()
	{
		return this.prerequisiteType == DungeonPrerequisite.PrerequisiteType.COMPARISON || this.prerequisiteType == DungeonPrerequisite.PrerequisiteType.MAXIMUM_COMPARISON || this.prerequisiteType == DungeonPrerequisite.PrerequisiteType.NUMBER_PASTS_COMPLETED;
	}

	// Token: 0x06004ECB RID: 20171 RVA: 0x001B3EFC File Offset: 0x001B20FC
	private bool IsStatComparison()
	{
		return this.prerequisiteType == DungeonPrerequisite.PrerequisiteType.COMPARISON;
	}

	// Token: 0x06004ECC RID: 20172 RVA: 0x001B3F08 File Offset: 0x001B2108
	private bool IsMaxComparison()
	{
		return this.prerequisiteType == DungeonPrerequisite.PrerequisiteType.MAXIMUM_COMPARISON;
	}

	// Token: 0x06004ECD RID: 20173 RVA: 0x001B3F14 File Offset: 0x001B2114
	private bool IsEncounter()
	{
		return this.prerequisiteType == DungeonPrerequisite.PrerequisiteType.ENCOUNTER || this.prerequisiteType == DungeonPrerequisite.PrerequisiteType.ENCOUNTER_OR_FLAG;
	}

	// Token: 0x06004ECE RID: 20174 RVA: 0x001B3F30 File Offset: 0x001B2130
	private bool IsCharacter()
	{
		return this.prerequisiteType == DungeonPrerequisite.PrerequisiteType.CHARACTER;
	}

	// Token: 0x06004ECF RID: 20175 RVA: 0x001B3F3C File Offset: 0x001B213C
	private bool IsTileset()
	{
		return this.prerequisiteType == DungeonPrerequisite.PrerequisiteType.TILESET;
	}

	// Token: 0x06004ED0 RID: 20176 RVA: 0x001B3F48 File Offset: 0x001B2148
	private bool IsFlag()
	{
		return this.prerequisiteType == DungeonPrerequisite.PrerequisiteType.FLAG || this.prerequisiteType == DungeonPrerequisite.PrerequisiteType.ENCOUNTER_OR_FLAG;
	}

	// Token: 0x06004ED1 RID: 20177 RVA: 0x001B3F64 File Offset: 0x001B2164
	private bool IsDemoMode()
	{
		return this.prerequisiteType == DungeonPrerequisite.PrerequisiteType.DEMO_MODE;
	}

	// Token: 0x06004ED2 RID: 20178 RVA: 0x001B3F70 File Offset: 0x001B2170
	public bool CheckConditionsFulfilled()
	{
		EncounterDatabaseEntry encounterDatabaseEntry = null;
		if (!string.IsNullOrEmpty(this.encounteredObjectGuid))
		{
			encounterDatabaseEntry = EncounterDatabase.GetEntry(this.encounteredObjectGuid);
		}
		switch (this.prerequisiteType)
		{
		case DungeonPrerequisite.PrerequisiteType.ENCOUNTER:
			if (encounterDatabaseEntry == null && this.encounteredRoom == null)
			{
				return true;
			}
			if (encounterDatabaseEntry != null)
			{
				int num = GameStatsManager.Instance.QueryEncounterable(encounterDatabaseEntry);
				switch (this.prerequisiteOperation)
				{
				case DungeonPrerequisite.PrerequisiteOperation.LESS_THAN:
					return num < this.requiredNumberOfEncounters;
				case DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO:
					return num == this.requiredNumberOfEncounters;
				case DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN:
					return num > this.requiredNumberOfEncounters;
				default:
					Debug.LogError("Switching on invalid stat comparison operation!");
					break;
				}
			}
			else if (this.encounteredRoom != null)
			{
				int num2 = GameStatsManager.Instance.QueryRoomEncountered(this.encounteredRoom.GUID);
				switch (this.prerequisiteOperation)
				{
				case DungeonPrerequisite.PrerequisiteOperation.LESS_THAN:
					return num2 < this.requiredNumberOfEncounters;
				case DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO:
					return num2 == this.requiredNumberOfEncounters;
				case DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN:
					return num2 > this.requiredNumberOfEncounters;
				default:
					Debug.LogError("Switching on invalid stat comparison operation!");
					break;
				}
			}
			return false;
		case DungeonPrerequisite.PrerequisiteType.COMPARISON:
		{
			float playerStatValue = GameStatsManager.Instance.GetPlayerStatValue(this.statToCheck);
			switch (this.prerequisiteOperation)
			{
			case DungeonPrerequisite.PrerequisiteOperation.LESS_THAN:
				return playerStatValue < this.comparisonValue;
			case DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO:
				return playerStatValue == this.comparisonValue;
			case DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN:
				return playerStatValue > this.comparisonValue;
			default:
				Debug.LogError("Switching on invalid stat comparison operation!");
				break;
			}
			break;
		}
		case DungeonPrerequisite.PrerequisiteType.CHARACTER:
		{
			PlayableCharacters playableCharacters = (PlayableCharacters)(-1);
			if (!BraveRandom.IgnoreGenerationDifferentiator)
			{
				if (GameManager.Instance.PrimaryPlayer != null)
				{
					playableCharacters = GameManager.Instance.PrimaryPlayer.characterIdentity;
				}
				else if (GameManager.PlayerPrefabForNewGame != null)
				{
					playableCharacters = GameManager.PlayerPrefabForNewGame.GetComponent<PlayerController>().characterIdentity;
				}
				else if (GameManager.Instance.BestGenerationDungeonPrefab != null)
				{
					playableCharacters = GameManager.Instance.BestGenerationDungeonPrefab.defaultPlayerPrefab.GetComponent<PlayerController>().characterIdentity;
				}
			}
			return this.requireCharacter == (playableCharacters == this.requiredCharacter);
		}
		case DungeonPrerequisite.PrerequisiteType.TILESET:
			if (GameManager.Instance.BestGenerationDungeonPrefab != null)
			{
				return this.requireTileset == (GameManager.Instance.BestGenerationDungeonPrefab.tileIndices.tilesetId == this.requiredTileset);
			}
			return this.requireTileset == (GameManager.Instance.Dungeon.tileIndices.tilesetId == this.requiredTileset);
		case DungeonPrerequisite.PrerequisiteType.FLAG:
			return GameStatsManager.Instance.GetFlag(this.saveFlagToCheck) == this.requireFlag;
		case DungeonPrerequisite.PrerequisiteType.DEMO_MODE:
			return !this.requireDemoMode;
		case DungeonPrerequisite.PrerequisiteType.MAXIMUM_COMPARISON:
		{
			float playerMaximum = GameStatsManager.Instance.GetPlayerMaximum(this.maxToCheck);
			switch (this.prerequisiteOperation)
			{
			case DungeonPrerequisite.PrerequisiteOperation.LESS_THAN:
				return playerMaximum < this.comparisonValue;
			case DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO:
				return playerMaximum == this.comparisonValue;
			case DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN:
				return playerMaximum > this.comparisonValue;
			default:
				Debug.LogError("Switching on invalid stat comparison operation!");
				break;
			}
			break;
		}
		case DungeonPrerequisite.PrerequisiteType.ENCOUNTER_OR_FLAG:
			if (GameStatsManager.Instance.GetFlag(this.saveFlagToCheck) == this.requireFlag)
			{
				return true;
			}
			if (encounterDatabaseEntry != null)
			{
				int num3 = GameStatsManager.Instance.QueryEncounterable(encounterDatabaseEntry);
				switch (this.prerequisiteOperation)
				{
				case DungeonPrerequisite.PrerequisiteOperation.LESS_THAN:
					return num3 < this.requiredNumberOfEncounters;
				case DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO:
					return num3 == this.requiredNumberOfEncounters;
				case DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN:
					return num3 > this.requiredNumberOfEncounters;
				default:
					Debug.LogError("Switching on invalid stat comparison operation!");
					break;
				}
			}
			else if (this.encounteredRoom != null)
			{
				int num4 = GameStatsManager.Instance.QueryRoomEncountered(this.encounteredRoom.GUID);
				switch (this.prerequisiteOperation)
				{
				case DungeonPrerequisite.PrerequisiteOperation.LESS_THAN:
					return num4 < this.requiredNumberOfEncounters;
				case DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO:
					return num4 == this.requiredNumberOfEncounters;
				case DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN:
					return num4 > this.requiredNumberOfEncounters;
				default:
					Debug.LogError("Switching on invalid stat comparison operation!");
					break;
				}
			}
			return false;
		case DungeonPrerequisite.PrerequisiteType.NUMBER_PASTS_COMPLETED:
			return (float)GameStatsManager.Instance.GetNumberPastsBeaten() >= this.comparisonValue;
		default:
			Debug.LogError("Switching on invalid prerequisite type!!!");
			break;
		}
		return false;
	}

	// Token: 0x06004ED3 RID: 20179 RVA: 0x001B43E0 File Offset: 0x001B25E0
	protected bool Equals(DungeonPrerequisite other)
	{
		return this.prerequisiteType == other.prerequisiteType && this.prerequisiteOperation == other.prerequisiteOperation && this.statToCheck == other.statToCheck && this.maxToCheck == other.maxToCheck && this.comparisonValue.Equals(other.comparisonValue) && this.useSessionStatValue.Equals(other.useSessionStatValue) && object.Equals(this.encounteredRoom, other.encounteredRoom) && object.Equals(this.encounteredObjectGuid, other.encounteredObjectGuid) && this.requiredNumberOfEncounters == other.requiredNumberOfEncounters && this.requiredCharacter == other.requiredCharacter && this.requireCharacter.Equals(other.requireCharacter) && this.requiredTileset == other.requiredTileset && this.requireTileset.Equals(other.requireTileset) && this.saveFlagToCheck == other.saveFlagToCheck && this.requireFlag.Equals(other.requireFlag) && this.requireDemoMode.Equals(other.requireDemoMode);
	}

	// Token: 0x06004ED4 RID: 20180 RVA: 0x001B4524 File Offset: 0x001B2724
	public override bool Equals(object obj)
	{
		return !object.ReferenceEquals(null, obj) && (object.ReferenceEquals(this, obj) || (obj.GetType() == base.GetType() && this.Equals((DungeonPrerequisite)obj)));
	}

	// Token: 0x06004ED5 RID: 20181 RVA: 0x001B4564 File Offset: 0x001B2764
	public override int GetHashCode()
	{
		int num = (int)this.prerequisiteType;
		num = (num * 397) ^ (int)this.prerequisiteOperation;
		num = (num * 397) ^ (int)this.statToCheck;
		num = (num * 397) ^ this.comparisonValue.GetHashCode();
		num = (num * 397) ^ this.useSessionStatValue.GetHashCode();
		num = (num * 397) ^ ((!(this.encounteredRoom != null)) ? 0 : this.encounteredRoom.GetHashCode());
		num = (num * 397) ^ ((this.encounteredObjectGuid == null) ? 0 : this.encounteredObjectGuid.GetHashCode());
		num = (num * 397) ^ this.requiredNumberOfEncounters;
		num = (num * 397) ^ (int)this.requiredCharacter;
		num = (num * 397) ^ this.requireCharacter.GetHashCode();
		num = (num * 397) ^ (int)this.requiredTileset;
		num = (num * 397) ^ this.requireTileset.GetHashCode();
		num = (num * 397) ^ (int)this.saveFlagToCheck;
		num = (num * 397) ^ this.requireFlag.GetHashCode();
		return (num * 397) ^ this.requireDemoMode.GetHashCode();
	}

	// Token: 0x06004ED6 RID: 20182 RVA: 0x001B46C0 File Offset: 0x001B28C0
	public static bool CheckConditionsFulfilled(DungeonPrerequisite[] prereqs)
	{
		if (prereqs == null)
		{
			return true;
		}
		for (int i = 0; i < prereqs.Length; i++)
		{
			if (prereqs[i] != null && !prereqs[i].CheckConditionsFulfilled())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004ED7 RID: 20183 RVA: 0x001B4704 File Offset: 0x001B2904
	public static bool CheckConditionsFulfilled(List<DungeonPrerequisite> prereqs)
	{
		if (prereqs == null)
		{
			return true;
		}
		for (int i = 0; i < prereqs.Count; i++)
		{
			if (prereqs[i] != null && !prereqs[i].CheckConditionsFulfilled())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0400458B RID: 17803
	public DungeonPrerequisite.PrerequisiteType prerequisiteType;

	// Token: 0x0400458C RID: 17804
	[ShowInInspectorIf("UsesOperation", false)]
	public DungeonPrerequisite.PrerequisiteOperation prerequisiteOperation;

	// Token: 0x0400458D RID: 17805
	[ShowInInspectorIf("IsStatComparison", false)]
	public TrackedStats statToCheck;

	// Token: 0x0400458E RID: 17806
	[ShowInInspectorIf("IsMaxComparison", false)]
	public TrackedMaximums maxToCheck;

	// Token: 0x0400458F RID: 17807
	[ShowInInspectorIf("IsComparison", false)]
	public float comparisonValue;

	// Token: 0x04004590 RID: 17808
	[ShowInInspectorIf("IsComparison", false)]
	public bool useSessionStatValue;

	// Token: 0x04004591 RID: 17809
	[ShowInInspectorIf("IsEncounter", false)]
	[EncounterIdentifier]
	public string encounteredObjectGuid;

	// Token: 0x04004592 RID: 17810
	[ShowInInspectorIf("IsEncounter", false)]
	public PrototypeDungeonRoom encounteredRoom;

	// Token: 0x04004593 RID: 17811
	[ShowInInspectorIf("IsEncounter", false)]
	public int requiredNumberOfEncounters = 1;

	// Token: 0x04004594 RID: 17812
	[ShowInInspectorIf("IsCharacter", false)]
	public PlayableCharacters requiredCharacter;

	// Token: 0x04004595 RID: 17813
	[ShowInInspectorIf("IsCharacter", false)]
	public bool requireCharacter = true;

	// Token: 0x04004596 RID: 17814
	[ShowInInspectorIf("IsTileset", false)]
	public GlobalDungeonData.ValidTilesets requiredTileset;

	// Token: 0x04004597 RID: 17815
	[ShowInInspectorIf("IsTileset", false)]
	public bool requireTileset = true;

	// Token: 0x04004598 RID: 17816
	[LongEnum]
	public GungeonFlags saveFlagToCheck;

	// Token: 0x04004599 RID: 17817
	[ShowInInspectorIf("IsFlag", false)]
	public bool requireFlag = true;

	// Token: 0x0400459A RID: 17818
	[ShowInInspectorIf("IsDemoMode", false)]
	public bool requireDemoMode;

	// Token: 0x02000E79 RID: 3705
	public enum PrerequisiteType
	{
		// Token: 0x0400459C RID: 17820
		ENCOUNTER,
		// Token: 0x0400459D RID: 17821
		COMPARISON,
		// Token: 0x0400459E RID: 17822
		CHARACTER,
		// Token: 0x0400459F RID: 17823
		TILESET,
		// Token: 0x040045A0 RID: 17824
		FLAG,
		// Token: 0x040045A1 RID: 17825
		DEMO_MODE,
		// Token: 0x040045A2 RID: 17826
		MAXIMUM_COMPARISON,
		// Token: 0x040045A3 RID: 17827
		ENCOUNTER_OR_FLAG,
		// Token: 0x040045A4 RID: 17828
		NUMBER_PASTS_COMPLETED
	}

	// Token: 0x02000E7A RID: 3706
	public enum PrerequisiteOperation
	{
		// Token: 0x040045A6 RID: 17830
		LESS_THAN,
		// Token: 0x040045A7 RID: 17831
		EQUAL_TO,
		// Token: 0x040045A8 RID: 17832
		GREATER_THAN
	}
}
