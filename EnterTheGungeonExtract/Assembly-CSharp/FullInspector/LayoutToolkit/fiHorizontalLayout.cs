using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FullInspector.LayoutToolkit
{
	// Token: 0x0200060A RID: 1546
	public class fiHorizontalLayout : fiLayout, IEnumerable
	{
		// Token: 0x06002421 RID: 9249 RVA: 0x0009D6F4 File Offset: 0x0009B8F4
		public fiHorizontalLayout()
		{
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x0009D714 File Offset: 0x0009B914
		public fiHorizontalLayout(fiLayout defaultRule)
		{
			this._defaultRule = defaultRule;
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x0009D73C File Offset: 0x0009B93C
		public void Add(fiLayout rule)
		{
			this.ActualAdd(string.Empty, 0f, fiExpandMode.Expand, rule);
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x0009D750 File Offset: 0x0009B950
		public void Add(float width)
		{
			this.ActualAdd(string.Empty, width, fiExpandMode.Fixed, this._defaultRule);
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x0009D768 File Offset: 0x0009B968
		public void Add(string id)
		{
			this.ActualAdd(id, 0f, fiExpandMode.Expand, this._defaultRule);
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x0009D780 File Offset: 0x0009B980
		public void Add(string id, float width)
		{
			this.ActualAdd(id, width, fiExpandMode.Fixed, this._defaultRule);
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x0009D794 File Offset: 0x0009B994
		public void Add(string id, fiLayout rule)
		{
			this.ActualAdd(id, 0f, fiExpandMode.Expand, rule);
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x0009D7A4 File Offset: 0x0009B9A4
		public void Add(float width, fiLayout rule)
		{
			this.ActualAdd(string.Empty, width, fiExpandMode.Fixed, rule);
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x0009D7B4 File Offset: 0x0009B9B4
		public void Add(string id, float width, fiLayout rule)
		{
			this.ActualAdd(id, width, fiExpandMode.Fixed, rule);
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x0009D7C0 File Offset: 0x0009B9C0
		private void ActualAdd(string id, float width, fiExpandMode expandMode, fiLayout rule)
		{
			this._items.Add(new fiHorizontalLayout.SectionItem
			{
				Id = id,
				MinWidth = width,
				ExpandMode = expandMode,
				Rule = rule
			});
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x0600242B RID: 9259 RVA: 0x0009D804 File Offset: 0x0009BA04
		private int ExpandCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this._items.Count; i++)
				{
					if (this._items[i].ExpandMode == fiExpandMode.Expand)
					{
						num++;
					}
				}
				if (num == 0)
				{
					num = 1;
				}
				return num;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x0600242C RID: 9260 RVA: 0x0009D858 File Offset: 0x0009BA58
		private float MinimumWidth
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this._items.Count; i++)
				{
					num += this._items[i].MinWidth;
				}
				return num;
			}
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x0009D8A0 File Offset: 0x0009BAA0
		public override Rect GetSectionRect(string sectionId, Rect initial)
		{
			float num = initial.width - this.MinimumWidth;
			if (num < 0f)
			{
				num = 0f;
			}
			float num2 = 1f / (float)this.ExpandCount;
			for (int i = 0; i < this._items.Count; i++)
			{
				fiHorizontalLayout.SectionItem sectionItem = this._items[i];
				float num3 = sectionItem.MinWidth;
				if (sectionItem.ExpandMode == fiExpandMode.Expand)
				{
					num3 += num * num2;
				}
				if (sectionItem.Id == sectionId || sectionItem.Rule.RespondsTo(sectionId))
				{
					initial.width = num3;
					initial = sectionItem.Rule.GetSectionRect(sectionId, initial);
					break;
				}
				initial.x += num3;
			}
			return initial;
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x0009D974 File Offset: 0x0009BB74
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

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x0600242F RID: 9263 RVA: 0x0009D9E0 File Offset: 0x0009BBE0
		public override float Height
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this._items.Count; i++)
				{
					num = Math.Max(num, this._items[i].Rule.Height);
				}
				return num;
			}
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x0009DA30 File Offset: 0x0009BC30
		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04001911 RID: 6417
		private List<fiHorizontalLayout.SectionItem> _items = new List<fiHorizontalLayout.SectionItem>();

		// Token: 0x04001912 RID: 6418
		private fiLayout _defaultRule = new fiVerticalLayout();

		// Token: 0x0200060B RID: 1547
		private struct SectionItem
		{
			// Token: 0x04001913 RID: 6419
			public string Id;

			// Token: 0x04001914 RID: 6420
			public float MinWidth;

			// Token: 0x04001915 RID: 6421
			public fiExpandMode ExpandMode;

			// Token: 0x04001916 RID: 6422
			public fiLayout Rule;
		}
	}
}
