//Russian Roulette

using System.ComponentModel;
using System.Formats.Asn1;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Versioning;

Random rnd = new Random();

while (true)
{
    Console.Clear();

    //Asks how many players there are
    int playerCountBegin = IntAnswerValidation("How many players are there?");
    int playerCount = playerCountBegin;
    int currentChamber = 0;
    int pot = 0;

    int[] activePlayers = new int[playerCount];
    Array.Fill(activePlayers, 1);
    string[] playerNames = new string[playerCount];

    //Asks for all player names and puts them into an array
    for (int i = 1; i <= playerCountBegin; i++)
    {
        Console.Clear();
        playerNames[i - 1] = StringAnswerValidation("What is the name of player " + i + "?");
    }

    //Asks how much each player wants to gamble and adds it to the pot
    for (int i = 0; i < playerCountBegin; i++)
    {
        Console.Clear();
        pot += IntAnswerValidation("How many $ does " + playerNames[i] + " want to gamble?");
    }

    //Makes sure the game doesn't continue when there's only one player left
    while (playerCount > 1)
    {
        //Sets one bullet in the chamber to be live
        int[] chamber = [0, 0, 0, 0, 0, 0];
        int loadedChamber = rnd.Next(0, 5);
        chamber[loadedChamber] = 1;

        //Calls upon the "Loading"-method to show the loading bar
        Loading("Loading chamber");

        //Randomizes the play order
        for (int i = 0; i < playerCountBegin; i++)
        {
            string tempStr = playerNames[i];
            int tempInt = rnd.Next(0, playerCountBegin);
            playerNames[i] = playerNames[tempInt];
            playerNames[tempInt] = tempStr;

        }

        Loading("Randomizing play order");

        //Prints out pot and play order
        Console.WriteLine("Pot: " + pot + "$");
        Console.WriteLine("");
        Console.WriteLine("Play order:");
        for (int i = 0; i < playerCountBegin; i++)
        {
            Console.WriteLine(i + 1 + " : " + playerNames[i]);
        }
        Console.WriteLine("");

        //Game begins
        for (int i = 0; i <= playerCountBegin; i++)
        {
            //Checks if the playercount has gone to 1, in which case the game is over
            if (playerCount == 1)
            {
                break;
            }

            //Checks if the game has made a full loop through the play order, if so, it goes back to 0 and keeps going
            if (i == playerCountBegin)
            {
                i = 0;
            }

            //Checks if the current player in the array is living, if so, it continues, otherwise it skips that player
            if (activePlayers[i] == 1)
            {
                //Player turn
                Console.WriteLine("It is " + playerNames[i] + "'s turn.");
                Console.WriteLine("Press (enter) when ready.");
                Console.ReadLine();

                //Checks if the current chamber is loaded, if so the player is killed and can no longer participate, otherwise the player survives and the game continues
                if (chamber[currentChamber] == 1)
                {
                    Console.Clear();
                    Console.WriteLine("*BANG* " + playerNames[i] + " shot themselves.");
                    Console.WriteLine("");
                    Console.WriteLine("Press (enter) to continue.");
                    Console.ReadLine();
                    playerCount--;
                    currentChamber = 0;
                    activePlayers[i] = 0;
                    if (playerCount == 1)
                    {
                        break;
                    }
                    //Reloads the chamber
                    chamber = [0, 0, 0, 0, 0, 0];
                    loadedChamber = rnd.Next(0, 5);
                    chamber[loadedChamber] = 1;
                    currentChamber = 0;
                    Loading("Loading chamber");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("*Click* Chamber empty");
                    Console.WriteLine("");

                    currentChamber++;
                }
            }
        }
    }

    //Checks who is alive and crowns a victor
    for (int i = 0; i < playerCountBegin; i++)
    {
        if (activePlayers[i] == 1)
        {
            Console.WriteLine("");
            Console.WriteLine(playerNames[i] + " is the only one left alive. They won: " + pot + "$");
            i = playerCountBegin;
        }
    }

    //Asks if they want to play again, if so, game starts over, if not, game closes
    string answer = YNAnswerValidation("Do you want to play again? y/n");
    if (answer == "n" || answer == "N")
    {
        Console.WriteLine("Goodbye.");
        break;
    }
}

//Loading-method
static void Loading(string loadingPhrase)
{
    Console.Clear();
    Console.WriteLine(loadingPhrase);
    Console.WriteLine(".");
    Thread.Sleep(350);
    Console.Clear();
    Console.WriteLine(loadingPhrase);
    Console.WriteLine("..");
    Thread.Sleep(350);
    Console.Clear();
    Console.WriteLine(loadingPhrase);
    Console.WriteLine("...");
    Thread.Sleep(350);
    Console.Clear();
}

//Method that checks if you answer with an int
static int IntAnswerValidation(string question)
{
    int answer;

    while (true)
    {
        Console.WriteLine(question);
        string stringAnswer = Console.ReadLine();
        if (int.TryParse(stringAnswer, out answer) == true)
        {
            return answer;
        }
        else
        {
            Console.Clear();
            Console.WriteLine("You have to answer with a number.");
        }
    }
}

//Method that checks so that you don't answer with a blank space
static string StringAnswerValidation(string question)
{
    while (true)
    {
        Console.WriteLine(question);
        string answer = Console.ReadLine();
        if (answer != "")
        {
            return answer;
        }
        else
        {
            Console.Clear();
            Console.WriteLine("You cannot answer with nothing here.");
        }
    }
}

//Method that checks so that you answer with y/n
static string YNAnswerValidation(string question)
{
    while (true)
    {
        Console.WriteLine(question);
        string answer = Console.ReadLine();
        if (answer == "y" || answer == "Y" || answer == "n" || answer == "N")
        {
            return answer;
        }
        else
        {
            Console.Clear();
            Console.WriteLine("You have to answer with (y) or (n).");
        }
    }
}