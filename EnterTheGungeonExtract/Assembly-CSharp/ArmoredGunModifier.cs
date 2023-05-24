using System;
using UnityEngine;

// Token: 0x0200161A RID: 5658
public class ArmoredGunModifier : MonoBehaviour
{
	// Token: 0x060083E1 RID: 33761 RVA: 0x003608C4 File Offset: 0x0035EAC4
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		if (this.ArmoredId < 0)
		{
			this.ArmoredId = PickupObjectDatabase.GetById(this.UnarmoredId).GetComponent<ArmoredGunModifier>().ArmoredId;
		}
		if (this.UnarmoredId < 0)
		{
			this.UnarmoredId = PickupObjectDatabase.GetById(this.ArmoredId).GetComponent<ArmoredGunModifier>().UnarmoredId;
		}
	}

	// Token: 0x060083E2 RID: 33762 RVA: 0x0036092C File Offset: 0x0035EB2C
	private void Update()
	{
		if (this.m_gun && !this.m_gun.CurrentOwner)
		{
			PlayerController bestActivePlayer = GameManager.Instance.BestActivePlayer;
			if (bestActivePlayer)
			{
				if (bestActivePlayer.healthHaver && bestActivePlayer.healthHaver.Armor > 0f)
				{
					Gun gun = PickupObjectDatabase.GetById(this.ArmoredId) as Gun;
					this.m_gun.sprite.SetSprite(gun.sprite.spriteId);
				}
				else
				{
					Gun gun2 = PickupObjectDatabase.GetById(this.UnarmoredId) as Gun;
					this.m_gun.sprite.SetSprite(gun2.sprite.spriteId);
				}
			}
		}
		else if (this.m_gun && this.m_gun.CurrentOwner && this.m_gun.CurrentOwner.healthHaver)
		{
			float num = this.m_gun.CurrentOwner.healthHaver.Armor;
			if (this.m_gun.OwnerHasSynergy(CustomSynergyType.NANOARMOR))
			{
				num = 20f;
			}
			if (this.m_armored && num <= 0f)
			{
				this.BecomeUnarmored();
			}
			else if (!this.m_armored && num > 0f)
			{
				this.BecomeArmored();
			}
		}
	}

	// Token: 0x060083E3 RID: 33763 RVA: 0x00360AA8 File Offset: 0x0035ECA8
	private void BecomeArmored()
	{
		if (this.m_armored)
		{
			return;
		}
		this.m_armored = true;
		this.m_gun.TransformToTargetGun(PickupObjectDatabase.GetById(this.ArmoredId) as Gun);
		this.m_gun.spriteAnimator.Play(this.ArmorUpAnimation);
	}

	// Token: 0x060083E4 RID: 33764 RVA: 0x00360AFC File Offset: 0x0035ECFC
	private void BecomeUnarmored()
	{
		if (!this.m_armored)
		{
			return;
		}
		this.m_armored = false;
		this.m_gun.TransformToTargetGun(PickupObjectDatabase.GetById(this.UnarmoredId) as Gun);
		this.m_gun.spriteAnimator.Play(this.ArmorLostAnimation);
	}

	// Token: 0x04008736 RID: 34614
	[PickupIdentifier]
	public int ArmoredId = -1;

	// Token: 0x04008737 RID: 34615
	[PickupIdentifier]
	public int UnarmoredId = -1;

	// Token: 0x04008738 RID: 34616
	[CheckAnimation(null)]
	public string ArmorUpAnimation;

	// Token: 0x04008739 RID: 34617
	[CheckAnimation(null)]
	public string ArmorLostAnimation;

	// Token: 0x0400873A RID: 34618
	private Gun m_gun;

	// Token: 0x0400873B RID: 34619
	private bool m_armored = true;
}
