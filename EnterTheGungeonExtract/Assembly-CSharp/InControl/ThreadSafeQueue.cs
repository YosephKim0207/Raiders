using System;
using System.Collections.Generic;

namespace InControl
{
	// Token: 0x02000810 RID: 2064
	internal class ThreadSafeQueue<T>
	{
		// Token: 0x06002BBC RID: 11196 RVA: 0x000DDCB0 File Offset: 0x000DBEB0
		public ThreadSafeQueue()
		{
			this.sync = new object();
			this.data = new Queue<T>();
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x000DDCD0 File Offset: 0x000DBED0
		public ThreadSafeQueue(int capacity)
		{
			this.sync = new object();
			this.data = new Queue<T>(capacity);
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x000DDCF0 File Offset: 0x000DBEF0
		public void Enqueue(T item)
		{
			object obj = this.sync;
			lock (obj)
			{
				this.data.Enqueue(item);
			}
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x000DDD34 File Offset: 0x000DBF34
		public bool Dequeue(out T item)
		{
			object obj = this.sync;
			lock (obj)
			{
				if (this.data.Count > 0)
				{
					item = this.data.Dequeue();
					return true;
				}
			}
			item = default(T);
			return false;
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x000DDDA8 File Offset: 0x000DBFA8
		public T Dequeue()
		{
			object obj = this.sync;
			lock (obj)
			{
				if (this.data.Count > 0)
				{
					return this.data.Dequeue();
				}
			}
			return default(T);
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x000DDE0C File Offset: 0x000DC00C
		public int Dequeue(ref IList<T> list)
		{
			object obj = this.sync;
			int num;
			lock (obj)
			{
				int count = this.data.Count;
				for (int i = 0; i < count; i++)
				{
					list.Add(this.data.Dequeue());
				}
				num = count;
			}
			return num;
		}

		// Token: 0x04001DEA RID: 7658
		private object sync;

		// Token: 0x04001DEB RID: 7659
		private Queue<T> data;
	}
}
