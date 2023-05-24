using System;

// Token: 0x02000F58 RID: 3928
public class RuntimeRoomExitData
{
	// Token: 0x0600549F RID: 21663 RVA: 0x001FCA60 File Offset: 0x001FAC60
	public RuntimeRoomExitData(PrototypeRoomExit exit)
	{
		this.referencedExit = exit;
	}

	// Token: 0x17000BE0 RID: 3040
	// (get) Token: 0x060054A0 RID: 21664 RVA: 0x001FCA70 File Offset: 0x001FAC70
	public int TotalExitLength
	{
		get
		{
			return this.additionalExitLength + this.referencedExit.exitLength;
		}
	}

	// Token: 0x17000BE1 RID: 3041
	// (get) Token: 0x060054A1 RID: 21665 RVA: 0x001FCA84 File Offset: 0x001FAC84
	public IntVector2 HalfExitAttachPoint
	{
		get
		{
			return this.referencedExit.GetHalfExitAttachPoint(this.TotalExitLength);
		}
	}

	// Token: 0x17000BE2 RID: 3042
	// (get) Token: 0x060054A2 RID: 21666 RVA: 0x001FCA98 File Offset: 0x001FAC98
	public IntVector2 ExitOrigin
	{
		get
		{
			return this.referencedExit.GetExitOrigin(this.TotalExitLength);
		}
	}

	// Token: 0x04004D80 RID: 19840
	[NonSerialized]
	public PrototypeRoomExit referencedExit;

	// Token: 0x04004D81 RID: 19841
	[NonSerialized]
	public int additionalExitLength;

	// Token: 0x04004D82 RID: 19842
	[NonSerialized]
	public bool jointedExit;

	// Token: 0x04004D83 RID: 19843
	[NonSerialized]
	public RuntimeRoomExitData linkedExit;

	// Token: 0x04004D84 RID: 19844
	[NonSerialized]
	public bool oneWayDoor;

	// Token: 0x04004D85 RID: 19845
	[NonSerialized]
	public bool isLockedDoor;

	// Token: 0x04004D86 RID: 19846
	[NonSerialized]
	public bool isCriticalPath;

	// Token: 0x04004D87 RID: 19847
	[NonSerialized]
	public bool isWarpWingStart;

	// Token: 0x04004D88 RID: 19848
	[NonSerialized]
	public DungeonFlowNode.ForcedDoorType forcedDoorType;

	// Token: 0x04004D89 RID: 19849
	[NonSerialized]
	public WarpWingPortalController warpWingPortal;
}
