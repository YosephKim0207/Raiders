using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200184F RID: 6223
[Serializable]
public class WeightedGameObject
{
	// Token: 0x170015EB RID: 5611
	// (get) Token: 0x0600932F RID: 37679 RVA: 0x003E2008 File Offset: 0x003E0208
	public GameObject gameObject
	{
		get
		{
			if (!this.m_hasCachedGameObject)
			{
				if (this.pickupId >= 0)
				{
					PickupObject byId = PickupObjectDatabase.GetById(this.pickupId);
					if (byId)
					{
						this.m_cachedGameObject = byId.gameObject;
					}
				}
				if (!this.m_cachedGameObject)
				{
					this.m_cachedGameObject = this.rawGameObject;
				}
				this.m_hasCachedGameObject = true;
			}
			return this.m_cachedGameObject;
		}
	}

	// Token: 0x06009330 RID: 37680 RVA: 0x003E2078 File Offset: 0x003E0278
	public void SetGameObject(GameObject gameObject)
	{
		this.m_cachedGameObject = gameObject;
		this.m_hasCachedGameObject = true;
	}

	// Token: 0x06009331 RID: 37681 RVA: 0x003E2088 File Offset: 0x003E0288
	public void SetGameObjectEditor(GameObject gameObject)
	{
		if (gameObject)
		{
			PickupObject component = gameObject.GetComponent<PickupObject>();
			if (component)
			{
				this.pickupId = component.PickupObjectId;
				this.rawGameObject = null;
				return;
			}
		}
		this.rawGameObject = gameObject;
	}

	// Token: 0x04009AB8 RID: 39608
	[FormerlySerializedAs("gameObject")]
	public GameObject rawGameObject;

	// Token: 0x04009AB9 RID: 39609
	[PickupIdentifier]
	public int pickupId = -1;

	// Token: 0x04009ABA RID: 39610
	public float weight;

	// Token: 0x04009ABB RID: 39611
	public bool forceDuplicatesPossible;

	// Token: 0x04009ABC RID: 39612
	public DungeonPrerequisite[] additionalPrerequisites;

	// Token: 0x04009ABD RID: 39613
	[NonSerialized]
	private bool m_hasCachedGameObject;

	// Token: 0x04009ABE RID: 39614
	[NonSerialized]
	private GameObject m_cachedGameObject;
}
