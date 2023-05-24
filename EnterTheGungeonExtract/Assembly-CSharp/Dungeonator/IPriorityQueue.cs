using System;

namespace Dungeonator
{
	// Token: 0x02000EF0 RID: 3824
	public interface IPriorityQueue<T>
	{
		// Token: 0x06005169 RID: 20841
		int Push(T item);

		// Token: 0x0600516A RID: 20842
		T Pop();

		// Token: 0x0600516B RID: 20843
		T Peek();

		// Token: 0x0600516C RID: 20844
		void Update(int i);
	}
}
