using System;
using System.Collections.Generic;

namespace Dungeonator
{
	// Token: 0x02000EF1 RID: 3825
	public class PriorityQueueB<T> : IPriorityQueue<T>
	{
		// Token: 0x0600516D RID: 20845 RVA: 0x001CE258 File Offset: 0x001CC458
		public PriorityQueueB()
		{
			this.mComparer = Comparer<T>.Default;
		}

		// Token: 0x0600516E RID: 20846 RVA: 0x001CE278 File Offset: 0x001CC478
		public PriorityQueueB(IComparer<T> comparer)
		{
			this.mComparer = comparer;
		}

		// Token: 0x0600516F RID: 20847 RVA: 0x001CE294 File Offset: 0x001CC494
		public PriorityQueueB(IComparer<T> comparer, int capacity)
		{
			this.mComparer = comparer;
			this.InnerList.Capacity = capacity;
		}

		// Token: 0x06005170 RID: 20848 RVA: 0x001CE2BC File Offset: 0x001CC4BC
		protected void SwitchElements(int i, int j)
		{
			T t = this.InnerList[i];
			this.InnerList[i] = this.InnerList[j];
			this.InnerList[j] = t;
		}

		// Token: 0x06005171 RID: 20849 RVA: 0x001CE2FC File Offset: 0x001CC4FC
		protected virtual int OnCompare(int i, int j)
		{
			return this.mComparer.Compare(this.InnerList[i], this.InnerList[j]);
		}

		// Token: 0x06005172 RID: 20850 RVA: 0x001CE324 File Offset: 0x001CC524
		public int Push(T item)
		{
			int num = this.InnerList.Count;
			this.InnerList.Add(item);
			while (num != 0)
			{
				int num2 = (num - 1) / 2;
				if (this.OnCompare(num, num2) >= 0)
				{
					return num;
				}
				this.SwitchElements(num, num2);
				num = num2;
			}
			return num;
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x001CE384 File Offset: 0x001CC584
		public T Pop()
		{
			T t = this.InnerList[0];
			int num = 0;
			this.InnerList[0] = this.InnerList[this.InnerList.Count - 1];
			this.InnerList.RemoveAt(this.InnerList.Count - 1);
			for (;;)
			{
				int num2 = num;
				int num3 = 2 * num + 1;
				int num4 = 2 * num + 2;
				if (this.InnerList.Count > num3 && this.OnCompare(num, num3) > 0)
				{
					num = num3;
				}
				if (this.InnerList.Count > num4 && this.OnCompare(num, num4) > 0)
				{
					num = num4;
				}
				if (num == num2)
				{
					break;
				}
				this.SwitchElements(num, num2);
			}
			return t;
		}

		// Token: 0x06005174 RID: 20852 RVA: 0x001CE44C File Offset: 0x001CC64C
		public void Update(int i)
		{
			int num = i;
			while (num != 0)
			{
				int num2 = (num - 1) / 2;
				if (this.OnCompare(num, num2) < 0)
				{
					this.SwitchElements(num, num2);
					num = num2;
				}
				else
				{
					IL_3A:
					if (num < i)
					{
						return;
					}
					for (;;)
					{
						int num3 = num;
						int num4 = 2 * num + 1;
						num2 = 2 * num + 2;
						if (this.InnerList.Count > num4 && this.OnCompare(num, num4) > 0)
						{
							num = num4;
						}
						if (this.InnerList.Count > num2 && this.OnCompare(num, num2) > 0)
						{
							num = num2;
						}
						if (num == num3)
						{
							break;
						}
						this.SwitchElements(num, num3);
					}
					return;
				}
			}
			goto IL_3A;
		}

		// Token: 0x06005175 RID: 20853 RVA: 0x001CE504 File Offset: 0x001CC704
		public T Peek()
		{
			if (this.InnerList.Count > 0)
			{
				return this.InnerList[0];
			}
			return default(T);
		}

		// Token: 0x06005176 RID: 20854 RVA: 0x001CE538 File Offset: 0x001CC738
		public void Clear()
		{
			this.InnerList.Clear();
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06005177 RID: 20855 RVA: 0x001CE548 File Offset: 0x001CC748
		public int Count
		{
			get
			{
				return this.InnerList.Count;
			}
		}

		// Token: 0x06005178 RID: 20856 RVA: 0x001CE558 File Offset: 0x001CC758
		public void RemoveLocation(T item)
		{
			int num = -1;
			for (int i = 0; i < this.InnerList.Count; i++)
			{
				if (this.mComparer.Compare(this.InnerList[i], item) == 0)
				{
					num = i;
				}
			}
			if (num != -1)
			{
				this.InnerList.RemoveAt(num);
			}
		}

		// Token: 0x17000B89 RID: 2953
		public T this[int index]
		{
			get
			{
				return this.InnerList[index];
			}
			set
			{
				this.InnerList[index] = value;
				this.Update(index);
			}
		}

		// Token: 0x04004982 RID: 18818
		protected List<T> InnerList = new List<T>();

		// Token: 0x04004983 RID: 18819
		protected IComparer<T> mComparer;
	}
}
