
using UnityEngine;
using System.Collections;

public interface IFireArms{
	void Shoot(float damage,float rateOfFire,float recoilModifier, bool isSemiAuto);
	void Reload();
	void Aim();
	void RecoilPattern();
}

