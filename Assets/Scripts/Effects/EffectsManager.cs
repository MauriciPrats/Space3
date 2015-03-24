using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectsManager : MonoBehaviour {



	private List<EffectTakingPlace> actualEffects = new List<EffectTakingPlace>(0);
	private bool canMoveBlocks = true;
	private bool makingMatches = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		canMoveBlocks = true;
		bool stillMatching = false;
		foreach (EffectTakingPlace effect in actualEffects) {
			if(!effect.isFinished()){
				effect.Update();
				if(effect.isBlockingMovingBlocks){
					canMoveBlocks = false;
				}
				if(effect.makesMatch){
					stillMatching = true;
				}

			}
		}

		if (!stillMatching && makingMatches) {
			canMoveBlocks = false;
			Almighty.gameManager.checkMatchesAndDoEffects();
		}
		makingMatches = stillMatching;
	}

	public void addNewEffect(EffectTakingPlace newEffect){
		bool replaced = false;
		for(int i = 0;i<actualEffects.Count;i++){
			if(actualEffects[i].isFinished()){
				replaced = true;
				//We replace the effect for the new one

				actualEffects[i] = newEffect;
				break;
			}
		}
		if (!replaced) {
			actualEffects.Add(newEffect);
		}
	}

	public bool canPlayerMoveBlocks(){
		return canMoveBlocks;
	}
}
