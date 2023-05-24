using System;
using UnityEngine;

namespace FullInspector.LayoutToolkit
{
	// Token: 0x0200060F RID: 1551
	public class fiCenterVertical : fiLayout
	{
		// Token: 0x0600243C RID: 9276 RVA: 0x0009DAEC File Offset: 0x0009BCEC
		public fiCenterVertical(string id, fiLayout centered)
		{
			this._id = id;
			this._centered = centered;
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x0009DB04 File Offset: 0x0009BD04
		public fiCenterVertical(fiLayout centered)
			: this(string.Empty, centered)
		{
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x0009DB14 File Offset: 0x0009BD14
		public override bool RespondsTo(string sectionId)
		{
			return this._id == sectionId || this._centered.RespondsTo(sectionId);
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x0009DB38 File Offset: 0x0009BD38
		public override Rect GetSectionRect(string sectionId, Rect initial)
		{
			float num = initial.height - this._centered.Height;
			initial.y += num / 2f;
			initial.height -= num;
			initial = this._centered.GetSectionRect(sectionId, initial);
			return initial;
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002440 RID: 9280 RVA: 0x0009DB90 File Offset: 0x0009BD90
		public override float Height
		{
			get
			{
				return this._centered.Height;
			}
		}

		// Token: 0x04001919 RID: 6425
		private string _id;

		// Token: 0x0400191A RID: 6426
		private fiLayout _centered;
	}
}
