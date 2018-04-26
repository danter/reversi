// ai.cpp
// Contains AI functions used by the Othello / Reversi game.
// Anders Asplund
// 2007-12-12

#include <cstdio>
#include <tchar.h>
#include <cstdlib>
#include <ctime>

#include "../include/reversi.h"


// Evaluates coordinates of the board, depending on if the spot is
// capturable for the player or not. The value depends on how many pieces you
// would capture, and if the spot is a corner or not. It will return the
// coordinates for the spot with the best score, it can then be used to
// capture that spot
sCord AIEvalBoard(const int board[], int score[], const char val) {
	int x,y, i;
	int test[BOARD_MAX+1];
	sCord c, tmp;

	for(x=0; x<BOARD_MAX; x++)
		test[x] = 0;

	hintPlayer(board, test, val);

	for(y=0; y<ROW; y++) {
		for(x=0; x<COL; x++) {
			if(test[y*ROW+x] == HINT) {
				tmp.y = y;
				tmp.x = x;
				score[y*ROW+x] = AIScoreCalc(board, tmp, val);
			}
			else
				score[y*ROW+x] = 0;
		}
	}

	// Add score for hotspots like corners 
	AIScoreTable(test);

	for(y=0; y<ROW; y++)
		for(x=0; x<COL; x++)
			if(score[y*ROW+x] > 0)
				score[y*ROW+x] += test[y*ROW+x];

	srand( static_cast<unsigned>(time(NULL)));

	// Check max score, and randomise between equal scores
	for(y=0, i=0; y<ROW; y++) {
		for(x=0; x<COL; x++) {
			if(score[y*ROW+x] > i) {
				i = score[y*ROW+x];
				c.y = y;
				c.x = x;
			}
			else if ( score[y*ROW+x] == i){
				const UINT random = rand() % 100;
				if(random < 50) {
					i = score[y*ROW+x];
					c.y = y;
					c.x = x;
				}
			}
		}
	}

	return c;
}

int AIScoreCalc(const int board[], sCord c, const char val) {
	int score=0;
	char tval;

	if( board[c.y*ROW+c.x] != ' ') {
		return 0;
	}

	if( val == BLACK )
		tval = WHITE;
	else if( val == WHITE)
		tval = BLACK;
	else {
		fprintf(stderr, "Unknown piece color in doMove(): exiting\n");
		exit(1);
	}

	for(auto y = c.y-1; y<=c.y+1; y++) {
		for(auto x = c.x-1; x<=c.x+1; x++) {
			if( insideBoard(x, y) ) {
				if(board[y*ROW+x] == tval) {
					if(traceMove(board, c, x-c.x, y-c.y, val)) {
						score += AITraceMove(board, c, x-c.x, y-c.y, val);
					}
				}
			}
		}
	}
	return score;
}

int AITraceMove(const int board[], sCord c, int dx, int dy, const char val) {
	int tmp=0;
	for(c.y+=dy, c.x+=dx; insideBoard(c.x, c.y); c.y+=dy, c.x+=dx) {
		if(board[c.y*ROW+c.x] == val)
			return tmp;
		else
			tmp++;
	}
	return tmp;
}


void AIScoreTable(int score[]) {
	int i, x, y;

	// Clear the array 
	for(y=0; y<ROW; y++)
		for(x=0; x<COL; x++)
			score[y*ROW+x] = 0;

	// 1 bonus for sides and 2 bonus for corners 
	for(i=0; i<COL; i++)
		score[0*ROW+i]++;

	for(i=0; i<ROW; i++)
		score[i*ROW+(COL-1)]++;

	for(i=0; i<ROW; i++)
		score[i*ROW+0]++;

	for(i=0; i<COL; i++)
		score[(ROW-1)*ROW+i]++;


	// Still a bit extra bonus for corners 
	score[0*ROW+0]++;
	score[0*ROW+(COL-1)]++;
	score[(ROW-1)*ROW+(COL-1)]++;
	score[(ROW-1)*ROW+0]++;

	// Preparation for bad zones 
	for(y=0; y<ROW; y++)
		for(x=0; x<COL; x++)
			score[y*ROW+x]+=2;

	// 1 minus for spots 1 square from sides and
	// 2 minus for spots 1 square from corners 
	for(i=1; i<COL-1; i++)
		score[1*ROW+i]--;

	for(i=1; i<ROW-1; i++)
		score[i*ROW+(COL-2)]--;

	for(i=1; i<ROW-1; i++)
		score[i*ROW+1]--;

	for(i=1; i<COL-1; i++)
		score[(ROW-2)*ROW+i]--;

	// 2 minus for spots 1 square from corners
	// not yet getting minus 
	score[1*ROW+0]-=2;
	score[0*ROW+1]-=2;
	score[0*ROW+(COL-2)]-=2;
	score[1*ROW+(COL-1)]-=2;
	score[(ROW-2)*ROW+(COL-1)]-=2;
	score[(ROW-1)*ROW+(COL-2)]-=2;
	score[(ROW-1)*ROW+1]-=2;
	score[(ROW-2)*ROW+0]-=2;
}
