using System;

namespace InControl
{
	// Token: 0x0200080D RID: 2061
	internal class RingBuffer<T>
	{
		// Token: 0x06002BB0 RID: 11184 RVA: 0x000DD754 File Offset: 0x000DB954
		public RingBuffer(int size)
		{
			if (size <= 0)
			{
				throw new ArgumentException("RingBuffer size must be 1 or greater.");
			}
			this.size = size + 1;
			this.data = new T[this.size];
			this.head = 0;
			this.tail = 0;
			this.sync = new object();
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x000DD7AC File Offset: 0x000DB9AC
		public void Enqueue(T value)
		{
			object obj = this.sync;
			lock (obj)
			{
				if (this.size > 1)
				{
					this.head = (this.head + 1) % this.size;
					if (this.head == this.tail)
					{
						this.tail = (this.tail + 1) % this.size;
					}
				}
				this.data[this.head] = value;
			}
		}

		// Token: 0x06002BB2 RID: 11186 RVA: 0x000DD83C File Offset: 0x000DBA3C
		public T Dequeue()
		{
			object obj = this.sync;
			T t;
			lock (obj)
			{
				if (this.size > 1 && this.tail != this.head)
				{
					this.tail = (this.tail + 1) % this.size;
				}
				t = this.data[this.tail];
			}
			return t;
		}

		// Token: 0x04001DE1 RID: 7649
		private int size;

		// Token: 0x04001DE2 RID: 7650
		private T[] data;

		// Token: 0x04001DE3 RID: 7651
		private int head;

		// Token: 0x04001DE4 RID: 7652
		private int tail;

		// Token: 0x04001DE5 RID: 7653
		private object sync;
	}
}
