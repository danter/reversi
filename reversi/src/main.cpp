// main.cpp
// The main function for the Othello / Reversi game
// Anders Asplund
// 2007-12-12

#include <cstdio>
#include <cstdlib>
#include <corecrt_share.h>

#include "../include/reversi.h"

#define LOG_NAME "gamelog.txt"


BOOL isAiPlayerTurn(sSettings set)
{
	const auto ai = set.ai;
	const auto player = set.pl;

	return 	ai == BOTH_AI ||
			ai == BLACK_AI && player == BLACK ||
			ai == WHITE_AI && player == WHITE;
}

sCord readInput(int board[], int hints[], sSettings set)
{
	sCord move;

	if(isAiPlayerTurn(set)) {
		move = AIEvalBoard(board, hints, set.pl);
		fprintf(stdout, "%c%d\n", move.x+65, move.y);
	}
	else {
		while(!validMove(board, move = readMove(), set.pl)) {
			printf("You can't place a piece there!\n");
			if(set.pl == BLACK)
				printf("\nBLACK, make your move: ");
			else
				printf("\nWHITE, make your move: ");
		}
	}

	return move;
}

void printScore(int board[])
{
	int blackScore, whiteScore;

	// Write score and close the game 
	printf("\nNeither BLACK nor WHITE can make a move, GAME OVER!\n");
	printf("The score was:\n");
	printf("BLACK: %d\n", blackScore = calcScore(board, BLACK));
	printf("WHITE: %d\n", whiteScore = calcScore(board, WHITE));

	if(blackScore == whiteScore)
		printf("The game was a DRAW\n");
	else if(whiteScore < blackScore)
		printf("Winner is BLACK\n");
	else
		printf("Winner is WHITE\n");
}

void changePlayer(sSettings *set)
{
	if(set->pl == BLACK)
		set->pl = WHITE;
	else
		set->pl = BLACK;
}

int main(int argc, char *argv[]) {
	auto canBlackMove = TRUE;
	auto canWhiteMove = TRUE;
	int board[BOARD_MAX+1];
	int hints[BOARD_MAX+1] = { 0 };

	const auto fp = _fsopen(LOG_NAME, "w", _SH_DENYNO);
	if (fp == NULL) {
		fprintf(stderr, "Can't open %s\n", LOG_NAME);
		return 1;
	}

	initGame(board);
	sSettings set = cmdRead(board, argc, argv, fp);

	set.ai = WHITE_AI;

//	AIScoreTable(temp);
//	drawBoard(board, temp);

	// Calculate hints
	if (set.ht == 1)
	{
		hintPlayer(board, hints, set.pl);
	}
	else if (set.ht == 2)
	{
		AIEvalBoard(board, hints, set.pl);
	}
	else
	{
		for (auto i = 0; i < BOARD_MAX; i++)
		{
			hints[i] = ' ';
		}
	}

	drawBoard(board, hints);


	// The main game loop 
	while(canBlackMove != 0 || canWhiteMove != 0) {

		if(set.pl == BLACK)
			printf("\nBLACK, make your move: ");
		else if(set.pl == WHITE)
			printf("\nWHITE, make your move: ");
		else break;

		// Input from either an AI or a player 
		const auto move = readInput(board, hints, set);

		// log and make the move 
		fprintf(fp, "%c%d ", move.x+65, move.y);
		doMove(board, move, set.pl);
		drawPiece(board, move, set.pl);

		changePlayer(&set);

		// Test if players can make a move 
		canBlackMove = testPlayer(board, BLACK);
		canWhiteMove = testPlayer(board, WHITE);

		// And change player if he can't 
		if(set.pl == BLACK && canBlackMove == 0) {
			printf("\nBLACK, can't make a move\n");
			set.pl = WHITE;
		}

		if(set.pl == WHITE && canWhiteMove == 0) {
			printf("\nWHITE, can't make a move\n");
			set.pl = BLACK;
		}

		/*	Calculate hints */
		if (set.ht == 1)
		{
			hintPlayer(board, hints, set.pl);
		}
		else if (set.ht == 2)
		{
			AIEvalBoard(board, hints, set.pl);
		}
		else
		{
			for (auto i = 0; i < BOARD_MAX; i++)
			{
				hints[i] = ' ';
			}
		}

		drawBoard(board, hints);
	}

	printScore(board);

	fclose(fp);
	return 0;
}
