using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullSerializer.Internal.DirectConverters
{
	// Token: 0x02000596 RID: 1430
	public class Bounds_DirectConverter : fsDirectConverter<Bounds>
	{
		// Token: 0x060021E6 RID: 8678 RVA: 0x000955F0 File Offset: 0x000937F0
		protected override fsResult DoSerialize(Bounds model, Dictionary<string, fsData> serialized)
		{
			fsResult fsResult = fsResult.Success;
			fsResult += base.SerializeMember<Vector3>(serialized, "center", model.center);
			return fsResult + base.SerializeMember<Vector3>(serialized, "size", model.size);
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x00095638 File Offset: 0x00093838
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Bounds model)
		{
			fsResult fsResult = fsResult.Success;
			Vector3 center = model.center;
			fsResult += base.DeserializeMember<Vector3>(data, "center", out center);
			model.center = center;
			Vector3 size = model.size;
			fsResult += base.DeserializeMember<Vector3>(data, "size", out size);
			model.size = size;
			return fsResult;
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x00095694 File Offset: 0x00093894
		public override object CreateInstance(fsData data, Type storageType)
		{
			return default(Bounds);
		}
	}
}
