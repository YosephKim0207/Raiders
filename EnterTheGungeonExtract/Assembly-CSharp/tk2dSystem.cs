using System;
using UnityEngine;

// Token: 0x02000BA1 RID: 2977
public class tk2dSystem : ScriptableObject
{
	// Token: 0x06003E5C RID: 15964 RVA: 0x0013BB9C File Offset: 0x00139D9C
	private tk2dSystem()
	{
	}

	// Token: 0x17000965 RID: 2405
	// (get) Token: 0x06003E5D RID: 15965 RVA: 0x0013BC00 File Offset: 0x00139E00
	public static tk2dSystem inst
	{
		get
		{
			if (tk2dSystem._inst == null)
			{
				tk2dSystem._inst = BraveResources.Load("tk2d/tk2dSystem", typeof(tk2dSystem), ".prefab") as tk2dSystem;
				if (tk2dSystem._inst == null)
				{
					tk2dSystem._inst = ScriptableObject.CreateInstance<tk2dSystem>();
				}
				UnityEngine.Object.DontDestroyOnLoad(tk2dSystem._inst);
			}
			return tk2dSystem._inst;
		}
	}

	// Token: 0x17000966 RID: 2406
	// (get) Token: 0x06003E5E RID: 15966 RVA: 0x0013BC6C File Offset: 0x00139E6C
	public static tk2dSystem inst_NoCreate
	{
		get
		{
			if (tk2dSystem._inst == null)
			{
				tk2dSystem._inst = BraveResources.Load("tk2d/tk2dSystem", typeof(tk2dSystem), ".prefab") as tk2dSystem;
			}
			return tk2dSystem._inst;
		}
	}

	// Token: 0x17000967 RID: 2407
	// (get) Token: 0x06003E5F RID: 15967 RVA: 0x0013BCA8 File Offset: 0x00139EA8
	// (set) Token: 0x06003E60 RID: 15968 RVA: 0x0013BCB0 File Offset: 0x00139EB0
	public static string CurrentPlatform
	{
		get
		{
			return tk2dSystem.currentPlatform;
		}
		set
		{
			if (value != tk2dSystem.currentPlatform)
			{
				tk2dSystem.currentPlatform = value;
			}
		}
	}

	// Token: 0x17000968 RID: 2408
	// (get) Token: 0x06003E61 RID: 15969 RVA: 0x0013BCC8 File Offset: 0x00139EC8
	public static bool OverrideBuildMaterial
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003E62 RID: 15970 RVA: 0x0013BCCC File Offset: 0x00139ECC
	public static tk2dAssetPlatform GetAssetPlatform(string platform)
	{
		tk2dSystem inst_NoCreate = tk2dSystem.inst_NoCreate;
		if (inst_NoCreate == null)
		{
			return null;
		}
		for (int i = 0; i < inst_NoCreate.assetPlatforms.Length; i++)
		{
			if (inst_NoCreate.assetPlatforms[i].name == platform)
			{
				return inst_NoCreate.assetPlatforms[i];
			}
		}
		return null;
	}

	// Token: 0x06003E63 RID: 15971 RVA: 0x0013BD28 File Offset: 0x00139F28
	private T LoadResourceByGUIDImpl<T>(string guid) where T : UnityEngine.Object
	{
		tk2dResource tk2dResource = BraveResources.Load("tk2d/tk2d_" + guid, typeof(tk2dResource), ".prefab") as tk2dResource;
		if (tk2dResource != null)
		{
			return tk2dResource.objectReference as T;
		}
		return (T)((object)null);
	}

	// Token: 0x06003E64 RID: 15972 RVA: 0x0013BD80 File Offset: 0x00139F80
	private T LoadResourceByNameImpl<T>(string name) where T : UnityEngine.Object
	{
		for (int i = 0; i < this.allResourceEntries.Length; i++)
		{
			if (this.allResourceEntries[i] != null && this.allResourceEntries[i].assetName == name)
			{
				return this.LoadResourceByGUIDImpl<T>(this.allResourceEntries[i].assetGUID);
			}
		}
		return (T)((object)null);
	}

	// Token: 0x06003E65 RID: 15973 RVA: 0x0013BDE8 File Offset: 0x00139FE8
	public static T LoadResourceByGUID<T>(string guid) where T : UnityEngine.Object
	{
		return tk2dSystem.inst.LoadResourceByGUIDImpl<T>(guid);
	}

	// Token: 0x06003E66 RID: 15974 RVA: 0x0013BDF8 File Offset: 0x00139FF8
	public static T LoadResourceByName<T>(string guid) where T : UnityEngine.Object
	{
		return tk2dSystem.inst.LoadResourceByNameImpl<T>(guid);
	}

	// Token: 0x0400310B RID: 12555
	public const string guidPrefix = "tk2d/tk2d_";

	// Token: 0x0400310C RID: 12556
	public const string assetName = "tk2d/tk2dSystem";

	// Token: 0x0400310D RID: 12557
	public const string assetFileName = "tk2dSystem.asset";

	// Token: 0x0400310E RID: 12558
	[NonSerialized]
	public tk2dAssetPlatform[] assetPlatforms = new tk2dAssetPlatform[]
	{
		new tk2dAssetPlatform("1x", 1f),
		new tk2dAssetPlatform("2x", 2f),
		new tk2dAssetPlatform("4x", 4f)
	};

	// Token: 0x0400310F RID: 12559
	private static tk2dSystem _inst;

	// Token: 0x04003110 RID: 12560
	private static string currentPlatform = string.Empty;

	// Token: 0x04003111 RID: 12561
	[SerializeField]
	private tk2dResourceTocEntry[] allResourceEntries = new tk2dResourceTocEntry[0];
}
