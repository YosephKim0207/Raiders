using System;
using System.Collections.Generic;

// Token: 0x0200084E RID: 2126
public interface ICollidableObject
{
	// Token: 0x170008B1 RID: 2225
	// (get) Token: 0x06002EC6 RID: 11974
	PixelCollider PrimaryPixelCollider { get; }

	// Token: 0x06002EC7 RID: 11975
	bool CanCollideWith(SpeculativeRigidbody rigidbody);

	// Token: 0x06002EC8 RID: 11976
	List<PixelCollider> GetPixelColliders();
}
