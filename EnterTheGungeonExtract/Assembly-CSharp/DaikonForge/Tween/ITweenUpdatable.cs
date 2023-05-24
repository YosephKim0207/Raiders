using System;

namespace DaikonForge.Tween
{
	// Token: 0x0200051B RID: 1307
	public interface ITweenUpdatable
	{
		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06001F6E RID: 8046
		TweenState State { get; }

		// Token: 0x06001F6F RID: 8047
		void Update();
	}
}
