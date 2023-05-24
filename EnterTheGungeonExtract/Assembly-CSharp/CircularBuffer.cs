using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000E4F RID: 3663
public class CircularBuffer<T> : IEnumerable<T>, IEnumerable
{
	// Token: 0x06004DF9 RID: 19961 RVA: 0x001AE5B8 File Offset: 0x001AC7B8
	public CircularBuffer(int capacity)
	{
		this.m_buffer = new T[capacity];
		this.m_head = capacity - 1;
	}

	// Token: 0x17000B07 RID: 2823
	// (get) Token: 0x06004DFA RID: 19962 RVA: 0x001AE5D8 File Offset: 0x001AC7D8
	// (set) Token: 0x06004DFB RID: 19963 RVA: 0x001AE5E0 File Offset: 0x001AC7E0
	public int Count { get; private set; }

	// Token: 0x17000B08 RID: 2824
	// (get) Token: 0x06004DFC RID: 19964 RVA: 0x001AE5EC File Offset: 0x001AC7EC
	// (set) Token: 0x06004DFD RID: 19965 RVA: 0x001AE5F8 File Offset: 0x001AC7F8
	public int Capacity
	{
		get
		{
			return this.m_buffer.Length;
		}
		set
		{
			if (value == this.m_buffer.Length)
			{
				return;
			}
			T[] array = new T[value];
			int num = 0;
			while (this.Count > 0 && num < value)
			{
				array[num++] = this.Dequeue();
			}
			this.m_buffer = array;
			this.Count = num;
			this.m_head = num - 1;
			this.m_tail = 0;
		}
	}

	// Token: 0x06004DFE RID: 19966 RVA: 0x001AE664 File Offset: 0x001AC864
	public T Enqueue(T item)
	{
		this.m_head = (this.m_head + 1) % this.Capacity;
		T t = this.m_buffer[this.m_head];
		this.m_buffer[this.m_head] = item;
		if (this.Count == this.Capacity)
		{
			this.m_tail = (this.m_tail + 1) % this.Capacity;
		}
		else
		{
			this.Count++;
		}
		return t;
	}

	// Token: 0x06004DFF RID: 19967 RVA: 0x001AE6E4 File Offset: 0x001AC8E4
	public T Dequeue()
	{
		if (this.Count == 0)
		{
			throw new InvalidOperationException("queue exhausted");
		}
		T t = this.m_buffer[this.m_tail];
		this.m_buffer[this.m_tail] = default(T);
		this.m_tail = (this.m_tail + 1) % this.Capacity;
		this.Count--;
		return t;
	}

	// Token: 0x06004E00 RID: 19968 RVA: 0x001AE758 File Offset: 0x001AC958
	public void Clear()
	{
		this.m_head = this.Capacity - 1;
		this.m_tail = 0;
		this.Count = 0;
	}

	// Token: 0x17000B09 RID: 2825
	public T this[int index]
	{
		get
		{
			if (index < 0 || index >= this.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this.m_buffer[(this.m_tail + index) % this.Capacity];
		}
		set
		{
			if (index < 0 || index >= this.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.m_buffer[(this.m_tail + index) % this.Capacity] = value;
		}
	}

	// Token: 0x06004E03 RID: 19971 RVA: 0x001AE7F0 File Offset: 0x001AC9F0
	public int IndexOf(T item)
	{
		for (int i = 0; i < this.Count; i++)
		{
			if (object.Equals(item, this[i]))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06004E04 RID: 19972 RVA: 0x001AE834 File Offset: 0x001ACA34
	public void Insert(int index, T item)
	{
		if (index < 0 || index > this.Count)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (this.Count == index)
		{
			this.Enqueue(item);
		}
		else
		{
			T t = this[this.Count - 1];
			for (int i = index; i < this.Count - 2; i++)
			{
				this[i + 1] = this[i];
			}
			this[index] = item;
			this.Enqueue(t);
		}
	}

	// Token: 0x06004E05 RID: 19973 RVA: 0x001AE8C0 File Offset: 0x001ACAC0
	public void RemoveAt(int index)
	{
		if (index < 0 || index >= this.Count)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		for (int i = index; i > 0; i--)
		{
			this[i] = this[i - 1];
		}
		this.Dequeue();
	}

	// Token: 0x06004E06 RID: 19974 RVA: 0x001AE914 File Offset: 0x001ACB14
	public IEnumerator<T> GetEnumerator()
	{
		if (this.Count == 0 || this.Capacity == 0)
		{
			yield break;
		}
		for (int i = 0; i < this.Count; i++)
		{
			yield return this[i];
		}
		yield break;
	}

	// Token: 0x06004E07 RID: 19975 RVA: 0x001AE930 File Offset: 0x001ACB30
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x04004452 RID: 17490
	private T[] m_buffer;

	// Token: 0x04004453 RID: 17491
	private int m_head;

	// Token: 0x04004454 RID: 17492
	private int m_tail;
}
