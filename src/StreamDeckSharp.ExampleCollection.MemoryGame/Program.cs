using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace StreamDeckSharp.ExampleCollection.MemoryGame
{
    class Program
    {
        private static readonly Random rnd = new Random();

        //positon of restart button
        private static int restartKey = 7;

        private static int mode = 0;
        private static int[] openCard = new int[2];

        private static KeyBitmap restartIcon;
        private static KeyBitmap[] iconsActive = new KeyBitmap[7];
        private static KeyBitmap[] iconsInactive = new KeyBitmap[7];

        //14 slots for memory (7x2 cards)
        private static int[] gameState = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6 };
        private static bool[] cardVisible = new bool[14];

        static void Main(string[] args)
        {
            initializeIconBitmaps();

            using (var s = StreamDeck.OpenDevice())
            {
                s.KeyStateChanged += StreamDeckKeyHandler;
                startGame(s);

                Console.WriteLine("Press any key (on the keyboard) to exit Stream Deck demo.");
                Console.ReadKey();
            }
        }

        private static void startGame(IStreamDeck deck)
        {
            //suffle memory cards
            openCard[0] = -1;
            openCard[1] = -1;
            mode = 0;
            suffleArray(gameState, rnd);

            for (int i = 0; i < cardVisible.Length; i++)
                cardVisible[i] = false;

            //Clear all tiles (except restart key)
            for (int i = 0; i < deck.KeyCount; i++)
                if (i != restartKey)
                    deck.ClearKey(i);

            //(Re-)Draw restart key image
            deck.SetKeyBitmap(restartKey, restartIcon);
        }

        private static void refreshKeyIcon(IStreamDeck deck, int cardId)
        {
            var keyId = cardId >= restartKey ? cardId + 1 : cardId;

            if (cardVisible[cardId])
            {
                if ((openCard[0] == cardId || openCard[1] == cardId))
                    deck.SetKeyBitmap(keyId, iconsInactive[gameState[cardId]]);
                else
                    deck.SetKeyBitmap(keyId, iconsActive[gameState[cardId]]);
            }
            else
            {
                deck.SetKeyBitmap(keyId, KeyBitmap.Black);
            }
        }

        private static void initializeIconBitmaps()
        {
            restartIcon = IconLoader.LoadIconByName("restart.png", true);
            for (int i = 0; i < iconsActive.Length; i++)
            {
                var name = $"card{i}.png";
                iconsActive[i] = IconLoader.LoadIconByName(name, true);
                iconsInactive[i] = IconLoader.LoadIconByName(name, false);
            }
        }

        private static void suffleArray<T>(T[] array, Random rnd)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int pick = rnd.Next(array.Length - i) + i;

                //Swap elements
                T tmp = array[i];
                array[i] = array[pick];
                array[pick] = tmp;
            }
        }


        private static Thread sleepThread;

        private static readonly AutoResetEvent threadSleeper = new AutoResetEvent(false);
        private static readonly object closeCardLock = new object();
        private static void closeCards(IStreamDeck deck)
        {
            lock (closeCardLock)
            {
                if (mode != 2) return;
                cardVisible[openCard[0]] = false;
                cardVisible[openCard[1]] = false;
                var c1 = openCard[0];
                var c2 = openCard[1];
                openCard[0] = -1;
                openCard[1] = -1;
                refreshKeyIcon(deck, c1);
                refreshKeyIcon(deck, c2);
                mode = 0;
            }
        }

        private static void StreamDeckKeyHandler(object sender, KeyEventArgs e)
        {
            var deck = sender as IStreamDeck;
            if (deck == null) return;

            if (e.Key == restartKey && e.IsDown)
            {
                startGame(deck);
                return;
            }

            if (e.IsDown)
            {
                if (mode == 2)
                {
                    threadSleeper.Set();
                    closeCards(deck);
                }

                var cardId = e.Key < restartKey ? e.Key : e.Key - 1;
                if (mode == 0)
                {
                    if (!cardVisible[cardId])
                    {
                        mode = 1;
                        openCard[0] = cardId;
                        cardVisible[cardId] = true;
                        refreshKeyIcon(deck, cardId);
                    }
                }
                else if (mode == 1)
                {
                    if (!cardVisible[cardId])
                    {
                        openCard[1] = cardId;
                        cardVisible[cardId] = true;
                        refreshKeyIcon(deck, cardId);
                        if (gameState[openCard[0]] == gameState[openCard[1]])
                        {
                            mode = 0;
                            var c1 = openCard[0];
                            var c2 = openCard[1];
                            openCard[0] = -1;
                            openCard[1] = -1;
                            refreshKeyIcon(deck, c1);
                            refreshKeyIcon(deck, c2);
                        }
                        else
                        {
                            mode = 2;
                            sleepThread = new Thread(() =>
                            {
                                var timeout = threadSleeper.WaitOne(2000);
                                closeCards(deck);
                            });
                            sleepThread.Start();
                        }
                    }
                }
            }
        }
    }
}
