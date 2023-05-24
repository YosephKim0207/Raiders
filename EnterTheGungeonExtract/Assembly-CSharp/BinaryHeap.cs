using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200181C RID: 6172
public class BinaryHeap<T> : ICollection<T>, IEnumerable<T>, IEnumerable where T : IComparable<T>
{
	// Token: 0x06009184 RID: 37252 RVA: 0x003D8FB8 File Offset: 0x003D71B8
	public BinaryHeap()
	{
	}

	// Token: 0x06009185 RID: 37253 RVA: 0x003D8FD4 File Offset: 0x003D71D4
	private BinaryHeap(T[] data, int count)
	{
		this.Capacity = count;
		this.m_count = count;
		Array.Copy(data, this.m_data, count);
	}

	// Token: 0x170015D3 RID: 5587
	// (get) Token: 0x06009186 RID: 37254 RVA: 0x003D900C File Offset: 0x003D720C
	public int Count
	{
		get
		{
			return this.m_count;
		}
	}

	// Token: 0x170015D4 RID: 5588
	// (get) Token: 0x06009187 RID: 37255 RVA: 0x003D9014 File Offset: 0x003D7214
	// (set) Token: 0x06009188 RID: 37256 RVA: 0x003D901C File Offset: 0x003D721C
	public int Capacity
	{
		get
		{
			return this.m_capacity;
		}
		set
		{
			int capacity = this.m_capacity;
			this.m_capacity = Math.Max(value, this.m_count);
			if (this.m_capacity != capacity)
			{
				T[] array = new T[this.m_capacity];
				Array.Copy(this.m_data, array, this.m_count);
				this.m_data = array;
			}
		}
	}

	// Token: 0x06009189 RID: 37257 RVA: 0x003D9074 File Offset: 0x003D7274
	public T Peek()
	{
		return this.m_data[0];
	}

	// Token: 0x0600918A RID: 37258 RVA: 0x003D9084 File Offset: 0x003D7284
	public void Clear()
	{
		this.m_count = 0;
		this.m_data = new T[this.m_capacity];
	}

	// Token: 0x0600918B RID: 37259 RVA: 0x003D90A0 File Offset: 0x003D72A0
	public void Add(T item)
	{
		if (this.m_count == this.m_capacity)
		{
			this.Capacity *= 2;
		}
		this.m_data[this.m_count] = item;
		this.UpHeap();
		this.m_count++;
	}

	// Token: 0x0600918C RID: 37260 RVA: 0x003D90F4 File Offset: 0x003D72F4
	public T Remove()
	{
		if (this.m_count == 0)
		{
			throw new InvalidOperationException("Cannot remove item, heap is empty.");
		}
		T t = this.m_data[0];
		this.m_count--;
		this.m_data[0] = this.m_data[this.m_count];
		this.m_data[this.m_count] = default(T);
		this.DownHeap();
		return t;
	}

	// Token: 0x0600918D RID: 37261 RVA: 0x003D9170 File Offset: 0x003D7370
	private void UpHeap()
	{
		this.m_sorted = false;
		int num = this.m_count;
		T t = this.m_data[num];
		int num2 = BinaryHeap<T>.Parent(num);
		while (num2 > -1 && t.CompareTo(this.m_data[num2]) < 0)
		{
			this.m_data[num] = this.m_data[num2];
			num = num2;
			num2 = BinaryHeap<T>.Parent(num);
		}
		this.m_data[num] = t;
	}

	// Token: 0x0600918E RID: 37262 RVA: 0x003D91F8 File Offset: 0x003D73F8
	private void DownHeap()
	{
		this.m_sorted = false;
		int num = 0;
		T t = this.m_data[num];
		for (;;)
		{
			int num2 = BinaryHeap<T>.Child1(num);
			if (num2 >= this.m_count)
			{
				break;
			}
			int num3 = BinaryHeap<T>.Child2(num);
			int num4;
			if (num3 >= this.m_count)
			{
				num4 = num2;
			}
			else
			{
				num4 = ((this.m_data[num2].CompareTo(this.m_data[num3]) >= 0) ? num3 : num2);
			}
			if (t.CompareTo(this.m_data[num4]) <= 0)
			{
				break;
			}
			this.m_data[num] = this.m_data[num4];
			num = num4;
		}
		this.m_data[num] = t;
	}

	// Token: 0x0600918F RID: 37263 RVA: 0x003D92DC File Offset: 0x003D74DC
	private void EnsureSort()
	{
		if (this.m_sorted)
		{
			return;
		}
		Array.Sort<T>(this.m_data, 0, this.m_count);
		this.m_sorted = true;
	}

	// Token: 0x06009190 RID: 37264 RVA: 0x003D9304 File Offset: 0x003D7504
	private static int Parent(int index)
	{
		return index - 1 >> 1;
	}

	// Token: 0x06009191 RID: 37265 RVA: 0x003D930C File Offset: 0x003D750C
	private static int Child1(int index)
	{
		return (index << 1) + 1;
	}

	// Token: 0x06009192 RID: 37266 RVA: 0x003D9314 File Offset: 0x003D7514
	private static int Child2(int index)
	{
		return (index << 1) + 2;
	}

	// Token: 0x06009193 RID: 37267 RVA: 0x003D931C File Offset: 0x003D751C
	public BinaryHeap<T> Copy()
	{
		return new BinaryHeap<T>(this.m_data, this.m_count);
	}

	// Token: 0x06009194 RID: 37268 RVA: 0x003D9330 File Offset: 0x003D7530
	public IEnumerator<T> GetEnumerator()
	{
		this.EnsureSort();
		for (int i = 0; i < this.m_count; i++)
		{
			yield return this.m_data[i];
		}
		yield break;
	}

	// Token: 0x06009195 RID: 37269 RVA: 0x003D934C File Offset: 0x003D754C
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x06009196 RID: 37270 RVA: 0x003D9354 File Offset: 0x003D7554
	public bool Contains(T item)
	{
		this.EnsureSort();
		return Array.BinarySearch<T>(this.m_data, 0, this.m_count, item) >= 0;
	}

	// Token: 0x06009197 RID: 37271 RVA: 0x003D9378 File Offset: 0x003D7578
	public void CopyTo(T[] array, int arrayIndex)
	{
		this.EnsureSort();
		Array.Copy(this.m_data, array, this.m_count);
	}

	// Token: 0x170015D5 RID: 5589
	// (get) Token: 0x06009198 RID: 37272 RVA: 0x003D9394 File Offset: 0x003D7594
	public bool IsReadOnly
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06009199 RID: 37273 RVA: 0x003D9398 File Offset: 0x003D7598
	public bool Remove(T item)
	{
		this.EnsureSort();
		int num = Array.BinarySearch<T>(this.m_data, 0, this.m_count, item);
		if (num < 0)
		{
			return false;
		}
		Array.Copy(this.m_data, num + 1, this.m_data, num, this.m_count - num);
		this.m_data[this.m_count] = default(T);
		this.m_count--;
		return true;
	}

	// Token: 0x040099CE RID: 39374
	private const int c_defaultSize = 4;

	// Token: 0x040099CF RID: 39375
	private T[] m_data = new T[4];

	// Token: 0x040099D0 RID: 39376
	private int m_count;

	// Token: 0x040099D1 RID: 39377
	private int m_capacity = 4;

	// Token: 0x040099D2 RID: 39378
	private bool m_sorted;
}
