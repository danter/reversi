// othello.cpp
// Contains general functions used by the Othello / Reversi game.
// Anders Asplund
// 2007-12-12

#include <cstdio>
#include <tchar.h>
#include <cctype>
#include <cstdlib>
#include <cstring>
#include <corecrt_share.h>

#include "../include/reversi.h"


void dispHelp(char argv[]) {
	fprintf(stdout, "\nUsage> %s -par1 -par2 -l <filename>\n\n", argv);
	fprintf(stdout, "-h\t\tShow this help.\n");
	fprintf(stdout, "-ht\t\tShow hints.\n");
	fprintf(stdout, "-hn\t\tShow numeric hints that the ai plays after.\n");
	fprintf(stdout, "-ai\t\tPlay against an ai.\n");
	fprintf(stdout, "-ai1\t\tWHITE player is an ai.\n");
	fprintf(stdout, "-ai2\t\tBLACK player is an ai.\n");
	fprintf(stdout, "-ai3\t\tLet 2 ai's play against eachother.\n");
	fprintf(stdout, "-l <file>\tLoads the saved game <file>.\n");
}

BOOL isArgument(const char* argType, const char* value) {
	return !strcmp(value, argType);
}

sSettings cmdRead(int board[], int argc, char *argv[], FILE *fp) {
	int load = 1;
	sSettings tmp;

	tmp.ai = NO_AI;
	tmp.ht = NO_HINTS;
	tmp.pl = BLACK;

	while( load < argc ) {
		const char* value = argv[load++];
		if(isArgument("-h", value)) {
			dispHelp(argv[0]);
			exit(1);
		}

		if(isArgument("-ai1", value) || isArgument("-ai", value)) {
			tmp.ai = WHITE_AI;
		}
		else if(isArgument("-ai2", value)) {
			tmp.ai = BLACK_AI;
		}
		else if(isArgument("-ai3", value)) {
			tmp.ai = BOTH_AI;
		}
		else if(isArgument("-ht", value)) {
			tmp.ht = HINTS;
		}
		else if(isArgument("-hn", value)) {
			tmp.ht = NUMERIC_HINTS;
		}
		else if(isArgument("-l", value)) {
			char* fileName = argv[load];
			fprintf(stdout, "File argument found, loading: %s \n", fileName);
			tmp.pl = loadGame(board, fileName, fp);
			if(tmp.pl == 0 ) {
				fprintf(stderr, "File load failed, exiting.\n\n");
				dispHelp(argv[0]);
				exit(1);
			}
			fprintf(stdout, "\n");
			load++;
		}
		else {
			fprintf(stdout, "Unknown parameter %s \n", value);
			dispHelp(argv[0]);
			exit(1);
		}
	}

	return tmp;
}


void initGame(int board[]) {
	for(auto i = 0; i<BOARD_MAX; i++)
		board[i] = ' ';

	// Place the 4 starting markers
	board[(ROW/2-1)*ROW+(COL/2-1)] = WHITE;
	board[(ROW/2)*ROW+(COL/2)] = WHITE;
	board[(ROW/2-1)*ROW+(COL/2)] = BLACK;
	board[(ROW/2)*ROW+(COL/2-1)] = BLACK;
}


int insideBoard(int x, int y) {
	if( y < ROW && y >= 0 && x < COL && x >= 0 )
		return 1;
	else
		return 0;
}


// Reads input from the player and returns a coordinate
sCord readMove() {
	sCord c;
	char tmp=0, tmp2=0;

	while((tmp = getchar()) != EOF) {
		if(toupper(tmp) == 'X') {
			tmp2 = getchar();
			if(isdigit(tmp2))
				c.y = tmp2-48;
			else {
				tmp2 = toupper(tmp2);
				c.y = tmp2-'A'+10;
			}
		}
		else if(isalpha(tmp)) {
			tmp = toupper(tmp);
			c.x = tmp-'A';
		}
		else if(isdigit(tmp)) {
			c.y = tmp-48;
		}
		else {
			return c;
		}
	}
	return c;
}


// Loads a saved game, returns the next player number, or negative value if failed
// WARNING: does not suport boards bigger then 10x10 squares!
UINT loadGame(int board[], char file[], FILE *fp) {
	int player = BLACK;
	char cTmp;
	sCord tmp;

	const auto fs = _fsopen(file, "r", _SH_DENYNO);
	if (fs == NULL) {
		return 0;
	}

	while((cTmp = getc(fs)) != EOF) {
		fprintf(stdout, "%c", cTmp);

		if(isalpha(cTmp)) {
			tmp.x = cTmp-65;
		}
		else if(isdigit(cTmp)) {
			tmp.y = cTmp-48;
		}
		else {
			fprintf(fp, "%c%d ", tmp.x+65, tmp.y);
			doMove(board, tmp, player);
			drawPiece(board, tmp, player);

			if( player == BLACK)
				player = WHITE;
			else
				player = BLACK;
		}
	}
	fclose(fs);
	return player;
}

void drawPiece(int board[], sCord c, const char val) {
	board[c.y*ROW+c.x] = val;
}


// returns 1 if the move is valid, othervise it returns 0
BOOL validMove(CINT board[], sCord c, const char val) {
	char tval;

	if(c.x<0 || c.x>COL-1 || c.y<0 || c.y>ROW-1) {
		return 0;
	}

	if( board[c.y*ROW+c.x] != ' ') {
		return FALSE;
	}

	if( val == BLACK )
		tval = WHITE;
	else if( val == WHITE)
		tval = BLACK;
	else {
		fprintf(stderr, "Unknown piece color in validMove(): exiting\n");
		exit(1);
	}

	for(auto y = c.y-1; y<=c.y+1; y++) {
		for(auto x = c.x-1; x<=c.x+1; x++) {
			if( insideBoard(x,y) ) {
				if(board[y*ROW+x] == tval) {
					if(traceMove(board, c, x-c.x, y-c.y, val)) {
						return TRUE;
					}
				}
			}

		}
	}
	return FALSE;
}


// Checks the move and turns the pieces
BOOL doMove(int board[], sCord c, const char val) {
	auto success=FALSE;
	char tval;

	if( board[c.y*ROW+c.x] != ' ') {
		return FALSE;
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
			if( insideBoard(x,y) ) {
				if(board[y*ROW+x] == tval) {
					if(traceMove(board, c, x-c.x, y-c.y, val)) {
						if(doTraceMove(board, c, x-c.x, y-c.y, val)) {
							success = TRUE;
						}
					}
				}
			}
		}
	}
	return success;
}


// Checks if a row of markers can be captured,
// ie. if a marker of your own color is at the end of the row.
// It takes the board, the current coordinate, the difference in x and y
// and what value it should check for
BOOL traceMove(CINT board[], sCord c, int dx, int dy, const char val) {
	for(c.y+=dy, c.x+=dx; insideBoard(c.x,c.y); c.y+=dy, c.x+=dx) {
		if(board[c.y*ROW+c.x] == val)
			return TRUE;
		else if(board[c.y*ROW+c.x] == ' ')
			return FALSE;
	}

	return FALSE;
}


// Almost the same as traceMove(), exept this function actually performs the move
// WARNING: traceMove() should be used before this or pieces that
// should not be turned will get turned. 
BOOL doTraceMove(int board[], sCord c, int dx, int dy, const char val) {
	for(c.y+=dy, c.x+=dx; insideBoard(c.x,c.y); c.y+=dy, c.x+=dx) {
		if(board[c.y*ROW+c.x] == val)
			return TRUE;
		else
			board[c.y*ROW+c.x] = val;
	}
	return FALSE;
}


// Tests if a player actually can do a move 
BOOL testPlayer(CINT board[], const char val) {
	int iTmp;
	sCord c;

	for(c.y=0, iTmp=0; c.y<ROW && iTmp == 0; c.y++) {
		for(c.x=0; c.x<COL && iTmp == 0; c.x++) {
			iTmp = validMove(board, c, val);
		}
	}
	return iTmp;
}

void hintPlayer(CINT board[], int hints[], const char val) {
	int iTmp;
	sCord c;

	for(iTmp=0; iTmp<BOARD_MAX; iTmp++)
		hints[iTmp] = ' ';

	for(c.y=0, iTmp=0; c.y<ROW; c.y++) {
		for(c.x=0; c.x<COL; c.x++) {
			iTmp = validMove(board, c, val);
			if(iTmp == 1)
				hints[c.y*ROW+c.x] = HINT;
		}
	}
}

// Calculates and returns the player score
int calcScore(CINT board[], const char player) {
	int i, score;

	for (i = 0, score = 0; i < BOARD_MAX; i++)
	{
		if (board[i] == player)
			score++;
	}

	return score;
}


void drawBoard(CINT board[], CINT hints[]) {
	printf("\n");

	printf("   ");
	for(auto i=0; i<COL; i++)
		printf("%c ", i+65);
	printf(" \n");

	printf("  \xda");
	for(auto i=0; i<COL-1; i++)
		printf("\xc4\xc2");
	printf("\xc4\xbf\n");


	for(auto y = 0; y<ROW; y++) {
		printf(" %x\xb3", y);
		for(auto x = 0; x<COL; x++) {
			const auto current_pos = y * ROW + x;

			if ( board[current_pos] == ' ') {
				if(hints[y*ROW+x] == 0 || hints[current_pos] == ' ')
					printf("%c\xb3", ' ');
				else if (hints[current_pos] == HINT)
					printf("%c\xb3", HINT);
				else if (hints[current_pos] < 0)
					printf("%d\xb3", hints[current_pos]);
				else
					printf("%x\xb3", hints[current_pos]);
			}
			else
				printf("%c\xb3", board[current_pos]);
		}
		printf("\n");
		if(y<ROW-1) {
			printf("  \xc3");
			for(auto i=0; i<COL-1; i++)
				printf("\xc4\xc5");
			printf("\xc4\xb4\n");
		}
	}
	printf("  \xc0");
	for(auto i=0; i<COL-1; i++)
		printf("\xc4\xc1");
	printf("\xc4\xd9\n");

	printf("\nBLACK score: %d\n", calcScore(board, BLACK));
	printf("WHITE score: %d\n", calcScore(board, WHITE));
}
