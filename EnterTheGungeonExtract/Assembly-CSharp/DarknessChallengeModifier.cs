using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001271 RID: 4721
public class DarknessChallengeModifier : ChallengeModifier
{
	// Token: 0x060069C0 RID: 27072 RVA: 0x00296F10 File Offset: 0x00295110
	private void Start()
	{
		this.m_material = new Material(this.DarknessEffectShader);
		Pixelator.Instance.AdditionalCoreStackRenderPass = this.m_material;
	}

	// Token: 0x060069C1 RID: 27073 RVA: 0x00296F34 File Offset: 0x00295134
	private Vector4 GetCenterPointInScreenUV(Vector2 centerPoint)
	{
		Vector3 vector = GameManager.Instance.MainCameraController.Camera.WorldToViewportPoint(centerPoint.ToVector3ZUp(0f));
		return new Vector4(vector.x, vector.y, 0f, 0f);
	}

	// Token: 0x060069C2 RID: 27074 RVA: 0x00296F80 File Offset: 0x00295180
	private void LateUpdate()
	{
		if (this.m_material != null)
		{
			float num = GameManager.Instance.PrimaryPlayer.FacingDirection;
			if (num > 270f)
			{
				num -= 360f;
			}
			if (num < -270f)
			{
				num += 360f;
			}
			this.m_material.SetFloat("_ConeAngle", this.FlashlightAngle);
			Vector4 centerPointInScreenUV = this.GetCenterPointInScreenUV(GameManager.Instance.PrimaryPlayer.CenterPosition);
			centerPointInScreenUV.z = num;
			Vector4 vector = centerPointInScreenUV;
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				num = GameManager.Instance.SecondaryPlayer.FacingDirection;
				if (num > 270f)
				{
					num -= 360f;
				}
				if (num < -270f)
				{
					num += 360f;
				}
				vector = this.GetCenterPointInScreenUV(GameManager.Instance.SecondaryPlayer.CenterPosition);
				vector.z = num;
			}
			this.m_material.SetVector("_Player1ScreenPosition", centerPointInScreenUV);
			this.m_material.SetVector("_Player2ScreenPosition", vector);
		}
	}

	// Token: 0x060069C3 RID: 27075 RVA: 0x00297090 File Offset: 0x00295290
	private IEnumerator LerpFlashlight(AdditionalBraveLight abl, bool turnOff)
	{
		float elapsed = 0f;
		float duration = 1f;
		float startPower = abl.LightIntensity;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = ((!turnOff) ? (elapsed / duration) : (1f - elapsed / duration));
			abl.LightIntensity = Mathf.Lerp(1f, startPower, t);
			yield return null;
		}
		if (turnOff)
		{
			UnityEngine.Object.Destroy(abl.gameObject);
		}
		yield break;
	}

	// Token: 0x060069C4 RID: 27076 RVA: 0x002970B4 File Offset: 0x002952B4
	private void OnDestroy()
	{
		if (Pixelator.Instance)
		{
			Pixelator.Instance.AdditionalCoreStackRenderPass = null;
		}
	}

	// Token: 0x04006639 RID: 26169
	public Shader DarknessEffectShader;

	// Token: 0x0400663A RID: 26170
	public float FlashlightAngle = 25f;

	// Token: 0x0400663B RID: 26171
	private AdditionalBraveLight[] flashlights;

	// Token: 0x0400663C RID: 26172
	private int m_valueMinID;

	// Token: 0x0400663D RID: 26173
	private RoomHandler m_room;

	// Token: 0x0400663E RID: 26174
	private Material m_material;
}
