using System;
using FullSerializer;
using UnityEngine;

namespace FullInspector.Serializers.FullSerializer
{
	// Token: 0x02000586 RID: 1414
	public class FullSerializerMetadata : fiISerializerMetadata
	{
		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x0600217E RID: 8574 RVA: 0x000936F4 File Offset: 0x000918F4
		public Guid SerializerGuid
		{
			get
			{
				return new Guid("bc898177-6ff4-423f-91bb-589bc83d8fde");
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x0600217F RID: 8575 RVA: 0x00093700 File Offset: 0x00091900
		public Type SerializerType
		{
			get
			{
				return typeof(FullSerializerSerializer);
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06002180 RID: 8576 RVA: 0x0009370C File Offset: 0x0009190C
		public Type[] SerializationOptInAnnotationTypes
		{
			get
			{
				return new Type[]
				{
					typeof(SerializeField),
					typeof(fsPropertyAttribute)
				};
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06002181 RID: 8577 RVA: 0x00093730 File Offset: 0x00091930
		public Type[] SerializationOptOutAnnotationTypes
		{
			get
			{
				return new Type[] { typeof(fsIgnoreAttribute) };
			}
		}
	}
}
