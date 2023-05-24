using System;

namespace FullSerializer.Internal
{
	// Token: 0x02000593 RID: 1427
	public class fsWeakReferenceConverter : fsConverter
	{
		// Token: 0x060021D9 RID: 8665 RVA: 0x000952B0 File Offset: 0x000934B0
		public override bool CanProcess(Type type)
		{
			return type == typeof(WeakReference);
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x000952C0 File Offset: 0x000934C0
		public override bool RequestCycleSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x000952C4 File Offset: 0x000934C4
		public override bool RequestInheritanceSupport(Type storageType)
		{
			return false;
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x000952C8 File Offset: 0x000934C8
		public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			WeakReference weakReference = (WeakReference)instance;
			fsResult fsResult = fsResult.Success;
			serialized = fsData.CreateDictionary();
			if (weakReference.IsAlive)
			{
				fsData fsData;
				fsResult fsResult2;
				fsResult = (fsResult2 = fsResult + this.Serializer.TrySerialize<object>(weakReference.Target, out fsData));
				if (fsResult2.Failed)
				{
					return fsResult;
				}
				serialized.AsDictionary["Target"] = fsData;
				serialized.AsDictionary["TrackResurrection"] = new fsData(weakReference.TrackResurrection);
			}
			return fsResult;
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x00095350 File Offset: 0x00093550
		public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			fsResult fsResult = fsResult.Success;
			fsResult fsResult2;
			fsResult = (fsResult2 = fsResult + base.CheckType(data, fsDataType.Object));
			if (fsResult2.Failed)
			{
				return fsResult;
			}
			if (data.AsDictionary.ContainsKey("Target"))
			{
				fsData fsData = data.AsDictionary["Target"];
				object obj = null;
				fsResult fsResult3;
				fsResult = (fsResult3 = fsResult + this.Serializer.TryDeserialize(fsData, typeof(object), ref obj));
				if (fsResult3.Failed)
				{
					return fsResult;
				}
				bool flag = false;
				if (data.AsDictionary.ContainsKey("TrackResurrection") && data.AsDictionary["TrackResurrection"].IsBool)
				{
					flag = data.AsDictionary["TrackResurrection"].AsBool;
				}
				instance = new WeakReference(obj, flag);
			}
			return fsResult;
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x00095430 File Offset: 0x00093630
		public override object CreateInstance(fsData data, Type storageType)
		{
			return new WeakReference(null);
		}
	}
}
