using System;
using System.Collections.Generic;
using System.Reflection;
using FullSerializer.Internal;
using FullSerializer.Internal.DirectConverters;

namespace FullSerializer
{
	// Token: 0x02000594 RID: 1428
	public class fsConverterRegistrar
	{
		// Token: 0x060021DF RID: 8671 RVA: 0x00095438 File Offset: 0x00093638
		static fsConverterRegistrar()
		{
			foreach (FieldInfo fieldInfo in typeof(fsConverterRegistrar).GetDeclaredFields())
			{
				if (fieldInfo.Name.StartsWith("Register_"))
				{
					fsConverterRegistrar.Converters.Add(fieldInfo.FieldType);
				}
			}
			foreach (MethodInfo methodInfo in typeof(fsConverterRegistrar).GetDeclaredMethods())
			{
				if (methodInfo.Name.StartsWith("Register_"))
				{
					methodInfo.Invoke(null, null);
				}
			}
		}

		// Token: 0x04001823 RID: 6179
		public static AnimationCurve_DirectConverter Register_AnimationCurve_DirectConverter;

		// Token: 0x04001824 RID: 6180
		public static Bounds_DirectConverter Register_Bounds_DirectConverter;

		// Token: 0x04001825 RID: 6181
		public static Gradient_DirectConverter Register_Gradient_DirectConverter;

		// Token: 0x04001826 RID: 6182
		public static Keyframe_DirectConverter Register_Keyframe_DirectConverter;

		// Token: 0x04001827 RID: 6183
		public static LayerMask_DirectConverter Register_LayerMask_DirectConverter;

		// Token: 0x04001828 RID: 6184
		public static Rect_DirectConverter Register_Rect_DirectConverter;

		// Token: 0x04001829 RID: 6185
		public static List<Type> Converters = new List<Type>();
	}
}
