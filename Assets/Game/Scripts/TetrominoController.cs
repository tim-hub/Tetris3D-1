using UnityEngine;
using System.Collections;

public class TetrominoController : MonoBehaviour {

	public static GameObject cube;

	public Tetromino piece;
	public GameObject[] blocks;

	public void SetPiece(Tetromino piece, Texture texture, Transform cubeParent) {
		this.piece = piece;
		blocks = new GameObject[4];
		for (int i = 0; i < 4; i++) {
			GameObject block = Instantiate(cube);
			block.transform.SetParent(cubeParent);
			block.transform.localRotation = Quaternion.identity;
			Material mat = block.GetComponent<MeshRenderer>().material;
			mat.mainTexture = texture;
			blocks[i] = block;
		}
		UpdateBlocks();
	}

	public void UpdateBlocks() {
		for (int i = 0; i < 4; i++) {
			blocks[i].transform.localPosition = new Vector3(piece.bx(i), piece.by(i) + 0.5f, piece.bz(i));
		}
	}
}
