using System;
using System.Collections.Generic;

namespace FullSerializer
{
	// Token: 0x020005A4 RID: 1444
	public abstract class fsDirectConverter<TModel> : fsDirectConverter
	{
		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x0600223B RID: 8763 RVA: 0x0009680C File Offset: 0x00094A0C
		public override Type ModelType
		{
			get
			{
				return typeof(TModel);
			}
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x00096818 File Offset: 0x00094A18
		public sealed override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
		{
			Dictionary<string, fsData> dictionary = new Dictionary<string, fsData>();
			fsResult fsResult = this.DoSerialize((TModel)((object)instance), dictionary);
			serialized = new fsData(dictionary);
			return fsResult;
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x00096844 File Offset: 0x00094A44
		public sealed override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
		{
			fsResult fsResult = fsResult.Success;
			fsResult fsResult2;
			fsResult = (fsResult2 = fsResult + base.CheckType(data, fsDataType.Object));
			if (fsResult2.Failed)
			{
				return fsResult;
			}
			TModel tmodel = (TModel)((object)instance);
			fsResult += this.DoDeserialize(data.AsDictionary, ref tmodel);
			instance = tmodel;
			return fsResult;
		}

		// Token: 0x0600223E RID: 8766
		protected abstract fsResult DoSerialize(TModel model, Dictionary<string, fsData> serialized);

		// Token: 0x0600223F RID: 8767
		protected abstract fsResult DoDeserialize(Dictionary<string, fsData> data, ref TModel model);
	}
}
