using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020018F2 RID: 6386
public class AkGameObjEnvironmentData
{
	// Token: 0x06009D6F RID: 40303 RVA: 0x003EFAC0 File Offset: 0x003EDCC0
	private void AddHighestPriorityEnvironmentsFromPortals(Vector3 position)
	{
		for (int i = 0; i < this.activePortals.Count; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				AkEnvironment akEnvironment = this.activePortals[i].environments[j];
				if (akEnvironment != null)
				{
					int num = this.activeEnvironmentsFromPortals.BinarySearch(akEnvironment, AkEnvironment.s_compareByPriority);
					if (num >= 0 && num < 4)
					{
						this.auxSendValues.Add(akEnvironment.GetAuxBusID(), this.activePortals[i].GetAuxSendValueForPosition(position, j));
						if (this.auxSendValues.isFull)
						{
							return;
						}
					}
				}
			}
		}
	}

	// Token: 0x06009D70 RID: 40304 RVA: 0x003EFB74 File Offset: 0x003EDD74
	private void AddHighestPriorityEnvironments(Vector3 position)
	{
		if (!this.auxSendValues.isFull && this.auxSendValues.Count() < this.activeEnvironments.Count)
		{
			for (int i = 0; i < this.activeEnvironments.Count; i++)
			{
				AkEnvironment akEnvironment = this.activeEnvironments[i];
				uint auxBusID = akEnvironment.GetAuxBusID();
				if ((!akEnvironment.isDefault || i == 0) && !this.auxSendValues.Contains(auxBusID))
				{
					this.auxSendValues.Add(auxBusID, akEnvironment.GetAuxSendValueForPosition(position));
					if (akEnvironment.excludeOthers || this.auxSendValues.isFull)
					{
						break;
					}
				}
			}
		}
	}

	// Token: 0x06009D71 RID: 40305 RVA: 0x003EFC34 File Offset: 0x003EDE34
	public void UpdateAuxSend(GameObject gameObject, Vector3 position)
	{
		if (!this.hasEnvironmentListChanged && !this.hasActivePortalListChanged && this.lastPosition == position)
		{
			return;
		}
		this.auxSendValues.Reset();
		this.AddHighestPriorityEnvironmentsFromPortals(position);
		this.AddHighestPriorityEnvironments(position);
		bool flag = this.auxSendValues.Count() == 0;
		if (!this.hasSentZero || !flag)
		{
			AkSoundEngine.SetEmitterAuxSendValues(gameObject, this.auxSendValues, (uint)this.auxSendValues.Count());
		}
		this.hasSentZero = flag;
		this.lastPosition = position;
		this.hasActivePortalListChanged = false;
		this.hasEnvironmentListChanged = false;
	}

	// Token: 0x06009D72 RID: 40306 RVA: 0x003EFCD8 File Offset: 0x003EDED8
	private void TryAddEnvironment(AkEnvironment env)
	{
		if (env != null)
		{
			int num = this.activeEnvironmentsFromPortals.BinarySearch(env, AkEnvironment.s_compareByPriority);
			if (num < 0)
			{
				this.activeEnvironmentsFromPortals.Insert(~num, env);
				num = this.activeEnvironments.BinarySearch(env, AkEnvironment.s_compareBySelectionAlgorithm);
				if (num < 0)
				{
					this.activeEnvironments.Insert(~num, env);
				}
				this.hasEnvironmentListChanged = true;
			}
		}
	}

	// Token: 0x06009D73 RID: 40307 RVA: 0x003EFD48 File Offset: 0x003EDF48
	private void RemoveEnvironment(AkEnvironment env)
	{
		this.activeEnvironmentsFromPortals.Remove(env);
		this.activeEnvironments.Remove(env);
		this.hasEnvironmentListChanged = true;
	}

	// Token: 0x06009D74 RID: 40308 RVA: 0x003EFD6C File Offset: 0x003EDF6C
	public void AddAkEnvironment(Collider environmentCollider, Collider gameObjectCollider)
	{
		AkEnvironmentPortal component = environmentCollider.GetComponent<AkEnvironmentPortal>();
		if (component != null)
		{
			this.activePortals.Add(component);
			this.hasActivePortalListChanged = true;
			for (int i = 0; i < 2; i++)
			{
				this.TryAddEnvironment(component.environments[i]);
			}
		}
		else
		{
			AkEnvironment component2 = environmentCollider.GetComponent<AkEnvironment>();
			this.TryAddEnvironment(component2);
		}
	}

	// Token: 0x06009D75 RID: 40309 RVA: 0x003EFDD4 File Offset: 0x003EDFD4
	private bool AkEnvironmentBelongsToActivePortals(AkEnvironment env)
	{
		for (int i = 0; i < this.activePortals.Count; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				if (env == this.activePortals[i].environments[j])
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06009D76 RID: 40310 RVA: 0x003EFE30 File Offset: 0x003EE030
	public void RemoveAkEnvironment(Collider environmentCollider, Collider gameObjectCollider)
	{
		AkEnvironmentPortal component = environmentCollider.GetComponent<AkEnvironmentPortal>();
		if (component != null)
		{
			for (int i = 0; i < 2; i++)
			{
				AkEnvironment akEnvironment = component.environments[i];
				if (akEnvironment != null && !gameObjectCollider.bounds.Intersects(akEnvironment.GetCollider().bounds))
				{
					this.RemoveEnvironment(akEnvironment);
				}
			}
			this.activePortals.Remove(component);
			this.hasActivePortalListChanged = true;
		}
		else
		{
			AkEnvironment component2 = environmentCollider.GetComponent<AkEnvironment>();
			if (component2 != null && !this.AkEnvironmentBelongsToActivePortals(component2))
			{
				this.RemoveEnvironment(component2);
			}
		}
	}

	// Token: 0x04009EEA RID: 40682
	private readonly List<AkEnvironment> activeEnvironments = new List<AkEnvironment>();

	// Token: 0x04009EEB RID: 40683
	private readonly List<AkEnvironment> activeEnvironmentsFromPortals = new List<AkEnvironment>();

	// Token: 0x04009EEC RID: 40684
	private readonly List<AkEnvironmentPortal> activePortals = new List<AkEnvironmentPortal>();

	// Token: 0x04009EED RID: 40685
	private readonly AkAuxSendArray auxSendValues = new AkAuxSendArray();

	// Token: 0x04009EEE RID: 40686
	private Vector3 lastPosition = Vector3.zero;

	// Token: 0x04009EEF RID: 40687
	private bool hasEnvironmentListChanged = true;

	// Token: 0x04009EF0 RID: 40688
	private bool hasActivePortalListChanged = true;

	// Token: 0x04009EF1 RID: 40689
	private bool hasSentZero;
}
