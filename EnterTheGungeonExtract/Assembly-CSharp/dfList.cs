using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020003DF RID: 991
public class dfList<T> : IList<T>, IDisposable, IPoolable, ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x06001404 RID: 5124 RVA: 0x0005CE40 File Offset: 0x0005B040
	internal dfList()
	{
		this.isElementTypeValueType = typeof(T).IsValueType;
		this.isElementTypePoolable = typeof(IPoolable).IsAssignableFrom(typeof(T));
	}

	// Token: 0x06001405 RID: 5125 RVA: 0x0005CE98 File Offset: 0x0005B098
	internal dfList(IList<T> listToClone)
		: this()
	{
		this.AddRange(listToClone);
	}

	// Token: 0x06001406 RID: 5126 RVA: 0x0005CEA8 File Offset: 0x0005B0A8
	internal dfList(int capacity)
		: this()
	{
		this.EnsureCapacity(capacity);
	}

	// Token: 0x06001407 RID: 5127 RVA: 0x0005CEB8 File Offset: 0x0005B0B8
	public static void ClearPool()
	{
		object obj = dfList<T>.pool;
		lock (obj)
		{
			dfList<T>.pool.Clear();
			dfList<T>.pool.TrimExcess();
		}
	}

	// Token: 0x06001408 RID: 5128 RVA: 0x0005CF04 File Offset: 0x0005B104
	public static dfList<T> Obtain()
	{
		object obj = dfList<T>.pool;
		dfList<T> dfList;
		lock (obj)
		{
			if (dfList<T>.pool.Count == 0)
			{
				dfList = new dfList<T>();
			}
			else
			{
				dfList = (dfList<T>)dfList<T>.pool.Dequeue();
			}
		}
		return dfList;
	}

	// Token: 0x06001409 RID: 5129 RVA: 0x0005CF64 File Offset: 0x0005B164
	public static dfList<T> Obtain(int capacity)
	{
		dfList<T> dfList = dfList<T>.Obtain();
		dfList.EnsureCapacity(capacity);
		return dfList;
	}

	// Token: 0x0600140A RID: 5130 RVA: 0x0005CF80 File Offset: 0x0005B180
	public void ReleaseItems()
	{
		if (!this.isElementTypePoolable)
		{
			throw new InvalidOperationException(string.Format("Element type {0} does not implement the {1} interface", typeof(T).Name, typeof(IPoolable).Name));
		}
		for (int i = 0; i < this.count; i++)
		{
			IPoolable poolable = this.items[i] as IPoolable;
			poolable.Release();
		}
		this.Clear();
	}

	// Token: 0x0600140B RID: 5131 RVA: 0x0005D000 File Offset: 0x0005B200
	public void Release()
	{
		object obj = dfList<T>.pool;
		lock (obj)
		{
			if (this.autoReleaseItems && this.isElementTypePoolable)
			{
				this.autoReleaseItems = false;
				this.ReleaseItems();
			}
			else
			{
				this.Clear();
			}
			dfList<T>.pool.Enqueue(this);
		}
	}

	// Token: 0x17000462 RID: 1122
	// (get) Token: 0x0600140C RID: 5132 RVA: 0x0005D070 File Offset: 0x0005B270
	// (set) Token: 0x0600140D RID: 5133 RVA: 0x0005D078 File Offset: 0x0005B278
	public bool AutoReleaseItems
	{
		get
		{
			return this.autoReleaseItems;
		}
		set
		{
			this.autoReleaseItems = value;
		}
	}

	// Token: 0x17000463 RID: 1123
	// (get) Token: 0x0600140E RID: 5134 RVA: 0x0005D084 File Offset: 0x0005B284
	public int Count
	{
		get
		{
			return this.count;
		}
	}

	// Token: 0x17000464 RID: 1124
	// (get) Token: 0x0600140F RID: 5135 RVA: 0x0005D08C File Offset: 0x0005B28C
	internal int Capacity
	{
		get
		{
			return this.items.Length;
		}
	}

	// Token: 0x17000465 RID: 1125
	// (get) Token: 0x06001410 RID: 5136 RVA: 0x0005D098 File Offset: 0x0005B298
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000466 RID: 1126
	public T this[int index]
	{
		get
		{
			if (index < 0 || index > this.count - 1)
			{
				throw new IndexOutOfRangeException();
			}
			return this.items[index];
		}
		set
		{
			if (index < 0 || index > this.count - 1)
			{
				throw new IndexOutOfRangeException();
			}
			this.items[index] = value;
		}
	}

	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x06001413 RID: 5139 RVA: 0x0005D0F4 File Offset: 0x0005B2F4
	internal T[] Items
	{
		get
		{
			return this.items;
		}
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x0005D0FC File Offset: 0x0005B2FC
	public void Enqueue(T item)
	{
		object obj = this.items;
		lock (obj)
		{
			this.Add(item);
		}
	}

	// Token: 0x06001415 RID: 5141 RVA: 0x0005D13C File Offset: 0x0005B33C
	public T Dequeue()
	{
		object obj = this.items;
		T t2;
		lock (obj)
		{
			if (this.count == 0)
			{
				throw new IndexOutOfRangeException();
			}
			T t = this.items[0];
			this.RemoveAt(0);
			t2 = t;
		}
		return t2;
	}

	// Token: 0x06001416 RID: 5142 RVA: 0x0005D19C File Offset: 0x0005B39C
	public T Pop()
	{
		object obj = this.items;
		T t2;
		lock (obj)
		{
			if (this.count == 0)
			{
				throw new IndexOutOfRangeException();
			}
			T t = this.items[this.count - 1];
			this.items[this.count - 1] = default(T);
			this.count--;
			t2 = t;
		}
		return t2;
	}

	// Token: 0x06001417 RID: 5143 RVA: 0x0005D224 File Offset: 0x0005B424
	public dfList<T> Clone()
	{
		dfList<T> dfList = dfList<T>.Obtain(this.count);
		Array.Copy(this.items, 0, dfList.items, 0, this.count);
		dfList.count = this.count;
		return dfList;
	}

	// Token: 0x06001418 RID: 5144 RVA: 0x0005D264 File Offset: 0x0005B464
	public void Reverse()
	{
		Array.Reverse(this.items, 0, this.count);
	}

	// Token: 0x06001419 RID: 5145 RVA: 0x0005D278 File Offset: 0x0005B478
	public void Sort()
	{
		Array.Sort<T>(this.items, 0, this.count, null);
	}

	// Token: 0x0600141A RID: 5146 RVA: 0x0005D290 File Offset: 0x0005B490
	public void Sort(IComparer<T> comparer)
	{
		Array.Sort<T>(this.items, 0, this.count, comparer);
	}

	// Token: 0x0600141B RID: 5147 RVA: 0x0005D2A8 File Offset: 0x0005B4A8
	public void Sort(Comparison<T> comparison)
	{
		if (comparison == null)
		{
			throw new ArgumentNullException("comparison");
		}
		if (this.count > 0)
		{
			using (dfList<T>.FunctorComparer functorComparer = dfList<T>.FunctorComparer.Obtain(comparison))
			{
				Array.Sort<T>(this.items, 0, this.count, functorComparer);
			}
		}
	}

	// Token: 0x0600141C RID: 5148 RVA: 0x0005D310 File Offset: 0x0005B510
	public void EnsureCapacity(int Size)
	{
		if (this.items.Length < Size)
		{
			int num = Size / 128 * 128 + 128;
			Array.Resize<T>(ref this.items, num);
		}
	}

	// Token: 0x0600141D RID: 5149 RVA: 0x0005D34C File Offset: 0x0005B54C
	public void AddRange(dfList<T> list)
	{
		int num = list.count;
		this.EnsureCapacity(this.count + num);
		Array.Copy(list.items, 0, this.items, this.count, num);
		this.count += num;
	}

	// Token: 0x0600141E RID: 5150 RVA: 0x0005D398 File Offset: 0x0005B598
	public void AddRange(IList<T> list)
	{
		int num = list.Count;
		this.EnsureCapacity(this.count + num);
		for (int i = 0; i < num; i++)
		{
			this.items[this.count++] = list[i];
		}
	}

	// Token: 0x0600141F RID: 5151 RVA: 0x0005D3F0 File Offset: 0x0005B5F0
	public void AddRange(T[] list)
	{
		int num = list.Length;
		this.EnsureCapacity(this.count + num);
		Array.Copy(list, 0, this.items, this.count, num);
		this.count += num;
	}

	// Token: 0x06001420 RID: 5152 RVA: 0x0005D434 File Offset: 0x0005B634
	public int IndexOf(T item)
	{
		return Array.IndexOf<T>(this.items, item, 0, this.count);
	}

	// Token: 0x06001421 RID: 5153 RVA: 0x0005D44C File Offset: 0x0005B64C
	public void Insert(int index, T item)
	{
		this.EnsureCapacity(this.count + 1);
		if (index < this.count)
		{
			Array.Copy(this.items, index, this.items, index + 1, this.count - index);
		}
		this.items[index] = item;
		this.count++;
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x0005D4AC File Offset: 0x0005B6AC
	public void InsertRange(int index, T[] array)
	{
		if (array == null)
		{
			throw new ArgumentNullException("items");
		}
		if (index < 0 || index > this.count)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		this.EnsureCapacity(this.count + array.Length);
		if (index < this.count)
		{
			Array.Copy(this.items, index, this.items, index + array.Length, this.count - index);
		}
		array.CopyTo(this.items, index);
		this.count += array.Length;
	}

	// Token: 0x06001423 RID: 5155 RVA: 0x0005D540 File Offset: 0x0005B740
	public void InsertRange(int index, dfList<T> list)
	{
		if (list == null)
		{
			throw new ArgumentNullException("items");
		}
		if (index < 0 || index > this.count)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		this.EnsureCapacity(this.count + list.count);
		if (index < this.count)
		{
			Array.Copy(this.items, index, this.items, index + list.count, this.count - index);
		}
		Array.Copy(list.items, 0, this.items, index, list.count);
		this.count += list.count;
	}

	// Token: 0x06001424 RID: 5156 RVA: 0x0005D5EC File Offset: 0x0005B7EC
	public void RemoveAll(Predicate<T> predicate)
	{
		int i = 0;
		while (i < this.count)
		{
			if (predicate(this.items[i]))
			{
				this.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06001425 RID: 5157 RVA: 0x0005D634 File Offset: 0x0005B834
	public void RemoveAt(int index)
	{
		if (index >= this.count)
		{
			throw new ArgumentOutOfRangeException();
		}
		this.count--;
		if (index < this.count)
		{
			Array.Copy(this.items, index + 1, this.items, index, this.count - index);
		}
		this.items[this.count] = default(T);
	}

	// Token: 0x06001426 RID: 5158 RVA: 0x0005D6A4 File Offset: 0x0005B8A4
	public void RemoveRange(int index, int length)
	{
		if (index < 0 || length < 0 || this.count - index < length)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (this.count > 0)
		{
			this.count -= length;
			if (index < this.count)
			{
				Array.Copy(this.items, index + length, this.items, index, this.count - index);
			}
			Array.Clear(this.items, this.count, length);
		}
	}

	// Token: 0x06001427 RID: 5159 RVA: 0x0005D728 File Offset: 0x0005B928
	public void Add(T item)
	{
		this.EnsureCapacity(this.count + 1);
		this.items[this.count++] = item;
	}

	// Token: 0x06001428 RID: 5160 RVA: 0x0005D760 File Offset: 0x0005B960
	public void Add(T item0, T item1)
	{
		this.EnsureCapacity(this.count + 2);
		this.items[this.count++] = item0;
		this.items[this.count++] = item1;
	}

	// Token: 0x06001429 RID: 5161 RVA: 0x0005D7B8 File Offset: 0x0005B9B8
	public void Add(T item0, T item1, T item2)
	{
		this.EnsureCapacity(this.count + 3);
		this.items[this.count++] = item0;
		this.items[this.count++] = item1;
		this.items[this.count++] = item2;
	}

	// Token: 0x0600142A RID: 5162 RVA: 0x0005D82C File Offset: 0x0005BA2C
	public void Clear()
	{
		if (!this.isElementTypeValueType)
		{
			Array.Clear(this.items, 0, this.items.Length);
		}
		this.count = 0;
	}

	// Token: 0x0600142B RID: 5163 RVA: 0x0005D854 File Offset: 0x0005BA54
	public void TrimExcess()
	{
		Array.Resize<T>(ref this.items, this.count);
	}

	// Token: 0x0600142C RID: 5164 RVA: 0x0005D868 File Offset: 0x0005BA68
	public bool Contains(T item)
	{
		if (item == null)
		{
			for (int i = 0; i < this.count; i++)
			{
				if (this.items[i] == null)
				{
					return true;
				}
			}
			return false;
		}
		EqualityComparer<T> @default = EqualityComparer<T>.Default;
		for (int j = 0; j < this.count; j++)
		{
			if (@default.Equals(this.items[j], item))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600142D RID: 5165 RVA: 0x0005D8EC File Offset: 0x0005BAEC
	public void CopyTo(T[] array)
	{
		this.CopyTo(array, 0);
	}

	// Token: 0x0600142E RID: 5166 RVA: 0x0005D8F8 File Offset: 0x0005BAF8
	public void CopyTo(T[] array, int arrayIndex)
	{
		Array.Copy(this.items, 0, array, arrayIndex, this.count);
	}

	// Token: 0x0600142F RID: 5167 RVA: 0x0005D910 File Offset: 0x0005BB10
	public void CopyTo(int sourceIndex, T[] dest, int destIndex, int length)
	{
		if (sourceIndex + length > this.count)
		{
			throw new IndexOutOfRangeException("sourceIndex");
		}
		if (dest == null)
		{
			throw new ArgumentNullException("dest");
		}
		if (destIndex + length > dest.Length)
		{
			throw new IndexOutOfRangeException("destIndex");
		}
		Array.Copy(this.items, sourceIndex, dest, destIndex, length);
	}

	// Token: 0x06001430 RID: 5168 RVA: 0x0005D970 File Offset: 0x0005BB70
	public bool Remove(T item)
	{
		int num = this.IndexOf(item);
		if (num == -1)
		{
			return false;
		}
		this.RemoveAt(num);
		return true;
	}

	// Token: 0x06001431 RID: 5169 RVA: 0x0005D998 File Offset: 0x0005BB98
	public List<T> ToList()
	{
		List<T> list = new List<T>(this.count);
		list.AddRange(this.ToArray());
		return list;
	}

	// Token: 0x06001432 RID: 5170 RVA: 0x0005D9C0 File Offset: 0x0005BBC0
	public T[] ToArray()
	{
		T[] array = new T[this.count];
		Array.Copy(this.items, 0, array, 0, this.count);
		return array;
	}

	// Token: 0x06001433 RID: 5171 RVA: 0x0005D9F0 File Offset: 0x0005BBF0
	public T[] ToArray(int index, int length)
	{
		T[] array = new T[this.count];
		if (this.count > 0)
		{
			this.CopyTo(index, array, 0, length);
		}
		return array;
	}

	// Token: 0x06001434 RID: 5172 RVA: 0x0005DA20 File Offset: 0x0005BC20
	public dfList<T> GetRange(int index, int length)
	{
		dfList<T> dfList = dfList<T>.Obtain(length);
		this.CopyTo(0, dfList.items, index, length);
		return dfList;
	}

	// Token: 0x06001435 RID: 5173 RVA: 0x0005DA44 File Offset: 0x0005BC44
	public bool Any(Func<T, bool> predicate)
	{
		for (int i = 0; i < this.count; i++)
		{
			if (predicate(this.items[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001436 RID: 5174 RVA: 0x0005DA84 File Offset: 0x0005BC84
	public T First()
	{
		if (this.count == 0)
		{
			throw new IndexOutOfRangeException();
		}
		return this.items[0];
	}

	// Token: 0x06001437 RID: 5175 RVA: 0x0005DAA4 File Offset: 0x0005BCA4
	public T FirstOrDefault()
	{
		if (this.count > 0)
		{
			return this.items[0];
		}
		return default(T);
	}

	// Token: 0x06001438 RID: 5176 RVA: 0x0005DAD4 File Offset: 0x0005BCD4
	public T FirstOrDefault(Func<T, bool> predicate)
	{
		for (int i = 0; i < this.count; i++)
		{
			if (predicate(this.items[i]))
			{
				return this.items[i];
			}
		}
		return default(T);
	}

	// Token: 0x06001439 RID: 5177 RVA: 0x0005DB28 File Offset: 0x0005BD28
	public T Last()
	{
		if (this.count == 0)
		{
			throw new IndexOutOfRangeException();
		}
		return this.items[this.count - 1];
	}

	// Token: 0x0600143A RID: 5178 RVA: 0x0005DB50 File Offset: 0x0005BD50
	public T LastOrDefault()
	{
		if (this.count == 0)
		{
			return default(T);
		}
		return this.items[this.count - 1];
	}

	// Token: 0x0600143B RID: 5179 RVA: 0x0005DB88 File Offset: 0x0005BD88
	public T LastOrDefault(Func<T, bool> predicate)
	{
		T t = default(T);
		for (int i = 0; i < this.count; i++)
		{
			if (predicate(this.items[i]))
			{
				t = this.items[i];
			}
		}
		return t;
	}

	// Token: 0x0600143C RID: 5180 RVA: 0x0005DBDC File Offset: 0x0005BDDC
	public dfList<T> Where(Func<T, bool> predicate)
	{
		dfList<T> dfList = dfList<T>.Obtain(this.count);
		for (int i = 0; i < this.count; i++)
		{
			if (predicate(this.items[i]))
			{
				dfList.Add(this.items[i]);
			}
		}
		return dfList;
	}

	// Token: 0x0600143D RID: 5181 RVA: 0x0005DC38 File Offset: 0x0005BE38
	public int Matching(Func<T, bool> predicate)
	{
		int num = 0;
		for (int i = 0; i < this.count; i++)
		{
			if (predicate(this.items[i]))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x0600143E RID: 5182 RVA: 0x0005DC7C File Offset: 0x0005BE7C
	public dfList<TResult> Select<TResult>(Func<T, TResult> selector)
	{
		dfList<TResult> dfList = dfList<TResult>.Obtain(this.count);
		for (int i = 0; i < this.count; i++)
		{
			dfList.Add(selector(this.items[i]));
		}
		return dfList;
	}

	// Token: 0x0600143F RID: 5183 RVA: 0x0005DCC8 File Offset: 0x0005BEC8
	public dfList<T> Concat(dfList<T> list)
	{
		dfList<T> dfList = dfList<T>.Obtain(this.count + list.count);
		dfList.AddRange(this);
		dfList.AddRange(list);
		return dfList;
	}

	// Token: 0x06001440 RID: 5184 RVA: 0x0005DCF8 File Offset: 0x0005BEF8
	public dfList<TResult> Convert<TResult>()
	{
		dfList<TResult> dfList = dfList<TResult>.Obtain(this.count);
		for (int i = 0; i < this.count; i++)
		{
			dfList.Add((TResult)((object)System.Convert.ChangeType(this.items[i], typeof(TResult))));
		}
		return dfList;
	}

	// Token: 0x06001441 RID: 5185 RVA: 0x0005DD54 File Offset: 0x0005BF54
	public void ForEach(Action<T> action)
	{
		int i = 0;
		while (i < this.Count)
		{
			action(this.items[i++]);
		}
	}

	// Token: 0x06001442 RID: 5186 RVA: 0x0005DD8C File Offset: 0x0005BF8C
	public IEnumerator<T> GetEnumerator()
	{
		return dfList<T>.PooledEnumerator.Obtain(this, null);
	}

	// Token: 0x06001443 RID: 5187 RVA: 0x0005DD98 File Offset: 0x0005BF98
	IEnumerator IEnumerable.GetEnumerator()
	{
		return dfList<T>.PooledEnumerator.Obtain(this, null);
	}

	// Token: 0x06001444 RID: 5188 RVA: 0x0005DDA4 File Offset: 0x0005BFA4
	public void Dispose()
	{
		this.Release();
	}

	// Token: 0x040011B8 RID: 4536
	private static Queue<object> pool = new Queue<object>(1024);

	// Token: 0x040011B9 RID: 4537
	private const int DEFAULT_CAPACITY = 128;

	// Token: 0x040011BA RID: 4538
	private T[] items = new T[128];

	// Token: 0x040011BB RID: 4539
	private int count;

	// Token: 0x040011BC RID: 4540
	private bool isElementTypeValueType;

	// Token: 0x040011BD RID: 4541
	private bool isElementTypePoolable;

	// Token: 0x040011BE RID: 4542
	private bool autoReleaseItems;

	// Token: 0x020003E0 RID: 992
	private class PooledEnumerator : IEnumerator<T>, IEnumerable<T>, IEnumerator, IDisposable, IEnumerable
	{
		// Token: 0x06001447 RID: 5191 RVA: 0x0005DDC8 File Offset: 0x0005BFC8
		public static dfList<T>.PooledEnumerator Obtain(dfList<T> list, Func<T, bool> predicate)
		{
			dfList<T>.PooledEnumerator pooledEnumerator = ((dfList<T>.PooledEnumerator.pool.Count <= 0) ? new dfList<T>.PooledEnumerator() : dfList<T>.PooledEnumerator.pool.Dequeue());
			pooledEnumerator.ResetInternal(list, predicate);
			return pooledEnumerator;
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0005DE04 File Offset: 0x0005C004
		public void Release()
		{
			if (this.isValid)
			{
				this.isValid = false;
				dfList<T>.PooledEnumerator.pool.Enqueue(this);
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x0005DE24 File Offset: 0x0005C024
		public T Current
		{
			get
			{
				if (!this.isValid)
				{
					throw new InvalidOperationException("The enumerator is no longer valid");
				}
				return this.currentValue;
			}
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0005DE44 File Offset: 0x0005C044
		private void ResetInternal(dfList<T> list, Func<T, bool> predicate)
		{
			this.isValid = true;
			this.list = list;
			this.predicate = predicate;
			this.currentIndex = 0;
			this.currentValue = default(T);
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x0005DE7C File Offset: 0x0005C07C
		public void Dispose()
		{
			this.Release();
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x0600144C RID: 5196 RVA: 0x0005DE84 File Offset: 0x0005C084
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x0005DE94 File Offset: 0x0005C094
		public bool MoveNext()
		{
			if (!this.isValid)
			{
				throw new InvalidOperationException("The enumerator is no longer valid");
			}
			while (this.currentIndex < this.list.Count)
			{
				T t = this.list[this.currentIndex++];
				if (this.predicate == null || this.predicate(t))
				{
					this.currentValue = t;
					return true;
				}
			}
			this.Release();
			this.currentValue = default(T);
			return false;
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0005DF30 File Offset: 0x0005C130
		public void Reset()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0005DF38 File Offset: 0x0005C138
		public IEnumerator<T> GetEnumerator()
		{
			return this;
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0005DF3C File Offset: 0x0005C13C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}

		// Token: 0x040011BF RID: 4543
		private static Queue<dfList<T>.PooledEnumerator> pool = new Queue<dfList<T>.PooledEnumerator>();

		// Token: 0x040011C0 RID: 4544
		private dfList<T> list;

		// Token: 0x040011C1 RID: 4545
		private Func<T, bool> predicate;

		// Token: 0x040011C2 RID: 4546
		private int currentIndex;

		// Token: 0x040011C3 RID: 4547
		private T currentValue;

		// Token: 0x040011C4 RID: 4548
		private bool isValid;
	}

	// Token: 0x020003E1 RID: 993
	private class FunctorComparer : IComparer<T>, IDisposable
	{
		// Token: 0x06001453 RID: 5203 RVA: 0x0005DF54 File Offset: 0x0005C154
		public static dfList<T>.FunctorComparer Obtain(Comparison<T> comparison)
		{
			dfList<T>.FunctorComparer functorComparer = ((dfList<T>.FunctorComparer.pool.Count <= 0) ? new dfList<T>.FunctorComparer() : dfList<T>.FunctorComparer.pool.Dequeue());
			functorComparer.comparison = comparison;
			return functorComparer;
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0005DF90 File Offset: 0x0005C190
		public void Release()
		{
			this.comparison = null;
			if (!dfList<T>.FunctorComparer.pool.Contains(this))
			{
				dfList<T>.FunctorComparer.pool.Enqueue(this);
			}
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x0005DFB4 File Offset: 0x0005C1B4
		public int Compare(T x, T y)
		{
			return this.comparison(x, y);
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x0005DFC4 File Offset: 0x0005C1C4
		public void Dispose()
		{
			this.Release();
		}

		// Token: 0x040011C5 RID: 4549
		private static Queue<dfList<T>.FunctorComparer> pool = new Queue<dfList<T>.FunctorComparer>();

		// Token: 0x040011C6 RID: 4550
		private Comparison<T> comparison;
	}
}
