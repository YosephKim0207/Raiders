using System;
using UnityEngine;

// Token: 0x02000B8E RID: 2958
[AddComponentMenu("2D Toolkit/Backend/tk2dFont")]
public class tk2dFont : MonoBehaviour
{
	// Token: 0x06003DD0 RID: 15824 RVA: 0x00135DFC File Offset: 0x00133FFC
	public void Upgrade()
	{
		if (this.version >= tk2dFont.CURRENT_VERSION)
		{
			return;
		}
		Debug.Log("Font '" + base.name + "' - Upgraded from version " + this.version.ToString());
		if (this.version == 0)
		{
			this.sizeDef.CopyFromLegacy(this.useTk2dCamera, this.targetOrthoSize, (float)this.targetHeight);
		}
		this.version = tk2dFont.CURRENT_VERSION;
	}

	// Token: 0x04003049 RID: 12361
	public TextAsset bmFont;

	// Token: 0x0400304A RID: 12362
	public Material material;

	// Token: 0x0400304B RID: 12363
	public Texture texture;

	// Token: 0x0400304C RID: 12364
	public Texture2D gradientTexture;

	// Token: 0x0400304D RID: 12365
	public bool dupeCaps;

	// Token: 0x0400304E RID: 12366
	public bool flipTextureY;

	// Token: 0x0400304F RID: 12367
	[HideInInspector]
	public bool proxyFont;

	// Token: 0x04003050 RID: 12368
	[SerializeField]
	[HideInInspector]
	private bool useTk2dCamera;

	// Token: 0x04003051 RID: 12369
	[SerializeField]
	[HideInInspector]
	private int targetHeight = 640;

	// Token: 0x04003052 RID: 12370
	[SerializeField]
	[HideInInspector]
	private float targetOrthoSize = 1f;

	// Token: 0x04003053 RID: 12371
	public tk2dSpriteCollectionSize sizeDef = tk2dSpriteCollectionSize.Default();

	// Token: 0x04003054 RID: 12372
	public int gradientCount = 1;

	// Token: 0x04003055 RID: 12373
	public bool manageMaterial;

	// Token: 0x04003056 RID: 12374
	[HideInInspector]
	public bool loadable;

	// Token: 0x04003057 RID: 12375
	public int charPadX;

	// Token: 0x04003058 RID: 12376
	public tk2dFontData data;

	// Token: 0x04003059 RID: 12377
	public static int CURRENT_VERSION = 1;

	// Token: 0x0400305A RID: 12378
	public int version;
}
