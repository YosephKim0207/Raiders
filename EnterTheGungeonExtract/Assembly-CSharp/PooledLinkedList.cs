using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200036F RID: 879
public class PooledLinkedList<T> : ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x06000E79 RID: 3705 RVA: 0x00044668 File Offset: 0x00042868
	public void ClearPool()
	{
		this.m_pool.Clear();
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x00044678 File Offset: 0x00042878
	public LinkedListNode<T> GetByIndexSlow(int index)
	{
		if (this.m_list.Count == 0)
		{
			throw new IndexOutOfRangeException();
		}
		LinkedListNode<T> linkedListNode = this.m_list.First;
		for (int i = 0; i < index; i++)
		{
			linkedListNode = linkedListNode.Next;
			if (linkedListNode == null)
			{
				throw new IndexOutOfRangeException();
			}
		}
		return linkedListNode;
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x000446D0 File Offset: 0x000428D0
	public void AddAfter(LinkedListNode<T> node, T value)
	{
		this.m_list.AddAfter(node, value);
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x000446E0 File Offset: 0x000428E0
	public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
	{
		this.m_list.AddAfter(node, newNode);
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x000446F0 File Offset: 0x000428F0
	public void AddBefore(LinkedListNode<T> node, T value)
	{
		this.m_list.AddBefore(node, value);
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x00044700 File Offset: 0x00042900
	public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
	{
		this.m_list.AddBefore(node, newNode);
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x00044710 File Offset: 0x00042910
	public void AddFirst(T value)
	{
		if (this.m_pool.Count > 0)
		{
			LinkedListNode<T> first = this.m_pool.First;
			this.m_pool.RemoveFirst();
			first.Value = value;
			this.m_list.AddFirst(first);
		}
		else
		{
			this.m_list.AddFirst(value);
		}
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x0004476C File Offset: 0x0004296C
	public void AddFirst(LinkedListNode<T> node)
	{
		this.m_list.AddFirst(node);
	}

	// Token: 0x06000E81 RID: 3713 RVA: 0x0004477C File Offset: 0x0004297C
	public void AddLast(T value)
	{
		if (this.m_pool.Count > 0)
		{
			LinkedListNode<T> first = this.m_pool.First;
			this.m_pool.RemoveFirst();
			first.Value = value;
			this.m_list.AddLast(first);
		}
		else
		{
			this.m_list.AddLast(value);
		}
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x000447D8 File Offset: 0x000429D8
	public void AddLast(LinkedListNode<T> node)
	{
		this.m_list.AddLast(node);
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x000447E8 File Offset: 0x000429E8
	public bool Remove(T value)
	{
		LinkedListNode<T> linkedListNode = this.m_list.Find(value);
		if (linkedListNode == null)
		{
			return false;
		}
		this.m_list.Remove(linkedListNode);
		linkedListNode.Value = default(T);
		this.m_pool.AddLast(linkedListNode);
		return true;
	}

	// Token: 0x06000E84 RID: 3716 RVA: 0x00044834 File Offset: 0x00042A34
	public void Remove(LinkedListNode<T> node, bool returnToPool)
	{
		this.m_list.Remove(node);
		if (returnToPool)
		{
			node.Value = default(T);
			this.m_pool.AddLast(node);
		}
	}

	// Token: 0x06000E85 RID: 3717 RVA: 0x00044870 File Offset: 0x00042A70
	public void RemoveFirst()
	{
		LinkedListNode<T> first = this.m_list.First;
		this.m_list.Remove(first);
		first.Value = default(T);
		this.m_pool.AddLast(first);
	}

	// Token: 0x06000E86 RID: 3718 RVA: 0x000448B0 File Offset: 0x00042AB0
	public void RemoveLast()
	{
		LinkedListNode<T> last = this.m_list.Last;
		this.m_list.Remove(last);
		last.Value = default(T);
		this.m_pool.AddLast(last);
	}

	// Token: 0x17000339 RID: 825
	// (get) Token: 0x06000E87 RID: 3719 RVA: 0x000448F0 File Offset: 0x00042AF0
	public LinkedListNode<T> First
	{
		get
		{
			return this.m_list.First;
		}
	}

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x06000E88 RID: 3720 RVA: 0x00044900 File Offset: 0x00042B00
	public LinkedListNode<T> Last
	{
		get
		{
			return this.m_list.Last;
		}
	}

	// Token: 0x1700033B RID: 827
	// (get) Token: 0x06000E89 RID: 3721 RVA: 0x00044910 File Offset: 0x00042B10
	public int Count
	{
		get
		{
			return this.m_list.Count;
		}
	}

	// Token: 0x1700033C RID: 828
	// (get) Token: 0x06000E8A RID: 3722 RVA: 0x00044920 File Offset: 0x00042B20
	public bool IsReadOnly
	{
		get
		{
			return ((ICollection<T>)this.m_list).IsReadOnly;
		}
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x00044930 File Offset: 0x00042B30
	void ICollection<T>.Add(T item)
	{
		this.AddLast(item);
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x0004493C File Offset: 0x00042B3C
	public void Clear()
	{
		while (this.m_list.Count > 0)
		{
			this.RemoveLast();
		}
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x0004495C File Offset: 0x00042B5C
	public bool Contains(T item)
	{
		return this.m_list.Contains(item);
	}

	// Token: 0x06000E8E RID: 3726 RVA: 0x0004496C File Offset: 0x00042B6C
	public void CopyTo(T[] array, int arrayIndex)
	{
		this.m_list.CopyTo(array, arrayIndex);
	}

	// Token: 0x06000E8F RID: 3727 RVA: 0x0004497C File Offset: 0x00042B7C
	public IEnumerator<T> GetEnumerator()
	{
		return this.m_list.GetEnumerator();
	}

	// Token: 0x06000E90 RID: 3728 RVA: 0x00044990 File Offset: 0x00042B90
	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)this.m_list).GetEnumerator();
	}

	// Token: 0x04000E4B RID: 3659
	private LinkedList<T> m_list = new LinkedList<T>();

	// Token: 0x04000E4C RID: 3660
	private LinkedList<T> m_pool = new LinkedList<T>();
}
