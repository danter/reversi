/*	reversi.h
	Variable defenitions and function declarations
	for the Othello / Reversi game.
	Anders Asplund
	2007-12-12
*/

#pragma once

/*	VARNING: loading and saving of boards bigger
	then 10x10 squares is not suported*/
#define COL 8
#define ROW 8

#define BOARD_MAX COL*ROW

#define TRUE 1
#define FALSE 0


#define BLACK '\xB0'
#define WHITE '\xDB'
#define HINT '+'


typedef const int CINT;

typedef unsigned int UINT;

typedef unsigned short int BOOL;

typedef enum {NO_HINTS, HINTS, NUMERIC_HINTS} BoardHints;
typedef enum {NO_AI, WHITE_AI, BLACK_AI, BOTH_AI} AiPlayer;

typedef struct {
	int x;
	int y;
} sCord;

typedef struct {
	AiPlayer ai;
	BoardHints ht;
	int pl;
} sSettings;


/*Reads input from the player and returns a coordinate*/
sCord readMove();

/* Reads the commandline and sets the settings */
sSettings cmdRead(int board[], int argc, char *argv[], FILE *fp);

/* Initiates the game board */
void initGame(int board[]);

/* Displays the command help */
void dispHelp(char argv[]);

/* Draws the board to the screen */
void drawBoard(CINT board[], CINT hints[]);

/* Draws a piece to the board */
void drawPiece(int board[], sCord c, const char val);

/* returns 1 if the move is a valid one, othervise it returns 0 */
BOOL validMove(CINT board[], sCord c, const char val);

/* Checks if the piece is inside the board */
int insideBoard(int x, int y);


/*	Checks if a row of markers can be captured,
	ie. if a marker of your own color is at the end of the row.
	It takes the board, the current coordinate, the difference in x and y
	and what value it should check for */
BOOL traceMove(CINT board[], sCord c, int dx, int dy, const char val);


/*	Checks the move and turns the pieces */
BOOL doMove(int board[], sCord c, const char val);

/*	Turns the pieces in one direction
	VARNING: traceMove() should be used before this or pieces that
	should not be turned will get turned. */
BOOL doTraceMove(int board[], sCord c, int dx, int dy, const char val);

/* Tests if a player actually can do a move */
BOOL testPlayer(CINT board[], const char val);

/* Creates hint data used by the drawBoard() and AI functions */
void hintPlayer(CINT board[], int hints[], const char val);

/* Calculates and returns the player score */
int calcScore(CINT board[], const char val);

/*	Loads a saved game, returns the next player number, or negative value if failed
	VARNING: does not suport boards bigger then 10x10 squares*/
UINT loadGame(int board[], char file[], FILE *fp);


/*	Evaluates coordinates of the board, depending on if the spot is
	capturable for the player or not. The value depends on how many pieces you
	would capture, and if the spot is a corner or not. It will return the
	coordinates for the spot with the best score.
*/
sCord AIEvalBoard(CINT board[], int score[], const char val);

/* Calculates a score depending on how good a square is to capture */
int AIScoreCalc(CINT board[], sCord c, const char val);

/* Calculates a score depending on how how much you capture on a square */
int AITraceMove(CINT board[], sCord c, int dx, int dy, const char val);

/* Creates an array with scores for hotspots like corners, sides etc etc*/
void AIScoreTable(int score[]);

