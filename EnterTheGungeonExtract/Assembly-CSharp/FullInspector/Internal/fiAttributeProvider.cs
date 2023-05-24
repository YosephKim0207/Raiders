using System;
using System.Linq;
using System.Reflection;
using FullSerializer.Internal;

namespace FullInspector.Internal
{
	// Token: 0x02000549 RID: 1353
	public class fiAttributeProvider : MemberInfo
	{
		// Token: 0x0600202B RID: 8235 RVA: 0x0008F334 File Offset: 0x0008D534
		private fiAttributeProvider(object[] attributes)
		{
			this._attributes = attributes;
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x0008F344 File Offset: 0x0008D544
		public static MemberInfo Create(params object[] attributes)
		{
			return new fiAttributeProvider(attributes);
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x0600202D RID: 8237 RVA: 0x0008F34C File Offset: 0x0008D54C
		public override Type DeclaringType
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x0600202E RID: 8238 RVA: 0x0008F354 File Offset: 0x0008D554
		public override MemberTypes MemberType
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x0600202F RID: 8239 RVA: 0x0008F35C File Offset: 0x0008D55C
		public override string Name
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06002030 RID: 8240 RVA: 0x0008F364 File Offset: 0x0008D564
		public override Type ReflectedType
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x0008F36C File Offset: 0x0008D56C
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this._attributes;
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x0008F374 File Offset: 0x0008D574
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this._attributes.Where((object attr) => attr.GetType().Resolve().IsAssignableFrom(attributeType.Resolve())).ToArray<object>();
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x0008F3AC File Offset: 0x0008D5AC
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this._attributes.Where((object attr) => attr.GetType().Resolve().IsAssignableFrom(attributeType.Resolve())).Any<object>();
		}

		// Token: 0x04001787 RID: 6023
		private readonly object[] _attributes;
	}
}
