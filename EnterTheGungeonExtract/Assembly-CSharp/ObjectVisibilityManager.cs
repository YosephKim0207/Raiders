using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001527 RID: 5415
public class ObjectVisibilityManager : BraveBehaviour
{
	// Token: 0x06007B93 RID: 31635 RVA: 0x00317BA0 File Offset: 0x00315DA0
	private void Start()
	{
		if (!this.m_initialized)
		{
			RoomHandler roomHandler = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
			if (roomHandler == null)
			{
				roomHandler = GameManager.Instance.Dungeon.data[base.transform.position.IntXY(VectorConversions.Round)].nearestRoom;
			}
			this.Initialize(roomHandler, false);
		}
	}

	// Token: 0x06007B94 RID: 31636 RVA: 0x00317C18 File Offset: 0x00315E18
	public void Initialize(RoomHandler room, bool allowEngagement = false)
	{
		if (room == null || !this || !base.gameObject)
		{
			Debug.LogWarning("Failing to initialize OVM!");
			return;
		}
		this.m_initialized = true;
		this.parentRoom = room;
		this.currentVisibility = room.visibility;
		this.parentRoom.BecameVisible += this.HandleParentRoomEntered;
		this.parentRoom.BecameInvisible += this.HandleParentRoomExited;
		this.m_object = base.gameObject;
		this.ChangeToVisibility(this.currentVisibility, allowEngagement);
	}

	// Token: 0x06007B95 RID: 31637 RVA: 0x00317CB4 File Offset: 0x00315EB4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.parentRoom != null)
		{
			this.parentRoom.BecameVisible -= this.HandleParentRoomEntered;
			this.parentRoom.BecameInvisible -= this.HandleParentRoomExited;
		}
	}

	// Token: 0x06007B96 RID: 31638 RVA: 0x00317D00 File Offset: 0x00315F00
	public void ResetRenderersList()
	{
		this.m_renderers.Clear();
	}

	// Token: 0x06007B97 RID: 31639 RVA: 0x00317D10 File Offset: 0x00315F10
	private void AcquireRenderers()
	{
		this.m_renderers = new List<Component>();
		this.m_renderers.AddRange(this.m_object.GetComponentsInChildren<ParticleSystem>());
		this.m_renderers.AddRange(this.m_object.GetComponentsInChildren<AIActor>());
		this.m_renderers.AddRange(this.m_object.GetComponentsInChildren<MeshRenderer>());
		this.m_renderers.AddRange(this.m_object.GetComponentsInChildren<Light>());
	}

	// Token: 0x06007B98 RID: 31640 RVA: 0x00317D80 File Offset: 0x00315F80
	private void ToggleRenderers(bool simpleEnabled, RoomHandler.VisibilityStatus visibilityStatus, bool allowEngagement)
	{
		for (int i = 0; i < this.m_renderers.Count; i++)
		{
			Component component = this.m_renderers[i];
			if (component)
			{
				if (component is Renderer)
				{
					Renderer renderer = component as Renderer;
					if (!this.m_ignoredRenderers.Contains(renderer))
					{
						if (renderer.enabled != simpleEnabled)
						{
							renderer.enabled = simpleEnabled;
						}
					}
				}
				else if (component is Light)
				{
					bool flag = visibilityStatus == RoomHandler.VisibilityStatus.CURRENT;
					if (base.gameObject.activeSelf)
					{
						Light light = component as Light;
						if (light.enabled != flag)
						{
							if (flag)
							{
								base.StartCoroutine(this.ActivateLight(light));
							}
							else
							{
								base.StartCoroutine(this.DeactivateLight(light));
							}
						}
					}
				}
				else if (component is ParticleSystem)
				{
					ParticleSystem particleSystem = component as ParticleSystem;
					particleSystem.GetComponent<Renderer>().enabled = simpleEnabled;
				}
				else if (component is AIActor)
				{
					AIActor aiactor = component as AIActor;
					aiactor.enabled = simpleEnabled;
					if (allowEngagement && simpleEnabled && base.gameObject.activeSelf && !aiactor.healthHaver.IsBoss)
					{
						aiactor.HasBeenEngaged = true;
					}
				}
				else if (component is Behaviour)
				{
					Behaviour behaviour = component as Behaviour;
					if (!behaviour || behaviour.enabled != simpleEnabled)
					{
						behaviour.enabled = simpleEnabled;
					}
				}
			}
		}
		if (base.aiShooter)
		{
			base.aiShooter.UpdateGunRenderers();
			base.aiShooter.UpdateHandRenderers();
		}
		if (this.OnToggleRenderers != null)
		{
			this.OnToggleRenderers();
		}
	}

	// Token: 0x06007B99 RID: 31641 RVA: 0x00317F64 File Offset: 0x00316164
	public void ChangeToVisibility(RoomHandler.VisibilityStatus status, bool allowEngagement = true)
	{
		if (!this)
		{
			return;
		}
		if (this.m_renderers == null || this.m_renderers.Count == 0)
		{
			this.AcquireRenderers();
		}
		if (this.m_renderers == null || this.m_renderers.Count == 0)
		{
			BraveUtility.Log("Expensive visibility management on unmanaged object...", Color.yellow, BraveUtility.LogVerbosity.IMPORTANT);
			return;
		}
		this.currentVisibility = status;
		bool flag = false;
		switch (this.currentVisibility)
		{
		case RoomHandler.VisibilityStatus.OBSCURED:
			flag = false;
			break;
		case RoomHandler.VisibilityStatus.VISITED:
			flag = true;
			break;
		case RoomHandler.VisibilityStatus.CURRENT:
			flag = true;
			break;
		case RoomHandler.VisibilityStatus.REOBSCURED:
			flag = false;
			break;
		}
		this.ToggleRenderers(flag, status, allowEngagement);
	}

	// Token: 0x06007B9A RID: 31642 RVA: 0x00318020 File Offset: 0x00316220
	private IEnumerator DeactivateLight(Light l)
	{
		while (this.m_activatingLight)
		{
			yield return null;
		}
		this.m_activatingLight = true;
		float startIntensity = l.intensity;
		float elapsed = 0f;
		while (elapsed < 0.5f)
		{
			elapsed += BraveTime.DeltaTime;
			l.intensity = Mathf.Lerp(startIntensity, 0f, Mathf.Pow(elapsed / 0.5f, 2f));
			yield return null;
		}
		l.enabled = false;
		l.intensity = startIntensity;
		this.m_activatingLight = false;
		yield break;
	}

	// Token: 0x06007B9B RID: 31643 RVA: 0x00318044 File Offset: 0x00316244
	private IEnumerator ActivateLight(Light l)
	{
		while (this.m_activatingLight)
		{
			yield return null;
		}
		this.m_activatingLight = true;
		float targetIntensity = l.intensity;
		l.intensity = 0f;
		l.enabled = true;
		float elapsed = 0f;
		while (elapsed < 0.5f)
		{
			elapsed += BraveTime.DeltaTime;
			l.intensity = Mathf.Lerp(0f, targetIntensity, Mathf.Pow(elapsed / 0.5f, 2f));
			yield return null;
		}
		l.intensity = targetIntensity;
		this.m_activatingLight = false;
		yield break;
	}

	// Token: 0x06007B9C RID: 31644 RVA: 0x00318068 File Offset: 0x00316268
	private void HandleParentRoomEntered(float delay)
	{
		if (this.SuppressPlayerEnteredRoom)
		{
			return;
		}
		this.ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
	}

	// Token: 0x06007B9D RID: 31645 RVA: 0x00318080 File Offset: 0x00316280
	private IEnumerator DelayedBecameVisible(float delay)
	{
		yield return new WaitForSeconds(delay);
		this.ChangeToVisibility(RoomHandler.VisibilityStatus.CURRENT, true);
		yield break;
	}

	// Token: 0x06007B9E RID: 31646 RVA: 0x003180A4 File Offset: 0x003162A4
	private void HandleParentRoomExited()
	{
		this.ChangeToVisibility(RoomHandler.VisibilityStatus.VISITED, true);
	}

	// Token: 0x06007B9F RID: 31647 RVA: 0x003180B0 File Offset: 0x003162B0
	public void AddIgnoredRenderer(Renderer renderer)
	{
		if (!this.m_ignoredRenderers.Contains(renderer))
		{
			this.m_ignoredRenderers.Add(renderer);
		}
	}

	// Token: 0x06007BA0 RID: 31648 RVA: 0x003180D0 File Offset: 0x003162D0
	public void RemoveIgnoredRenderer(Renderer renderer)
	{
		this.m_ignoredRenderers.Remove(renderer);
	}

	// Token: 0x04007E36 RID: 32310
	public RoomHandler parentRoom;

	// Token: 0x04007E37 RID: 32311
	private RoomHandler.VisibilityStatus currentVisibility;

	// Token: 0x04007E38 RID: 32312
	private List<Component> m_renderers;

	// Token: 0x04007E39 RID: 32313
	private bool m_initialized;

	// Token: 0x04007E3A RID: 32314
	private GameObject m_object;

	// Token: 0x04007E3B RID: 32315
	private List<Renderer> m_ignoredRenderers = new List<Renderer>();

	// Token: 0x04007E3C RID: 32316
	public bool SuppressPlayerEnteredRoom;

	// Token: 0x04007E3D RID: 32317
	public Action OnToggleRenderers;

	// Token: 0x04007E3E RID: 32318
	private bool m_activatingLight;
}
