using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020016E9 RID: 5865
public class GoopDefinitionModificationSynergyProcessor : MonoBehaviour
{
	// Token: 0x0600885C RID: 34908 RVA: 0x00388944 File Offset: 0x00386B44
	public void Awake()
	{
		this.m_gooper = base.GetComponent<GoopModifier>();
		int num = -1;
		if (PlayerController.AnyoneHasActiveBonusSynergy(this.RequiredSynergy, out num))
		{
			if (this.MakesGoopIgnitable && this.m_gooper)
			{
				if (!GoopDefinitionModificationSynergyProcessor.m_modifiedGoops.ContainsKey(this.m_gooper.goopDefinition))
				{
					GoopDefinition goopDefinition = UnityEngine.Object.Instantiate<GoopDefinition>(this.m_gooper.goopDefinition);
					goopDefinition.CanBeIgnited = true;
					GoopDefinitionModificationSynergyProcessor.m_modifiedGoops.Add(this.m_gooper.goopDefinition, goopDefinition);
				}
				this.m_gooper.goopDefinition = GoopDefinitionModificationSynergyProcessor.m_modifiedGoops[this.m_gooper.goopDefinition];
			}
			if (this.ChangesGoopDefinition && this.m_gooper)
			{
				this.m_gooper.goopDefinition = this.ChangedDefinition;
			}
		}
	}

	// Token: 0x04008DBB RID: 36283
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008DBC RID: 36284
	public bool MakesGoopIgnitable;

	// Token: 0x04008DBD RID: 36285
	public bool ChangesGoopDefinition;

	// Token: 0x04008DBE RID: 36286
	public GoopDefinition ChangedDefinition;

	// Token: 0x04008DBF RID: 36287
	private BasicBeamController m_beam;

	// Token: 0x04008DC0 RID: 36288
	private GoopModifier m_gooper;

	// Token: 0x04008DC1 RID: 36289
	private static Dictionary<GoopDefinition, GoopDefinition> m_modifiedGoops = new Dictionary<GoopDefinition, GoopDefinition>();
}
