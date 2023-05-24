public float GetAngleForShot(float alternateAngleSign = 1f, float varianceMultiplier = 1f, float? overrideAngleVariance = null)
	{
		float num = alternateAngleSign * this.angleFromAim;
		float num2 = ((overrideAngleVariance == null) ? this.GetAngleVariance(varianceMultiplier) : overrideAngleVariance.Value);
		return num + num2;
	}

public float GetAngleVariance(float customVariance, float varianceMultiplier)
	{
		float num = BraveMathCollege.GetLowDiscrepancyRandom(ProjectileModule.m_angleVarianceIterator) * (2f * customVariance) - customVariance;
		ProjectileModule.m_angleVarianceIterator++;
		return num * varianceMultiplier;
	}