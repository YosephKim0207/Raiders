using System;

// Token: 0x02000BA0 RID: 2976
[Serializable]
public class tk2dAssetPlatform
{
	// Token: 0x06003E5B RID: 15963 RVA: 0x0013BB70 File Offset: 0x00139D70
	public tk2dAssetPlatform(string name, float scale)
	{
		this.name = name;
		this.scale = scale;
	}

	// Token: 0x04003109 RID: 12553
	public string name = string.Empty;

	// Token: 0x0400310A RID: 12554
	public float scale = 1f;
}
