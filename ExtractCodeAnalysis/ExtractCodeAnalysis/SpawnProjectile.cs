SpawnManager.SpawnProjectile(projectile.gameObject, shootPoint.ToVector3ZisY(0f) + Quaternion.Euler(0f, 0f, fireAngle) * mod.positionOffset, Quaternion.Euler(0f, 0f, fireAngle + angleForShot), true);
SpawnManager.SpawnProjectile(projectile.gameObject, shootPoint.ToVector3ZisY(0f) + Quaternion.Euler(0f, 0f, fireAngle) * mod.InversePositionOffset, Quaternion.Euler(0f, 0f, fireAngle - angleForShot), true);
SpawnManager.SpawnProjectile(currentProjectile.gameObject, shootPoint.ToVector3ZisY(0f), Quaternion.Euler(0f, 0f, fireAngle + num), true);


// rot
BraveMathCollege.Atan2Degrees(aimDirection);
Quaternion.Euler(0f, 0f, fireAngle + angleForShot);

// 이하 공식들 중 하나 사용하여 회전각을 구함 
public static float Atan2Degrees(float y, float x) {
	return Mathf.Atan2(y, x) * 57.29578f;
}

// Token: 0x060091DC RID: 37340 RVA: 0x003DAC3C File Offset: 0x003D8E3C
public static float Atan2Degrees(Vector2 v) {
	return Mathf.Atan2(v.y, v.x) * 57.29578f;
}

public static float ToAngle(this Vector2 vector) {
	return BraveMathCollege.Atan2Degrees(vector);
}


// pos
shootPoint.ToVector3ZisY(0f) + Quaternion.Euler(0f, 0f, fireAngle) * mod.positionOffset;

