using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BCC RID: 3020
[AddComponentMenu("2D Toolkit/Backend/tk2dSpriteCollection")]
public class tk2dSpriteCollection : MonoBehaviour
{
	// Token: 0x170009B2 RID: 2482
	// (get) Token: 0x06003FFA RID: 16378 RVA: 0x00145174 File Offset: 0x00143374
	// (set) Token: 0x06003FFB RID: 16379 RVA: 0x0014517C File Offset: 0x0014337C
	public Texture2D[] DoNotUse__TextureRefs
	{
		get
		{
			return this.textureRefs;
		}
		set
		{
			this.textureRefs = value;
		}
	}

	// Token: 0x170009B3 RID: 2483
	// (get) Token: 0x06003FFC RID: 16380 RVA: 0x00145188 File Offset: 0x00143388
	public bool HasPlatformData
	{
		get
		{
			return this.platforms.Count > 1;
		}
	}

	// Token: 0x06003FFD RID: 16381 RVA: 0x00145198 File Offset: 0x00143398
	public void Upgrade()
	{
		if (this.version == 4)
		{
			return;
		}
		Debug.Log("SpriteCollection '" + base.name + "' - Upgraded from version " + this.version.ToString());
		if (this.version == 0)
		{
			if (this.pixelPerfectPointSampled)
			{
				this.filterMode = FilterMode.Point;
			}
			else
			{
				this.filterMode = FilterMode.Bilinear;
			}
			this.userDefinedTextureSettings = true;
		}
		if (this.version < 3 && this.textureRefs != null && this.textureParams != null && this.textureRefs.Length == this.textureParams.Length)
		{
			for (int i = 0; i < this.textureRefs.Length; i++)
			{
				this.textureParams[i].texture = this.textureRefs[i];
			}
			this.textureRefs = null;
		}
		if (this.version < 4)
		{
			this.sizeDef.CopyFromLegacy(this.useTk2dCamera, this.targetOrthoSize, (float)this.targetHeight);
		}
		this.version = 4;
	}

	// Token: 0x0400329B RID: 12955
	public const int CURRENT_VERSION = 4;

	// Token: 0x0400329C RID: 12956
	[SerializeField]
	private tk2dSpriteCollectionDefinition[] textures;

	// Token: 0x0400329D RID: 12957
	[SerializeField]
	private Texture2D[] textureRefs;

	// Token: 0x0400329E RID: 12958
	public tk2dSpriteSheetSource[] spriteSheets;

	// Token: 0x0400329F RID: 12959
	public tk2dSpriteCollectionFont[] fonts;

	// Token: 0x040032A0 RID: 12960
	public tk2dSpriteCollectionDefault defaults;

	// Token: 0x040032A1 RID: 12961
	public List<tk2dSpriteCollectionPlatform> platforms = new List<tk2dSpriteCollectionPlatform>();

	// Token: 0x040032A2 RID: 12962
	public bool managedSpriteCollection;

	// Token: 0x040032A3 RID: 12963
	public bool loadable;

	// Token: 0x040032A4 RID: 12964
	public tk2dSpriteCollection.AtlasFormat atlasFormat;

	// Token: 0x040032A5 RID: 12965
	public int maxTextureSize = 2048;

	// Token: 0x040032A6 RID: 12966
	public bool forceTextureSize;

	// Token: 0x040032A7 RID: 12967
	public int forcedTextureWidth = 2048;

	// Token: 0x040032A8 RID: 12968
	public int forcedTextureHeight = 2048;

	// Token: 0x040032A9 RID: 12969
	public tk2dSpriteCollection.TextureCompression textureCompression;

	// Token: 0x040032AA RID: 12970
	public int atlasWidth;

	// Token: 0x040032AB RID: 12971
	public int atlasHeight;

	// Token: 0x040032AC RID: 12972
	public bool forceSquareAtlas;

	// Token: 0x040032AD RID: 12973
	public float atlasWastage;

	// Token: 0x040032AE RID: 12974
	public bool allowMultipleAtlases;

	// Token: 0x040032AF RID: 12975
	public bool removeDuplicates = true;

	// Token: 0x040032B0 RID: 12976
	public tk2dSpriteCollectionDefinition[] textureParams;

	// Token: 0x040032B1 RID: 12977
	public tk2dSpriteCollectionData spriteCollection;

	// Token: 0x040032B2 RID: 12978
	public bool premultipliedAlpha;

	// Token: 0x040032B3 RID: 12979
	public bool shouldGenerateTilemapReflectionData;

	// Token: 0x040032B4 RID: 12980
	public Material[] altMaterials;

	// Token: 0x040032B5 RID: 12981
	public Material[] atlasMaterials;

	// Token: 0x040032B6 RID: 12982
	public Texture2D[] atlasTextures;

	// Token: 0x040032B7 RID: 12983
	public TextAsset[] atlasTextureFiles = new TextAsset[0];

	// Token: 0x040032B8 RID: 12984
	[SerializeField]
	private bool useTk2dCamera;

	// Token: 0x040032B9 RID: 12985
	[SerializeField]
	private int targetHeight = 640;

	// Token: 0x040032BA RID: 12986
	[SerializeField]
	private float targetOrthoSize = 10f;

	// Token: 0x040032BB RID: 12987
	public tk2dSpriteCollectionSize sizeDef = tk2dSpriteCollectionSize.Default();

	// Token: 0x040032BC RID: 12988
	public float globalScale = 1f;

	// Token: 0x040032BD RID: 12989
	public float globalTextureRescale = 1f;

	// Token: 0x040032BE RID: 12990
	public List<tk2dSpriteCollection.AttachPointTestSprite> attachPointTestSprites = new List<tk2dSpriteCollection.AttachPointTestSprite>();

	// Token: 0x040032BF RID: 12991
	[SerializeField]
	private bool pixelPerfectPointSampled;

	// Token: 0x040032C0 RID: 12992
	public FilterMode filterMode = FilterMode.Bilinear;

	// Token: 0x040032C1 RID: 12993
	public TextureWrapMode wrapMode = TextureWrapMode.Clamp;

	// Token: 0x040032C2 RID: 12994
	public bool userDefinedTextureSettings;

	// Token: 0x040032C3 RID: 12995
	public bool mipmapEnabled;

	// Token: 0x040032C4 RID: 12996
	public int anisoLevel = 1;

	// Token: 0x040032C5 RID: 12997
	public tk2dSpriteDefinition.PhysicsEngine physicsEngine;

	// Token: 0x040032C6 RID: 12998
	public float physicsDepth = 0.1f;

	// Token: 0x040032C7 RID: 12999
	public bool disableTrimming;

	// Token: 0x040032C8 RID: 13000
	public bool disableRotation;

	// Token: 0x040032C9 RID: 13001
	public tk2dSpriteCollection.NormalGenerationMode normalGenerationMode;

	// Token: 0x040032CA RID: 13002
	public int padAmount = -1;

	// Token: 0x040032CB RID: 13003
	public bool autoUpdate = true;

	// Token: 0x040032CC RID: 13004
	public float editorDisplayScale = 1f;

	// Token: 0x040032CD RID: 13005
	public int version;

	// Token: 0x040032CE RID: 13006
	public string assetName = string.Empty;

	// Token: 0x02000BCD RID: 3021
	public enum NormalGenerationMode
	{
		// Token: 0x040032D0 RID: 13008
		None,
		// Token: 0x040032D1 RID: 13009
		NormalsOnly,
		// Token: 0x040032D2 RID: 13010
		NormalsAndTangents
	}

	// Token: 0x02000BCE RID: 3022
	public enum TextureCompression
	{
		// Token: 0x040032D4 RID: 13012
		Uncompressed,
		// Token: 0x040032D5 RID: 13013
		Reduced16Bit,
		// Token: 0x040032D6 RID: 13014
		Compressed,
		// Token: 0x040032D7 RID: 13015
		Dithered16Bit_Alpha,
		// Token: 0x040032D8 RID: 13016
		Dithered16Bit_NoAlpha
	}

	// Token: 0x02000BCF RID: 3023
	public enum AtlasFormat
	{
		// Token: 0x040032DA RID: 13018
		UnityTexture,
		// Token: 0x040032DB RID: 13019
		Png
	}

	// Token: 0x02000BD0 RID: 3024
	[Serializable]
	public class AttachPointTestSprite
	{
		// Token: 0x06003FFF RID: 16383 RVA: 0x001452C8 File Offset: 0x001434C8
		public bool CompareTo(tk2dSpriteCollection.AttachPointTestSprite src)
		{
			return src.attachPointName == this.attachPointName && src.spriteCollection == this.spriteCollection && src.spriteId == this.spriteId;
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x00145308 File Offset: 0x00143508
		public void CopyFrom(tk2dSpriteCollection.AttachPointTestSprite src)
		{
			this.attachPointName = src.attachPointName;
			this.spriteCollection = src.spriteCollection;
			this.spriteId = src.spriteId;
		}

		// Token: 0x040032DC RID: 13020
		public string attachPointName = string.Empty;

		// Token: 0x040032DD RID: 13021
		public tk2dSpriteCollectionData spriteCollection;

		// Token: 0x040032DE RID: 13022
		public int spriteId = -1;
	}
}
