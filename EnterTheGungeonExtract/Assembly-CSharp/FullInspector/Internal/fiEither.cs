using System;

namespace FullInspector.Internal
{
	// Token: 0x02000551 RID: 1361
	public struct fiEither<TA, TB>
	{
		// Token: 0x06002055 RID: 8277 RVA: 0x0008FA8C File Offset: 0x0008DC8C
		public fiEither(TA valueA)
		{
			this._hasA = true;
			this._valueA = valueA;
			this._valueB = default(TB);
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x0008FAB8 File Offset: 0x0008DCB8
		public fiEither(TB valueB)
		{
			this._hasA = false;
			this._valueA = default(TA);
			this._valueB = valueB;
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06002057 RID: 8279 RVA: 0x0008FAE4 File Offset: 0x0008DCE4
		public TA ValueA
		{
			get
			{
				if (!this.IsA)
				{
					throw new InvalidOperationException("Either does not contain value A");
				}
				return this._valueA;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06002058 RID: 8280 RVA: 0x0008FB04 File Offset: 0x0008DD04
		public TB ValueB
		{
			get
			{
				if (!this.IsB)
				{
					throw new InvalidOperationException("Either does not contain value B");
				}
				return this._valueB;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06002059 RID: 8281 RVA: 0x0008FB24 File Offset: 0x0008DD24
		public bool IsA
		{
			get
			{
				return this._hasA;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x0600205A RID: 8282 RVA: 0x0008FB2C File Offset: 0x0008DD2C
		public bool IsB
		{
			get
			{
				return !this._hasA;
			}
		}

		// Token: 0x04001796 RID: 6038
		private TA _valueA;

		// Token: 0x04001797 RID: 6039
		private TB _valueB;

		// Token: 0x04001798 RID: 6040
		private bool _hasA;
	}
}
