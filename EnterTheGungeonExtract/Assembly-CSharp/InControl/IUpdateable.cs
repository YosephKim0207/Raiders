using System;

namespace InControl
{
	// Token: 0x0200080B RID: 2059
	public interface IUpdateable
	{
		// Token: 0x06002BAD RID: 11181
		void Update(ulong updateTick, float deltaTime);
	}
}
