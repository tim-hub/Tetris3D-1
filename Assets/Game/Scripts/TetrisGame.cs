using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TetrisGame {
	public int[, ,] grid =  new int[5,12,5];
	public Tetromino CurrentPiece;
	public List<int> ClearedPlanes = new List<int>();
	public bool EndGame = false;
	public int Score = 0;

	public void NewPiece() {
		int type = UnityEngine.Random.Range(0, Tetromino.types.GetLength(0));
		CurrentPiece = new Tetromino(0, 9, 0, type);
	}

	public void LockPiece() {
		for (int i = 0; i < 4; i++) {
			grid [CurrentPiece.bx(i) + 2, CurrentPiece.by(i), CurrentPiece.bz(i) + 2] = 1;
			if (CurrentPiece.by(i) >= 7)
				EndGame = true;
		}
		if (!EndGame)
			Score += 10;
		if (!EndGame) {
			ClearCheck();
		}
	}

	public bool Rotate(int axis, int direction) {
		CurrentPiece.Rotate(axis, direction);
		if (CheckForCollision()) {
			CurrentPiece.Rotate(axis, -direction);
			return false;
		}
		return true;
	}

	public void ClearCheck() {
		bool skipx = false;
		ClearedPlanes.Clear();
		for (int y=9; y >= 0; y--) {
			for(int x=0; x < 5; x++) {
				for(int z=0; z < 5; z++) {
					if (grid[x, y, z] == 0) {
						skipx = true;
						break;
					}
				}
				if(skipx) {
					break;
				}
			}
			if(!skipx){
				ClearedPlanes.Add(y);
				ClearAbove(y);
			}
			skipx = false;
		}
		if (ClearedPlanes.Count > 0) {
			Score += 500 * (int)Math.Pow(2, ClearedPlanes.Count);
		}
	}

	public bool Move(int dx, int dy, int dz) {
		CurrentPiece.x += dx;
		CurrentPiece.y += dy;
		CurrentPiece.z += dz;
		if (CheckForCollision()) {
			CurrentPiece.x -= dx;
			CurrentPiece.y -= dy;
			CurrentPiece.z -= dz;
			return false;
		}
		return true;
	}

	public void ClearAbove(int height) {
		for (int y = height; y < 7; y++) {
			for (int x = 0; x < 5; x++) {
				for (int z = 0; z < 5; z++) {
					grid[x,y,z] = grid[x,y+1,z];
				}
			}
		}
	}

	private bool CheckForCollision() {
		for (int i = 0; i < 4; i++) {
			if (CurrentPiece.by(i) == -1)
				return true;
			if (Math.Abs(CurrentPiece.bz(i)) > 2 || Math.Abs(CurrentPiece.bx(i)) > 2)
			   return true;
			if (grid[CurrentPiece.bx(i) + 2, CurrentPiece.by(i), CurrentPiece.bz(i) + 2] != 0)
				return true;
		}
		return false;
	}
}

