
using UnityEngine;
using System.Collections;

public interface IFireArms{
	void Shoot(float force,float damage,float rateOfFire, bool isSemiAuto);
	void Reload();
	void Aim();
	void RecoilPattern();
}

