using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200038B RID: 907
[AddComponentMenu("Daikon Forge/User Interface/Texture Atlas")]
[ExecuteInEditMode]
[Serializable]
public class dfAtlas : MonoBehaviour
{
	// Token: 0x17000362 RID: 866
	// (get) Token: 0x06000F87 RID: 3975 RVA: 0x000482F8 File Offset: 0x000464F8
	public Texture2D Texture
	{
		get
		{
			return (!(this.replacementAtlas != null)) ? (this.material.mainTexture as Texture2D) : this.replacementAtlas.Texture;
		}
	}

	// Token: 0x17000363 RID: 867
	// (get) Token: 0x06000F88 RID: 3976 RVA: 0x0004832C File Offset: 0x0004652C
	public int Count
	{
		get
		{
			return (!(this.replacementAtlas != null)) ? this.items.Count : this.replacementAtlas.Count;
		}
	}

	// Token: 0x17000364 RID: 868
	// (get) Token: 0x06000F89 RID: 3977 RVA: 0x0004835C File Offset: 0x0004655C
	public List<dfAtlas.ItemInfo> Items
	{
		get
		{
			return (!(this.replacementAtlas != null)) ? this.items : this.replacementAtlas.Items;
		}
	}

	// Token: 0x17000365 RID: 869
	// (get) Token: 0x06000F8A RID: 3978 RVA: 0x00048388 File Offset: 0x00046588
	// (set) Token: 0x06000F8B RID: 3979 RVA: 0x000483B4 File Offset: 0x000465B4
	public Material Material
	{
		get
		{
			return (!(this.replacementAtlas != null)) ? this.material : this.replacementAtlas.Material;
		}
		set
		{
			if (this.replacementAtlas != null)
			{
				this.replacementAtlas.Material = value;
			}
			else
			{
				this.material = value;
			}
		}
	}

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x06000F8C RID: 3980 RVA: 0x000483E0 File Offset: 0x000465E0
	// (set) Token: 0x06000F8D RID: 3981 RVA: 0x000483E8 File Offset: 0x000465E8
	public dfAtlas Replacement
	{
		get
		{
			return this.replacementAtlas;
		}
		set
		{
			this.replacementAtlas = value;
		}
	}

	// Token: 0x17000367 RID: 871
	public dfAtlas.ItemInfo this[string key]
	{
		get
		{
			if (this.replacementAtlas != null)
			{
				return this.replacementAtlas[key];
			}
			if (string.IsNullOrEmpty(key))
			{
				return null;
			}
			if (this.map.Count == 0)
			{
				this.RebuildIndexes();
			}
			dfAtlas.ItemInfo itemInfo = null;
			if (this.map.TryGetValue(key, out itemInfo))
			{
				return itemInfo;
			}
			return null;
		}
	}

	// Token: 0x06000F8F RID: 3983 RVA: 0x0004845C File Offset: 0x0004665C
	internal static bool Equals(dfAtlas lhs, dfAtlas rhs)
	{
		return object.ReferenceEquals(lhs, rhs) || (!(lhs == null) && !(rhs == null) && lhs.material == rhs.material);
	}

	// Token: 0x06000F90 RID: 3984 RVA: 0x00048498 File Offset: 0x00046698
	public void AddItem(dfAtlas.ItemInfo item)
	{
		this.items.Add(item);
		this.RebuildIndexes();
	}

	// Token: 0x06000F91 RID: 3985 RVA: 0x000484AC File Offset: 0x000466AC
	public void AddItems(IEnumerable<dfAtlas.ItemInfo> list)
	{
		this.items.AddRange(list);
		this.RebuildIndexes();
	}

	// Token: 0x06000F92 RID: 3986 RVA: 0x000484C0 File Offset: 0x000466C0
	public void Remove(string name)
	{
		for (int i = this.items.Count - 1; i >= 0; i--)
		{
			if (this.items[i].name == name)
			{
				this.items.RemoveAt(i);
			}
		}
		this.RebuildIndexes();
	}

	// Token: 0x06000F93 RID: 3987 RVA: 0x0004851C File Offset: 0x0004671C
	public void RebuildIndexes()
	{
		if (this.map == null)
		{
			this.map = new Dictionary<string, dfAtlas.ItemInfo>();
		}
		else
		{
			this.map.Clear();
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			dfAtlas.ItemInfo itemInfo = this.items[i];
			this.map[itemInfo.name] = itemInfo;
		}
	}

	// Token: 0x04000ECB RID: 3787
	[SerializeField]
	protected Material material;

	// Token: 0x04000ECC RID: 3788
	[SerializeField]
	protected List<dfAtlas.ItemInfo> items = new List<dfAtlas.ItemInfo>();

	// Token: 0x04000ECD RID: 3789
	public dfAtlas.TextureAtlasGenerator generator;

	// Token: 0x04000ECE RID: 3790
	public string imageFileGUID;

	// Token: 0x04000ECF RID: 3791
	public string dataFileGUID;

	// Token: 0x04000ED0 RID: 3792
	private Dictionary<string, dfAtlas.ItemInfo> map = new Dictionary<string, dfAtlas.ItemInfo>();

	// Token: 0x04000ED1 RID: 3793
	private dfAtlas replacementAtlas;

	// Token: 0x0200038C RID: 908
	public enum TextureAtlasGenerator
	{
		// Token: 0x04000ED3 RID: 3795
		Internal,
		// Token: 0x04000ED4 RID: 3796
		TexturePacker
	}

	// Token: 0x0200038D RID: 909
	[Serializable]
	public class ItemInfo : IComparable<dfAtlas.ItemInfo>, IEquatable<dfAtlas.ItemInfo>
	{
		// Token: 0x06000F95 RID: 3989 RVA: 0x000485B8 File Offset: 0x000467B8
		public int CompareTo(dfAtlas.ItemInfo other)
		{
			return this.name.CompareTo(other.name);
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x000485CC File Offset: 0x000467CC
		public override int GetHashCode()
		{
			return this.name.GetHashCode();
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x000485DC File Offset: 0x000467DC
		public override bool Equals(object obj)
		{
			return obj is dfAtlas.ItemInfo && this.name.Equals(((dfAtlas.ItemInfo)obj).name);
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00048604 File Offset: 0x00046804
		public bool Equals(dfAtlas.ItemInfo other)
		{
			return this.name.Equals(other.name);
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x00048618 File Offset: 0x00046818
		public static bool operator ==(dfAtlas.ItemInfo lhs, dfAtlas.ItemInfo rhs)
		{
			return object.ReferenceEquals(lhs, rhs) || (lhs != null && rhs != null && lhs.name.Equals(rhs.name));
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x00048648 File Offset: 0x00046848
		public static bool operator !=(dfAtlas.ItemInfo lhs, dfAtlas.ItemInfo rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04000ED5 RID: 3797
		public string name;

		// Token: 0x04000ED6 RID: 3798
		public Rect region;

		// Token: 0x04000ED7 RID: 3799
		public RectOffset border = new RectOffset();

		// Token: 0x04000ED8 RID: 3800
		public bool rotated;

		// Token: 0x04000ED9 RID: 3801
		public Vector2 sizeInPixels = Vector2.zero;

		// Token: 0x04000EDA RID: 3802
		[SerializeField]
		public string textureGUID = string.Empty;

		// Token: 0x04000EDB RID: 3803
		public bool deleted;

		// Token: 0x04000EDC RID: 3804
		[SerializeField]
		public Texture2D texture;
	}
}
