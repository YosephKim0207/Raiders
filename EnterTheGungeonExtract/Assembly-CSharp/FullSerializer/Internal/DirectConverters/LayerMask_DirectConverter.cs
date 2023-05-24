using System;
using System.Collections.Generic;
using UnityEngine;

namespace FullSerializer.Internal.DirectConverters
{
	// Token: 0x02000599 RID: 1433
	public class LayerMask_DirectConverter : fsDirectConverter<LayerMask>
	{
		// Token: 0x060021F2 RID: 8690 RVA: 0x000958F4 File Offset: 0x00093AF4
		protected override fsResult DoSerialize(LayerMask model, Dictionary<string, fsData> serialized)
		{
			fsResult success = fsResult.Success;
			return success + base.SerializeMember<int>(serialized, "value", model.value);
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x00095924 File Offset: 0x00093B24
		protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref LayerMask model)
		{
			fsResult fsResult = fsResult.Success;
			int value = model.value;
			fsResult += base.DeserializeMember<int>(data, "value", out value);
			model.value = value;
			return fsResult;
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x0009595C File Offset: 0x00093B5C
		public override object CreateInstance(fsData data, Type storageType)
		{
			return default(LayerMask);
		}
	}
}
