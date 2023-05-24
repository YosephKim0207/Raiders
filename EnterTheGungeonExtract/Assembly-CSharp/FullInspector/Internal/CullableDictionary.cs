using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullInspector.Internal
{
	// Token: 0x0200054E RID: 1358
	public class CullableDictionary<TKey, TValue, TDictionary> : ICullableDictionary<TKey, TValue> where TDictionary : IDictionary<TKey, TValue>, new()
	{
		// Token: 0x06002040 RID: 8256 RVA: 0x0008F474 File Offset: 0x0008D674
		public CullableDictionary()
		{
			this._primary = new TDictionary();
			this._culled = new TDictionary();
		}

		// Token: 0x17000644 RID: 1604
		public TValue this[TKey key]
		{
			get
			{
				TValue tvalue;
				if (!this.TryGetValue(key, out tvalue))
				{
					throw new KeyNotFoundException(string.Empty + key);
				}
				return tvalue;
			}
			set
			{
				this._culled.Remove(key);
				this._primary[key] = value;
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06002043 RID: 8259 RVA: 0x0008F4F0 File Offset: 0x0008D6F0
		public IEnumerable<KeyValuePair<TKey, TValue>> Items
		{
			get
			{
				foreach (KeyValuePair<TKey, TValue> item in this._primary)
				{
					yield return item;
				}
				foreach (KeyValuePair<TKey, TValue> item2 in this._culled)
				{
					yield return item2;
				}
				yield break;
			}
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x0008F514 File Offset: 0x0008D714
		public void Add(TKey key, TValue value)
		{
			this._primary.Add(key, value);
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x0008F52C File Offset: 0x0008D72C
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (this._culled.TryGetValue(key, out value))
			{
				this._culled.Remove(key);
				this._primary.Add(key, value);
				return true;
			}
			return this._primary.TryGetValue(key, out value);
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x0008F594 File Offset: 0x0008D794
		public void BeginCullZone()
		{
			if (!this._isCulling)
			{
				fiUtility.Swap<TDictionary>(ref this._primary, ref this._culled);
				this._isCulling = true;
			}
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x0008F5BC File Offset: 0x0008D7BC
		public void EndCullZone()
		{
			if (this._isCulling)
			{
				this._isCulling = false;
			}
			if (fiSettings.EmitGraphMetadataCulls && this._culled.Count > 0)
			{
				foreach (KeyValuePair<TKey, TValue> keyValuePair in this._culled)
				{
					Debug.Log("fiGraphMetadata culling \"" + keyValuePair.Key + "\"");
				}
			}
			this._culled.Clear();
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06002048 RID: 8264 RVA: 0x0008F678 File Offset: 0x0008D878
		public bool IsEmpty
		{
			get
			{
				return this._primary.Count == 0 && this._culled.Count == 0;
			}
		}

		// Token: 0x0400178A RID: 6026
		[SerializeField]
		private TDictionary _primary;

		// Token: 0x0400178B RID: 6027
		[SerializeField]
		private TDictionary _culled;

		// Token: 0x0400178C RID: 6028
		[SerializeField]
		private bool _isCulling;
	}
}
