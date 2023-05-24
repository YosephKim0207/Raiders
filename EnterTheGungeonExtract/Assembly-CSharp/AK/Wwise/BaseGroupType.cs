using System;

namespace AK.Wwise
{
	// Token: 0x020018D3 RID: 6355
	[Serializable]
	public class BaseGroupType : BaseType
	{
		// Token: 0x06009CD9 RID: 40153 RVA: 0x003ED92C File Offset: 0x003EBB2C
		protected uint GetGroupID()
		{
			return (uint)this.groupID;
		}

		// Token: 0x06009CDA RID: 40154 RVA: 0x003ED934 File Offset: 0x003EBB34
		public override bool IsValid()
		{
			return base.IsValid() && this.groupID != 0;
		}

		// Token: 0x04009E80 RID: 40576
		public int groupID;
	}
}
