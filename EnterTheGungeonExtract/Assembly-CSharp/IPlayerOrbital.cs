using System;
using UnityEngine;

// Token: 0x02001422 RID: 5154
public interface IPlayerOrbital
{
	// Token: 0x060074F5 RID: 29941
	void Reinitialize();

	// Token: 0x060074F6 RID: 29942
	Transform GetTransform();

	// Token: 0x060074F7 RID: 29943
	void ToggleRenderer(bool visible);

	// Token: 0x060074F8 RID: 29944
	int GetOrbitalTier();

	// Token: 0x060074F9 RID: 29945
	void SetOrbitalTier(int tier);

	// Token: 0x060074FA RID: 29946
	int GetOrbitalTierIndex();

	// Token: 0x060074FB RID: 29947
	void SetOrbitalTierIndex(int tierIndex);

	// Token: 0x060074FC RID: 29948
	float GetOrbitalRadius();

	// Token: 0x060074FD RID: 29949
	float GetOrbitalRotationalSpeed();
}
