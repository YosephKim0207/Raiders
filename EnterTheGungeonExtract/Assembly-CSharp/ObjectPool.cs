using System;
using System.Diagnostics;
using System.Threading;

// Token: 0x020015AA RID: 5546
public class ObjectPool<T> where T : class
{
	// Token: 0x06007F41 RID: 32577 RVA: 0x00336438 File Offset: 0x00334638
	public ObjectPool(ObjectPool<T>.Factory factory, ObjectPool<T>.Cleanup cleanup = null)
		: this(factory, Environment.ProcessorCount * 2, cleanup)
	{
	}

	// Token: 0x06007F42 RID: 32578 RVA: 0x0033644C File Offset: 0x0033464C
	public ObjectPool(ObjectPool<T>.Factory factory, int size, ObjectPool<T>.Cleanup cleanup = null)
	{
		this._factory = factory;
		this._items = new ObjectPool<T>.Element[size - 1];
		this._cleanup = cleanup;
	}

	// Token: 0x06007F43 RID: 32579 RVA: 0x00336470 File Offset: 0x00334670
	private T CreateInstance()
	{
		return this._factory();
	}

	// Token: 0x06007F44 RID: 32580 RVA: 0x0033648C File Offset: 0x0033468C
	public T Allocate()
	{
		T t = this._firstItem;
		if (t == null || t != Interlocked.CompareExchange<T>(ref this._firstItem, (T)((object)null), t))
		{
			t = this.AllocateSlow();
		}
		return t;
	}

	// Token: 0x06007F45 RID: 32581 RVA: 0x003364D8 File Offset: 0x003346D8
	private T AllocateSlow()
	{
		ObjectPool<T>.Element[] items = this._items;
		for (int i = 0; i < items.Length; i++)
		{
			T value = items[i].Value;
			if (value != null && value == Interlocked.CompareExchange<T>(ref items[i].Value, (T)((object)null), value))
			{
				return value;
			}
		}
		return this.CreateInstance();
	}

	// Token: 0x06007F46 RID: 32582 RVA: 0x00336548 File Offset: 0x00334748
	public void Clear()
	{
		this._firstItem = (T)((object)null);
		ObjectPool<T>.Element[] items = this._items;
		for (int i = 0; i < items.Length; i++)
		{
			items[i].Value = (T)((object)null);
		}
	}

	// Token: 0x06007F47 RID: 32583 RVA: 0x00336590 File Offset: 0x00334790
	public void Free(ref T obj)
	{
		if (obj == null)
		{
			return;
		}
		if (this._firstItem == null)
		{
			if (this._cleanup != null)
			{
				this._cleanup(obj);
			}
			this._firstItem = obj;
		}
		else
		{
			this.FreeSlow(obj);
		}
		obj = (T)((object)null);
	}

	// Token: 0x06007F48 RID: 32584 RVA: 0x00336604 File Offset: 0x00334804
	private void FreeSlow(T obj)
	{
		ObjectPool<T>.Element[] items = this._items;
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].Value == null)
			{
				if (this._cleanup != null)
				{
					this._cleanup(obj);
				}
				items[i].Value = obj;
				break;
			}
		}
	}

	// Token: 0x040081F2 RID: 33266
	private T _firstItem;

	// Token: 0x040081F3 RID: 33267
	private readonly ObjectPool<T>.Element[] _items;

	// Token: 0x040081F4 RID: 33268
	private readonly ObjectPool<T>.Factory _factory;

	// Token: 0x040081F5 RID: 33269
	private readonly ObjectPool<T>.Cleanup _cleanup;

	// Token: 0x020015AB RID: 5547
	[DebuggerDisplay("{Value,nq}")]
	private struct Element
	{
		// Token: 0x040081F6 RID: 33270
		public T Value;
	}

	// Token: 0x020015AC RID: 5548
	// (Invoke) Token: 0x06007F4A RID: 32586
	public delegate T Factory();

	// Token: 0x020015AD RID: 5549
	// (Invoke) Token: 0x06007F4E RID: 32590
	public delegate void Cleanup(T obj);
}
