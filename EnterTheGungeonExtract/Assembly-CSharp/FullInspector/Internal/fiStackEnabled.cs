using System;

namespace FullInspector.Internal
{
	// Token: 0x02000581 RID: 1409
	public class fiStackEnabled
	{
		// Token: 0x0600215B RID: 8539 RVA: 0x00093268 File Offset: 0x00091468
		public void Push()
		{
			this._count++;
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x00093278 File Offset: 0x00091478
		public void Pop()
		{
			this._count--;
			if (this._count < 0)
			{
				this._count = 0;
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x0600215D RID: 8541 RVA: 0x0009329C File Offset: 0x0009149C
		public bool Enabled
		{
			get
			{
				return this._count > 0;
			}
		}

		// Token: 0x04001819 RID: 6169
		private int _count;
	}
}
