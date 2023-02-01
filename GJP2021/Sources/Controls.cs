using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace GJP2021.Sources
{
    public class Controls
    {
        public bool RL = false;
        public List<Keys> KeysDown = new();
        public MouseState MouseState = new();
        public bool UseAbility = false;

        public bool IsKeyDown(Keys key)
        {
            if (RL)
            {
                return KeysDown.Contains(key);
            }
            else
            {
                return Keyboard.GetState().IsKeyDown(key);
            }
        }

        public MouseState GetMouseState()
        {
            if (RL)
            {
                return MouseState;
            }
            else
            {
                return Mouse.GetState();
            }
        }
    }
}
