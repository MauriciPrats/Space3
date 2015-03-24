using UnityEngine;
using System.Collections;

public class Block{

	public GameObject gameObject;

	public bool enabled = true;

	public string type;

	public int i, j;

	public Block(GameObject gO,int i,int j,string type){
		gameObject = gO;
		gameObject.GetComponent<MovableBlock> ().setBlock(this);
		this.i = i;
		this.j = j;
		this.type = type;
	}
}
