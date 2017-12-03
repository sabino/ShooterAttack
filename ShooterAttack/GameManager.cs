using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ShooterAttack
{
    class GameManager
    {
        public static int Score = 0;
        public static int CurrentWave = 0;
        public static int BaseGateCount = 8;
        public static int MaxGatesCount = 15;
        public static int CurrentGatesCount = 8;
        public static Vector2 PlayerStartLoc = new Vector2(32, 32);


        public static void StartNewWave()
        {
            CurrentWave++;
            if (CurrentGatesCount < MaxGatesCount)
                CurrentGatesCount++;

            Player.BaseSprite.WorldLocation = PlayerStartLoc;
            Camera.Position = Vector2.Zero;
            WeaponManager.CurrentWeaponType =
                WeaponManager.WeaponType.Normal;
            WeaponManager.Shots.Clear();
            WeaponManager.PowerUps.Clear();
            EffectsManager.Effects.Clear();
            EnemyManager.Enemies.Clear();
            Map.GenerateRandomMap();
            GoalManager.GenerateComputers(CurrentGatesCount);
        }

        public static void StartNewGame()
        {
            CurrentWave = 0;
            Score = 0;
            StartNewWave();
        }

    }
}
