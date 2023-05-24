using System;

namespace FullInspector
{
	// Token: 0x02000670 RID: 1648
	public class TypeSpecifier<TBaseType>
	{
		// Token: 0x060025A0 RID: 9632 RVA: 0x000A11B0 File Offset: 0x0009F3B0
		public TypeSpecifier()
		{
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x000A11B8 File Offset: 0x0009F3B8
		public TypeSpecifier(Type type)
		{
			this.Type = type;
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x000A11C8 File Offset: 0x0009F3C8
		public static implicit operator Type(TypeSpecifier<TBaseType> specifier)
		{
			return specifier.Type;
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x000A11D0 File Offset: 0x0009F3D0
		public static implicit operator TypeSpecifier<TBaseType>(Type type)
		{
			return new TypeSpecifier<TBaseType>
			{
				Type = type
			};
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x000A11EC File Offset: 0x0009F3EC
		public override bool Equals(object obj)
		{
			TypeSpecifier<TBaseType> typeSpecifier = obj as TypeSpecifier<TBaseType>;
			return typeSpecifier != null && this.Type == typeSpecifier.Type;
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x000A1218 File Offset: 0x0009F418
		public override int GetHashCode()
		{
			return this.Type.GetHashCode();
		}

		// Token: 0x040019A5 RID: 6565
		public Type Type;
	}
}
