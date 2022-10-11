/*
FERRIX CODING

CODE:
0: no explored, water
1: no explored, ship
2: explored, water
3: explored, ship
*/


using System;

namespace sobreviviendo
{
    class Program
    {
        public static string[] Letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" }; 
            //  It'll be useful to know which letter represent each number

        public static void PrintTable(int[,] Table, bool boxs_hidden = true)
        {
            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("               NAVAL   BATTLE               ");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("IA TABLE:");
            Console.WriteLine("--------------------------------------------");
            Console.Write(" X | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10|");
            Console.Write("\n--------------------------------------------");
            for (int i = 0; i < 10; i++) // Rows
            {
                Console.Write("\n" + Letters[i].ToUpper() + " ");
                for (int j = 0; j < 10; j++) // Columns
                {
                    string celd = "";

                    if (boxs_hidden) {
                        if (Table[i, j] == 0 || Table[i, j] == 1) { celd = " "; }
                        if (Table[i, j] == 2) { celd = "A"; }
                        if (Table[i, j] == 3) { celd = "X"; }
                    } else {
                        celd = Table[i,j].ToString();
                    }
                    
                    Console.Write(" | " + celd);
                    if (j == 9) { Console.Write(" | "); }
                }
                Console.Write("\n--------------------------------------------");
            }
            Console.WriteLine();
        }

        public static int checkRest(int[,] Table) // Return the rest box ships alive
        {
            int box_ships = 0;
            for (int a = 0; a < 10; a++)
            {
                for (int b = 0; b < 10; b++)
                {
                    if (Table[a, b] == 1) { box_ships += 1; }
                }
            }
            return box_ships;
            // It goes thought the loop adding 1 when it see any no explored ship-celd
        }

        public static int Indexing(string[] iterable, string search) 
        { // This function have to be replaced, I put it because I am noob with c# syntax
            int count = -1;
            foreach(string i in iterable)
            {
                count++;
                if (i == search) {return count;}
            }
            return count;
        }

        public static int[] Translate(string Box) // Translate type "J10" in 9,9
        {
            int row = Indexing(Letters, Box[0].ToString());
            int column = -1;
            if (Box.Length == 3) {column = 9;} // 10 Is the only number who take another extra character
            if (Box.Length == 2) {
                column = Int32.Parse(Box[1].ToString())-1; // -1 because remember, the persons don't start counting in 0
            }

            int[] rv = {row, column}; return rv;
        }

        public static string Check(int[,] Table, string Box) 
        {
            // It change the no explored 0 or 1 to the explored 2 or 3
            int row = Translate(Box)[0]; int column = Translate(Box)[1];
            if (Table[row, column] == 2 || Table[row, column] == 3 ) {return "alreadyDone";}
            if (Table[row, column] == 0 ) { Table[row, column] = 2; return "water";}
            if (Table[row, column] == 1 ) { Table[row, column] = 3;}
            
            // I have to avoid indexing out of range
            bool more_boxes = false;
            if (row != 0) {if (Table[row-1, column] == 1) {more_boxes = true;}}
            if (row != 9) {if (Table[row+1, column] == 1) {more_boxes = true;}}
            if (column != 0) {if (Table[row, column-1] == 1) {more_boxes = true;}}
            if (column != 9) {if (Table[row, column+1] == 1) {more_boxes = true;}}

            if (more_boxes) {return "touched";} // If there is any box-ship around, the ship isn't destroyed yet
            else {return "destroyed";}
        }

        public static void PutShip(int[,] Table, string Box, string direction, int ship_length)
        {
            int init_row = Translate(Box)[0]; int init_column = Translate(Box)[1];
            int row = init_row; int column = init_column;
            for (int number_box = 1; number_box <= ship_length; number_box++)
            {
                Table[row,column] = 1;
                if (direction == "up") {row = init_row - number_box;}
                if (direction == "down") {row = init_row + number_box;}
                if (direction == "left") {column = init_column - number_box;}
                if (direction == "right") {column = init_column + number_box;}
            }
        }
        // I only putted ships vertically down, but it's here for you


        static void Main(string[] args)
        {
            bool exit = false;
            int waters = 0;

            int[,] ComputerTable = new int[10, 10]; // Tablero

            PutShip(ComputerTable, "A1", "down", 2);
            PutShip(ComputerTable, "I1", "down", 2);
            PutShip(ComputerTable, "A5", "down", 4);
            PutShip(ComputerTable, "E3", "down", 4);
            PutShip(ComputerTable, "C8", "down", 4);
            PutShip(ComputerTable, "F6", "down", 5);

            while (!exit)
            {
                PrintTable(ComputerTable);
                Console.WriteLine("There are " + checkRest(ComputerTable) + " box-ships left");
                
                Console.WriteLine($"You have watered {waters}.");
                Console.Write("ATTACK: ");
                string _entry = Console.ReadLine().ToUpper();
                if (_entry == "EXIT") {break;}

                try {
                    var attack = Check(ComputerTable, _entry);
                    if (attack == "alreadyDone") {Console.WriteLine("You've already attacked this celd");}
                    if (attack == "touched") {Console.WriteLine($"BOT: {_entry}, TOUCHED"); waters = 0;}
                    if (attack == "destroyed") {Console.WriteLine($"BOT: {_entry}, DESTROYED"); waters = 0;}
                    if (attack == "water") {Console.WriteLine($"BOT: {_entry}, WATER"); waters++;}
                } 
                catch {
                    Console.WriteLine("Something went wrong, entry again...");
                }

                if (waters == 8) { 
                    Console.WriteLine("GAME OVER");
                    exit = true; 
                    }
            }
        }
}
} // FERRIX CODING