using System;
using FullInspector;

// Token: 0x02000355 RID: 853
public class BraveGoodInspectorSettings : fiSettingsProcessor
{
	// Token: 0x06000D75 RID: 3445 RVA: 0x0003F3D8 File Offset: 0x0003D5D8
	public void Process()
	{
		fiSettings.SerializeAutoProperties = false;
		fiSettings.RootDirectory = "Assets/Libraries/FullInspector2/";
	}
}
