using System;
using System.Collections;
using System.Collections.Generic;

namespace FullInspector
{
	// Token: 0x0200055A RID: 1370
	public static class fiGraphMetadataCallbacks
	{
		// Token: 0x06002091 RID: 8337 RVA: 0x0009098C File Offset: 0x0008EB8C
		public static IList Cast<T>(IList<T> list)
		{
			if (list is IList)
			{
				return (IList)list;
			}
			return new fiGraphMetadataCallbacks.ListWrapper<T>(list);
		}

		// Token: 0x040017AE RID: 6062
		public static Action<fiGraphMetadata, IList, int> ListMetadataCallback = delegate(fiGraphMetadata m, IList l, int i)
		{
		};

		// Token: 0x040017AF RID: 6063
		public static Action<fiGraphMetadata, InspectedProperty> PropertyMetadataCallback = delegate(fiGraphMetadata m, InspectedProperty p)
		{
		};

		// Token: 0x0200055B RID: 1371
		private sealed class ListWrapper<T> : IList, ICollection, IEnumerable
		{
			// Token: 0x06002095 RID: 8341 RVA: 0x000909D4 File Offset: 0x0008EBD4
			public ListWrapper(IList<T> list)
			{
				this._list = list;
			}

			// Token: 0x06002096 RID: 8342 RVA: 0x000909E4 File Offset: 0x0008EBE4
			public int Add(object value)
			{
				this._list.Add((T)((object)value));
				return this._list.Count - 1;
			}

			// Token: 0x06002097 RID: 8343 RVA: 0x00090A04 File Offset: 0x0008EC04
			public void Clear()
			{
				this._list.Clear();
			}

			// Token: 0x06002098 RID: 8344 RVA: 0x00090A14 File Offset: 0x0008EC14
			public bool Contains(object value)
			{
				return this._list.Contains((T)((object)value));
			}

			// Token: 0x06002099 RID: 8345 RVA: 0x00090A28 File Offset: 0x0008EC28
			public int IndexOf(object value)
			{
				return this._list.IndexOf((T)((object)value));
			}

			// Token: 0x0600209A RID: 8346 RVA: 0x00090A3C File Offset: 0x0008EC3C
			public void Insert(int index, object value)
			{
				this._list.Insert(index, (T)((object)value));
			}

			// Token: 0x17000656 RID: 1622
			// (get) Token: 0x0600209B RID: 8347 RVA: 0x00090A50 File Offset: 0x0008EC50
			public bool IsFixedSize
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000657 RID: 1623
			// (get) Token: 0x0600209C RID: 8348 RVA: 0x00090A54 File Offset: 0x0008EC54
			public bool IsReadOnly
			{
				get
				{
					return this._list.IsReadOnly;
				}
			}

			// Token: 0x0600209D RID: 8349 RVA: 0x00090A64 File Offset: 0x0008EC64
			public void Remove(object value)
			{
				this._list.Remove((T)((object)value));
			}

			// Token: 0x0600209E RID: 8350 RVA: 0x00090A78 File Offset: 0x0008EC78
			public void RemoveAt(int index)
			{
				this._list.RemoveAt(index);
			}

			// Token: 0x17000658 RID: 1624
			public object this[int index]
			{
				get
				{
					return this._list[index];
				}
				set
				{
					this._list[index] = (T)((object)value);
				}
			}

			// Token: 0x060020A1 RID: 8353 RVA: 0x00090AB0 File Offset: 0x0008ECB0
			public void CopyTo(Array array, int index)
			{
				this._list.CopyTo((T[])array, index);
			}

			// Token: 0x17000659 RID: 1625
			// (get) Token: 0x060020A2 RID: 8354 RVA: 0x00090AC4 File Offset: 0x0008ECC4
			public int Count
			{
				get
				{
					return this._list.Count;
				}
			}

			// Token: 0x1700065A RID: 1626
			// (get) Token: 0x060020A3 RID: 8355 RVA: 0x00090AD4 File Offset: 0x0008ECD4
			public bool IsSynchronized
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700065B RID: 1627
			// (get) Token: 0x060020A4 RID: 8356 RVA: 0x00090AD8 File Offset: 0x0008ECD8
			public object SyncRoot
			{
				get
				{
					return this;
				}
			}

			// Token: 0x060020A5 RID: 8357 RVA: 0x00090ADC File Offset: 0x0008ECDC
			public IEnumerator GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x040017B0 RID: 6064
			private readonly IList<T> _list;
		}
	}
}
