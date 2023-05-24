using System;

namespace FullSerializer.Internal
{
	// Token: 0x020005BD RID: 1469
	public struct fsVersionedType
	{
		// Token: 0x060022E5 RID: 8933 RVA: 0x000999F8 File Offset: 0x00097BF8
		public object Migrate(object ancestorInstance)
		{
			return Activator.CreateInstance(this.ModelType, new object[] { ancestorInstance });
		}

		// Token: 0x060022E6 RID: 8934 RVA: 0x00099A10 File Offset: 0x00097C10
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"fsVersionedType [ModelType=",
				this.ModelType,
				", VersionString=",
				this.VersionString,
				", Ancestors.Length=",
				this.Ancestors.Length,
				"]"
			});
		}

		// Token: 0x060022E7 RID: 8935 RVA: 0x00099A6C File Offset: 0x00097C6C
		public static bool operator ==(fsVersionedType a, fsVersionedType b)
		{
			return a.ModelType == b.ModelType;
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x00099A80 File Offset: 0x00097C80
		public static bool operator !=(fsVersionedType a, fsVersionedType b)
		{
			return a.ModelType != b.ModelType;
		}

		// Token: 0x060022E9 RID: 8937 RVA: 0x00099A98 File Offset: 0x00097C98
		public override bool Equals(object obj)
		{
			return obj is fsVersionedType && this.ModelType == ((fsVersionedType)obj).ModelType;
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x00099ACC File Offset: 0x00097CCC
		public override int GetHashCode()
		{
			return this.ModelType.GetHashCode();
		}

		// Token: 0x0400187F RID: 6271
		public fsVersionedType[] Ancestors;

		// Token: 0x04001880 RID: 6272
		public string VersionString;

		// Token: 0x04001881 RID: 6273
		public Type ModelType;
	}
}
