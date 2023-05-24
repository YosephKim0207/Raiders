using System;
using System.Collections;
using System.Collections.Generic;
using FullInspector.Internal;

namespace FullInspector
{
	// Token: 0x02000558 RID: 1368
	internal class IntDictionary<TValue> : IDictionary<int, TValue>, ICollection<KeyValuePair<int, TValue>>, IEnumerable<KeyValuePair<int, TValue>>, IEnumerable
	{
		// Token: 0x0600207A RID: 8314 RVA: 0x000903A8 File Offset: 0x0008E5A8
		public void Add(int key, TValue value)
		{
			if (key < 0)
			{
				this._negatives.Add(key, value);
			}
			else
			{
				while (key >= this._positives.Count)
				{
					this._positives.Add(fiOption<TValue>.Empty);
				}
				if (this._positives[key].HasValue)
				{
					throw new Exception("Already have a key for " + key);
				}
				this._positives[key] = fiOption.Just<TValue>(value);
			}
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x00090438 File Offset: 0x0008E638
		public bool ContainsKey(int key)
		{
			if (key < 0)
			{
				return this._negatives.ContainsKey(key);
			}
			return key < this._positives.Count && this._positives[key].HasValue;
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x0600207C RID: 8316 RVA: 0x00090484 File Offset: 0x0008E684
		public ICollection<int> Keys
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x0009048C File Offset: 0x0008E68C
		public bool Remove(int key)
		{
			if (key < 0)
			{
				return this._negatives.Remove(key);
			}
			if (key >= this._positives.Count)
			{
				return false;
			}
			if (this._positives[key].IsEmpty)
			{
				return false;
			}
			this._positives[key] = fiOption<TValue>.Empty;
			return true;
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x000904F0 File Offset: 0x0008E6F0
		public bool TryGetValue(int key, out TValue value)
		{
			if (key < 0)
			{
				return this._negatives.TryGetValue(key, out value);
			}
			value = default(TValue);
			if (key >= this._positives.Count)
			{
				return false;
			}
			if (this._positives[key].IsEmpty)
			{
				return false;
			}
			value = this._positives[key].Value;
			return true;
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x0600207F RID: 8319 RVA: 0x0009056C File Offset: 0x0008E76C
		public ICollection<TValue> Values
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000651 RID: 1617
		public TValue this[int key]
		{
			get
			{
				if (key < 0)
				{
					return this._negatives[key];
				}
				if (key >= this._positives.Count)
				{
					throw new KeyNotFoundException(string.Empty + key);
				}
				if (this._positives[key].IsEmpty)
				{
					throw new KeyNotFoundException(string.Empty + key);
				}
				return this._positives[key].Value;
			}
			set
			{
				if (key < 0)
				{
					this._negatives[key] = value;
				}
				else
				{
					while (key >= this._positives.Count)
					{
						this._positives.Add(fiOption<TValue>.Empty);
					}
					this._positives[key] = fiOption.Just<TValue>(value);
				}
			}
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x00090660 File Offset: 0x0008E860
		public void Add(KeyValuePair<int, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x00090678 File Offset: 0x0008E878
		public void Clear()
		{
			this._negatives.Clear();
			this._positives.Clear();
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x00090690 File Offset: 0x0008E890
		public bool Contains(KeyValuePair<int, TValue> item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x00090698 File Offset: 0x0008E898
		public void CopyTo(KeyValuePair<int, TValue>[] array, int arrayIndex)
		{
			foreach (KeyValuePair<int, TValue> keyValuePair in this)
			{
				if (arrayIndex >= array.Length)
				{
					break;
				}
				array[arrayIndex++] = keyValuePair;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06002086 RID: 8326 RVA: 0x00090708 File Offset: 0x0008E908
		public int Count
		{
			get
			{
				int num = this._negatives.Count;
				for (int i = 0; i < this._positives.Count; i++)
				{
					if (this._positives[i].HasValue)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06002087 RID: 8327 RVA: 0x0009075C File Offset: 0x0008E95C
		public bool IsReadOnly
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x00090764 File Offset: 0x0008E964
		public bool Remove(KeyValuePair<int, TValue> item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x0009076C File Offset: 0x0008E96C
		public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
		{
			foreach (KeyValuePair<int, TValue> value in this._negatives)
			{
				yield return value;
			}
			for (int i = 0; i < this._positives.Count; i++)
			{
				if (this._positives[i].HasValue)
				{
					yield return new KeyValuePair<int, TValue>(i, this._positives[i].Value);
				}
			}
			yield break;
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x00090788 File Offset: 0x0008E988
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040017A5 RID: 6053
		private List<fiOption<TValue>> _positives = new List<fiOption<TValue>>();

		// Token: 0x040017A6 RID: 6054
		private Dictionary<int, TValue> _negatives = new Dictionary<int, TValue>();
	}
}
