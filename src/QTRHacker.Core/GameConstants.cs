﻿namespace QTRHacker.Core;

public static class GameConstants
{
	public const int MaxItemTypes = 5456;
	public static int[] NPCFrameCount = new int[688]
	{
		1, 2, 2, 3, 6, 2, 2, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 2, 25, 23, 25,
		21, 15, 26, 2, 10, 1, 16, 16, 16, 3,
		1, 15, 3, 1, 3, 1, 1, 21, 25, 1,
		1, 1, 3, 3, 15, 3, 7, 7, 6, 5,
		6, 5, 3, 3, 23, 6, 3, 6, 6, 2,
		5, 6, 5, 7, 7, 4, 5, 8, 1, 5,
		1, 2, 4, 16, 5, 4, 4, 15, 16, 16,
		16, 2, 4, 6, 6, 18, 16, 1, 1, 1,
		1, 1, 1, 4, 3, 1, 1, 1, 1, 1,
		1, 5, 6, 7, 16, 1, 1, 25, 23, 12,
		20, 21, 1, 2, 2, 3, 6, 1, 1, 1,
		15, 4, 11, 1, 23, 6, 6, 6, 1, 2,
		2, 1, 3, 4, 1, 2, 1, 4, 2, 1,
		15, 3, 25, 4, 5, 7, 3, 2, 12, 12,
		4, 4, 4, 8, 8, 13, 5, 6, 4, 15,
		23, 3, 15, 8, 5, 4, 13, 15, 12, 4,
		14, 14, 3, 2, 5, 3, 2, 3, 23, 5,
		14, 16, 5, 2, 2, 12, 3, 3, 3, 3,
		2, 2, 2, 2, 2, 7, 14, 15, 16, 8,
		3, 15, 15, 16, 2, 3, 20, 25, 23, 26,
		4, 4, 16, 16, 20, 20, 20, 2, 2, 2,
		2, 8, 12, 3, 4, 2, 4, 25, 26, 26,
		6, 3, 3, 3, 3, 3, 5, 4, 4, 5,
		4, 6, 7, 15, 4, 7, 6, 1, 1, 2,
		4, 3, 5, 3, 3, 3, 4, 5, 6, 4,
		2, 1, 8, 4, 4, 1, 8, 1, 4, 15,
		15, 15, 15, 15, 15, 16, 15, 15, 15, 15,
		15, 3, 3, 3, 3, 3, 3, 16, 3, 6,
		12, 21, 21, 20, 16, 15, 15, 5, 5, 6,
		6, 5, 2, 7, 2, 6, 6, 6, 6, 6,
		15, 15, 15, 15, 15, 11, 4, 2, 2, 3,
		3, 3, 16, 15, 16, 10, 14, 12, 1, 10,
		8, 3, 3, 2, 2, 2, 2, 7, 15, 15,
		15, 6, 3, 10, 10, 6, 9, 8, 9, 8,
		20, 10, 6, 23, 1, 4, 24, 2, 4, 6,
		6, 13, 15, 15, 15, 15, 4, 4, 26, 23,
		8, 2, 4, 4, 4, 4, 2, 2, 4, 12,
		12, 9, 9, 9, 1, 9, 11, 2, 2, 9,
		5, 6, 4, 18, 8, 11, 1, 4, 5, 8,
		4, 1, 1, 1, 1, 4, 2, 5, 4, 11,
		5, 11, 1, 1, 1, 10, 10, 15, 8, 17,
		6, 6, 1, 12, 12, 13, 15, 9, 5, 10,
		7, 7, 7, 7, 7, 7, 7, 4, 4, 16,
		16, 25, 5, 7, 3, 13, 2, 6, 2, 19,
		19, 19, 20, 26, 3, 1, 1, 1, 1, 1,
		16, 21, 9, 16, 7, 6, 18, 13, 20, 12,
		12, 20, 6, 14, 14, 14, 14, 6, 1, 3,
		25, 19, 20, 22, 2, 4, 4, 4, 11, 9,
		8, 1, 9, 1, 8, 8, 12, 12, 11, 11,
		11, 11, 11, 11, 11, 11, 11, 1, 6, 9,
		1, 1, 1, 1, 1, 1, 4, 1, 10, 1,
		8, 4, 1, 5, 8, 8, 8, 8, 9, 9,
		5, 4, 8, 16, 8, 2, 3, 3, 6, 6,
		7, 13, 4, 4, 4, 4, 1, 1, 1, 8,
		25, 11, 14, 14, 14, 17, 17, 17, 5, 5,
		5, 14, 14, 14, 9, 9, 9, 9, 17, 17,
		16, 16, 18, 18, 10, 10, 10, 10, 4, 1,
		6, 9, 6, 4, 4, 4, 14, 4, 25, 13,
		3, 7, 6, 6, 1, 4, 4, 4, 4, 4,
		4, 4, 15, 15, 8, 8, 2, 6, 15, 15,
		6, 13, 5, 5, 7, 5, 14, 14, 4, 6,
		21, 1, 1, 1, 11, 12, 6, 6, 17, 6,
		16, 21, 16, 23, 5, 16, 2, 28, 28, 6,
		6, 6, 6, 6, 6, 6, 7, 7, 7, 7,
		7, 7, 7, 3, 4, 6, 27, 16, 2, 2,
		4, 3, 4, 23, 6, 1, 1, 2, 8, 8,
		14, 6, 6, 6, 6, 6, 2, 4, 14, 14,
		14, 14, 14, 14, 14, 1, 1, 13
	};
}