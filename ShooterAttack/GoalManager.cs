using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShooterAttack
{
    static class GoalManager
    {
        private static List<EnemyGate> enemyGates = new List<EnemyGate>();
        private static int activeCount = 0;

        private static int minDistanceFromPlayer = 250;

        private static Random rand = new Random();

        private static Texture2D texture;
        private static Rectangle initialActiveFrame;
        private static Rectangle initialDisabledFrame;
        private static int activeFrameCount;
        private static int disabledFrameCount;

        public static int ActiveGates { get { return activeCount; } }

        public static void Initialize(Texture2D textureSheet, Rectangle initialActiveRectangle, Rectangle initialDisabledRectangle, int activeFrames, int disabledFrames)
        {
            texture = textureSheet;
            initialActiveFrame = initialActiveRectangle;
            initialDisabledFrame = initialDisabledRectangle;
            activeFrameCount = activeFrames;
            disabledFrameCount = disabledFrames;
        }

        public static EnemyGate GateInSquare(Vector2 mapLocation)
        {
            foreach (EnemyGate enemyGate in enemyGates)
            {
                if (enemyGate.MapLocation == mapLocation)
                    return enemyGate;
            }

            return null;
        }

        public static void DetectShutdowns()
        {
            foreach (EnemyGate gate in enemyGates)
            {
                if (gate.Active)
                {
                    if (gate.IsCircleColliding(Player.BaseSprite.WorldCenter, Player.BaseSprite.CollisionRadius))
                    {
                        gate.Deactivate();
                        activeCount--;
                        GameManager.Score += 100;
                    }
                }
            }
        }

        public static void AddEnemyGate()
        {
            int startX = rand.Next(2, Map.MapWidth - 2);
            int startY = rand.Next(0, Map.MapHeight - 2);

            Vector2 tileLocation = new Vector2(startX, startY);

            if ((GateInSquare(tileLocation) != null) || (Map.IsWallTile(tileLocation)))
                return;

            if (Vector2.Distance(Map.GetSquareCenter(startX, startY), Player.BaseSprite.WorldCenter) < minDistanceFromPlayer)
                return;


            List<Vector2> path = PathFinder.FindPath(new Vector2(startX, startY), Map.GetSquareAtPixel(Player.BaseSprite.WorldCenter));

            if (path != null)
            {
                Rectangle squareRect = Map.SquareWorldRectangle(startX, startY);

                Sprite activeSprite = new Sprite(new Vector2(squareRect.X, squareRect.Y), texture, initialActiveFrame, Vector2.Zero);

                for (int x = 1; x < 3; x++)
                    activeSprite.AddFrame(new Rectangle(initialActiveFrame.X + (x * initialActiveFrame.Width), initialActiveFrame.Y, initialActiveFrame.Width, initialActiveFrame.Height));

                activeSprite.CollisionRadius = 15;

                Sprite disabledSprite = new Sprite(
                    new Vector2(squareRect.X, squareRect.Y), texture, initialDisabledFrame, Vector2.Zero);

                EnemyGate gate = new EnemyGate(activeSprite, disabledSprite, new Vector2(startX, startY));

                float timerOffset = (float)rand.Next(1, 100);
                gate.LastSpawnCounter = timerOffset / 100f;

                enemyGates.Add(gate);

                activeCount++;
            }
        }

        public static void GenerateComputers(int computerCount)
        {
            enemyGates.Clear();
            activeCount = 0;

            while (activeCount < computerCount)
                AddEnemyGate();
        }

        public static void Update(GameTime gameTime)
        {
            DetectShutdowns();
            foreach (EnemyGate gate in enemyGates)
                gate.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (EnemyGate gate in enemyGates)
                gate.Draw(spriteBatch);
        }

    }
}
