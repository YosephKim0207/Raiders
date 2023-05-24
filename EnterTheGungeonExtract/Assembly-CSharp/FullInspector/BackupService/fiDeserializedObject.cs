using System;
using System.Collections.Generic;
using FullInspector.Internal;
using FullSerializer.Internal;

namespace FullInspector.BackupService
{
	// Token: 0x020005EE RID: 1518
	public class fiDeserializedObject
	{
		// Token: 0x060023C7 RID: 9159 RVA: 0x0009CAAC File Offset: 0x0009ACAC
		public fiDeserializedObject(fiSerializedObject serializedState)
		{
			Type type = serializedState.Target.Target.GetType();
			fiSerializationOperator fiSerializationOperator = new fiSerializationOperator
			{
				SerializedObjects = serializedState.ObjectReferences
			};
			Type serializerType = BehaviorTypeToSerializerTypeMap.GetSerializerType(type);
			BaseSerializer baseSerializer = (BaseSerializer)fiSingletons.Get(serializerType);
			InspectedType inspectedType = InspectedType.Get(type);
			this.Members = new List<fiDeserializedMember>();
			foreach (fiSerializedMember fiSerializedMember in serializedState.Members)
			{
				InspectedProperty propertyByName = inspectedType.GetPropertyByName(fiSerializedMember.Name);
				if (propertyByName != null)
				{
					object obj = baseSerializer.Deserialize(fsPortableReflection.AsMemberInfo(propertyByName.StorageType), fiSerializedMember.Value, fiSerializationOperator);
					this.Members.Add(new fiDeserializedMember
					{
						InspectedProperty = propertyByName,
						Value = obj,
						ShouldRestore = fiSerializedMember.ShouldRestore
					});
				}
			}
		}

		// Token: 0x040018E0 RID: 6368
		public List<fiDeserializedMember> Members;
	}
}
