using System;

namespace FullSerializer.Internal
{
	// Token: 0x020005B5 RID: 1461
	public struct fsOption<T>
	{
		// Token: 0x060022BA RID: 8890 RVA: 0x00099250 File Offset: 0x00097450
		public fsOption(T value)
		{
			this._hasValue = true;
			this._value = value;
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x060022BB RID: 8891 RVA: 0x00099260 File Offset: 0x00097460
		public bool HasValue
		{
			get
			{
				return this._hasValue;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x060022BC RID: 8892 RVA: 0x00099268 File Offset: 0x00097468
		public bool IsEmpty
		{
			get
			{
				return !this._hasValue;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x060022BD RID: 8893 RVA: 0x00099274 File Offset: 0x00097474
		public T Value
		{
			get
			{
				if (this.IsEmpty)
				{
					throw new InvalidOperationException("fsOption is empty");
				}
				return this._value;
			}
		}

		// Token: 0x0400186E RID: 6254
		private bool _hasValue;

		// Token: 0x0400186F RID: 6255
		private T _value;

		// Token: 0x04001870 RID: 6256
		public static fsOption<T> Empty;
	}
}
