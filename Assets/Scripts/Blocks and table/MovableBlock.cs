using UnityEngine;
using System.Collections;

public class MovableBlock : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private Block block;

	private Vector3 originalPosition;

	int lastHoveredI,lastHoveredJ;

	public void setBlock(Block block){
		this.block = block;
	}

	void updatePosition(){
		Vector3 position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		transform.position = new Vector3 (position.x,position.y, -2f);
	}

	void OnMouseDown(){
		if (Almighty.gameManager.canMoveBlocks ()) {
			originalPosition = transform.position;
			updatePosition ();
			lastHoveredI = -1;
			lastHoveredJ = -1;
		}
	} 

	void OnMouseUp(){
		if (Almighty.gameManager.canMoveBlocks ()) {
			int i = 0; 
			int j = 0;
			Almighty.gameManager.getTable ().getCorrespondantIndexPosition (out i, out j, transform.position);
			if (Almighty.gameManager.getTable ().insideBounds (i, j) && Almighty.gameManager.canHoverBlockToPosition(block,i,j)) {
				Block blockToReplace = Almighty.gameManager.getTable ().getBlock (i, j);
				Almighty.gameManager.moveBlock (blockToReplace, block.i, block.j);
				Almighty.gameManager.moveBlock (block, i, j);
				//Almighty.gameManager.getTable ().replaceBlock (i, j, block);
				Almighty.gameManager.checkMatchesAndDoEffects ();
			} else {
				Almighty.gameManager.moveBlock (block, block.i, block.j);
			}
			Almighty.gameManager.finishHovering();
		}
	} 



	void OnMouseDrag(){
		if (Almighty.gameManager.canMoveBlocks ()) {
			updatePosition ();
			int i = 0; 
			int j = 0;
			Almighty.gameManager.getTable ().getCorrespondantIndexPosition (out i, out j, transform.position);

			if(Almighty.gameManager.getTable().insideBounds(i,j) && Almighty.gameManager.canHoverBlockToPosition(block,i,j)){
				//Almighty.gameManager.hoverOverBlock(Almighty.gameManager.getTable().getBlock(i,j),block.i,block.j);
				if(i==block.i && j == block.j){
					Almighty.gameManager.endLastHovering();
				}else{
					if(i!=lastHoveredI || j != lastHoveredJ){
						Almighty.gameManager.hoverOverBlock(Almighty.gameManager.getTable().getBlock(i,j),block.i,block.j);
					}
				}
			}else{
				Almighty.gameManager.endLastHovering();
			}
			lastHoveredI = i;
			lastHoveredJ = j;

		}
	}
}
