using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001040 RID: 4160
[RequireComponent(typeof(GenericIntroDoer))]
public class GunonIntroDoer : SpecificIntroDoer
{
	// Token: 0x06005B50 RID: 23376 RVA: 0x0022FC9C File Offset: 0x0022DE9C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005B51 RID: 23377 RVA: 0x0022FCA4 File Offset: 0x0022DEA4
	public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
	{
		base.aiAnimator.LockFacingDirection = true;
		base.aiAnimator.FacingDirection = -90f;
		RoomHandler parentRoom = base.aiActor.ParentRoom;
		if (parentRoom != null)
		{
			List<TorchController> componentsInRoom = parentRoom.GetComponentsInRoom<TorchController>();
			for (int i = 0; i < componentsInRoom.Count; i++)
			{
				TorchController torchController = componentsInRoom[i];
				if (torchController && torchController.specRigidbody)
				{
					torchController.specRigidbody.CollideWithOthers = false;
				}
			}
		}
	}

	// Token: 0x06005B52 RID: 23378 RVA: 0x0022FD2C File Offset: 0x0022DF2C
	public override void EndIntro()
	{
		base.aiAnimator.LockFacingDirection = false;
		base.aiAnimator.EndAnimation();
	}
}
