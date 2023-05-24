using System;
using UnityEngine;

namespace FullInspector.LayoutToolkit
{
	// Token: 0x0200060D RID: 1549
	public class fiLayoutHeight : fiLayout
	{
		// Token: 0x06002435 RID: 9269 RVA: 0x0009DA40 File Offset: 0x0009BC40
		public fiLayoutHeight(float height)
		{
			this._id = string.Empty;
			this._height = height;
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x0009DA5C File Offset: 0x0009BC5C
		public fiLayoutHeight(string sectionId, float height)
		{
			this._id = sectionId;
			this._height = height;
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x0009DA74 File Offset: 0x0009BC74
		public override bool RespondsTo(string sectionId)
		{
			return this._id == sectionId;
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x0009DA84 File Offset: 0x0009BC84
		public override Rect GetSectionRect(string sectionId, Rect initial)
		{
			initial.height = this._height;
			return initial;
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x0009DA94 File Offset: 0x0009BC94
		public void SetHeight(float height)
		{
			this._height = height;
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x0600243A RID: 9274 RVA: 0x0009DAA0 File Offset: 0x0009BCA0
		public override float Height
		{
			get
			{
				return this._height;
			}
		}

		// Token: 0x04001917 RID: 6423
		private string _id;

		// Token: 0x04001918 RID: 6424
		private float _height;
	}
}
