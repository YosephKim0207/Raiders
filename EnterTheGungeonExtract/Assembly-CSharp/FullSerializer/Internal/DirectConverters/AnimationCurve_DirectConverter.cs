using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullSerializer.Internal.DirectConverters
{
	// Token: 0x02000595 RID: 1429
	public class AnimationCurve_DirectConverter : fsDirectConverter<AnimationCurve>
	{
		// Token: 0x060021E2 RID: 8674 RVA: 0x000954FC File Offset: 0x000936FC
		protected override fsResult DoSerialize(AnimationCurve model, Dictionary<string, fsData> serialized)
		{
			fsResult fsResult = fsResult.Success;
			fsResult += base.SerializeMember<Keyframe[]>(serialized, "keys", model.keys);
			fsResult += base.SerializeMember<WrapMode>(serialized, "preWrapMode", model.preWrapMode);
			return fsResult + base.SerializeMember<WrapMode>(serialized, "postWrapMode", model.postWrapMode);
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x0009555C File Offset: 0x0009375C
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref AnimationCurve model)
		{
			fsResult fsResult = fsResult.Success;
			Keyframe[] keys = model.keys;
			fsResult += base.DeserializeMember<Keyframe[]>(data, "keys", out keys);
			model.keys = keys;
			WrapMode preWrapMode = model.preWrapMode;
			fsResult += base.DeserializeMember<WrapMode>(data, "preWrapMode", out preWrapMode);
			model.preWrapMode = preWrapMode;
			WrapMode postWrapMode = model.postWrapMode;
			fsResult += base.DeserializeMember<WrapMode>(data, "postWrapMode", out postWrapMode);
			model.postWrapMode = postWrapMode;
			return fsResult;
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x000955E0 File Offset: 0x000937E0
		public override object CreateInstance(fsData data, Type storageType)
		{
			return new AnimationCurve();
		}
	}
}
