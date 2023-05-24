using System;
using System.Reflection;

namespace FullInspector
{
	// Token: 0x020005D2 RID: 1490
	public struct InspectedMember
	{
		// Token: 0x06002350 RID: 9040 RVA: 0x0009ACC0 File Offset: 0x00098EC0
		public InspectedMember(InspectedProperty property)
		{
			this._property = property;
			this._method = null;
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x0009ACD0 File Offset: 0x00098ED0
		public InspectedMember(InspectedMethod method)
		{
			this._property = null;
			this._method = method;
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06002352 RID: 9042 RVA: 0x0009ACE0 File Offset: 0x00098EE0
		public InspectedProperty Property
		{
			get
			{
				if (!this.IsProperty)
				{
					throw new InvalidOperationException("Member is not a property");
				}
				return this._property;
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002353 RID: 9043 RVA: 0x0009AD00 File Offset: 0x00098F00
		public InspectedMethod Method
		{
			get
			{
				if (!this.IsMethod)
				{
					throw new InvalidOperationException("Member is not a method");
				}
				return this._method;
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002354 RID: 9044 RVA: 0x0009AD20 File Offset: 0x00098F20
		public bool IsMethod
		{
			get
			{
				return this._method != null;
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002355 RID: 9045 RVA: 0x0009AD30 File Offset: 0x00098F30
		public bool IsProperty
		{
			get
			{
				return this._property != null;
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06002356 RID: 9046 RVA: 0x0009AD40 File Offset: 0x00098F40
		public string Name
		{
			get
			{
				return this.MemberInfo.Name;
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06002357 RID: 9047 RVA: 0x0009AD50 File Offset: 0x00098F50
		public MemberInfo MemberInfo
		{
			get
			{
				if (this.IsMethod)
				{
					return this._method.Method;
				}
				return this._property.MemberInfo;
			}
		}

		// Token: 0x0400189A RID: 6298
		private InspectedProperty _property;

		// Token: 0x0400189B RID: 6299
		private InspectedMethod _method;
	}
}
