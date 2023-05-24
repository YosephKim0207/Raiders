using System;
using UnityEngine;

// Token: 0x020014C0 RID: 5312
public class StatHolder : MonoBehaviour
{
	// Token: 0x060078C0 RID: 30912 RVA: 0x00304928 File Offset: 0x00302B28
	private void Start()
	{
		if (this.RequiresPlayerItemActive)
		{
			PlayerItem component = base.GetComponent<PlayerItem>();
			if (component)
			{
				PlayerItem playerItem = component;
				playerItem.OnActivationStatusChanged = (Action<PlayerItem>)Delegate.Combine(playerItem.OnActivationStatusChanged, new Action<PlayerItem>(delegate(PlayerItem a)
				{
					if (a.LastOwner)
					{
						a.LastOwner.stats.RecalculateStats(a.LastOwner, false, false);
					}
				}));
			}
		}
	}

	// Token: 0x04007B07 RID: 31495
	public bool RequiresPlayerItemActive;

	// Token: 0x04007B08 RID: 31496
	public StatModifier[] modifiers;
}
