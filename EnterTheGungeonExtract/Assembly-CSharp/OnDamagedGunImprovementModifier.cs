using System;
using UnityEngine;

// Token: 0x02001447 RID: 5191
public class OnDamagedGunImprovementModifier : MonoBehaviour
{
	// Token: 0x060075DB RID: 30171 RVA: 0x002EEB1C File Offset: 0x002ECD1C
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		Gun gun = this.m_gun;
		gun.OnInitializedWithOwner = (Action<GameActor>)Delegate.Combine(gun.OnInitializedWithOwner, new Action<GameActor>(this.OnGunInitialized));
		Gun gun2 = this.m_gun;
		gun2.OnDropped = (Action)Delegate.Combine(gun2.OnDropped, new Action(this.OnGunDroppedOrDestroyed));
		if (this.m_gun.CurrentOwner != null)
		{
			this.OnGunInitialized(this.m_gun.CurrentOwner);
		}
	}

	// Token: 0x060075DC RID: 30172 RVA: 0x002EEBAC File Offset: 0x002ECDAC
	private void OnGunInitialized(GameActor obj)
	{
		if (this.m_playerOwner != null)
		{
			this.OnGunDroppedOrDestroyed();
		}
		if (obj == null)
		{
			return;
		}
		if (obj is PlayerController)
		{
			this.m_playerOwner = obj as PlayerController;
			this.m_playerOwner.healthHaver.OnHealthChanged += this.OnHealthChanged;
		}
	}

	// Token: 0x060075DD RID: 30173 RVA: 0x002EEC10 File Offset: 0x002ECE10
	private void OnHealthChanged(float resultValue, float maxValue)
	{
		this.m_gun.AdditionalClipCapacity = Mathf.FloorToInt((maxValue - resultValue) * 2f);
		this.m_playerOwner.stats.RecalculateStats(this.m_playerOwner, false, false);
	}

	// Token: 0x060075DE RID: 30174 RVA: 0x002EEC44 File Offset: 0x002ECE44
	private void OnDestroy()
	{
		this.OnGunDroppedOrDestroyed();
	}

	// Token: 0x060075DF RID: 30175 RVA: 0x002EEC4C File Offset: 0x002ECE4C
	private void OnGunDroppedOrDestroyed()
	{
		if (this.m_playerOwner != null)
		{
			this.m_playerOwner.healthHaver.OnHealthChanged -= this.OnHealthChanged;
			this.m_playerOwner = null;
		}
	}

	// Token: 0x04007797 RID: 30615
	public int AdditionalClipCapacity;

	// Token: 0x04007798 RID: 30616
	private Gun m_gun;

	// Token: 0x04007799 RID: 30617
	private PlayerController m_playerOwner;
}
