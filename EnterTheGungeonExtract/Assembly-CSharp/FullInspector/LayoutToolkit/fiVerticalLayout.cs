using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FullInspector.LayoutToolkit
{
	// Token: 0x02000610 RID: 1552
	public class fiVerticalLayout : fiLayout, IEnumerable
	{
		// Token: 0x06002442 RID: 9282 RVA: 0x0009DBB4 File Offset: 0x0009BDB4
		public void Add(fiLayout rule)
		{
			this.Add(string.Empty, rule);
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x0009DBC4 File Offset: 0x0009BDC4
		public void Add(string sectionId, fiLayout rule)
		{
			this._items.Add(new fiVerticalLayout.SectionItem
			{
				Id = sectionId,
				Rule = rule
			});
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x0009DBF8 File Offset: 0x0009BDF8
		public void Add(string sectionId, float height)
		{
			this.Add(sectionId, new fiLayoutHeight(sectionId, height));
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x0009DC08 File Offset: 0x0009BE08
		public void Add(float height)
		{
			this.Add(string.Empty, height);
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x0009DC18 File Offset: 0x0009BE18
		public override Rect GetSectionRect(string sectionId, Rect initial)
		{
			for (int i = 0; i < this._items.Count; i++)
			{
				fiVerticalLayout.SectionItem sectionItem = this._items[i];
				if (sectionItem.Id == sectionId || sectionItem.Rule.RespondsTo(sectionId))
				{
					if (sectionItem.Rule.RespondsTo(sectionId))
					{
						initial = sectionItem.Rule.GetSectionRect(sectionId, initial);
					}
					else
					{
						initial.height = sectionItem.Rule.Height;
					}
					break;
				}
				initial.y += sectionItem.Rule.Height;
			}
			return initial;
		}

		// Token: 0x06002447 RID: 9287 RVA: 0x0009DCCC File Offset: 0x0009BECC
		public override bool RespondsTo(string sectionId)
		{
			for (int i = 0; i < this._items.Count; i++)
			{
				if (this._items[i].Id == sectionId || this._items[i].Rule.RespondsTo(sectionId))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002448 RID: 9288 RVA: 0x0009DD38 File Offset: 0x0009BF38
		public override float Height
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this._items.Count; i++)
				{
					num += this._items[i].Rule.Height;
				}
				return num;
			}
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x0009DD84 File Offset: 0x0009BF84
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400191B RID: 6427
		private List<fiVerticalLayout.SectionItem> _items = new List<fiVerticalLayout.SectionItem>();

		// Token: 0x02000611 RID: 1553
		private struct SectionItem
		{
			// Token: 0x0400191C RID: 6428
			public string Id;

			// Token: 0x0400191D RID: 6429
			public fiLayout Rule;
		}
	}
}
