using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullSerializer.Internal.DirectConverters
{
	// Token: 0x0200059A RID: 1434
	public class Rect_DirectConverter : fsDirectConverter<Rect>
	{
		// Token: 0x060021F6 RID: 8694 RVA: 0x00095980 File Offset: 0x00093B80
		protected override fsResult DoSerialize(Rect model, Dictionary<string, fsData> serialized)
		{
			fsResult fsResult = fsResult.Success;
			fsResult += base.SerializeMember<float>(serialized, "xMin", model.xMin);
			fsResult += base.SerializeMember<float>(serialized, "yMin", model.yMin);
			fsResult += base.SerializeMember<float>(serialized, "xMax", model.xMax);
			return fsResult + base.SerializeMember<float>(serialized, "yMax", model.yMax);
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x000959FC File Offset: 0x00093BFC
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Rect model)
		{
			fsResult fsResult = fsResult.Success;
			float xMin = model.xMin;
			fsResult += base.DeserializeMember<float>(data, "xMin", out xMin);
			model.xMin = xMin;
			float yMin = model.yMin;
			fsResult += base.DeserializeMember<float>(data, "yMin", out yMin);
			model.yMin = yMin;
			float xMax = model.xMax;
			fsResult += base.DeserializeMember<float>(data, "xMax", out xMax);
			model.xMax = xMax;
			float yMax = model.yMax;
			fsResult += base.DeserializeMember<float>(data, "yMax", out yMax);
			model.yMax = yMax;
			return fsResult;
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x00095AA0 File Offset: 0x00093CA0
		public override object CreateInstance(fsData data, Type storageType)
		{
			return default(Rect);
		}
	}
}
