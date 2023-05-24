using System;
using System.Collections.Generic;

namespace Dungeonator
{
	// Token: 0x02000EA3 RID: 3747
	public class RuntimeInjectionMetadata
	{
		// Token: 0x06004F52 RID: 20306 RVA: 0x001B80D0 File Offset: 0x001B62D0
		public RuntimeInjectionMetadata(SharedInjectionData data)
		{
			this.injectionData = data;
		}

		// Token: 0x06004F53 RID: 20307 RVA: 0x001B80EC File Offset: 0x001B62EC
		public void CopyMetadata(RuntimeInjectionMetadata other)
		{
			this.forceSecret = other.forceSecret;
		}

		// Token: 0x040046C0 RID: 18112
		public SharedInjectionData injectionData;

		// Token: 0x040046C1 RID: 18113
		public bool forceSecret;

		// Token: 0x040046C2 RID: 18114
		[NonSerialized]
		public bool HasAssignedModDataExactRoom;

		// Token: 0x040046C3 RID: 18115
		[NonSerialized]
		public ProceduralFlowModifierData AssignedModifierData;

		// Token: 0x040046C4 RID: 18116
		public Dictionary<ProceduralFlowModifierData, bool> SucceededRandomizationCheckMap = new Dictionary<ProceduralFlowModifierData, bool>();
	}
}
