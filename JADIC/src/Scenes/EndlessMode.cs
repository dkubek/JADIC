using System;
using System.Drawing;
using System.Collections.Generic;
using JADIC.Controls;

namespace JADIC.Scenes
{
    public class EndlessMode : Scene
    {
        private int enemySpawnCooldown = 100;
        private int maxEnemies = 1;
        private int enemies = 0;
        private int spawnCooldown = 0;

        private double enemyProjectileProbability = 0.3;
        private double spawnProbability = 0.3;

        public EndlessMode(World world) : base(world) { }

        public override void Initialize()
        {
            InitializePlayer();
        }

        private void InitializePlayer()
        {
            var playerControl = new PlayerControl(MainWorld.MainPlayer);
            var controls = new Control(playerControl);
            MainWorld.MainPlayer.Controls = controls;
        }

        public override void Update()
        {
            UpdatePlayer();
            UpdateGameObjects();

            var collisions = ExtractCollisions();
            HandleCollisions(collisions);

            PerformActions();

            UpdateDifficulty();
            SpawnEnemies();

            if (GameEnded())
            {
                isRunning = false;
            }

            CleanUp();
        }

        private void UpdateDifficulty()
        {
            double x = MainWorld.TotalScore / 1000;
            enemyProjectileProbability = Math.Min(0.07 * x + 0.3, 0.7);
            spawnProbability = Math.Min(0.02 * x + 0.3, 0.5);
        }

        private void SpawnEnemies()
        {
            if (spawnCooldown == 0)
            {
                if (enemies < maxEnemies)
                {
                    if (MainWorld.RandomGen.NextDouble() < spawnProbability)
                    {
                        generateEnemy();
                        spawnCooldown = enemySpawnCooldown;
                    }
                }
            }
            else
            {
                spawnCooldown--;
            }
        }

        private void generateEnemy()
        {
            var enemy = new Enemy(
                new Point(
                    MainWorld.Resolution.Width,
                    MainWorld.RandomGen.Next(100, MainWorld.Resolution.Height - 100)),
                enemyProjectileProbability);

            var destination = new Point(MainWorld.Resolution.Width / 2, enemy.Position.Y);

            var controls = new List<ControlElement>();
            controls.Add(new LinearTransition(destination, 3));
            controls.Add(new Follow(MainWorld.MainPlayer, 5));
            enemy.Controls = new Control(controls);

            MainWorld.GameObjects.Add(enemy);
        }

        private bool GameEnded()
            => MainWorld.MainPlayer.Lives <= 0;
    }
}