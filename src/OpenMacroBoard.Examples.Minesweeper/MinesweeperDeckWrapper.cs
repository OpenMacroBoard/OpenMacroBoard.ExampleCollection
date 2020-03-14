using OpenMacroBoard.SDK;
using System;
using System.Diagnostics;
using System.Threading;

namespace OpenMacroBoard.Examples.Minesweeper
{
    internal sealed class MinesweeperDeckWrapper : IDisposable
    {
        private readonly GridKeyPositionCollection keyGrid;
        private readonly IMacroBoard deck;
        private readonly IMinesweeperIconSet iconSet;

        private readonly int keyCount;
        private readonly int shortPressDurationMs;

        private readonly Stopwatch stopwatch = Stopwatch.StartNew();
        private readonly long[] downTimestamp;
        private readonly Timer bgTimer;

        private MinesweeperGame game;

        public MinesweeperDeckWrapper(
            IMacroBoard deck,
            IMinesweeperIconSet iconSet,
            int shortPressDurationMs
        )
        {
            keyGrid = (deck.Keys as GridKeyPositionCollection) ?? throw new InvalidOperationException("Deck not supported");

            this.deck = deck;
            this.iconSet = iconSet;
            this.shortPressDurationMs = shortPressDurationMs;

            keyCount = keyGrid.KeyCountX * keyGrid.KeyCountY;
            downTimestamp = new long[keyCount];

            SetupMinefield();

            bgTimer = new Timer(TimerCallback, null, 0, 100);
            deck.KeyStateChanged += Deck_KeyStateChanged;
        }

        private void SetupMinefield()
        {
            game = new MinesweeperGame(keyGrid.KeyCountX, keyGrid.KeyCountY);
            SetupMines();
            UpdateField();
        }

        private void SetupMines()
        {
            var random = new Random();

            var setMines = 3;
            while (setMines > 0)
            {
                var pos = random.Next(keyCount);
                (var x, var y) = CoordsFromPos(pos);

                if (game.SetMine(x, y))
                {
                    setMines--;
                }
            }
        }

        private void TimerCallback(object state)
        {
            var t = stopwatch.ElapsedMilliseconds;

            for (var i = 0; i < keyCount; i++)
            {
                var dt = downTimestamp[i];

                if (dt <= 0)
                {
                    continue;
                }

                if ((t - dt) > shortPressDurationMs)
                {
                    downTimestamp[i] = 0;
                    LongPress(i);
                }
            }
        }

        private void Deck_KeyStateChanged(object sender, KeyEventArgs e)
        {
            if (e.IsDown)
            {
                downTimestamp[e.Key] = stopwatch.ElapsedMilliseconds;
            }
            else
            {
                if (downTimestamp[e.Key] <= 0)
                {
                    return;
                }

                downTimestamp[e.Key] = 0;
                ShortPress(e.Key);
            }
        }



        private void UpdateField()
        {
            for (var i = 0; i < keyCount; i++)
            {
                UpdateFieldBitmap(i);
            }
        }

        private void UpdateFieldBitmap(int pos)
        {
            (var x, var y) = CoordsFromPos(pos);

            var cellData = game[x, y];
            var bitmap = GetBitmapForCell(cellData);

            deck.SetKeyBitmap(pos, bitmap);
        }

        private KeyBitmap GetBitmapForCell(FieldValue cell)
        {
            if (game.GameState == MinesweeperGameState.Lost)
            {
                if (cell.IsMarkedWithFlag)
                {
                    if (cell.IsMine)
                    {
                        return iconSet.DefusedMine;
                    }
                    else if (iconSet.FlagWithoutMine != null)
                    {
                        return iconSet.FlagWithoutMine;
                    }
                }
            }
            else if (game.GameState == MinesweeperGameState.Won)
            {
                if (cell.IsMine)
                {
                    return iconSet.DefusedMine;
                }
            }
            else
            {
                if (cell.IsMarkedWithFlag)
                {
                    return iconSet.Flag;
                }

                if (!cell.IsVisible)
                {
                    return iconSet.HiddenCell;
                }
            }

            if (cell.IsMine)
            {
                return iconSet.ExplodedMine;
            }

            return iconSet[cell.NeighbourMineCount];
        }

        private void ShortPress(int pos)
        {
            if (game.GameOver)
            {
                SetupMinefield();
                return;
            }

            (var x, var y) = CoordsFromPos(pos);
            game.OpenField(x, y);
            UpdateField();
        }

        private void LongPress(int pos)
        {
            (var x, var y) = CoordsFromPos(pos);
            game.ToggleFlag(x, y);
            UpdateField();
        }

        private (int x, int y) CoordsFromPos(int pos)
        {
            return (pos % keyGrid.KeyCountX, pos / keyGrid.KeyCountX);
        }

        public void Dispose()
        {
            deck.KeyStateChanged -= Deck_KeyStateChanged;
            bgTimer.Dispose();
        }
    }
}
