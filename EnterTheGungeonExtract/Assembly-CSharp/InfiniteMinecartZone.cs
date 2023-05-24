using System;
using UnityEngine;

// Token: 0x020011E4 RID: 4580
public class InfiniteMinecartZone : DungeonPlaceableBehaviour
{
	// Token: 0x06006630 RID: 26160 RVA: 0x0027B474 File Offset: 0x00279674
	public void Start()
	{
	}

	// Token: 0x06006631 RID: 26161 RVA: 0x0027B478 File Offset: 0x00279678
	private void Update()
	{
		if (this.IsPlayerInRegion() && this.m_remainingLoops > 0)
		{
			InfiniteMinecartZone.InInfiniteMinecartZone = true;
			PlayerController primaryPlayer = GameManager.Instance.PrimaryPlayer;
			if (!this.processed && primaryPlayer.IsInMinecart)
			{
				ParticleSystem componentInChildren = primaryPlayer.currentMineCart.Sparks_A.GetComponentInChildren<ParticleSystem>();
				ParticleSystem componentInChildren2 = primaryPlayer.currentMineCart.Sparks_B.GetComponentInChildren<ParticleSystem>();
				componentInChildren.simulationSpace = ParticleSystemSimulationSpace.Local;
				componentInChildren2.simulationSpace = ParticleSystemSimulationSpace.Local;
			}
			IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Round);
			IntRect intRect = new IntRect(intVector.x, intVector.y, this.RegionWidth, this.RegionHeight);
			if (primaryPlayer.CenterPosition.x > (float)intVector.x + (float)this.RegionWidth * 0.75f && primaryPlayer.IsInMinecart)
			{
				this.m_remainingLoops--;
				Vector2 vector = GameManager.Instance.MainCameraController.transform.position.XY() - primaryPlayer.currentMineCart.transform.position.XY();
				PathMover component = primaryPlayer.currentMineCart.GetComponent<PathMover>();
				Vector2 vector2 = component.transform.position.XY();
				component.WarpToNearestPoint(intVector.ToVector2() + new Vector2(0f, (float)this.RegionHeight / 2f));
				Vector2 vector3 = component.transform.position.XY() - vector2;
				for (int i = 0; i < primaryPlayer.orbitals.Count; i++)
				{
					primaryPlayer.orbitals[i].GetTransform().position = primaryPlayer.orbitals[i].GetTransform().position + vector3.ToVector3ZisY(0f);
					if (primaryPlayer.orbitals[i] is PlayerOrbital)
					{
						(primaryPlayer.orbitals[i] as PlayerOrbital).ReinitializeWithDelta(vector3);
					}
					else
					{
						primaryPlayer.orbitals[i].Reinitialize();
					}
				}
				for (int j = 0; j < primaryPlayer.trailOrbitals.Count; j++)
				{
					primaryPlayer.trailOrbitals[j].transform.position = primaryPlayer.trailOrbitals[j].transform.position + vector3.ToVector3ZisY(0f);
					primaryPlayer.trailOrbitals[j].specRigidbody.Reinitialize();
				}
				primaryPlayer.currentMineCart.ForceUpdatePositions();
				GameManager.Instance.MainCameraController.transform.position = (primaryPlayer.currentMineCart.transform.position.XY() + vector).ToVector3ZUp(GameManager.Instance.MainCameraController.transform.position.z);
			}
		}
		else
		{
			InfiniteMinecartZone.InInfiniteMinecartZone = false;
		}
	}

	// Token: 0x06006632 RID: 26162 RVA: 0x0027B78C File Offset: 0x0027998C
	private bool IsPlayerInRegion()
	{
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Round);
		IntRect intRect = new IntRect(intVector.x, intVector.y, this.RegionWidth, this.RegionHeight);
		return intRect.Contains(GameManager.Instance.PrimaryPlayer.CenterPosition);
	}

	// Token: 0x04006202 RID: 25090
	public static bool InInfiniteMinecartZone;

	// Token: 0x04006203 RID: 25091
	public int RegionWidth = 10;

	// Token: 0x04006204 RID: 25092
	public int RegionHeight = 3;

	// Token: 0x04006205 RID: 25093
	private int m_remainingLoops = 10;

	// Token: 0x04006206 RID: 25094
	private bool processed;
}
