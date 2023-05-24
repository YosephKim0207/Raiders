using System;
using UnityEngine;

// Token: 0x0200170F RID: 5903
public class SpawnPlayerProcessorOnAcquisition : MonoBehaviour
{
	// Token: 0x0600892C RID: 35116 RVA: 0x0038E8A0 File Offset: 0x0038CAA0
	public void Awake()
	{
		this.m_passiveItem = base.GetComponent<PassiveItem>();
		this.m_playerItem = base.GetComponent<PlayerItem>();
		if (this.m_passiveItem)
		{
			PassiveItem passiveItem = this.m_passiveItem;
			passiveItem.OnPickedUp = (Action<PlayerController>)Delegate.Combine(passiveItem.OnPickedUp, new Action<PlayerController>(this.HandlePickedUp));
		}
		if (this.m_playerItem)
		{
			PlayerItem playerItem = this.m_playerItem;
			playerItem.OnPickedUp = (Action<PlayerController>)Delegate.Combine(playerItem.OnPickedUp, new Action<PlayerController>(this.HandlePickedUp));
		}
	}

	// Token: 0x0600892D RID: 35117 RVA: 0x0038E934 File Offset: 0x0038CB34
	private void HandlePickedUp(PlayerController p)
	{
		if (!p)
		{
			return;
		}
		if (p.SpawnedSubobjects.ContainsKey(this.Identifier))
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PrefabToSpawn);
		gameObject.transform.parent = p.transform;
		gameObject.transform.localPosition = Vector3.zero;
		p.SpawnedSubobjects.Add(this.Identifier, gameObject);
	}

	// Token: 0x04008F12 RID: 36626
	public GameObject PrefabToSpawn;

	// Token: 0x04008F13 RID: 36627
	public string Identifier;

	// Token: 0x04008F14 RID: 36628
	private PassiveItem m_passiveItem;

	// Token: 0x04008F15 RID: 36629
	private PlayerItem m_playerItem;
}
