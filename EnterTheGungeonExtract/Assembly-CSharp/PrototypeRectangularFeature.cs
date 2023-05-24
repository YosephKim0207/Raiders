using System;

// Token: 0x02000F48 RID: 3912
[Serializable]
public struct PrototypeRectangularFeature
{
	// Token: 0x06005439 RID: 21561 RVA: 0x001F8450 File Offset: 0x001F6650
	public static PrototypeRectangularFeature CreateMirror(PrototypeRectangularFeature source, IntVector2 roomDimensions)
	{
		PrototypeRectangularFeature prototypeRectangularFeature = default(PrototypeRectangularFeature);
		prototypeRectangularFeature.dimensions = source.dimensions;
		prototypeRectangularFeature.basePosition = source.basePosition;
		prototypeRectangularFeature.basePosition.x = roomDimensions.x - (prototypeRectangularFeature.basePosition.x + prototypeRectangularFeature.dimensions.x);
		return prototypeRectangularFeature;
	}

	// Token: 0x04004CF6 RID: 19702
	public IntVector2 basePosition;

	// Token: 0x04004CF7 RID: 19703
	public IntVector2 dimensions;
}
