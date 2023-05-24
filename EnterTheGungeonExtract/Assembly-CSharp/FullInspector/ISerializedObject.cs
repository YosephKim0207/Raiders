using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullInspector
{
	// Token: 0x020005C9 RID: 1481
	public interface ISerializedObject
	{
		// Token: 0x06002323 RID: 8995
		void RestoreState();

		// Token: 0x06002324 RID: 8996
		void SaveState();

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06002325 RID: 8997
		// (set) Token: 0x06002326 RID: 8998
		bool IsRestored { get; set; }

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06002327 RID: 8999
		// (set) Token: 0x06002328 RID: 9000
		List<UnityEngine.Object> SerializedObjectReferences { get; set; }

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06002329 RID: 9001
		// (set) Token: 0x0600232A RID: 9002
		List<string> SerializedStateKeys { get; set; }

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x0600232B RID: 9003
		// (set) Token: 0x0600232C RID: 9004
		List<string> SerializedStateValues { get; set; }
	}
}
