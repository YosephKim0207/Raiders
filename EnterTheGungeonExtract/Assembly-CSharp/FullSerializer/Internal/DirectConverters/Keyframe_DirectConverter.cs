using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullSerializer.Internal.DirectConverters
{
	// Token: 0x02000598 RID: 1432
	public class Keyframe_DirectConverter : fsDirectConverter<Keyframe>
	{
		// Token: 0x060021EE RID: 8686 RVA: 0x00095770 File Offset: 0x00093970
		protected override fsResult DoSerialize(Keyframe model, Dictionary<string, fsData> serialized)
		{
			fsResult fsResult = fsResult.Success;
			fsResult += base.SerializeMember<float>(serialized, "time", model.time);
			fsResult += base.SerializeMember<float>(serialized, "value", model.value);
			fsResult += base.SerializeMember<int>(serialized, "tangentMode", model.tangentMode);
			fsResult += base.SerializeMember<float>(serialized, "inTangent", model.inTangent);
			return fsResult + base.SerializeMember<float>(serialized, "outTangent", model.outTangent);
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x00095808 File Offset: 0x00093A08
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Keyframe model)
		{
			fsResult fsResult = fsResult.Success;
			float time = model.time;
			fsResult += base.DeserializeMember<float>(data, "time", out time);
			model.time = time;
			float value = model.value;
			fsResult += base.DeserializeMember<float>(data, "value", out value);
			model.value = value;
			int tangentMode = model.tangentMode;
			fsResult += base.DeserializeMember<int>(data, "tangentMode", out tangentMode);
			model.tangentMode = tangentMode;
			float inTangent = model.inTangent;
			fsResult += base.DeserializeMember<float>(data, "inTangent", out inTangent);
			model.inTangent = inTangent;
			float outTangent = model.outTangent;
			fsResult += base.DeserializeMember<float>(data, "outTangent", out outTangent);
			model.outTangent = outTangent;
			return fsResult;
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x000958D0 File Offset: 0x00093AD0
		public override object CreateInstance(fsData data, Type storageType)
		{
			return default(Keyframe);
		}
	}
}
