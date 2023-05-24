using System;
using System.Collections.Generic;

// Token: 0x0200035E RID: 862
public class BraveDictionary<TKey, TValue>
{
	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x00040E58 File Offset: 0x0003F058
	public int Count
	{
		get
		{
			return this.m_keys.Count;
		}
	}

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x00040E68 File Offset: 0x0003F068
	public List<TKey> Keys
	{
		get
		{
			return this.m_keys;
		}
	}

	// Token: 0x17000319 RID: 793
	// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x00040E70 File Offset: 0x0003F070
	public List<TValue> Values
	{
		get
		{
			return this.m_values;
		}
	}

	// Token: 0x06000DA7 RID: 3495 RVA: 0x00040E78 File Offset: 0x0003F078
	public bool TryGetValue(TKey key, out TValue value)
	{
		value = default(TValue);
		if (key == null)
		{
			return false;
		}
		for (int i = 0; i < this.m_keys.Count; i++)
		{
			TKey tkey = this.m_keys[i];
			if (tkey.Equals(key))
			{
				value = this.m_values[i];
				return true;
			}
		}
		return false;
	}

	// Token: 0x1700031A RID: 794
	public TValue this[TKey key]
	{
		get
		{
			if (key == null)
			{
				throw new ArgumentNullException();
			}
			for (int i = 0; i < this.m_keys.Count; i++)
			{
				TKey tkey = this.m_keys[i];
				if (tkey.Equals(key))
				{
					return this.m_values[i];
				}
			}
			throw new KeyNotFoundException();
		}
		set
		{
			if (key == null)
			{
				throw new ArgumentNullException();
			}
			for (int i = 0; i < this.m_keys.Count; i++)
			{
				TKey tkey = this.m_keys[i];
				if (tkey.Equals(key))
				{
					this.m_values[i] = value;
				}
			}
			this.m_keys.Add(key);
			this.m_values.Add(value);
		}
	}

	// Token: 0x04000DFD RID: 3581
	private List<TKey> m_keys = new List<TKey>();

	// Token: 0x04000DFE RID: 3582
	private List<TValue> m_values = new List<TValue>();
}
