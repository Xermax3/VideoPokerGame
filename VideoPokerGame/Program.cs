/*
    Maxence Roy
    Programming II Final Project: Video Poker Game
    Latest update: May 21st, 2020
    Description: 
        A program that lets player play solo poker until they decide to quit or run out of money.
        Cards and Decks are classes with their own properties and methods, which can be seen in their respective .cs file.
        As per usual poker rules, the player first bets a certain amount of money, which is removed from their bank.
        Then, the player sees their hand (5 cards) and has the option to swap up to 4 cards.
        Afterwards, the program looks for any combinations (full house, flush, three of a kind, nothing, etc.) and
        multiplies the amount of money in the bet according, that amount being added to the bank.
    Tested by: Pierre-Alexandre Roy (brother)
*/


using System;
using System.Text;
using static System.Console;

namespace VideoPokerGame
{
    public enum FaceValue { Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace };

    public enum Suit { Clubs, Diamonds, Hearts, Spades }

    class Card
    {
        private FaceValue _faceValue;
        private Suit _suit;

        /// <summary> Default constructor for card, which assigns the default card value: Ace of Clubs. </summary>
        public Card()
        {
            _faceValue = FaceValue.Ace;
            _suit = Suit.Clubs;
        }

        /// <summary> Constructor for card, in which a specific face value and suit is assigned. </summary>
        public Card(FaceValue faceValue_, Suit suit_)
        {
            _faceValue = faceValue_;
            _suit = suit_;
        }

        /// <summary> Getter for the card's face value. </summary>
        public FaceValue GetFaceValue() { return _faceValue; }

        /// <summary> Getter for the card's suit. </summary>
        public Suit GetSuit() { return _suit; }

        /// <summary> Returns the face value and suit symbol of the card. Ex: '[A♠]' </summary>
        public string ToStringShort()
        {
            string faceValueSymbol = "?", suitSymbol = "?"; // Placeholder

            switch (_faceValue)
            {
                case FaceValue.Ace:
                    faceValueSymbol = "A";
                    break;
                case FaceValue.Two:
                    faceValueSymbol = "2";
                    break;
                case FaceValue.Three:
                    faceValueSymbol = "3";
                    break;
                case FaceValue.Four:
                    faceValueSymbol = "4";
                    break;
                case FaceValue.Five:
                    faceValueSymbol = "5";
                    break;
                case FaceValue.Six:
                    faceValueSymbol = "6";
                    break;
                case FaceValue.Seven:
                    faceValueSymbol = "7";
                    break;
                case FaceValue.Eight:
                    faceValueSymbol = "8";
                    break;
                case FaceValue.Nine:
                    faceValueSymbol = "9";
                    break;
                case FaceValue.Ten:
                    faceValueSymbol = "10";
                    break;
                case FaceValue.Jack:
                    faceValueSymbol = "J";
                    break;
                case FaceValue.Queen:
                    faceValueSymbol = "Q";
                    break;
                case FaceValue.King:
                    faceValueSymbol = "K";
                    break;
            }
            switch (_suit)
            {
                case Suit.Clubs:
                    suitSymbol = "♣";
                    break;
                case Suit.Diamonds:
                    suitSymbol = "♦";
                    break;
                case Suit.Hearts:
                    suitSymbol = "♥";
                    break;
                case Suit.Spades:
                    suitSymbol = "♠";
                    break;
            }

            return string.Format("[{0}{1}]", faceValueSymbol, suitSymbol);
        }

        /// <summary> Returns the card's symbol and its full name. Ex: '[7♦] Seven of Diamonds' </summary>
        public override string ToString()
        {
            return string.Format("{0} {1} of {2}", ToStringShort(), _faceValue.ToString(), _suit.ToString());
        }
    }

    class Deck
    {
        private Card[] cards = new Card[Enum.GetNames(typeof(FaceValue)).Length * Enum.GetNames(typeof(Suit)).Length];
        private int topCardIndex = 0;
        private static Random rng;

        /// <summary> Contructor that fills the deck with all 52 unique cards. Cards are not suffled. </summary>
        public Deck()
        {
            rng = new Random();
            int cardIndex = 0;

            foreach (FaceValue faceValue in (FaceValue[])Enum.GetValues(typeof(FaceValue)))
            {
                foreach (Suit suit in (Suit[])Enum.GetValues(typeof(Suit)))
                {
                    cards[cardIndex++] = new Card(faceValue, suit); // Value is returned, then index is incremented
                }
            }
        }

        /// <summary> Suffles the deck. Each card is swaps with a random one, then the top card index returns to 0. </summary>
        public void Shuffle()
        {
            topCardIndex = 0;
            int newIndex;

            for (int i = 0; i < cards.Length; i++)
            {
                newIndex = rng.Next(cards.Length);
                Card swappedCard = cards[newIndex];
                cards[newIndex] = cards[i];
                cards[i] = swappedCard;
            }
        }

        /// <summary> Returns the top card of the deck. Will throw an exception if deck has dealt all its cards. </summary>
        public Card DealCard()
        {
            if (topCardIndex >= cards.Length)
                throw new IndexOutOfRangeException("The deck has dealt all its cards; there are no more to deal.");
            return cards[topCardIndex++]; // Value is returned, then index is incremented
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (Card card in cards)
            {
                stringBuilder.AppendLine(card.ToString());
            }

            return stringBuilder.ToString();
        }
    }

    class Program
    {
        // For Title Screen
        static readonly string[] TITLE_TEXT =
        {
            "$$$$$$$\\            $$\\                           ",     // $$$$$$$\            $$\                           
            "$$  __$$\\           $$ |                          ",      // $$  __$$\           $$ |                          
            "$$ |  $$ | $$$$$$\\  $$ |  $$\\  $$$$$$\\   $$$$$$\\  ",   // $$ |  $$ | $$$$$$\  $$ |  $$\  $$$$$$\   $$$$$$\  
            "$$$$$$$  |$$  __$$\\ $$ | $$  |$$  __$$\\ $$  __$$\\ ",    // $$$$$$$  |$$  __$$\ $$ | $$  |$$  __$$\ $$  __$$\ 
            "$$  ____/ $$ /  $$ |$$$$$$  / $$$$$$$$ |$$ |  \\__|",      // $$  ____/ $$ /  $$ |$$$$$$  / $$$$$$$$ |$$ |  \__|
            "$$ |      $$ |  $$ |$$  _$$<  $$   ____|$$ |      ",       // $$ |      $$ |  $$ |$$  _$$<  $$   ____|$$ |      
            "$$ |      \\$$$$$$  |$$ | \\$$\\ \\$$$$$$$\\ $$ |      ",  // $$ |      \$$$$$$  |$$ | \$$\ \$$$$$$$\ $$ |      
            "\\__|       \\______/ \\__|  \\__| \\_______|\\__|      "  // \__|       \______/ \__|  \__| \_______|\__|      
        };
        static readonly int TITLE_SIZE_Y = TITLE_TEXT.Length;
        const int TITLE_POS_Y = 5;
        static readonly ConsoleColor[] TITLE_COLORS = 
        {
            ConsoleColor.DarkGreen, ConsoleColor.Green, ConsoleColor.Green, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Cyan, ConsoleColor.DarkCyan
        };
        const ulong TITLE_COLOR_CHANGE_TIME = 20000;
        const string TITLE_AUTHOR_TEXT = "Made by Maxence Roy (2020)";
        const int TITLE_AUTHOR_POS_Y = 15;
        static readonly string[] TITLE_OPTIONS_TEXT = { "Enter one of the options:", "1. Play", "2. Test", "3. How to Play", "4. Quit" };
        const int TITLE_OPTIONS_POS_Y = 19;

        // For Game
        const int START_BANKROLL = 1000;
        const int MIN_BET = 1;
        const int HAND_SIZE = 5;
        const int MAX_SWAPS = 4;
        const int SWAP_CONTINUE_INPUT = 0;
        const string GAME_END_INPUT = "N";

        // For Test
        static readonly Card[]
            ROYAL_FLUSH_EXAMPLE = {     new Card(FaceValue.Ace, Suit.Hearts),    new Card(FaceValue.Jack, Suit.Hearts),    new Card(FaceValue.King, Suit.Hearts),  new Card(FaceValue.Ten, Suit.Hearts),    new Card(FaceValue.Queen, Suit.Hearts) },
            STRAIGHT_FLUSH_EXAMPLE = {  new Card(FaceValue.Two, Suit.Clubs),     new Card(FaceValue.Ace, Suit.Clubs),      new Card(FaceValue.Five, Suit.Clubs),   new Card(FaceValue.Four, Suit.Clubs),    new Card(FaceValue.Three, Suit.Clubs) },
            FOUR_OF_A_KIND_EXAMPLE = {  new Card(FaceValue.Six, Suit.Hearts),    new Card(FaceValue.Four, Suit.Hearts),    new Card(FaceValue.Six, Suit.Spades),   new Card(FaceValue.Six, Suit.Diamonds),  new Card(FaceValue.Six, Suit.Clubs) },
            FULL_HOUSE_EXAMPLE = {      new Card(FaceValue.Ten, Suit.Diamonds),  new Card(FaceValue.Nine, Suit.Diamonds),  new Card(FaceValue.Nine, Suit.Hearts),  new Card(FaceValue.Nine, Suit.Spades),   new Card(FaceValue.Ten, Suit.Spades) },
            FLUSH_EXAMPLE = {           new Card(FaceValue.Four, Suit.Spades),   new Card(FaceValue.Five, Suit.Spades),    new Card(FaceValue.Seven, Suit.Spades), new Card(FaceValue.King, Suit.Spades),   new Card(FaceValue.Jack, Suit.Spades) },
            STRAIGHT_EXAMPLE = {        new Card(FaceValue.Jack, Suit.Diamonds), new Card(FaceValue.Ten, Suit.Clubs),      new Card(FaceValue.Eight, Suit.Hearts), new Card(FaceValue.Seven, Suit.Clubs),   new Card(FaceValue.Nine, Suit.Clubs) },
            THREE_OF_A_KIND_EXAMPLE = { new Card(FaceValue.Seven, Suit.Hearts),  new Card(FaceValue.Two, Suit.Spades),     new Card(FaceValue.Two, Suit.Diamonds), new Card(FaceValue.Two, Suit.Hearts),    new Card(FaceValue.Five, Suit.Hearts) },
            TWO_PAIR_EXAMPLE = {        new Card(FaceValue.Eight, Suit.Spades),  new Card(FaceValue.Eight, Suit.Clubs),    new Card(FaceValue.Three, Suit.Hearts), new Card(FaceValue.Three, Suit.Spades),  new Card(FaceValue.Four, Suit.Diamonds) },
            ONE_PAIR_EXAMPLE = {        new Card(FaceValue.King, Suit.Clubs),    new Card(FaceValue.Queen, Suit.Diamonds), new Card(FaceValue.Queen, Suit.Clubs),  new Card(FaceValue.Jack, Suit.Clubs),    new Card(FaceValue.Eight, Suit.Diamonds) },
            NOTHING_EXAMPLE = {         new Card(FaceValue.Queen, Suit.Spades),  new Card(FaceValue.Three, Suit.Diamonds), new Card(FaceValue.Ace, Suit.Diamonds), new Card(FaceValue.Five, Suit.Diamonds), new Card(FaceValue.King, Suit.Diamonds) };
        const int TEST_END_INPUT = 0;

        // For EvaluateHand
        const int
            ROYAL_FLUSH_PAYOUT      = 250,
            STRAIGHT_FLUSH_PAYOUT   = 50,
            FOUR_OF_A_KIND_PAYOUT   = 25,
            FULL_HOUSE_PAYOUT       = 9,
            FLUSH_PAYOUT            = 6,
            STRAIGHT_PAYOUT         = 4,
            THREE_OF_A_KIND_PAYOUT  = 3,
            TWO_PAIR_PAYOUT         = 2,
            ONE_PAIR_PAYOUT         = 1,
            NO_PAYOUT               = 0;


        static void Main(string[] args)
        {
            OutputEncoding = Encoding.UTF8; // Allows the Console to display Unicode characters, such as card suit symbols.

            int DefaultWindowWidth = WindowWidth;
            int titlePosX = (DefaultWindowWidth - TITLE_TEXT[0].Length) / 2;
            int titleAuthorPosX = (DefaultWindowWidth - TITLE_AUTHOR_TEXT.Length) / 2;
            ConsoleKey menuChoice = ConsoleKey.D0; // Placeholder
            ConsoleColor origColor = ForegroundColor;
            ulong frame = 1;
            ulong menuIteration = 0;
            bool showMenu = true;

            do
            {
                frame++;

                if (showMenu)
                {
                    CursorVisible = false;
                    Clear();
                    SetCursorPosition(titleAuthorPosX, TITLE_AUTHOR_POS_Y);
                    Write(TITLE_AUTHOR_TEXT);
                    for (int i = 0; i < TITLE_OPTIONS_TEXT.Length; i++)
                    {
                        SetCursorPosition((DefaultWindowWidth - TITLE_OPTIONS_TEXT[i].Length) / 2, TITLE_OPTIONS_POS_Y + i);
                        Write(TITLE_OPTIONS_TEXT[i]);
                    }
                }
                if (showMenu || frame % TITLE_COLOR_CHANGE_TIME == 1) // Animate the menu
                {
                    menuIteration++;
                    for (int i = 0; i < TITLE_SIZE_Y; i++)
                    {
                        ForegroundColor = TITLE_COLORS[((ulong)i + menuIteration) % (ulong)TITLE_SIZE_Y];
                        SetCursorPosition(titlePosX, TITLE_POS_Y + i);
                        Write(TITLE_TEXT[i]);
                    }
                }

                menuChoice = ReadKeyMenu();
                switch (menuChoice)
                {
                    case ConsoleKey.D1:
                        showMenu = true;
                        ForegroundColor = origColor;
                        CursorVisible = true;
                        PlayGame();
                        break;
                    case ConsoleKey.D2:
                        showMenu = true;
                        ForegroundColor = origColor;
                        CursorVisible = true;
                        TestGame();
                        break;
                    case ConsoleKey.D3:
                        showMenu = true;
                        ForegroundColor = origColor;
                        HowToPlay();
                        break;
                    case ConsoleKey.D4:
                    default:
                        showMenu = false;
                        break;
                }
            } while (menuChoice != ConsoleKey.D4);
        }

        /// <summary> Returns any key that the user presses. If no key is being pressed, the default value is returned. </summary>
        static ConsoleKey ReadKeyMenu()
        {
            if (KeyAvailable) // Makes sure a key has been pressed
            {
                return ReadKey(true).Key;
            }
            return default;
        }


        /// <summary> Starts a normal game of poker. </summary>
        static void PlayGame()
        {
            bool gameEnd = false;
            int bankroll = START_BANKROLL;
            Deck deck = new Deck();

            while (!gameEnd)
            {
                Clear();
                deck.Shuffle();
                WriteLine("How much money would you like to bet? (In dollars, between {0} and {1})", MIN_BET, bankroll);
                int bet = GetInt(MIN_BET, bankroll);
                bankroll -= bet;
                PrintBankroll(bankroll);
                Card[] hand = new Card[HAND_SIZE];
                for (int i = 0; i < HAND_SIZE; i++)
                {
                    hand[i] = deck.DealCard();
                }

                WriteLine("\nYour hand (before swaps):");
                PrintHand(hand);
                bool[] cardsToSwap = new bool[HAND_SIZE];
                for (int i = 0; i < HAND_SIZE; i++)
                {
                    cardsToSwap[i] = false;
                }
                bool stopSwap = false;
                int swapsLeft = MAX_SWAPS;
                WriteLine("Enter a card's number to swap it (can change up to {0} different cards). Enter {1} to continue.", MAX_SWAPS, SWAP_CONTINUE_INPUT);
                while (!stopSwap)
                {
                    int swapChoice = GetInt(0, HAND_SIZE);
                    if (swapChoice == SWAP_CONTINUE_INPUT)
                        stopSwap = true;
                    else
                    {
                        if (!cardsToSwap[swapChoice - 1])
                        {
                            cardsToSwap[swapChoice - 1] = true;
                            swapsLeft--;
                            PrintWithColor(string.Format("Card #{0} has been successfully swapped!", swapChoice), ConsoleColor.Green);
                            if (swapsLeft > 0)
                                WriteLine("Swaps left: {0}", swapsLeft);
                            else
                            {
                                stopSwap = true;
                                WriteLine("You're out of swaps now. Let's see what you got!");
                            }
                        }
                        else
                            PrintWithColor(string.Format("Card #{0} has already been swapped!", swapChoice), ConsoleColor.Red);
                    }
                }
                SwapCards(hand, cardsToSwap, deck);
                WriteLine("\nYour hand (after swaps):");
                PrintHand(hand);

                int multiplier = EvaluateHand(hand);
                int moneyEarned = multiplier * bet;
                bankroll += moneyEarned;
                WriteLine("Money earned: {0:c} ({1:c} x {2})", moneyEarned, bet, multiplier);
                PrintBankroll(bankroll);
                if (bankroll < MIN_BET)
                {
                    PrintWithColor("\nLooks like you're out of money! Game Over!", ConsoleColor.Red);
                    gameEnd = true;
                }
                else
                {
                    WriteLine("\nEnter anything to play again. Enter '{0}' to quit.", GAME_END_INPUT);
                    string playChoice = ReadLine();
                    if (playChoice.ToUpper() == GAME_END_INPUT)
                    {
                        PrintWithColor(string.Format("\nThanks for playing! You leave with {0:c} in hand.", bankroll), ConsoleColor.Green);
                        gameEnd = true;
                    }
                }
            }
            ReadLine();
        }

        /// <summary> Displays a menu where the user can test all possible combinations in the game. </summary>
        static void TestGame()
        {
            int testChoice;

            do
            {
                Clear();
                WriteLine("Select one of the following options:");
                WriteLine("1. Royal Flush");
                WriteLine("2. Straight Flush");
                WriteLine("3. Four Of A Kind");
                WriteLine("4. Full House");
                WriteLine("5. Flush");
                WriteLine("6. Straight");
                WriteLine("7. Three Of A Kind");
                WriteLine("8. Two Pair");
                WriteLine("9. One Pair");
                WriteLine("10. No winning hand");
                WriteLine("0. EXIT");
                testChoice = GetInt(0, 10);

                if (testChoice != TEST_END_INPUT)
                {
                    Card[] hand = new Card[HAND_SIZE];
                    int bankroll = START_BANKROLL;

                    switch (testChoice)
                    {
                        case 1: // Royal Flush
                            Array.Copy(ROYAL_FLUSH_EXAMPLE, hand, HAND_SIZE);
                            break;
                        case 2: // Straight Flush
                            Array.Copy(STRAIGHT_FLUSH_EXAMPLE, hand, HAND_SIZE);
                            break;
                        case 3: // Four Of A Kind
                            Array.Copy(FOUR_OF_A_KIND_EXAMPLE, hand, HAND_SIZE);
                            break;
                        case 4: // Full House
                            Array.Copy(FULL_HOUSE_EXAMPLE, hand, HAND_SIZE);
                            break;
                        case 5: // Flush
                            Array.Copy(FLUSH_EXAMPLE, hand, HAND_SIZE);
                            break;
                        case 6: // Straight
                            Array.Copy(STRAIGHT_EXAMPLE, hand, HAND_SIZE);
                            break;
                        case 7: // Three Of A Kind
                            Array.Copy(THREE_OF_A_KIND_EXAMPLE, hand, HAND_SIZE);
                            break;
                        case 8: // Two Pair
                            Array.Copy(TWO_PAIR_EXAMPLE, hand, HAND_SIZE);
                            break;
                        case 9: // One Pair
                            Array.Copy(ONE_PAIR_EXAMPLE, hand, HAND_SIZE);
                            break;
                        case 10: // Nothing
                            Array.Copy(NOTHING_EXAMPLE, hand, HAND_SIZE);
                            break;
                    }

                    WriteLine("\nHow much money would you like to bet? (In dollars, between {0} and {1})", MIN_BET, bankroll);
                    int bet = GetInt(MIN_BET, bankroll);
                    bankroll -= bet;
                    PrintBankroll(bankroll);

                    WriteLine("\nYour hand:");
                    PrintHand(hand);

                    int multiplier = EvaluateHand(hand);
                    int moneyEarned = multiplier * bet;
                    bankroll += moneyEarned;
                    WriteLine("Money earned: {0:c} ({1:c} x {2})", moneyEarned, bet, multiplier);
                    PrintBankroll(bankroll);
                    ReadLine();
                }
            } while (testChoice != TEST_END_INPUT);
        }

        /// <summary> Displays basic information abotu poker and all the combinations possible. </summary>
        static void HowToPlay()
        {
            Clear();
            WriteLine("\n  HOW TO PLAY");
            WriteLine("\n  This game simulates a single-player game of Poker.");
            WriteLine("  The player starts with {0:c} in their bank and bets a certain amount of money.", START_BANKROLL);
            WriteLine("  Then, they receive 5 cards and can swap up to 4 of them.");
            WriteLine("  Finally, the game checks for any possible combinations (see below) and multiplies the bet accordingly.");
            WriteLine("  If there's no combination, the money is lost.");
            WriteLine("  The game repeats until either the player quits or runs out of money.");
            WriteLine("\n\n  COMBINATIONS");
            WriteLine("\n  {0,-17} {1,6}   {2,-24}", "Name", "Payout", "Example");
            WriteLine("  {0,-17} {1,6}   {2,-24}", "Royal Flush", "x" + ROYAL_FLUSH_PAYOUT, HandToStringShort(ROYAL_FLUSH_EXAMPLE));
            WriteLine("  {0,-17} {1,6}   {2,-24}", "Straight Flush", "x" + STRAIGHT_FLUSH_PAYOUT, HandToStringShort(STRAIGHT_FLUSH_EXAMPLE));
            WriteLine("  {0,-17} {1,6}   {2,-24}", "Four Of A Kind", "x" + FOUR_OF_A_KIND_PAYOUT, HandToStringShort(FOUR_OF_A_KIND_EXAMPLE));
            WriteLine("  {0,-17} {1,6}   {2,-24}", "Full House", "x" + FULL_HOUSE_PAYOUT, HandToStringShort(FULL_HOUSE_EXAMPLE));
            WriteLine("  {0,-17} {1,6}   {2,-24}", "Flush", "x" + FLUSH_PAYOUT, HandToStringShort(FLUSH_EXAMPLE));
            WriteLine("  {0,-17} {1,6}   {2,-24}", "Straight", "x" + STRAIGHT_PAYOUT, HandToStringShort(STRAIGHT_EXAMPLE));
            WriteLine("  {0,-17} {1,6}   {2,-24}", "Three Of A Kind", "x" + THREE_OF_A_KIND_PAYOUT, HandToStringShort(THREE_OF_A_KIND_EXAMPLE));
            WriteLine("  {0,-17} {1,6}   {2,-24}", "Two Pair", "x" + TWO_PAIR_PAYOUT, HandToStringShort(TWO_PAIR_EXAMPLE));
            WriteLine("  {0,-17} {1,6}   {2,-24}", "One Pair", "x" + ONE_PAIR_PAYOUT, HandToStringShort(ONE_PAIR_EXAMPLE));
            WriteLine("  {0,-17} {1,6}   {2,-24}", "Nothing", "x" + NO_PAYOUT, HandToStringShort(NOTHING_EXAMPLE));
            PrintWithColor("\n\n\n  Press any key to go back to menu.", ConsoleColor.Yellow);
            ReadKey();
        }

        /// <summary> Repeatedly asks the user to give an integer until one within the limits is given. </summary>
        static int GetInt(int min = 0, int max = int.MaxValue)
        {
            int num;

            while (true)
            {
                if (int.TryParse(ReadLine(), out num))
                {
                    if (num < min || num > max)
                        PrintWithColor(string.Format("Error: Number is outside the limits ({0} to {1})", min, max), ConsoleColor.Red);
                    else
                        return num;
                }
                else
                    PrintWithColor("Error: Input is not a number.", ConsoleColor.Red);
            }
        }

        /// <summary> Prints a string with a specific ForegroundColor, then ForeGroundColor returns to its original value. </summary>
        static void PrintWithColor(string text, ConsoleColor newColor)
        {
            ConsoleColor origColor = ForegroundColor;
            ForegroundColor = newColor;
            WriteLine(text);
            ForegroundColor = origColor;
        }

        /// <summary> Prints every card in hand with Card.ToString() </summary>
        static void PrintHand(Card[] hand)
        {
            for (int i = 0; i < hand.Length; i++)
            {
                WriteLine("Card #{0}: {1}", i + 1, hand[i].ToString());
            }
            WriteLine();
        }

        /// <summary> Returns a string containing the symbols of each card. </summary>
        static string HandToStringShort(Card[] hand)
        {
            return string.Format("{0} {1} {2} {3} {4}", hand[0].ToStringShort(), hand[1].ToStringShort(), hand[2].ToStringShort(), hand[3].ToStringShort(), hand[4].ToStringShort());
        }

        /// <summary> Prints the number that the method receives (bankroll) as currency. </summary>
        static void PrintBankroll(int bankroll)
        {
            PrintWithColor(string.Format("Money in bank: {0:c}", bankroll), ConsoleColor.Yellow);
        }

        /// <summary> Goes through every card, if cardsToSwap[index] is true, the card in that same index will be given a new value. </summary>
        static void SwapCards(Card[] hand, bool[] cardsToSwap, Deck deck)
        {
            for (int i = 0; i < cardsToSwap.Length; i++)
            {
                if (cardsToSwap[i])
                    hand[i] = deck.DealCard();
            }
        }


        /// <summary> 
        /// Checks every combination possible, from the one with the highest payout to the one with the least.
        /// Once a combination is found, its payout (number to multiply with the bet) is returned.
        /// </summary>
        static int EvaluateHand(Card[] hand)
        {
            Array.Sort(hand, (x, y) => y.GetFaceValue().CompareTo(x.GetFaceValue())); // VERY IMPORTANT: Sorts array by FaceValue in descending order
            if (IsRoyalFlush(hand))
            {
                PrintWithColor("Winning hand: Royal Flush", ConsoleColor.Green);
                return ROYAL_FLUSH_PAYOUT;
            }
            if (IsStraightFlush(hand))
            {
                PrintWithColor("Winning hand: Straight Flush", ConsoleColor.Green);
                return STRAIGHT_FLUSH_PAYOUT;
            }
            if (IsFourOfAKind(hand))
            {
                PrintWithColor("Winning hand: Four Of A Kind", ConsoleColor.Green);
                return FOUR_OF_A_KIND_PAYOUT;
            }
            if (IsFullHouse(hand))
            {
                PrintWithColor("Winning hand: Full House", ConsoleColor.Green);
                return FULL_HOUSE_PAYOUT;
            }
            if (IsFlush(hand))
            {
                PrintWithColor("Winning hand: Flush", ConsoleColor.Green);
                return FLUSH_PAYOUT;
            }
            if (IsStraight(hand))
            {
                PrintWithColor("Winning hand: Straight", ConsoleColor.Green);
                return STRAIGHT_PAYOUT;
            }
            if (IsThreeOfAKind(hand))
            {
                PrintWithColor("Winning hand: Three Of A Kind", ConsoleColor.Green);
                return THREE_OF_A_KIND_PAYOUT;
            }
            if (IsTwoPair(hand))
            {
                PrintWithColor("Winning hand: Two Pair", ConsoleColor.Green);
                return TWO_PAIR_PAYOUT;
            }
            if (IsOnePair(hand))
            {
                PrintWithColor("Winning hand: One Pair", ConsoleColor.Green);
                return ONE_PAIR_PAYOUT;
            }
            PrintWithColor("No winning hand", ConsoleColor.Red);
            return NO_PAYOUT;
        }

        /// <summary>
        /// Returns if hand is a Royal Flush (all cards are the same suit and consecutive, from 10 to A). 
        /// Note that the hand is sorted by face value beforehand (from largest to smallest).
        /// </summary>
        static bool IsRoyalFlush(Card[] hand)
        {
            if (IsStraightFlush(hand) && hand[1].GetFaceValue() == FaceValue.King)
                return true;
            return false;
        }

        /// <summary>
        /// Returns if hand is a Straight Flush (all cards have the same suit and are consecutive).
        /// Note that the hand is sorted by face value beforehand (from largest to smallest).
        /// </summary>
        static bool IsStraightFlush(Card[] hand)
        {
            if (IsFlush(hand) && IsStraight(hand))
                return true;
            return false;
        }

        /// <summary>
        /// Returns if hand is a Four Of A Kind (4 cards have the same face value).
        /// Note that the hand is sorted by face value beforehand (from largest to smallest).
        /// </summary>
        static bool IsFourOfAKind(Card[] hand)
        {
            for (int i = 0; i < hand.Length - 3; i++)
            {
                FaceValue compare = hand[i].GetFaceValue();
                if (compare == hand[i + 1].GetFaceValue() && compare == hand[i + 2].GetFaceValue() && compare == hand[i + 3].GetFaceValue())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns if hand is a Full House (3 cards have a same face value, and the 2 others too).
        /// Note that the hand is sorted by face value beforehand (from largest to smallest).
        /// </summary>
        static bool IsFullHouse(Card[] hand)
        {
            FaceValue compare1 = hand[0].GetFaceValue();
            FaceValue compare2 = hand[hand.Length - 1].GetFaceValue();

            if (compare1 == hand[1].GetFaceValue() && compare2 == hand[hand.Length - 1].GetFaceValue() && (compare1 == hand[2].GetFaceValue() || compare2 == hand[2].GetFaceValue()))
                return true;
            return false;
        }

        /// <summary>
        /// Returns if hand is a Flush (all cards have the same suit).
        /// Note that the hand is sorted by face value beforehand (from largest to smallest).
        /// </summary>
        static bool IsFlush(Card[] hand)
        {
            for (int i = 1; i < hand.Length; i++)
            {
                if (hand[0].GetSuit() != hand[i].GetSuit())
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns if hand is a Straight (all cards are consecutive).
        /// Note that the hand is sorted by face value beforehand (from largest to smallest).
        /// </summary>
        static bool IsStraight(Card[] hand)
        {
            // If the first card is Ace and the last card is Two, the only Straight possible is Ace-Five. However, Ace is considered the highest FaceValue. 
            // So, in this particular case, the Ace is interpreted as Six so a potential Straight can be recognized.
            FaceValue compare = (hand[0].GetFaceValue() == FaceValue.Ace && hand[hand.Length - 1].GetFaceValue() == FaceValue.Two) ? FaceValue.Six : hand[0].GetFaceValue();
            for (int i = 1; i < hand.Length; i++)
            {
                if (compare != hand[i].GetFaceValue() + i)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Description: Returns if hand is a Three Of A Kind (3 cards have the same face value).
        /// Note that the hand is sorted by face value beforehand (from largest to smallest).
        /// </summary>
        static bool IsThreeOfAKind(Card[] hand)
        {
            for (int i = 0; i < hand.Length - 2; i++)
            {
                FaceValue compare = hand[i].GetFaceValue();
                if (compare == hand[i + 1].GetFaceValue() && compare == hand[i + 2].GetFaceValue())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns if hand is a Two Pair (2 cards have the same face value, twice inside the hand).
        /// Note that the hand is sorted by face value beforehand (from largest to smallest).
        /// </summary>
        static bool IsTwoPair(Card[] hand)
        {
            bool foundFirstPair = false;

            for (int i = 0; i < hand.Length - 1; i++)
            {
                if (hand[i].GetFaceValue() == hand[i + 1].GetFaceValue())
                {
                    if (foundFirstPair)
                        return true;
                    else
                    {
                        foundFirstPair = true;
                        i++;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns if hand is a One Pair (2 cards have the same face value). The pair's face value must be Jack or better.
        /// Note that the hand is sorted by face value beforehand (from largest to smallest).
        /// </summary>
        static bool IsOnePair(Card[] hand)
        {
            for (int i = 0; i < hand.Length - 1; i++)
            {
                if (hand[i].GetFaceValue() > FaceValue.Ten && hand[i].GetFaceValue() == hand[i + 1].GetFaceValue())
                    return true;
            }
            return false;
        }
    }
}
