using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeonator
{
	// Token: 0x02000F65 RID: 3941
	[Serializable]
	public class SharedInjectionData : ScriptableObject
	{
		// Token: 0x04004DEF RID: 19951
		[SerializeField]
		public List<ProceduralFlowModifierData> InjectionData;

		// Token: 0x04004DF0 RID: 19952
		[SerializeField]
		public bool UseInvalidWeightAsNoInjection = true;

		// Token: 0x04004DF1 RID: 19953
		[SerializeField]
		public bool PreventInjectionOfFailedPrerequisites;

		// Token: 0x04004DF2 RID: 19954
		[SerializeField]
		public bool IsNPCCell;

		// Token: 0x04004DF3 RID: 19955
		[SerializeField]
		public bool IgnoreUnmetPrerequisiteEntries;

		// Token: 0x04004DF4 RID: 19956
		[SerializeField]
		public bool OnlyOne;

		// Token: 0x04004DF5 RID: 19957
		[ShowInInspectorIf("OnlyOne", false)]
		public float ChanceToSpawnOne = 0.5f;

		// Token: 0x04004DF6 RID: 19958
		[SerializeField]
		public List<SharedInjectionData> AttachedInjectionData;
	}
}
