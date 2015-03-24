using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	//Managers
	public GameObject effectsManagerGO;
	public GameObject tableGO;
	public GameObject prefabsManagerGO;

	private EffectsManager effectsManager;
	private Table table;
	private PrefabsManager prefabsManager;


	private Block hoveringBlock = null;





	// Use this for initialization
	void Start () {
		effectsManager = effectsManagerGO.GetComponent<EffectsManager> ();
		table = tableGO.GetComponent<Table> ();
		prefabsManager = prefabsManagerGO.GetComponent<PrefabsManager> ();
		Almighty.registerGameManager (this);
	}

	
	// Update is called once per frame
	void Update () {

	}

	public void checkMatchesAndDoEffects(){
		List<Match> matches = table.checkForMatches();
		foreach (Match match in matches) {
			MatchLine1 matchEffect = new MatchLine1();
			matchEffect.Initialize(match,1f);
			effectsManager.addNewEffect(matchEffect);
		}

	}

	public void moveBlock(Block block,int toI,int toJ){
		MoveBlockToPlace moveBlockEffect = new MoveBlockToPlace ();
		Almighty.gameManager.getTable().replaceBlock(toI,toJ,block);
		moveBlockEffect.Initialize (0.2f, toI, toJ,block);
		effectsManager.addNewEffect (moveBlockEffect);
	}

	public void moveBlockTemporarily(Block block,int toI,int toJ){
		MoveBlockToPlace moveBlockEffect = new MoveBlockToPlace ();
		moveBlockEffect.Initialize (0.2f, toI, toJ,block);
		effectsManager.addNewEffect (moveBlockEffect);
	}

	public void SwapBlocksTemporarily(int i1,int j1,int i2,int j2){
		moveBlock (table.getBlock(i1,j1), i2, j2);
		moveBlock (table.getBlock(i2,j2),i1, j1);
	}

	public void hoverOverBlock(Block hoverBlock,int i,int j){
		endLastHovering ();

		moveBlockTemporarily (hoverBlock, i, j);

		this.hoveringBlock = hoverBlock;
	}
	public void endLastHovering(){
		if (this.hoveringBlock != null) {
			moveBlock (hoveringBlock, hoveringBlock.i, hoveringBlock.j);
		}
		hoveringBlock = null;
	}

	public void finishHovering(){
		hoveringBlock = null;
	}


	public bool canHoverBlockToPosition(Block block,int positioni,int positionj){
		if (block.i == positioni || block.j == positionj) {
			return true;
		}

		return false;

	}



	public Table getTable(){
		return table;
	}

	public PrefabsManager getPrefabsManager(){
		return prefabsManager;
	}

	public bool canMoveBlocks(){
		return effectsManager.canPlayerMoveBlocks ();
	}
}
