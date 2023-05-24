using System;

// Token: 0x02000F4F RID: 3919
[Serializable]
public class PrototypePlacedObjectFieldData
{
	// Token: 0x04004D4D RID: 19789
	public PrototypePlacedObjectFieldData.FieldType fieldType;

	// Token: 0x04004D4E RID: 19790
	public string fieldName;

	// Token: 0x04004D4F RID: 19791
	public float floatValue;

	// Token: 0x04004D50 RID: 19792
	public bool boolValue;

	// Token: 0x02000F50 RID: 3920
	public enum FieldType
	{
		// Token: 0x04004D52 RID: 19794
		FLOAT,
		// Token: 0x04004D53 RID: 19795
		BOOL
	}
}
