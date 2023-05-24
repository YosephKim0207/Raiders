using System;
using UnityEngine;

namespace FullSerializer
{
	// Token: 0x0200059E RID: 1438
	public static class fsConfig
	{
		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x0600220C RID: 8716 RVA: 0x0009619C File Offset: 0x0009439C
		// (set) Token: 0x0600220D RID: 8717 RVA: 0x000961A4 File Offset: 0x000943A4
		public static fsMemberSerialization DefaultMemberSerialization
		{
			get
			{
				return fsConfig._defaultMemberSerialization;
			}
			set
			{
				fsConfig._defaultMemberSerialization = value;
				fsMetaType.ClearCache();
			}
		}

		// Token: 0x04001831 RID: 6193
		public static Type[] SerializeAttributes = new Type[]
		{
			typeof(SerializeField),
			typeof(fsPropertyAttribute)
		};

		// Token: 0x04001832 RID: 6194
		public static Type[] IgnoreSerializeAttributes = new Type[]
		{
			typeof(NonSerializedAttribute),
			typeof(fsIgnoreAttribute)
		};

		// Token: 0x04001833 RID: 6195
		private static fsMemberSerialization _defaultMemberSerialization = fsMemberSerialization.Default;

		// Token: 0x04001834 RID: 6196
		public static bool SerializeNonAutoProperties = false;

		// Token: 0x04001835 RID: 6197
		public static bool SerializeNonPublicSetProperties = true;

		// Token: 0x04001836 RID: 6198
		public static bool IsCaseSensitive = true;

		// Token: 0x04001837 RID: 6199
		public static string CustomDateTimeFormatString = null;

		// Token: 0x04001838 RID: 6200
		public static bool Serialize64BitIntegerAsString = false;

		// Token: 0x04001839 RID: 6201
		public static bool SerializeEnumsAsInteger = false;
	}
}
