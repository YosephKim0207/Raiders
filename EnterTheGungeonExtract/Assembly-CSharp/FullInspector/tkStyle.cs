using System;

namespace FullInspector
{
	// Token: 0x0200066B RID: 1643
	public abstract class tkStyle<T, TContext>
	{
		// Token: 0x06002582 RID: 9602
		public abstract void Activate(T obj, TContext context);

		// Token: 0x06002583 RID: 9603
		public abstract void Deactivate(T obj, TContext context);
	}
}
