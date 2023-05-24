using System;

// Token: 0x02000BCB RID: 3019
[Serializable]
public class tk2dSpriteCollectionPlatform
{
	// Token: 0x170009B1 RID: 2481
	// (get) Token: 0x06003FF7 RID: 16375 RVA: 0x0014505C File Offset: 0x0014325C
	public bool Valid
	{
		get
		{
			return this.name.Length > 0 && this.spriteCollection != null;
		}
	}

	// Token: 0x06003FF8 RID: 16376 RVA: 0x00145080 File Offset: 0x00143280
	public void CopyFrom(tk2dSpriteCollectionPlatform source)
	{
		this.name = source.name;
		this.spriteCollection = source.spriteCollection;
	}

	// Token: 0x04003299 RID: 12953
	public string name = string.Empty;

	// Token: 0x0400329A RID: 12954
	public tk2dSpriteCollection spriteCollection;
}
