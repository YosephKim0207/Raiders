using System;

namespace FullInspector.Internal
{
	// Token: 0x02000577 RID: 1399
	public struct fiOption<T>
	{
		// Token: 0x06002101 RID: 8449 RVA: 0x000919B0 File Offset: 0x0008FBB0
		public fiOption(T value)
		{
			this._hasValue = true;
			this._value = value;
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06002102 RID: 8450 RVA: 0x000919C0 File Offset: 0x0008FBC0
		public bool HasValue
		{
			get
			{
				return this._hasValue;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06002103 RID: 8451 RVA: 0x000919C8 File Offset: 0x0008FBC8
		public bool IsEmpty
		{
			get
			{
				return !this._hasValue;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06002104 RID: 8452 RVA: 0x000919D4 File Offset: 0x0008FBD4
		public T Value
		{
			get
			{
				if (!this.HasValue)
				{
					throw new InvalidOperationException("There is no value inside the option");
				}
				return this._value;
			}
		}

		// Token: 0x040017D9 RID: 6105
		private bool _hasValue;

		// Token: 0x040017DA RID: 6106
		private T _value;

		// Token: 0x040017DB RID: 6107
		public static fiOption<T> Empty = new fiOption<T>
		{
			_hasValue = false,
			_value = default(T)
		};
	}
}
