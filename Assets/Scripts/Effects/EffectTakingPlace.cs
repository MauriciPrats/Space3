using UnityEngine;
using System.Collections;

public class EffectTakingPlace{

	public bool isBlockingMovingBlocks = false;
	public bool makesMatch = false;

	public virtual void Update(){
		Debug.Log ("Update of The parent");

	}

	public virtual bool isFinished(){
		Debug.Log ("IsFinished of The parent");
		return false;
	}

}
