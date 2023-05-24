using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200083A RID: 2106
public class KeyValueList<K, V> : IList, ICollection, IEnumerable
{
	// Token: 0x06002DC9 RID: 11721 RVA: 0x000EECA8 File Offset: 0x000ECEA8
	public KeyValueList()
	{
	}

	// Token: 0x06002DCA RID: 11722 RVA: 0x000EECC8 File Offset: 0x000ECEC8
	public KeyValueList(ref List<K> keyListRef, ref List<V> valListRef)
	{
		this.keyList = keyListRef;
		this.valList = valListRef;
	}

	// Token: 0x06002DCB RID: 11723 RVA: 0x000EECF8 File Offset: 0x000ECEF8
	public KeyValueList(KeyValueList<K, V> otherKeyValueList)
	{
		this.AddRange(otherKeyValueList);
	}

	// Token: 0x06002DCC RID: 11724 RVA: 0x000EED20 File Offset: 0x000ECF20
	public bool TryGetValue(K key, out V value)
	{
		int num = this.keyList.IndexOf(key);
		if (num == -1)
		{
			value = default(V);
			return false;
		}
		value = this.valList[num];
		return true;
	}

	// Token: 0x06002DCD RID: 11725 RVA: 0x000EED68 File Offset: 0x000ECF68
	public int Add(object value)
	{
		throw new NotImplementedException("Use KeyValueList[key] = value or insert");
	}

	// Token: 0x06002DCE RID: 11726 RVA: 0x000EED74 File Offset: 0x000ECF74
	public void Clear()
	{
		this.keyList.Clear();
		this.valList.Clear();
	}

	// Token: 0x06002DCF RID: 11727 RVA: 0x000EED8C File Offset: 0x000ECF8C
	public bool Contains(V value)
	{
		return this.valList.Contains(value);
	}

	// Token: 0x06002DD0 RID: 11728 RVA: 0x000EED9C File Offset: 0x000ECF9C
	public bool ContainsKey(K key)
	{
		return this.keyList.Contains(key);
	}

	// Token: 0x06002DD1 RID: 11729 RVA: 0x000EEDAC File Offset: 0x000ECFAC
	public int IndexOf(K key)
	{
		return this.keyList.IndexOf(key);
	}

	// Token: 0x06002DD2 RID: 11730 RVA: 0x000EEDBC File Offset: 0x000ECFBC
	public void Insert(int index, K key, V value)
	{
		if (this.keyList.Contains(key))
		{
			throw new Exception("Cannot insert duplicate key.");
		}
		this.keyList.Insert(index, key);
		this.valList.Insert(index, value);
	}

	// Token: 0x06002DD3 RID: 11731 RVA: 0x000EEDF4 File Offset: 0x000ECFF4
	public void Insert(int index, KeyValuePair<K, V> kvp)
	{
		this.Insert(index, kvp.Key, kvp.Value);
	}

	// Token: 0x06002DD4 RID: 11732 RVA: 0x000EEE0C File Offset: 0x000ED00C
	public void Insert(int index, object value)
	{
		string text = "Use Insert(K key, V value) or Insert(KeyValuePair<K, V>)";
		throw new NotImplementedException(text);
	}

	// Token: 0x06002DD5 RID: 11733 RVA: 0x000EEE28 File Offset: 0x000ED028
	public void Remove(K key)
	{
		int num = this.keyList.IndexOf(key);
		if (num == -1)
		{
			throw new KeyNotFoundException();
		}
		this.keyList.RemoveAt(num);
		this.valList.RemoveAt(num);
	}

	// Token: 0x06002DD6 RID: 11734 RVA: 0x000EEE68 File Offset: 0x000ED068
	public void Remove(object value)
	{
		throw new NotImplementedException("Use Remove(K key)");
	}

	// Token: 0x06002DD7 RID: 11735 RVA: 0x000EEE74 File Offset: 0x000ED074
	public void RemoveAt(int index)
	{
		this.keyList.RemoveAt(index);
		this.valList.RemoveAt(index);
	}

	// Token: 0x1700087A RID: 2170
	public V this[K key]
	{
		get
		{
			V v;
			if (this.TryGetValue(key, out v))
			{
				return v;
			}
			throw new KeyNotFoundException();
		}
		set
		{
			int num = this.keyList.IndexOf(key);
			if (num == -1)
			{
				this.keyList.Add(key);
				this.valList.Add(value);
			}
			else
			{
				this.valList[num] = value;
			}
		}
	}

	// Token: 0x06002DDA RID: 11738 RVA: 0x000EEF00 File Offset: 0x000ED100
	public V GetAt(int index)
	{
		if (index >= this.valList.Count)
		{
			throw new IndexOutOfRangeException();
		}
		return this.valList[index];
	}

	// Token: 0x06002DDB RID: 11739 RVA: 0x000EEF28 File Offset: 0x000ED128
	public void SetAt(int index, V value)
	{
		if (index >= this.valList.Count)
		{
			throw new IndexOutOfRangeException();
		}
		this.valList[index] = value;
	}

	// Token: 0x06002DDC RID: 11740 RVA: 0x000EEF50 File Offset: 0x000ED150
	public void CopyTo(V[] array, int index)
	{
		this.valList.CopyTo(array, index);
	}

	// Token: 0x06002DDD RID: 11741 RVA: 0x000EEF60 File Offset: 0x000ED160
	public void CopyTo(KeyValueList<K, V> otherKeyValueList, int index)
	{
		foreach (KeyValuePair<K, V> keyValuePair in this)
		{
			otherKeyValueList[keyValuePair.Key] = keyValuePair.Value;
		}
	}

	// Token: 0x06002DDE RID: 11742 RVA: 0x000EEFC4 File Offset: 0x000ED1C4
	public void AddRange(KeyValueList<K, V> otherKeyValueList)
	{
		otherKeyValueList.CopyTo(this, 0);
	}

	// Token: 0x1700087B RID: 2171
	// (get) Token: 0x06002DDF RID: 11743 RVA: 0x000EEFD0 File Offset: 0x000ED1D0
	public int Count
	{
		get
		{
			return this.valList.Count;
		}
	}

	// Token: 0x06002DE0 RID: 11744 RVA: 0x000EEFE0 File Offset: 0x000ED1E0
	public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
	{
		foreach (K key in this.keyList)
		{
			yield return new KeyValuePair<K, V>(key, this[key]);
		}
		yield break;
	}

	// Token: 0x06002DE1 RID: 11745 RVA: 0x000EEFFC File Offset: 0x000ED1FC
	IEnumerator IEnumerable.GetEnumerator()
	{
		foreach (K key in this.keyList)
		{
			yield return new KeyValuePair<K, V>(key, this[key]);
		}
		yield break;
	}

	// Token: 0x06002DE2 RID: 11746 RVA: 0x000EF018 File Offset: 0x000ED218
	public override string ToString()
	{
		string[] array = new string[this.keyList.Count];
		string text = "{0}:{1}";
		int num = 0;
		foreach (KeyValuePair<K, V> keyValuePair in this)
		{
			array[num] = string.Format(text, keyValuePair.Key, keyValuePair.Value);
			num++;
		}
		return string.Format("[{0}]", string.Join(", ", array));
	}

	// Token: 0x1700087C RID: 2172
	// (get) Token: 0x06002DE3 RID: 11747 RVA: 0x000EF0C0 File Offset: 0x000ED2C0
	public bool IsFixedSize
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700087D RID: 2173
	// (get) Token: 0x06002DE4 RID: 11748 RVA: 0x000EF0C4 File Offset: 0x000ED2C4
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700087E RID: 2174
	// (get) Token: 0x06002DE5 RID: 11749 RVA: 0x000EF0C8 File Offset: 0x000ED2C8
	public bool IsSynchronized
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x1700087F RID: 2175
	// (get) Token: 0x06002DE6 RID: 11750 RVA: 0x000EF0D0 File Offset: 0x000ED2D0
	public object SyncRoot
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x06002DE7 RID: 11751 RVA: 0x000EF0D8 File Offset: 0x000ED2D8
	public bool Contains(object value)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06002DE8 RID: 11752 RVA: 0x000EF0E0 File Offset: 0x000ED2E0
	public int IndexOf(object value)
	{
		throw new NotImplementedException();
	}

	// Token: 0x17000879 RID: 2169
	object IList.this[int index]
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x06002DEB RID: 11755 RVA: 0x000EF0F8 File Offset: 0x000ED2F8
	public void CopyTo(Array array, int index)
	{
		throw new NotImplementedException();
	}

	// Token: 0x04001F1C RID: 7964
	private List<K> keyList = new List<K>();

	// Token: 0x04001F1D RID: 7965
	private List<V> valList = new List<V>();
}
