using UnityEngine;
using System.Collections;

public class Tetromino {
	public static int[,,] types = {
		{{1, 0, 0}, {0, 0, 0}, {-1, 0, 0}, {-2, 0, 0}}, // Line
		{{1, 0, 0}, {0, 0, 0}, {-1, 0, 0}, {-1, 0, 1}}, // L
		{{1, 0, 0}, {0, 0, 0}, {0, 0, 1}, {-1, 0, 0}}, // T
		{{1, 0, 0}, {0, 0, 0}, {0, 0, 1}, {-1, 0, 1}}, // S
		{{1, 0, 0}, {0, 0, 0}, {1, 0, 1}, {0, 0, 1}}, // Square
		{{1, 0, 0}, {0, 0, 0}, {0, 1, 0}, {0, 0, 1}}, // Corner
		{{1, 0, 0}, {0, 0, 0}, {0, 1, 0}, {0, 1, 1}} // 3D squiggly
	};

	public int type;

	//position
	public int x;
	public int y;
	public int z;

	// The relative position of each of the four cubes 
	private int[] blockx = new int[4];
	private int[] blocky = new int[4];
	private int[] blockz = new int[4];

	public int bx(int i) {
		return x + blockx[i];
	}
	public int by(int i) {
		return y + blocky[i];
	}
	public int bz(int i) {
		return z + blockz[i];
	}

	public Tetromino(int x, int y, int z, int type) {
		this.type = type;
		this.x = x;
		this.y = y;
		this.z = z;

		for (int i = 0; i < 4; i++) {
			blockx[i] = types[type, i, 0];
			blocky[i] = types[type, i, 1];
			blockz[i] = types[type, i, 2];
		}
	}

	public void Rotate(int axis, int direction) {
		int[] otherAxis1, otherAxis2;
		if (axis == 0) { // rotating around the x-axis
			otherAxis1 = blocky;
			otherAxis2 = blockz;
		} else if (axis == 1) { // rotating around the y-axis
			otherAxis1 = blockx;
			otherAxis2 = blockz;
		} else { // rotating around the z-axis
			otherAxis1 = blockx;
			otherAxis2 = blocky;
		}
		for (int i = 0; i < 4; i++) {
			int temp = otherAxis1[i];
			if (direction > 0) {
				otherAxis1[i] = otherAxis2[i];
				otherAxis2[i] = -temp;
			} else {
				otherAxis1[i] = -otherAxis2[i];
				otherAxis2[i] = temp;
			}
		}
	}
}
