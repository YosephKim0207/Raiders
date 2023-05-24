using System;

// Token: 0x02001458 RID: 5208
[Serializable]
public class PassiveGooperSynergy
{
	// Token: 0x04007824 RID: 30756
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04007825 RID: 30757
	public GoopDefinition overrideGoopType;

	// Token: 0x04007826 RID: 30758
	public DamageTypeModifier AdditionalDamageModifier;

	// Token: 0x04007827 RID: 30759
	[NonSerialized]
	public bool m_processed;
}
