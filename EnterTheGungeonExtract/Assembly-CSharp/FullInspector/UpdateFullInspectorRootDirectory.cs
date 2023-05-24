using System;

namespace FullInspector
{
	// Token: 0x020012E1 RID: 4833
	public class UpdateFullInspectorRootDirectory : fiSettingsProcessor
	{
		// Token: 0x06006C4D RID: 27725 RVA: 0x002AA148 File Offset: 0x002A8348
		public void Process()
		{
			fiSettings.RootDirectory = "Assets/Libraries/FullInspector2/";
		}
	}
}
