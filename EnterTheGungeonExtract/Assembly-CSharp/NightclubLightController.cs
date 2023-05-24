using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001524 RID: 5412
public class NightclubLightController : MonoBehaviour
{
	// Token: 0x06007B82 RID: 31618 RVA: 0x003175CC File Offset: 0x003157CC
	private IEnumerator Start()
	{
		if (NightclubLightController.m_registeredPositions == null)
		{
			NightclubLightController.m_registeredPositions = new List<Vector2>();
		}
		NightclubLightController.m_registeredPositions.Clear();
		yield return null;
		this.m_parentRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
		this.m_transform = base.transform.parent;
		this.m_light = base.GetComponent<ShadowSystem>();
		this.m_colors = base.GetComponent<SceneLightManager>();
		NightclubLightController.m_registeredPositions.Add(this.m_transform.position.XY());
		this.m_positionIndex = NightclubLightController.m_registeredPositions.Count - 1;
		this.m_lightMaterial = this.m_light.renderer.sharedMaterial;
		this.m_colorID = Shader.PropertyToID("_TintColor");
		yield break;
	}

	// Token: 0x06007B83 RID: 31619 RVA: 0x003175E8 File Offset: 0x003157E8
	private void Update()
	{
		if (this.m_light == null)
		{
			return;
		}
		this.m_elapsed += ((!GameManager.IsBossIntro) ? BraveTime.DeltaTime : GameManager.INVARIANT_DELTA_TIME);
		if (this.m_elapsed > this.LightChangePeriod)
		{
			this.m_elapsed -= this.LightChangePeriod;
			this.m_lightMaterial.SetColor(this.m_colorID, this.m_colors.validColors[UnityEngine.Random.Range(0, this.m_colors.validColors.Length)]);
		}
		if (!this.m_inMotion)
		{
			base.StartCoroutine(this.HandleMotion());
		}
	}

	// Token: 0x06007B84 RID: 31620 RVA: 0x003176A4 File Offset: 0x003158A4
	private bool CheckPositionValid(Vector2 targetPosition)
	{
		bool flag = true;
		for (int i = 0; i < NightclubLightController.m_registeredPositions.Count; i++)
		{
			if (Vector2.Distance(NightclubLightController.m_registeredPositions[i], targetPosition) < 3f)
			{
				flag = false;
				break;
			}
		}
		return flag;
	}

	// Token: 0x06007B85 RID: 31621 RVA: 0x003176F4 File Offset: 0x003158F4
	private IEnumerator HandleMotion()
	{
		this.m_inMotion = true;
		Vector2 currentPosition = this.m_transform.position.XY();
		Vector2 targetPosition = new Vector2(UnityEngine.Random.Range(this.ValidMotionRect.xMin, this.ValidMotionRect.xMax), UnityEngine.Random.Range(this.ValidMotionRect.yMin, this.ValidMotionRect.yMax));
		targetPosition += this.m_parentRoom.area.basePosition.ToVector2();
		while (!this.CheckPositionValid(targetPosition))
		{
			targetPosition.Set(UnityEngine.Random.Range(this.ValidMotionRect.xMin, this.ValidMotionRect.xMax), UnityEngine.Random.Range(this.ValidMotionRect.yMin, this.ValidMotionRect.yMax));
			targetPosition += this.m_parentRoom.area.basePosition.ToVector2();
		}
		NightclubLightController.m_registeredPositions[this.m_positionIndex] = targetPosition;
		float elapsed = 0f;
		float duration = Vector2.Distance(targetPosition, currentPosition) / this.MotionMaxSpeed;
		while (elapsed < duration)
		{
			this.m_elapsed += ((!GameManager.IsBossIntro) ? BraveTime.DeltaTime : GameManager.INVARIANT_DELTA_TIME);
			float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			Vector2 setPosition = Vector2.Lerp(currentPosition, targetPosition, t);
			this.m_transform.position = setPosition.ToVector3ZUp(setPosition.y);
			yield return null;
		}
		yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 3f));
		this.m_inMotion = false;
		yield break;
	}

	// Token: 0x04007E1B RID: 32283
	private static List<Vector2> m_registeredPositions;

	// Token: 0x04007E1C RID: 32284
	public float LightChangePeriod = 1f;

	// Token: 0x04007E1D RID: 32285
	public float MotionMaxSpeed = 5f;

	// Token: 0x04007E1E RID: 32286
	public Rect ValidMotionRect;

	// Token: 0x04007E1F RID: 32287
	private float m_elapsed;

	// Token: 0x04007E20 RID: 32288
	private Transform m_transform;

	// Token: 0x04007E21 RID: 32289
	private ShadowSystem m_light;

	// Token: 0x04007E22 RID: 32290
	private SceneLightManager m_colors;

	// Token: 0x04007E23 RID: 32291
	private RoomHandler m_parentRoom;

	// Token: 0x04007E24 RID: 32292
	private Material m_lightMaterial;

	// Token: 0x04007E25 RID: 32293
	private int m_colorID;

	// Token: 0x04007E26 RID: 32294
	private int m_positionIndex;

	// Token: 0x04007E27 RID: 32295
	private bool m_inMotion;
}
