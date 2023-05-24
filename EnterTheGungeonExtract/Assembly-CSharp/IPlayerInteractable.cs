using System;
using UnityEngine;

// Token: 0x02001331 RID: 4913
public interface IPlayerInteractable
{
	// Token: 0x06006F5D RID: 28509
	float GetDistanceToPoint(Vector2 point);

	// Token: 0x06006F5E RID: 28510
	void OnEnteredRange(PlayerController interactor);

	// Token: 0x06006F5F RID: 28511
	void OnExitRange(PlayerController interactor);

	// Token: 0x06006F60 RID: 28512
	void Interact(PlayerController interactor);

	// Token: 0x06006F61 RID: 28513
	string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped);

	// Token: 0x06006F62 RID: 28514
	float GetOverrideMaxDistance();
}
