using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public Transform CubeParent;
	public GameObject PlayField;
	public GameObject cube;
	public GameObject GameOver;
	public GameObject Pause;
	public Texture[] PieceTextures;
	public Text ScoreText;
	public AudioSource ClearPlaneSound;
	public AudioSource MovePieceSound;

	TetrisGame game;
	TetrominoController currentPiece;
	float fallDelay = 1f;
	float timeUntilFall;
	List<GameObject> allCubes = new List<GameObject>();
	bool paused = false;
	int currentRotation;

	void Start() {
		game = new TetrisGame();
		timeUntilFall = fallDelay;
		TetrominoController.cube = cube;
		CreateNewPiece();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.P)) {
			TogglePause();
		}
		if (paused) return;

		timeUntilFall -= Time.deltaTime;
		if (timeUntilFall <= 0) {
			// Move the piece down
			if (!MovePiece(0, -1, 0)) {
				LockPiece();
			}
			timeUntilFall = fallDelay;
		}
		// Move keys
		if (Input.GetKeyDown(KeyCode.A)) {
			MovePiece(-1, 0, 0);
		} else if (Input.GetKeyDown(KeyCode.D)) {
			MovePiece(1, 0, 0);
		} else if (Input.GetKeyDown(KeyCode.W)) {
			MovePiece(0, 0, 1);
		} else if (Input.GetKeyDown(KeyCode.S)) {
			MovePiece(0, 0, -1);
		}
		// Rotate keys
		if (Input.GetKeyDown(KeyCode.J)) {
			RotatePiece(1, 1); // Y-axis
		} else if (Input.GetKeyDown(KeyCode.L)) {
			RotatePiece(1, -1);
		} else if (Input.GetKeyDown(KeyCode.I)) {
			RotatePiece(0, 1); // X-axis
		} else if (Input.GetKeyDown(KeyCode.K)) {
			RotatePiece(0, -1);
		} else if (Input.GetKeyDown(KeyCode.U)) {
			RotatePiece(2, 1); // Z-axis
		} else if (Input.GetKeyDown(KeyCode.O)) {
			RotatePiece(2, -1);
		} 

		if (Input.GetKeyDown (KeyCode.Q)) {
			StartCoroutine(RotateView(-1));
		} else if(Input.GetKeyDown (KeyCode.E)) {
			StartCoroutine(RotateView(1));
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			fallDelay = 0.1f;
			timeUntilFall = Mathf.Min(timeUntilFall, 0.1f);
		}
		if (Input.GetKeyUp(KeyCode.Space)) {
			fallDelay = 1f;
		}
	}

	public IEnumerator RotateView(int direction) {
		currentRotation = (currentRotation + direction + 4) % 4;
		for (int i = 0; i < 18; i++) {
			PlayField.transform.Rotate(0, 5 * direction, 0);
			yield return null;
		}
	}

	private bool MovePiece(int dx, int dy, int dz) {
		MovePieceSound.Play();
		int temp = dx;
		switch (currentRotation) {
		case 1:
			dx = -dz;
			dz = temp;
			break;
		case 2:
			dx = -dx;
			dz = -dz;
			break;
		case 3:
			dx = dz;
			dz = -temp;
			break;
		}
		bool m = game.Move(dx, dy, dz);
		if (m) {
			currentPiece.UpdateBlocks();
		}
		return m;
	}

	private bool RotatePiece(int axis, int direction) {
		bool r = game.Rotate(axis, direction);
		if (r) {
			currentPiece.UpdateBlocks();
		}
		return r;
	}

	private void CreateNewPiece() {
		game.NewPiece();
		Destroy(currentPiece);
		currentPiece = gameObject.AddComponent<TetrominoController>();
		Texture texture = PieceTextures[game.CurrentPiece.type];
		currentPiece.SetPiece(game.CurrentPiece, texture, CubeParent);
		allCubes.AddRange(currentPiece.blocks);
	}

	private void LockPiece() {
		game.LockPiece();
		if (game.EndGame) {
			int highScore = PlayerPrefs.GetInt("HighScore", 0);
			if (game.Score > highScore) {
				PlayerPrefs.SetInt("HighScore", game.Score);
			}
			GameOver.SetActive(true);
			this.enabled = false;
			return;
		}
		if (game.ClearedPlanes.Count > 0) {
			ClearPlaneSound.Play();
		}
		foreach (int plane in game.ClearedPlanes) {
			for (int j = 0; j < allCubes.Count; j++) {
				GameObject cube = allCubes[j];
				if (cube.transform.position.y > plane && cube.transform.position.y < plane + 1) {
					Destroy(cube);
					allCubes.Remove(cube);
					j--;
				} else if (cube.transform.position.y > plane + 1) {
					cube.transform.position += Vector3.down;
				}
			}
		}
		CreateNewPiece();
		ScoreText.text = "Score: " + game.Score.ToString();
	}
	
	public void TogglePause() {
		paused = !paused;
		Pause.SetActive(paused);
	}
}
