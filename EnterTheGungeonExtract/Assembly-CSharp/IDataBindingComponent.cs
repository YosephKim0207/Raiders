using System;

// Token: 0x02000387 RID: 903
public interface IDataBindingComponent
{
	// Token: 0x1700035C RID: 860
	// (get) Token: 0x06000F79 RID: 3961
	bool IsBound { get; }

	// Token: 0x06000F7A RID: 3962
	void Bind();

	// Token: 0x06000F7B RID: 3963
	void Unbind();
}
