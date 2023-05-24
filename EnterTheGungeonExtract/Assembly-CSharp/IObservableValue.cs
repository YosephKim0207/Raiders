using System;

// Token: 0x02000388 RID: 904
public interface IObservableValue
{
	// Token: 0x1700035D RID: 861
	// (get) Token: 0x06000F7C RID: 3964
	object Value { get; }

	// Token: 0x1700035E RID: 862
	// (get) Token: 0x06000F7D RID: 3965
	bool HasChanged { get; }
}
