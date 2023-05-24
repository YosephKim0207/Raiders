using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020014C3 RID: 5315
public class StunEnemiesInRoomItem : MonoBehaviour
{
	// Token: 0x060078D0 RID: 30928 RVA: 0x00304E20 File Offset: 0x00303020
	protected void AffectEnemy(AIActor target)
	{
		if (target && target.behaviorSpeculator)
		{
			target.behaviorSpeculator.Stun(this.StunDuration, true);
		}
	}

	// Token: 0x060078D1 RID: 30929 RVA: 0x00304E50 File Offset: 0x00303050
	private void Start()
	{
		AkSoundEngine.PostEvent("Play_OBJ_item_throw_01", base.gameObject);
		DebrisObject component = base.GetComponent<DebrisObject>();
		component.killTranslationOnBounce = false;
		if (component)
		{
			DebrisObject debrisObject = component;
			debrisObject.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject.OnGrounded, new Action<DebrisObject>(this.OnHitGround));
		}
	}

	// Token: 0x060078D2 RID: 30930 RVA: 0x00304EAC File Offset: 0x003030AC
	private void OnHitGround(DebrisObject obj)
	{
		Pixelator.Instance.FadeToColor(0.1f, Color.white, true, 0.1f);
		RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
		List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (activeEnemies != null)
		{
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				this.AffectEnemy(activeEnemies[i]);
			}
		}
		if (this.DoChaffParticles)
		{
			GlobalSparksDoer.DoRandomParticleBurst(100, absoluteRoom.area.basePosition.ToVector3(), absoluteRoom.area.basePosition.ToVector3() + absoluteRoom.area.dimensions.ToVector3(), Vector3.up / 3f, 180f, 0f, new float?(0.125f), new float?(this.StunDuration), new Color?(Color.yellow), GlobalSparksDoer.SparksType.FLOATY_CHAFF);
			AkSoundEngine.PostEvent("Play_OBJ_chaff_blast_01", base.gameObject);
		}
		if (this.AllowStealing)
		{
			List<BaseShopController> allShops = StaticReferenceManager.AllShops;
			for (int j = 0; j < allShops.Count; j++)
			{
				if (allShops[j] && allShops[j].GetAbsoluteParentRoom() == absoluteRoom)
				{
					allShops[j].SetCapableOfBeingStolenFrom(true, "StunEnemiesInRoomItem", new float?(this.StunDuration));
				}
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04007B16 RID: 31510
	public float StunDuration = 5f;

	// Token: 0x04007B17 RID: 31511
	public bool DoChaffParticles = true;

	// Token: 0x04007B18 RID: 31512
	public bool AllowStealing = true;
}
