using System;
using System.Collections.Generic;
using tk2dRuntime;
using UnityEngine;

// Token: 0x02000BDA RID: 3034
[AddComponentMenu("2D Toolkit/Backend/tk2dSpriteCollectionData")]
public class tk2dSpriteCollectionData : MonoBehaviour
{
	// Token: 0x170009B9 RID: 2489
	// (get) Token: 0x06004019 RID: 16409 RVA: 0x001458DC File Offset: 0x00143ADC
	// (set) Token: 0x0600401A RID: 16410 RVA: 0x001458E4 File Offset: 0x00143AE4
	public bool Transient { get; set; }

	// Token: 0x0600401B RID: 16411 RVA: 0x001458F0 File Offset: 0x00143AF0
	public BagelCollider[] GetBagelColliders(int spriteId)
	{
		int num = this.SpriteIDsWithBagelColliders.IndexOf(spriteId);
		if (num >= 0)
		{
			return this.SpriteDefinedBagelColliders[num].bagelColliders;
		}
		return null;
	}

	// Token: 0x0600401C RID: 16412 RVA: 0x00145924 File Offset: 0x00143B24
	public void SetBagelColliders(int spriteId, BagelCollider[] bcs)
	{
		if (bcs == null || bcs.Length == 0)
		{
			int num = this.SpriteIDsWithBagelColliders.IndexOf(spriteId);
			if (num >= 0)
			{
				this.SpriteIDsWithBagelColliders.RemoveAt(num);
				this.SpriteDefinedBagelColliders.RemoveAt(num);
			}
		}
		else if (this.SpriteIDsWithBagelColliders.Contains(spriteId))
		{
			this.SpriteDefinedBagelColliders[this.SpriteIDsWithBagelColliders.IndexOf(spriteId)] = new BagelColliderData(bcs);
		}
		else
		{
			this.SpriteIDsWithBagelColliders.Add(spriteId);
			this.SpriteDefinedBagelColliders.Add(new BagelColliderData(bcs));
		}
	}

	// Token: 0x0600401D RID: 16413 RVA: 0x001459C0 File Offset: 0x00143BC0
	public tk2dSpriteDefinition.AttachPoint[] GetAttachPoints(int spriteId)
	{
		int num = this.SpriteIDsWithAttachPoints.IndexOf(spriteId);
		if (num >= 0)
		{
			return this.SpriteDefinedAttachPoints[num].attachPoints;
		}
		return null;
	}

	// Token: 0x0600401E RID: 16414 RVA: 0x001459F4 File Offset: 0x00143BF4
	public void ClearAttachPoints(int spriteId)
	{
		int num = this.SpriteIDsWithAttachPoints.IndexOf(spriteId);
		if (num >= 0)
		{
			this.SpriteIDsWithAttachPoints.RemoveAt(num);
			this.SpriteDefinedAttachPoints.RemoveAt(num);
		}
	}

	// Token: 0x0600401F RID: 16415 RVA: 0x00145A30 File Offset: 0x00143C30
	public void SetAttachPoints(int spriteId, tk2dSpriteDefinition.AttachPoint[] aps)
	{
		if (aps == null || aps.Length == 0)
		{
			this.ClearAttachPoints(spriteId);
			return;
		}
		if (this.SpriteIDsWithAttachPoints.Contains(spriteId))
		{
			this.SpriteDefinedAttachPoints[this.SpriteIDsWithAttachPoints.IndexOf(spriteId)] = new AttachPointData(aps);
		}
		else
		{
			this.SpriteIDsWithAttachPoints.Add(spriteId);
			this.SpriteDefinedAttachPoints.Add(new AttachPointData(aps));
		}
	}

	// Token: 0x06004020 RID: 16416 RVA: 0x00145AA4 File Offset: 0x00143CA4
	public void ClearDependencies(int spriteId)
	{
		int num = this.SpriteIDsWithNeighborDependencies.IndexOf(spriteId);
		if (num >= 0)
		{
			this.SpriteIDsWithNeighborDependencies.RemoveAt(num);
			this.SpriteDefinedIndexNeighborDependencies.RemoveAt(num);
		}
	}

	// Token: 0x06004021 RID: 16417 RVA: 0x00145AE0 File Offset: 0x00143CE0
	public List<IndexNeighborDependency> NewDependencies(int spriteId)
	{
		List<IndexNeighborDependency> dependencies = this.GetDependencies(spriteId);
		if (dependencies != null)
		{
			return dependencies;
		}
		List<IndexNeighborDependency> list = new List<IndexNeighborDependency>();
		this.SpriteIDsWithNeighborDependencies.Add(spriteId);
		this.SpriteDefinedIndexNeighborDependencies.Add(new NeighborDependencyData(list));
		return list;
	}

	// Token: 0x06004022 RID: 16418 RVA: 0x00145B24 File Offset: 0x00143D24
	public List<IndexNeighborDependency> GetDependencies(int spriteId)
	{
		int num = this.SpriteIDsWithNeighborDependencies.IndexOf(spriteId);
		if (num >= 0)
		{
			return this.SpriteDefinedIndexNeighborDependencies[num].neighborDependencies;
		}
		return null;
	}

	// Token: 0x06004023 RID: 16419 RVA: 0x00145B58 File Offset: 0x00143D58
	public SimpleTilesetAnimationSequence NewAnimationSequence(int spriteId)
	{
		SimpleTilesetAnimationSequence animationSequence = this.GetAnimationSequence(spriteId);
		if (animationSequence != null)
		{
			return animationSequence;
		}
		SimpleTilesetAnimationSequence simpleTilesetAnimationSequence = new SimpleTilesetAnimationSequence();
		this.SpriteIDsWithAnimationSequences.Add(spriteId);
		this.SpriteDefinedAnimationSequences.Add(simpleTilesetAnimationSequence);
		return simpleTilesetAnimationSequence;
	}

	// Token: 0x06004024 RID: 16420 RVA: 0x00145B94 File Offset: 0x00143D94
	public SimpleTilesetAnimationSequence GetAnimationSequence(int spriteId)
	{
		int num = this.SpriteIDsWithAnimationSequences.IndexOf(spriteId);
		if (num >= 0)
		{
			return this.SpriteDefinedAnimationSequences[num];
		}
		return null;
	}

	// Token: 0x170009BA RID: 2490
	// (get) Token: 0x06004025 RID: 16421 RVA: 0x00145BC4 File Offset: 0x00143DC4
	public int Count
	{
		get
		{
			return this.inst.spriteDefinitions.Length;
		}
	}

	// Token: 0x06004026 RID: 16422 RVA: 0x00145BD4 File Offset: 0x00143DD4
	public int GetSpriteIdByName(string name)
	{
		return this.GetSpriteIdByName(name, 0);
	}

	// Token: 0x06004027 RID: 16423 RVA: 0x00145BE0 File Offset: 0x00143DE0
	public int GetSpriteIdByName(string name, int defaultValue)
	{
		this.inst.InitDictionary();
		int num = defaultValue;
		if (!this.inst.spriteNameLookupDict.TryGetValue(name, out num))
		{
			return defaultValue;
		}
		return num;
	}

	// Token: 0x06004028 RID: 16424 RVA: 0x00145C18 File Offset: 0x00143E18
	public tk2dSpriteDefinition GetSpriteDefinition(string name)
	{
		int spriteIdByName = this.GetSpriteIdByName(name, -1);
		if (spriteIdByName == -1)
		{
			return null;
		}
		return this.spriteDefinitions[spriteIdByName];
	}

	// Token: 0x06004029 RID: 16425 RVA: 0x00145C40 File Offset: 0x00143E40
	public void InitDictionary()
	{
		if (this.spriteNameLookupDict == null)
		{
			this.spriteNameLookupDict = new Dictionary<string, int>(this.spriteDefinitions.Length);
			for (int i = 0; i < this.spriteDefinitions.Length; i++)
			{
				this.spriteNameLookupDict[this.spriteDefinitions[i].name] = i;
			}
		}
	}

	// Token: 0x170009BB RID: 2491
	// (get) Token: 0x0600402A RID: 16426 RVA: 0x00145CA0 File Offset: 0x00143EA0
	public tk2dSpriteDefinition FirstValidDefinition
	{
		get
		{
			foreach (tk2dSpriteDefinition tk2dSpriteDefinition in this.inst.spriteDefinitions)
			{
				if (tk2dSpriteDefinition.Valid)
				{
					return tk2dSpriteDefinition;
				}
			}
			return null;
		}
	}

	// Token: 0x0600402B RID: 16427 RVA: 0x00145CE0 File Offset: 0x00143EE0
	public bool IsValidSpriteId(int id)
	{
		return id >= 0 && id < this.inst.spriteDefinitions.Length && this.inst.spriteDefinitions[id].Valid;
	}

	// Token: 0x170009BC RID: 2492
	// (get) Token: 0x0600402C RID: 16428 RVA: 0x00145D10 File Offset: 0x00143F10
	public int FirstValidDefinitionIndex
	{
		get
		{
			tk2dSpriteCollectionData inst = this.inst;
			for (int i = 0; i < inst.spriteDefinitions.Length; i++)
			{
				if (inst.spriteDefinitions[i].Valid)
				{
					return i;
				}
			}
			return -1;
		}
	}

	// Token: 0x0600402D RID: 16429 RVA: 0x00145D54 File Offset: 0x00143F54
	public void InitMaterialIds()
	{
		if (this.inst.materialIdsValid)
		{
			return;
		}
		int num = -1;
		Dictionary<Material, int> dictionary = new Dictionary<Material, int>();
		for (int i = 0; i < this.inst.materials.Length; i++)
		{
			if (num == -1 && this.inst.materials[i] != null)
			{
				num = i;
			}
			dictionary[this.materials[i]] = i;
		}
		if (num == -1)
		{
			Debug.LogError("Init material ids failed.");
		}
		else
		{
			foreach (tk2dSpriteDefinition tk2dSpriteDefinition in this.inst.spriteDefinitions)
			{
				if (!dictionary.TryGetValue(tk2dSpriteDefinition.material, out tk2dSpriteDefinition.materialId))
				{
					tk2dSpriteDefinition.materialId = num;
				}
			}
			this.inst.materialIdsValid = true;
		}
	}

	// Token: 0x0600402E RID: 16430 RVA: 0x00145E34 File Offset: 0x00144034
	public List<Tuple<int, TilesetIndexMetadata>> GetIndicesForTileType(TilesetIndexMetadata.TilesetFlagType flagType)
	{
		if (this.spriteDefinitions == null)
		{
			return null;
		}
		List<Tuple<int, TilesetIndexMetadata>> list = new List<Tuple<int, TilesetIndexMetadata>>();
		for (int i = 0; i < this.spriteDefinitions.Length; i++)
		{
			if (this.spriteDefinitions[i].metadata != null)
			{
				if ((this.spriteDefinitions[i].metadata.type & flagType) != (TilesetIndexMetadata.TilesetFlagType)0)
				{
					Tuple<int, TilesetIndexMetadata> tuple = new Tuple<int, TilesetIndexMetadata>(i, this.spriteDefinitions[i].metadata);
					list.Add(tuple);
				}
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list;
	}

	// Token: 0x170009BD RID: 2493
	// (get) Token: 0x0600402F RID: 16431 RVA: 0x00145EC8 File Offset: 0x001440C8
	public tk2dSpriteCollectionData inst
	{
		get
		{
			if (this.platformSpecificData == null)
			{
				if (this.hasPlatformData)
				{
					string currentPlatform = tk2dSystem.CurrentPlatform;
					string text = string.Empty;
					for (int i = 0; i < this.spriteCollectionPlatforms.Length; i++)
					{
						if (this.spriteCollectionPlatforms[i] == currentPlatform)
						{
							text = this.spriteCollectionPlatformGUIDs[i];
							break;
						}
					}
					if (text.Length == 0)
					{
						text = this.spriteCollectionPlatformGUIDs[0];
					}
					this.platformSpecificData = tk2dSystem.LoadResourceByGUID<tk2dSpriteCollectionData>(text);
				}
				else
				{
					this.platformSpecificData = this;
				}
			}
			this.platformSpecificData.Init();
			return this.platformSpecificData;
		}
	}

	// Token: 0x06004030 RID: 16432 RVA: 0x00145F74 File Offset: 0x00144174
	private void Init()
	{
		if (this.materialInsts != null)
		{
			return;
		}
		if (this.spriteDefinitions == null)
		{
			this.spriteDefinitions = new tk2dSpriteDefinition[0];
		}
		if (this.materials == null)
		{
			this.materials = new Material[0];
		}
		this.materialInsts = new Material[this.materials.Length];
		if (this.needMaterialInstance)
		{
			if (tk2dSystem.OverrideBuildMaterial)
			{
				for (int i = 0; i < this.materials.Length; i++)
				{
					this.materialInsts[i] = new Material(Shader.Find("tk2d/BlendVertexColor"));
				}
			}
			else
			{
				bool flag = false;
				if (this.pngTextures.Length > 0)
				{
					flag = true;
					this.textureInsts = new Texture2D[this.pngTextures.Length];
					for (int j = 0; j < this.pngTextures.Length; j++)
					{
						Texture2D texture2D = new Texture2D(4, 4, TextureFormat.ARGB32, this.textureMipMaps);
						texture2D.LoadImage(this.pngTextures[j].bytes);
						this.textureInsts[j] = texture2D;
						texture2D.filterMode = this.textureFilterMode;
					}
				}
				for (int k = 0; k < this.materials.Length; k++)
				{
					this.materialInsts[k] = UnityEngine.Object.Instantiate<Material>(this.materials[k]);
					if (flag)
					{
						int num = ((this.materialPngTextureId.Length != 0) ? this.materialPngTextureId[k] : 0);
						this.materialInsts[k].mainTexture = this.textureInsts[num];
					}
				}
			}
			for (int l = 0; l < this.spriteDefinitions.Length; l++)
			{
				tk2dSpriteDefinition tk2dSpriteDefinition = this.spriteDefinitions[l];
				tk2dSpriteDefinition.materialInst = this.materialInsts[tk2dSpriteDefinition.materialId];
			}
		}
		else
		{
			for (int m = 0; m < this.materials.Length; m++)
			{
				this.materialInsts[m] = this.materials[m];
			}
			for (int n = 0; n < this.spriteDefinitions.Length; n++)
			{
				tk2dSpriteDefinition tk2dSpriteDefinition2 = this.spriteDefinitions[n];
				tk2dSpriteDefinition2.materialInst = tk2dSpriteDefinition2.material;
			}
		}
		tk2dEditorSpriteDataUnloader.Register(this);
	}

	// Token: 0x06004031 RID: 16433 RVA: 0x001461AC File Offset: 0x001443AC
	public static tk2dSpriteCollectionData CreateFromTexture(Texture texture, tk2dSpriteCollectionSize size, string[] names, Rect[] regions, Vector2[] anchors)
	{
		return SpriteCollectionGenerator.CreateFromTexture(texture, size, names, regions, anchors);
	}

	// Token: 0x06004032 RID: 16434 RVA: 0x001461BC File Offset: 0x001443BC
	public static tk2dSpriteCollectionData CreateFromTexturePacker(tk2dSpriteCollectionSize size, string texturePackerData, Texture texture)
	{
		return SpriteCollectionGenerator.CreateFromTexturePacker(size, texturePackerData, texture);
	}

	// Token: 0x06004033 RID: 16435 RVA: 0x001461C8 File Offset: 0x001443C8
	public void ResetPlatformData()
	{
		tk2dEditorSpriteDataUnloader.Unregister(this);
		if (this.platformSpecificData != null)
		{
			this.platformSpecificData.DestroyTextureInsts();
		}
		this.DestroyTextureInsts();
		if (this.platformSpecificData)
		{
			this.platformSpecificData = null;
		}
		this.materialInsts = null;
	}

	// Token: 0x06004034 RID: 16436 RVA: 0x0014621C File Offset: 0x0014441C
	private void DestroyTextureInsts()
	{
		foreach (Texture2D texture2D in this.textureInsts)
		{
			UnityEngine.Object.DestroyImmediate(texture2D);
		}
		this.textureInsts = new Texture2D[0];
	}

	// Token: 0x06004035 RID: 16437 RVA: 0x0014625C File Offset: 0x0014445C
	public void UnloadTextures()
	{
		tk2dSpriteCollectionData inst = this.inst;
		foreach (Texture2D texture2D in inst.textures)
		{
			Resources.UnloadAsset(texture2D);
		}
		inst.DestroyTextureInsts();
	}

	// Token: 0x06004036 RID: 16438 RVA: 0x001462A0 File Offset: 0x001444A0
	private void OnDestroy()
	{
		if (this.Transient)
		{
			foreach (Material material in this.materials)
			{
				UnityEngine.Object.DestroyImmediate(material);
			}
		}
		else if (this.needMaterialInstance)
		{
			foreach (Material material2 in this.materialInsts)
			{
				UnityEngine.Object.DestroyImmediate(material2);
			}
			this.materialInsts = new Material[0];
			foreach (Texture2D texture2D in this.textureInsts)
			{
				UnityEngine.Object.DestroyImmediate(texture2D);
			}
			this.textureInsts = new Texture2D[0];
		}
		this.ResetPlatformData();
	}

	// Token: 0x04003312 RID: 13074
	public const int CURRENT_VERSION = 3;

	// Token: 0x04003313 RID: 13075
	public int version;

	// Token: 0x04003314 RID: 13076
	public bool materialIdsValid;

	// Token: 0x04003315 RID: 13077
	public bool needMaterialInstance;

	// Token: 0x04003317 RID: 13079
	public tk2dSpriteDefinition[] spriteDefinitions;

	// Token: 0x04003318 RID: 13080
	[SerializeField]
	public List<int> SpriteIDsWithBagelColliders = new List<int>();

	// Token: 0x04003319 RID: 13081
	[SerializeField]
	public List<BagelColliderData> SpriteDefinedBagelColliders = new List<BagelColliderData>();

	// Token: 0x0400331A RID: 13082
	[SerializeField]
	public List<int> SpriteIDsWithAttachPoints = new List<int>();

	// Token: 0x0400331B RID: 13083
	[SerializeField]
	public List<AttachPointData> SpriteDefinedAttachPoints = new List<AttachPointData>();

	// Token: 0x0400331C RID: 13084
	[SerializeField]
	public List<int> SpriteIDsWithNeighborDependencies = new List<int>();

	// Token: 0x0400331D RID: 13085
	[SerializeField]
	public List<NeighborDependencyData> SpriteDefinedIndexNeighborDependencies = new List<NeighborDependencyData>();

	// Token: 0x0400331E RID: 13086
	[SerializeField]
	public List<int> SpriteIDsWithAnimationSequences = new List<int>();

	// Token: 0x0400331F RID: 13087
	[SerializeField]
	public List<SimpleTilesetAnimationSequence> SpriteDefinedAnimationSequences = new List<SimpleTilesetAnimationSequence>();

	// Token: 0x04003320 RID: 13088
	private Dictionary<string, int> spriteNameLookupDict;

	// Token: 0x04003321 RID: 13089
	public bool premultipliedAlpha;

	// Token: 0x04003322 RID: 13090
	public bool shouldGenerateTilemapReflectionData;

	// Token: 0x04003323 RID: 13091
	public Material material;

	// Token: 0x04003324 RID: 13092
	public Material[] materials;

	// Token: 0x04003325 RID: 13093
	[NonSerialized]
	public Material[] materialInsts;

	// Token: 0x04003326 RID: 13094
	[NonSerialized]
	public Texture2D[] textureInsts = new Texture2D[0];

	// Token: 0x04003327 RID: 13095
	public Texture[] textures;

	// Token: 0x04003328 RID: 13096
	public TextAsset[] pngTextures = new TextAsset[0];

	// Token: 0x04003329 RID: 13097
	public int[] materialPngTextureId = new int[0];

	// Token: 0x0400332A RID: 13098
	public FilterMode textureFilterMode = FilterMode.Bilinear;

	// Token: 0x0400332B RID: 13099
	public bool textureMipMaps;

	// Token: 0x0400332C RID: 13100
	public bool allowMultipleAtlases;

	// Token: 0x0400332D RID: 13101
	public string spriteCollectionGUID;

	// Token: 0x0400332E RID: 13102
	public string spriteCollectionName;

	// Token: 0x0400332F RID: 13103
	public string assetName = string.Empty;

	// Token: 0x04003330 RID: 13104
	public bool loadable;

	// Token: 0x04003331 RID: 13105
	public float invOrthoSize = 1f;

	// Token: 0x04003332 RID: 13106
	public float halfTargetHeight = 1f;

	// Token: 0x04003333 RID: 13107
	public int buildKey;

	// Token: 0x04003334 RID: 13108
	public string dataGuid = string.Empty;

	// Token: 0x04003335 RID: 13109
	public bool managedSpriteCollection;

	// Token: 0x04003336 RID: 13110
	public bool hasPlatformData;

	// Token: 0x04003337 RID: 13111
	public string[] spriteCollectionPlatforms;

	// Token: 0x04003338 RID: 13112
	public string[] spriteCollectionPlatformGUIDs;

	// Token: 0x04003339 RID: 13113
	private tk2dSpriteCollectionData platformSpecificData;
}
