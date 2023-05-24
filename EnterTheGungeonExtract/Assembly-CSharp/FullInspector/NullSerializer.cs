using System;
using System.Reflection;

namespace FullInspector
{
	// Token: 0x020005D0 RID: 1488
	[Obsolete("Please use [fiInspectorOnly]")]
	public class NullSerializer : BaseSerializer
	{
		// Token: 0x0600234C RID: 9036 RVA: 0x0009ACB8 File Offset: 0x00098EB8
		public override string Serialize(MemberInfo storageType, object value, ISerializationOperator serializationOperator)
		{
			return null;
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x0009ACBC File Offset: 0x00098EBC
		public override object Deserialize(MemberInfo storageType, string serializedState, ISerializationOperator serializationOperator)
		{
			return null;
		}
	}
}
