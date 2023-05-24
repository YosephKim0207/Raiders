using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullSerializer.Internal.DirectConverters
{
	// Token: 0x02000597 RID: 1431
	public class Gradient_DirectConverter : fsDirectConverter<Gradient>
	{
		// Token: 0x060021EA RID: 8682 RVA: 0x000956B8 File Offset: 0x000938B8
		protected override fsResult DoSerialize(Gradient model, Dictionary<string, fsData> serialized)
		{
			fsResult fsResult = fsResult.Success;
			fsResult += base.SerializeMember<GradientAlphaKey[]>(serialized, "alphaKeys", model.alphaKeys);
			return fsResult + base.SerializeMember<GradientColorKey[]>(serialized, "colorKeys", model.colorKeys);
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x00095700 File Offset: 0x00093900
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Gradient model)
		{
			fsResult fsResult = fsResult.Success;
			GradientAlphaKey[] alphaKeys = model.alphaKeys;
			fsResult += base.DeserializeMember<GradientAlphaKey[]>(data, "alphaKeys", out alphaKeys);
			model.alphaKeys = alphaKeys;
			GradientColorKey[] colorKeys = model.colorKeys;
			fsResult += base.DeserializeMember<GradientColorKey[]>(data, "colorKeys", out colorKeys);
			model.colorKeys = colorKeys;
			return fsResult;
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x00095760 File Offset: 0x00093960
		public override object CreateInstance(fsData data, Type storageType)
		{
			return new Gradient();
		}
	}
}
