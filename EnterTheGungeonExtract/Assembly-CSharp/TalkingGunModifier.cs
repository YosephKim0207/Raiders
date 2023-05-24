using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020012B5 RID: 4789
public class TalkingGunModifier : MonoBehaviour, IGunInheritable
{
	// Token: 0x06006B24 RID: 27428 RVA: 0x002A19EC File Offset: 0x0029FBEC
	private void Start()
	{
		this.m_gun = base.GetComponent<Gun>();
		this.m_gun.AddAdditionalFlipTransform(this.talkPoint);
		Gun gun = this.m_gun;
		gun.PostProcessProjectile = (Action<Projectile>)Delegate.Combine(gun.PostProcessProjectile, new Action<Projectile>(this.PostprocessFriendship));
		Gun gun2 = this.m_gun;
		gun2.OnInitializedWithOwner = (Action<GameActor>)Delegate.Combine(gun2.OnInitializedWithOwner, new Action<GameActor>(this.OnGunReinitialized));
		if (this.m_gun.CurrentOwner != null)
		{
			this.OnGunReinitialized(this.m_gun.CurrentOwner);
		}
		Gun gun3 = this.m_gun;
		gun3.OnDropped = (Action)Delegate.Combine(gun3.OnDropped, new Action(this.HandleDropped));
	}

	// Token: 0x06006B25 RID: 27429 RVA: 0x002A1AB4 File Offset: 0x0029FCB4
	private void OnGunReinitialized(GameActor newOwner)
	{
		this.m_owner = this.m_gun.CurrentOwner as PlayerController;
		this.m_owner.OnRoomClearEvent += this.HandleRoomCleared;
	}

	// Token: 0x06006B26 RID: 27430 RVA: 0x002A1AE4 File Offset: 0x0029FCE4
	private void HandleDropped()
	{
		if (this.m_owner)
		{
			this.m_owner.OnRoomClearEvent -= this.HandleRoomCleared;
		}
		this.m_owner = null;
	}

	// Token: 0x06006B27 RID: 27431 RVA: 0x002A1B14 File Offset: 0x0029FD14
	private void PostprocessFriendship(Projectile obj)
	{
		BounceProjModifier component = obj.GetComponent<BounceProjModifier>();
		PierceProjModifier component2 = obj.GetComponent<PierceProjModifier>();
		if (this.m_friendship >= this.roomsToRankUp)
		{
			if (this.m_friendship < this.roomsToRankUp * 2)
			{
				obj.baseData.damage += 3f;
				obj.baseData.speed += 6f;
				if (component)
				{
					obj.GetComponent<BounceProjModifier>().numberOfBounces += 2;
				}
				if (component2)
				{
					obj.GetComponent<PierceProjModifier>().penetration += 3;
				}
			}
			else
			{
				obj.baseData.damage += 6f;
				obj.baseData.speed += 6f;
				if (component2)
				{
					obj.GetComponent<PierceProjModifier>().BeastModeLevel = PierceProjModifier.BeastModeStatus.BEAST_MODE_LEVEL_ONE;
				}
				HomingModifier homingModifier = obj.gameObject.AddComponent<HomingModifier>();
				homingModifier.HomingRadius = 8f;
				homingModifier.AngularVelocity = 360f;
			}
		}
	}

	// Token: 0x06006B28 RID: 27432 RVA: 0x002A1C30 File Offset: 0x0029FE30
	private void ClearTextBoxForReal()
	{
		TextBoxManager.ClearTextBox(this.talkPoint);
		if (this.talkPoint && this.talkPoint.childCount > 0)
		{
			for (int i = this.talkPoint.childCount - 1; i >= 0; i--)
			{
				Transform child = this.talkPoint.GetChild(i);
				if (child)
				{
					UnityEngine.Object.Destroy(child.gameObject);
				}
			}
		}
	}

	// Token: 0x06006B29 RID: 27433 RVA: 0x002A1CAC File Offset: 0x0029FEAC
	private void Update()
	{
		if (this.m_gun && this.m_gun.sprite)
		{
			this.talkPoint.transform.localPosition = new Vector3(0.875f, (!this.m_gun.sprite.FlipY) ? 1.3125f : (-1.3125f), 0f);
			if (this.m_owner && this.m_owner.CurrentRoom != null && this.m_owner.CurrentRoom.IsSealed && this.m_owner.CurrentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All) && this.talkPoint.childCount > 0)
			{
				if (this.m_destroyTimer < 0.25f)
				{
					this.m_destroyTimer += BraveTime.DeltaTime;
				}
				else
				{
					this.ClearTextBoxForReal();
				}
			}
			else
			{
				this.m_destroyTimer = 0f;
			}
		}
		this.talkPoint.rotation = Quaternion.identity;
	}

	// Token: 0x06006B2A RID: 27434 RVA: 0x002A1DCC File Offset: 0x0029FFCC
	private IEnumerator HandleDelayedTalk(PlayerController obj)
	{
		yield return new WaitForSeconds(1f);
		if (!obj.IsInCombat && base.gameObject.activeSelf)
		{
			if (this.m_friendship < this.roomsToRankUp)
			{
				this.DoAmbientTalk(this.talkPoint, Vector3.zero, "#MASKGUN_ROOMCLEAR_ENMITY", 4f, this.m_enmityCounter);
				this.m_enmityCounter++;
			}
			else if (this.m_friendship < this.roomsToRankUp * 2)
			{
				this.DoAmbientTalk(this.talkPoint, Vector3.zero, "#MASKGUN_ROOMCLEAR_BEGRUDGING", 4f, this.m_begrudgingCounter);
				this.m_begrudgingCounter++;
			}
			else
			{
				this.DoAmbientTalk(this.talkPoint, Vector3.zero, "#MASKGUN_ROOMCLEAR_FRIENDS", 4f, this.m_friendCounter);
				this.m_friendCounter++;
			}
		}
		yield break;
	}

	// Token: 0x06006B2B RID: 27435 RVA: 0x002A1DF0 File Offset: 0x0029FFF0
	private void HandleRoomCleared(PlayerController obj)
	{
		if (!this)
		{
			return;
		}
		if (base.gameObject.activeSelf && this.m_gun.CurrentOwner != null && UnityEngine.Random.value < this.ChanceToGainFriendship)
		{
			obj.StartCoroutine(this.HandleDelayedTalk(obj));
			this.m_friendship++;
		}
	}

	// Token: 0x06006B2C RID: 27436 RVA: 0x002A1E5C File Offset: 0x002A005C
	private void OnDestroy()
	{
		if (this.m_owner)
		{
			this.m_owner.OnRoomClearEvent -= this.HandleRoomCleared;
		}
	}

	// Token: 0x06006B2D RID: 27437 RVA: 0x002A1E88 File Offset: 0x002A0088
	private void OnDisable()
	{
		this.ClearTextBoxForReal();
	}

	// Token: 0x06006B2E RID: 27438 RVA: 0x002A1E90 File Offset: 0x002A0090
	public void DoAmbientTalk(Transform baseTransform, Vector3 offset, string stringKey, float duration, int index)
	{
		TextBoxManager.ShowTextBox(baseTransform.position + offset, baseTransform, duration, StringTableManager.GetStringSequential(stringKey, ref index, true), string.Empty, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
	}

	// Token: 0x06006B2F RID: 27439 RVA: 0x002A1EC4 File Offset: 0x002A00C4
	public void InheritData(Gun sourceGun)
	{
		TalkingGunModifier component = sourceGun.GetComponent<TalkingGunModifier>();
		if (component)
		{
			this.m_friendship = component.m_friendship;
			this.m_enmityCounter = component.m_enmityCounter;
			this.m_begrudgingCounter = component.m_begrudgingCounter;
			this.m_friendCounter = component.m_friendCounter;
		}
	}

	// Token: 0x06006B30 RID: 27440 RVA: 0x002A1F14 File Offset: 0x002A0114
	public void MidGameSerialize(List<object> data, int dataIndex)
	{
		data.Add(this.m_friendship);
		data.Add(this.m_enmityCounter);
		data.Add(this.m_begrudgingCounter);
		data.Add(this.m_friendCounter);
	}

	// Token: 0x06006B31 RID: 27441 RVA: 0x002A1F68 File Offset: 0x002A0168
	public void MidGameDeserialize(List<object> data, ref int dataIndex)
	{
		this.m_friendship = (int)data[dataIndex];
		this.m_enmityCounter = (int)data[dataIndex + 1];
		this.m_begrudgingCounter = (int)data[dataIndex + 2];
		this.m_friendCounter = (int)data[dataIndex + 3];
		dataIndex += 4;
	}

	// Token: 0x04006810 RID: 26640
	public Transform talkPoint;

	// Token: 0x04006811 RID: 26641
	public int roomsToRankUp = 10;

	// Token: 0x04006812 RID: 26642
	public float ChanceToGainFriendship = 0.5f;

	// Token: 0x04006813 RID: 26643
	private Gun m_gun;

	// Token: 0x04006814 RID: 26644
	private int m_friendship;

	// Token: 0x04006815 RID: 26645
	private int m_enmityCounter;

	// Token: 0x04006816 RID: 26646
	private int m_begrudgingCounter;

	// Token: 0x04006817 RID: 26647
	private int m_friendCounter;

	// Token: 0x04006818 RID: 26648
	private PlayerController m_owner;

	// Token: 0x04006819 RID: 26649
	private float m_destroyTimer;
}
