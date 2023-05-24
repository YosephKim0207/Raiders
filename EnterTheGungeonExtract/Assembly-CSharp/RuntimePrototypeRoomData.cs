using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000F5E RID: 3934
public class RuntimePrototypeRoomData
{
	// Token: 0x060054CE RID: 21710 RVA: 0x0020181C File Offset: 0x001FFA1C
	public RuntimePrototypeRoomData(PrototypeDungeonRoom source)
	{
		this.RoomId = source.RoomId;
		this.associatedMinimapIcon = source.associatedMinimapIcon;
		this.placedObjects = source.placedObjects;
		this.placedObjectPositions = source.placedObjectPositions;
		this.additionalObjectLayers = source.runtimeAdditionalObjectLayers ?? source.additionalObjectLayers;
		this.paths = source.paths;
		this.roomEvents = source.roomEvents;
		this.rewardChestSpawnPosition = source.rewardChestSpawnPosition;
		this.usesCustomAmbient = source.usesCustomAmbientLight;
		this.customAmbient = source.customAmbientLight;
		if (this.usesCustomAmbient)
		{
			this.usesDifferentCustomAmbientLowQuality = this.usesCustomAmbient;
			this.customAmbientLowQuality = new Color(this.customAmbient.r + 0.35f, this.customAmbient.g + 0.35f, this.customAmbient.b + 0.35f);
		}
		else
		{
			this.usesDifferentCustomAmbientLowQuality = false;
		}
		this.UsesCustomMusic = source.UseCustomMusic;
		this.CustomMusicEvent = source.CustomMusicEvent;
		this.UsesCustomMusicState = source.UseCustomMusicState;
		this.CustomMusicState = source.OverrideMusicState;
		this.UsesCustomSwitch = source.UseCustomMusicSwitch;
		this.CustomMusicSwitch = source.CustomMusicSwitch;
		this.GUID = source.GUID;
	}

	// Token: 0x060054CF RID: 21711 RVA: 0x0020196C File Offset: 0x001FFB6C
	public bool DoesUnsealOnClear()
	{
		for (int i = 0; i < this.roomEvents.Count; i++)
		{
			if (this.roomEvents[i].condition == RoomEventTriggerCondition.ON_ENEMIES_CLEARED && this.roomEvents[i].action == RoomEventTriggerAction.UNSEAL_ROOM)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04004DAC RID: 19884
	public int RoomId;

	// Token: 0x04004DAD RID: 19885
	public string GUID;

	// Token: 0x04004DAE RID: 19886
	public IntVector2 rewardChestSpawnPosition;

	// Token: 0x04004DAF RID: 19887
	public GameObject associatedMinimapIcon;

	// Token: 0x04004DB0 RID: 19888
	public List<PrototypePlacedObjectData> placedObjects;

	// Token: 0x04004DB1 RID: 19889
	public List<Vector2> placedObjectPositions;

	// Token: 0x04004DB2 RID: 19890
	public List<PrototypeRoomObjectLayer> additionalObjectLayers;

	// Token: 0x04004DB3 RID: 19891
	public List<SerializedPath> paths;

	// Token: 0x04004DB4 RID: 19892
	public List<RoomEventDefinition> roomEvents;

	// Token: 0x04004DB5 RID: 19893
	public bool usesCustomAmbient;

	// Token: 0x04004DB6 RID: 19894
	public Color customAmbient;

	// Token: 0x04004DB7 RID: 19895
	public bool usesDifferentCustomAmbientLowQuality;

	// Token: 0x04004DB8 RID: 19896
	public Color customAmbientLowQuality;

	// Token: 0x04004DB9 RID: 19897
	public bool UsesCustomMusicState;

	// Token: 0x04004DBA RID: 19898
	public DungeonFloorMusicController.DungeonMusicState CustomMusicState;

	// Token: 0x04004DBB RID: 19899
	public bool UsesCustomMusic;

	// Token: 0x04004DBC RID: 19900
	public string CustomMusicEvent;

	// Token: 0x04004DBD RID: 19901
	public bool UsesCustomSwitch;

	// Token: 0x04004DBE RID: 19902
	public string CustomMusicSwitch;
}
