using System;
using UnityEngine;

// Token: 0x02000F5F RID: 3935
[Serializable]
public struct SerializedPathNode
{
	// Token: 0x060054D0 RID: 21712 RVA: 0x002019C8 File Offset: 0x001FFBC8
	public SerializedPathNode(IntVector2 pos)
	{
		this.position = pos;
		this.placement = SerializedPathNode.SerializedNodePlacement.SouthWest;
		this.delayTime = 0f;
		this.UsesAlternateTarget = false;
		this.AlternateTargetNodeIndex = -1;
		this.AlternateTargetPathIndex = -1;
	}

	// Token: 0x060054D1 RID: 21713 RVA: 0x002019F8 File Offset: 0x001FFBF8
	public SerializedPathNode(SerializedPathNode sourceNode, IntVector2 positionAdjustment)
	{
		this.position = sourceNode.position + positionAdjustment;
		this.placement = sourceNode.placement;
		this.delayTime = sourceNode.delayTime;
		this.UsesAlternateTarget = sourceNode.UsesAlternateTarget;
		this.AlternateTargetNodeIndex = sourceNode.AlternateTargetNodeIndex;
		this.AlternateTargetPathIndex = sourceNode.AlternateTargetPathIndex;
	}

	// Token: 0x060054D2 RID: 21714 RVA: 0x00201A5C File Offset: 0x001FFC5C
	public static SerializedPathNode CreateMirror(SerializedPathNode source, IntVector2 roomDimensions)
	{
		SerializedPathNode serializedPathNode = default(SerializedPathNode);
		serializedPathNode.position = source.position;
		serializedPathNode.position.x = roomDimensions.x - (serializedPathNode.position.x + 1);
		serializedPathNode.delayTime = source.delayTime;
		serializedPathNode.placement = source.placement;
		serializedPathNode.UsesAlternateTarget = source.UsesAlternateTarget;
		serializedPathNode.AlternateTargetPathIndex = source.AlternateTargetPathIndex;
		serializedPathNode.AlternateTargetNodeIndex = source.AlternateTargetNodeIndex;
		return serializedPathNode;
	}

	// Token: 0x17000BE5 RID: 3045
	// (get) Token: 0x060054D3 RID: 21715 RVA: 0x00201AE8 File Offset: 0x001FFCE8
	public Vector2 RoomPosition
	{
		get
		{
			IntVector2 normalizedVectorFromPlacement = this.GetNormalizedVectorFromPlacement();
			return this.position.ToCenterVector2() + new Vector2(0.5f * (float)normalizedVectorFromPlacement.x, 0.5f * (float)normalizedVectorFromPlacement.y);
		}
	}

	// Token: 0x060054D4 RID: 21716 RVA: 0x00201B30 File Offset: 0x001FFD30
	public IntVector2 GetNormalizedVectorFromPlacement()
	{
		switch (this.placement)
		{
		case SerializedPathNode.SerializedNodePlacement.Center:
			return IntVector2.Zero;
		case SerializedPathNode.SerializedNodePlacement.North:
			return IntVector2.North;
		case SerializedPathNode.SerializedNodePlacement.NorthEast:
			return IntVector2.NorthEast;
		case SerializedPathNode.SerializedNodePlacement.East:
			return IntVector2.East;
		case SerializedPathNode.SerializedNodePlacement.SouthEast:
			return IntVector2.SouthEast;
		case SerializedPathNode.SerializedNodePlacement.South:
			return IntVector2.South;
		case SerializedPathNode.SerializedNodePlacement.SouthWest:
			return IntVector2.SouthWest;
		case SerializedPathNode.SerializedNodePlacement.West:
			return IntVector2.West;
		case SerializedPathNode.SerializedNodePlacement.NorthWest:
			return IntVector2.NorthWest;
		default:
			return IntVector2.Zero;
		}
	}

	// Token: 0x04004DBF RID: 19903
	public IntVector2 position;

	// Token: 0x04004DC0 RID: 19904
	public float delayTime;

	// Token: 0x04004DC1 RID: 19905
	public SerializedPathNode.SerializedNodePlacement placement;

	// Token: 0x04004DC2 RID: 19906
	public bool UsesAlternateTarget;

	// Token: 0x04004DC3 RID: 19907
	public int AlternateTargetPathIndex;

	// Token: 0x04004DC4 RID: 19908
	public int AlternateTargetNodeIndex;

	// Token: 0x02000F60 RID: 3936
	public enum SerializedNodePlacement
	{
		// Token: 0x04004DC6 RID: 19910
		Center,
		// Token: 0x04004DC7 RID: 19911
		North,
		// Token: 0x04004DC8 RID: 19912
		NorthEast,
		// Token: 0x04004DC9 RID: 19913
		East,
		// Token: 0x04004DCA RID: 19914
		SouthEast,
		// Token: 0x04004DCB RID: 19915
		South,
		// Token: 0x04004DCC RID: 19916
		SouthWest,
		// Token: 0x04004DCD RID: 19917
		West,
		// Token: 0x04004DCE RID: 19918
		NorthWest
	}
}
