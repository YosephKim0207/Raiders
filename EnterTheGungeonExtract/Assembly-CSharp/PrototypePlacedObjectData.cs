using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000F51 RID: 3921
[Serializable]
public class PrototypePlacedObjectData
{
	// Token: 0x06005474 RID: 21620 RVA: 0x001FB444 File Offset: 0x001F9644
	public bool IsEnemyForReinforcementLayerCheck()
	{
		return (!(this.placeableContents != null) || this.placeableContents.ContainsEnemy || this.placeableContents.ContainsEnemylikeObjectForReinforcement) && !(this.nonenemyBehaviour != null);
	}

	// Token: 0x06005475 RID: 21621 RVA: 0x001FB498 File Offset: 0x001F9698
	public bool GetBoolFieldValueByName(string name)
	{
		if (this.fieldData == null)
		{
			return false;
		}
		for (int i = 0; i < this.fieldData.Count; i++)
		{
			if (this.fieldData[i].fieldName == name)
			{
				return this.fieldData[i].boolValue;
			}
		}
		return false;
	}

	// Token: 0x06005476 RID: 21622 RVA: 0x001FB500 File Offset: 0x001F9700
	public float GetFieldValueByName(string name)
	{
		for (int i = 0; i < this.fieldData.Count; i++)
		{
			if (this.fieldData[i].fieldName == name)
			{
				return this.fieldData[i].floatValue;
			}
		}
		return 1f;
	}

	// Token: 0x06005477 RID: 21623 RVA: 0x001FB55C File Offset: 0x001F975C
	public int GetWidth(bool accountForFieldData = false)
	{
		if (accountForFieldData && this.fieldData != null)
		{
			for (int i = 0; i < this.fieldData.Count; i++)
			{
				if (this.fieldData[i].fieldName == "DwarfConfigurableWidth")
				{
					return Mathf.RoundToInt(this.GetFieldValueByName("DwarfConfigurableWidth"));
				}
			}
		}
		if (this.placeableContents != null)
		{
			return this.placeableContents.width;
		}
		if (this.nonenemyBehaviour != null)
		{
			return this.nonenemyBehaviour.placeableWidth;
		}
		if (!string.IsNullOrEmpty(this.enemyBehaviourGuid))
		{
			return EnemyDatabase.GetEntry(this.enemyBehaviourGuid).placeableWidth;
		}
		return 1;
	}

	// Token: 0x06005478 RID: 21624 RVA: 0x001FB624 File Offset: 0x001F9824
	public int GetHeight(bool accountForFieldData = false)
	{
		if (accountForFieldData && this.fieldData != null)
		{
			for (int i = 0; i < this.fieldData.Count; i++)
			{
				if (this.fieldData[i].fieldName == "DwarfConfigurableHeight")
				{
					return Mathf.RoundToInt(this.GetFieldValueByName("DwarfConfigurableHeight"));
				}
			}
		}
		if (this.placeableContents != null)
		{
			return this.placeableContents.height;
		}
		if (this.nonenemyBehaviour != null)
		{
			return this.nonenemyBehaviour.placeableHeight;
		}
		if (!string.IsNullOrEmpty(this.enemyBehaviourGuid))
		{
			return EnemyDatabase.GetEntry(this.enemyBehaviourGuid).placeableHeight;
		}
		return 1;
	}

	// Token: 0x06005479 RID: 21625 RVA: 0x001FB6EC File Offset: 0x001F98EC
	public PrototypePlacedObjectData CreateMirror(IntVector2 roomDimensions)
	{
		PrototypePlacedObjectData prototypePlacedObjectData = new PrototypePlacedObjectData();
		prototypePlacedObjectData.placeableContents = this.placeableContents;
		prototypePlacedObjectData.nonenemyBehaviour = this.nonenemyBehaviour;
		prototypePlacedObjectData.enemyBehaviourGuid = this.enemyBehaviourGuid;
		prototypePlacedObjectData.unspecifiedContents = this.unspecifiedContents;
		prototypePlacedObjectData.contentsBasePosition = this.contentsBasePosition;
		prototypePlacedObjectData.contentsBasePosition.x = (float)roomDimensions.x - (prototypePlacedObjectData.contentsBasePosition.x + (float)this.GetWidth(true));
		prototypePlacedObjectData.layer = this.layer;
		prototypePlacedObjectData.spawnChance = this.spawnChance;
		prototypePlacedObjectData.xMPxOffset = this.xMPxOffset;
		prototypePlacedObjectData.yMPxOffset = this.yMPxOffset;
		prototypePlacedObjectData.fieldData = new List<PrototypePlacedObjectFieldData>();
		for (int i = 0; i < this.fieldData.Count; i++)
		{
			PrototypePlacedObjectFieldData prototypePlacedObjectFieldData = new PrototypePlacedObjectFieldData();
			prototypePlacedObjectFieldData.boolValue = this.fieldData[i].boolValue;
			prototypePlacedObjectFieldData.fieldName = this.fieldData[i].fieldName;
			prototypePlacedObjectFieldData.fieldType = this.fieldData[i].fieldType;
			prototypePlacedObjectFieldData.floatValue = this.fieldData[i].floatValue;
			prototypePlacedObjectData.fieldData.Add(prototypePlacedObjectFieldData);
		}
		prototypePlacedObjectData.instancePrerequisites = new DungeonPrerequisite[this.instancePrerequisites.Length];
		for (int j = 0; j < this.instancePrerequisites.Length; j++)
		{
			prototypePlacedObjectData.instancePrerequisites[j] = this.instancePrerequisites[j];
		}
		prototypePlacedObjectData.assignedPathIDx = this.assignedPathIDx;
		prototypePlacedObjectData.assignedPathStartNode = this.assignedPathStartNode;
		prototypePlacedObjectData.linkedTriggerAreaIDs = new List<int>();
		for (int k = 0; k < this.linkedTriggerAreaIDs.Count; k++)
		{
			prototypePlacedObjectData.linkedTriggerAreaIDs.Add(this.linkedTriggerAreaIDs[k]);
		}
		return prototypePlacedObjectData;
	}

	// Token: 0x04004D54 RID: 19796
	public DungeonPlaceable placeableContents;

	// Token: 0x04004D55 RID: 19797
	[FormerlySerializedAs("behaviourContents")]
	public DungeonPlaceableBehaviour nonenemyBehaviour;

	// Token: 0x04004D56 RID: 19798
	public string enemyBehaviourGuid;

	// Token: 0x04004D57 RID: 19799
	public GameObject unspecifiedContents;

	// Token: 0x04004D58 RID: 19800
	public Vector2 contentsBasePosition;

	// Token: 0x04004D59 RID: 19801
	public int layer;

	// Token: 0x04004D5A RID: 19802
	public float spawnChance = 1f;

	// Token: 0x04004D5B RID: 19803
	public int xMPxOffset;

	// Token: 0x04004D5C RID: 19804
	public int yMPxOffset;

	// Token: 0x04004D5D RID: 19805
	public List<PrototypePlacedObjectFieldData> fieldData;

	// Token: 0x04004D5E RID: 19806
	public DungeonPrerequisite[] instancePrerequisites;

	// Token: 0x04004D5F RID: 19807
	public List<int> linkedTriggerAreaIDs;

	// Token: 0x04004D60 RID: 19808
	public int assignedPathIDx = -1;

	// Token: 0x04004D61 RID: 19809
	public int assignedPathStartNode;
}
