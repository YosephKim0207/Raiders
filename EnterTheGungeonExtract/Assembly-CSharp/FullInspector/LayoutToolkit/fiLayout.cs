using System;
using UnityEngine;

namespace FullInspector.LayoutToolkit
{
	// Token: 0x0200060C RID: 1548
	public abstract class fiLayout
	{
		// Token: 0x06002432 RID: 9266
		public abstract bool RespondsTo(string id);

		// Token: 0x06002433 RID: 9267
		public abstract Rect GetSectionRect(string id, Rect initial);

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06002434 RID: 9268
		public abstract float Height { get; }
	}
}
