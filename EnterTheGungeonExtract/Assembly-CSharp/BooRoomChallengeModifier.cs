using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001261 RID: 4705
public class BooRoomChallengeModifier : ChallengeModifier
{
	// Token: 0x06006973 RID: 26995 RVA: 0x00294B90 File Offset: 0x00292D90
	private void Start()
	{
		if (Pixelator.Instance.AdditionalCoreStackRenderPass == null)
		{
			this.m_material = new Material(this.DarknessEffectShader);
			Pixelator.Instance.AdditionalCoreStackRenderPass = this.m_material;
		}
	}

	// Token: 0x06006974 RID: 26996 RVA: 0x00294BC8 File Offset: 0x00292DC8
	private Vector4 GetCenterPointInScreenUV(Vector2 centerPoint)
	{
		Vector3 vector = GameManager.Instance.MainCameraController.Camera.WorldToViewportPoint(centerPoint.ToVector3ZUp(0f));
		return new Vector4(vector.x, vector.y, 0f, 0f);
	}

	// Token: 0x06006975 RID: 26997 RVA: 0x00294C14 File Offset: 0x00292E14
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
			this.m_material.SetFloat("_ConeAngle", this.ConeAngle);
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

	// Token: 0x06006976 RID: 26998 RVA: 0x00294D24 File Offset: 0x00292F24
	private void Update()
	{
		RoomHandler currentRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
		List<AIActor> activeEnemies = currentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		Vector2 vector = Vector2.zero;
		Vector2 vector2 = Vector2.zero;
		float num;
		if (GameManager.Instance.PrimaryPlayer.CurrentGun && !GameManager.Instance.PrimaryPlayer.IsGhost)
		{
			num = GameManager.Instance.PrimaryPlayer.CurrentGun.CurrentAngle;
		}
		else
		{
			num = BraveMathCollege.Atan2Degrees(GameManager.Instance.PrimaryPlayer.unadjustedAimPoint.XY());
		}
		vector = GameManager.Instance.PrimaryPlayer.CenterPosition;
		float num2;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			if (GameManager.Instance.SecondaryPlayer.CurrentGun && !GameManager.Instance.SecondaryPlayer.IsGhost)
			{
				num2 = GameManager.Instance.SecondaryPlayer.CurrentGun.CurrentAngle;
			}
			else
			{
				num2 = BraveMathCollege.Atan2Degrees(GameManager.Instance.SecondaryPlayer.unadjustedAimPoint.XY());
			}
			vector2 = GameManager.Instance.SecondaryPlayer.CenterPosition;
		}
		else
		{
			vector2 = vector;
			num2 = num;
		}
		for (int i = 0; i < activeEnemies.Count; i++)
		{
			AIActor aiactor = activeEnemies[i];
			if (aiactor && aiactor.healthHaver && aiactor.IsNormalEnemy && !aiactor.healthHaver.IsBoss && !aiactor.healthHaver.IsDead)
			{
				Vector2 centerPosition = aiactor.CenterPosition;
				float num3 = BraveMathCollege.Atan2Degrees(centerPosition - vector);
				float num4 = BraveMathCollege.Atan2Degrees(centerPosition - vector2);
				bool flag = BraveMathCollege.AbsAngleBetween(num, num3) < this.ConeAngle || BraveMathCollege.AbsAngleBetween(num2, num4) < this.ConeAngle;
				if (flag)
				{
					if (aiactor.behaviorSpeculator)
					{
						aiactor.behaviorSpeculator.Stun(0.25f, true);
					}
					if (aiactor.IsBlackPhantom)
					{
						aiactor.UnbecomeBlackPhantom();
					}
				}
				else if (!aiactor.IsBlackPhantom)
				{
					aiactor.BecomeBlackPhantom();
				}
			}
		}
	}

	// Token: 0x06006977 RID: 26999 RVA: 0x00294F7C File Offset: 0x0029317C
	private void OnDestroy()
	{
		if (this.m_material != null && Pixelator.Instance)
		{
			Pixelator.Instance.AdditionalCoreStackRenderPass = null;
		}
	}

	// Token: 0x040065D8 RID: 26072
	public float ConeAngle = 45f;

	// Token: 0x040065D9 RID: 26073
	public Shader DarknessEffectShader;

	// Token: 0x040065DA RID: 26074
	private Material m_material;
}
