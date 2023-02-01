using System;
using GJP2021.Sources.Abilities;
using GJP2021.Sources.GameStates;
using GJP2021.Sources.Paint;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GJP2021.Sources.Characters
{
    public class Player
    {
        private float _speedX;
        private float _speedY;
        private readonly float _maxSpeed;
        private readonly float _acceleration;
        private readonly float _dragCoefficient;
        private readonly float _dragConstant;
        private readonly Vector2 _bounds;
        private float _speedBoostMultiplier = 1;
        private float _speedBoostCurrentDuration = 0;
        private float _speedBoostDuration = 0;
        private bool _speedBoostActive = false;
        public Vector2 Position;
        public int Score = 0;

        public float Health { get; private set; } = 100F;

        private const float MaxHealth = 100F;

        private static Texture2D GetHealthTexture() => Kolori.Instance.TextureMap["health_bar"];

        private PaintColors _trailColor;
        private readonly PaintPeriodicSpawner _periodicPaintSpawner;
        private float _timer;
        private float _timerSound;
        private bool _pauseKeyDown;
        private bool _abilityKeyDown;
        private readonly Random _random;

        public bool Paused { get; set; }

        public PaintColors TrailColor
        {
            get => _trailColor;
            set
            {
                Heal(MaxHealth);
                _trailColor = value;
                _periodicPaintSpawner.SetColor(PaintCircle.ColorMap[_trailColor]);
            }
        }

        private Player(float x, float y, float maxSpeed, float acceleration, Vector2 bounds)
        {
            Position = new Vector2(x, y);
            _maxSpeed = maxSpeed;
            _acceleration = acceleration;
            _bounds = bounds;
            _speedX = 0;
            _speedY = 0;
            _dragCoefficient = 0.5f;
            _dragConstant = 80;
            _periodicPaintSpawner = new PaintPeriodicSpawner(
                PaintCircle.Red,
                new Color(32, 32, 32),
                35,
                10,
                30,
                0.05F,
                0.1F,
                30
            );
            _trailColor = PaintColors.RED;
            _random = new();
        }

        public Vector2 GetSpeedVector()
        {
            return new(_speedX, _speedY);
        }

        public void DrawDisplay()
        {
            DrawHealth();
            DrawAbility();
        }

        private void DrawAbility()
        {
            var texture = GetAbilityTexture();
            var x = Kolori.Instance.GetWindowWidth() - 64 - 16;
            var y = Kolori.Instance.GetWindowHeight() - 48 - 16;

            var colorIndex = Array.IndexOf(Enum.GetValues(_trailColor.GetType()), _trailColor);
            var colorOffset = colorIndex * 32;

            //Icon slot
            Kolori.Instance.SpriteBatch.Draw(
                texture,
                new Vector2(x, y),
                new Rectangle(0, 0, 64, 48),
                Color.White
            );

            //Icon
            Kolori.Instance.SpriteBatch.Draw(
                texture,
                new Vector2(x + 24, y + 8),
                new Rectangle(64, colorOffset, 32, 32),
                Color.White
            );
        }

        private static Texture2D GetAbilityTexture() => Kolori.Instance.TextureMap["ability_icons"];

        public void DrawHealth()
        {
            var texture = GetHealthTexture();
            const int x = 16;
            var y = Kolori.Instance.GetWindowHeight() - 92 - 16;

            var colorIndex = Array.IndexOf(Enum.GetValues(_trailColor.GetType()), _trailColor);
            var colorOffset = colorIndex * 38;

            //Empty Health Bar
            Kolori.Instance.SpriteBatch.Draw(
                texture,
                new Vector2(x, y),
                new Rectangle(0, 0, 288, 88),
                Color.White
            );

            //Bucket fluid
            Kolori.Instance.SpriteBatch.Draw(
                texture,
                new Vector2(x + 40, y + 12),
                new Rectangle(238, 88 + colorOffset, 26, 36),
                Color.White
            );

            //Health
            var healthPercent = (int)Math.Floor(238F * (Health / MaxHealth));
            Kolori.Instance.SpriteBatch.Draw(
                texture,
                new Vector2(x + 46, y + 46),
                new Rectangle(0, 88 + colorOffset, healthPercent, 38),
                Color.White
            );
        }

        public void Draw()
        {
            var texture = GetTexture();

            Kolori.Instance.SpriteBatch.Draw(
                texture,
                Position - new Vector2(texture.Width / 2F, texture.Height / 2F),
                Color.White
            );

            Utils.DrawOutlinedText(
                "Fonts/lunchds",
                24,
                "Score: " + Score,
                new Vector2(10, 10),
                Color.Crimson,
                Color.Black
            );

            var healthDigits = Math.Floor(Math.Log10(Health)) + 1;
            var spaces = new string(' ', (int)Math.Max(0, 3 - healthDigits));
            var healthString = "[" + (int)Health + spaces + "|" + (int)MaxHealth + "]";
            Utils.DrawOutlinedText(
                "Fonts/lunchds",
                24,
                healthString,
                new Vector2(90 + 16, Kolori.Instance.GetWindowHeight() - 92 - 16 + 5),
                Color.Crimson,
                Color.Black
            );

            var ability = GetAbility();

            if (ability == null)
            {
                return;
            }

            var abilityX = Kolori.Instance.GetWindowWidth() - 64 - 16;
            var abilityY = Kolori.Instance.GetWindowHeight() - 48 - 16 - 5;
            Utils.DrawOutlinedText(
                "Fonts/lunchds",
                24,
                "" + ability.PaintCost,
                new Vector2(abilityX, abilityY),
                Color.Crimson,
                Color.Black,
                Utils.HorizontalFontAlignment.RIGHT,
                Utils.VerticalFontAlignment.CENTER
            );
        }

        public void HandlePause()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _pauseKeyDown = true;
                Kolori.Instance.SoundMap["pause_screen"].Play();
            }
            else
            {
                if (_pauseKeyDown)
                {
                    Paused = !Paused;
                }

                _pauseKeyDown = false;
            }
        }

        public void HandleAbility(IngameState gameState)
        {
            if (gameState.Controls.UseAbility)
            {
                var ability = GetAbility();
                ability?.TryUse(this, gameState);
                gameState.Controls.UseAbility = false;
            }
            else
            {
                if (gameState.Controls.IsKeyDown(Keys.Space))
                {
                    _abilityKeyDown = true;
                }
                else
                {
                    var ability = GetAbility();
                    if (_abilityKeyDown && ability != null)
                    {
                        ability.TryUse(this, gameState);
                    }

                    _abilityKeyDown = false;
                }
            }
        }

        private Ability GetAbility()
        {
            return Ability.Abilities.ContainsKey(_trailColor)
                ? Ability.Abilities[_trailColor]
                : null;
        }

        public void Update(GameTime gameTime, IngameState ingameState, PaintCircles paintCircles)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _periodicPaintSpawner.Update(gameTime, paintCircles, Position);
            _timer += delta;
            if (_timer >= 0.01)
            {
                _timer = 0;
                Damage(0.1F);
            }

            if (_timerSound >= 0.7)
            {
                _timerSound = 0;
                Kolori.Instance.SoundMap["player_move"].Play(
                    (float)Math.Clamp(_random.NextDouble(), 0.2, 0.8),
                    (float)Math.Clamp(_random.NextDouble(), 0.2, 0.8),
                    (float)Math.Clamp(_random.NextDouble(), 0.2, 0.8)
                );
            }

            if (_speedBoostActive)
            {
                _speedBoostCurrentDuration += delta;
                if (_speedBoostCurrentDuration >= _speedBoostDuration)
                {
                    EndSpeedBoost();
                }
            }

            HandleAcceleration(gameTime, ingameState);

            Position.X += _speedX * delta;
            Position.Y += _speedY * delta;

            var w = GetTexture().Width / 2F;
            var h = GetTexture().Height / 2F;

            if (Position.Y <= h || Position.Y >= _bounds.Y - h)
            {
                _speedY = 0;
            }

            if (Position.X <= w || Position.X >= _bounds.X - w)
            {
                _speedX = 0;
            }

            Position.Y = Math.Clamp(Position.Y, w, _bounds.Y - w);
            Position.X = Math.Clamp(Position.X, h, _bounds.X - h);
        }

        public void SetSpeed(float speedX = 0F, float speedY = 0F)
        {
            _speedX = speedX;
            _speedY = speedY;
        }

        private void HandleAcceleration(GameTime gameTime, IngameState ingameState)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var controls = ingameState.Controls;
            var a = controls.IsKeyDown(Keys.A) || controls.IsKeyDown(Keys.Left);
            var w = controls.IsKeyDown(Keys.W) || controls.IsKeyDown(Keys.Up);
            var s = controls.IsKeyDown(Keys.S) || controls.IsKeyDown(Keys.Down);
            var d = controls.IsKeyDown(Keys.D) || controls.IsKeyDown(Keys.Right);
            var mx = Convert.ToInt32(d) - Convert.ToInt32(a);
            var my = Convert.ToInt32(s) - Convert.ToInt32(w);
            var diagonalBias = 1F;
            if (mx != 0 && my != 0)
            {
                diagonalBias = 1 / (float)Math.Sqrt(2);
            }

            var ax = _acceleration * mx * diagonalBias;
            var ay = _acceleration * my * diagonalBias;

            if (mx == 0)
            {
                _speedX -= _speedX * (1 - _dragCoefficient) * delta;
                _speedX = _speedX switch
                {
                    > 0 => Math.Max(0, _speedX - _dragConstant * delta),
                    < 0 => Math.Min(0, _speedX + _dragConstant * delta),
                    _ => _speedX
                };
            }

            if (my == 0)
            {
                _speedY -= _speedY * (1 - _dragCoefficient) * delta;
                _speedY = _speedY switch
                {
                    > 0 => Math.Max(0, _speedY - _dragConstant * delta),
                    < 0 => Math.Min(0, _speedY + _dragConstant * delta),
                    _ => _speedY
                };
            }

            _speedX += ax * _speedBoostMultiplier * delta;
            _speedY += ay * _speedBoostMultiplier * delta;
            var biasX = Math.Abs(_speedX) / (float)Math.Sqrt(_speedX * _speedX + _speedY * _speedY);
            var biasY = Math.Abs(_speedY) / (float)Math.Sqrt(_speedX * _speedX + _speedY * _speedY);
            _speedX = Math.Clamp(
                _speedX,
                -_maxSpeed * _speedBoostMultiplier * biasX,
                _maxSpeed * _speedBoostMultiplier * biasX
            );
            _speedY = Math.Clamp(
                _speedY,
                -_maxSpeed * _speedBoostMultiplier * biasY,
                _maxSpeed * _speedBoostMultiplier * biasY
            );
        }

        public void SetSpeedBoost(float multiplier, float duration)
        {
            _speedBoostMultiplier = multiplier;
            _speedBoostDuration = duration;
            _speedBoostCurrentDuration = 0;
            _speedBoostActive = true;
        }

        public void EndSpeedBoost()
        {
            _speedBoostActive = false;
            _speedBoostMultiplier = 1;
            _speedBoostCurrentDuration = 0;
            _speedBoostDuration = 0;
        }

        private Texture2D GetTexture() =>
            Kolori.Instance.TextureMap["player_" + _trailColor.ToString().ToLower()];

        public void Heal(float amount)
        {
            Health = Math.Min(MaxHealth, Health + amount);
        }

        public void Damage(float amount)
        {
            Health -= amount;
        }

        public bool IsAlive()
        {
            return Health > 0;
        }

        public static PlayerBuilder Builder()
        {
            return new();
        }

        internal dynamic ToDict()
        {
            return new
            {
                Position = new { Position.X, Position.Y },
                Speed = new { _speedX, _speedY },
                SpeedBoost = new
                {
                    _speedBoostMultiplier,
                    _speedBoostCurrentDuration,
                    _speedBoostDuration,
                    _speedBoostActive
                },
                TrailColor = TrailColor.ToString(),
                Score,
                Health,
            };
        }

        public class PlayerBuilder
        {
            private float _x;
            private float _y;
            private float _maxSpeed;
            private float _acceleration;
            private Vector2 _bounds;

            internal PlayerBuilder()
            {
                _x = 0;
                _y = 0;
                _maxSpeed = 0;
                _acceleration = 0;
            }

            public PlayerBuilder SetPosition(float x, float y)
            {
                _x = x;
                _y = y;
                return this;
            }

            public PlayerBuilder SetAcceleration(float acceleration)
            {
                _acceleration = acceleration;
                return this;
            }

            public PlayerBuilder SetMaxSpeed(float maxSpeed)
            {
                _maxSpeed = maxSpeed;
                return this;
            }

            public PlayerBuilder SetBounds(Vector2 bounds)
            {
                _bounds = bounds;
                return this;
            }

            public Player Build()
            {
                return new(_x, _y, _maxSpeed, _acceleration, _bounds);
            }
        }
    }
}
