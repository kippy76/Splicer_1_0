using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Collections;

namespace Splicer
{
    class IO
    {
        private KeyboardState oldKState, newKState;
        ButtonState[] mNew;
        ButtonState[] mOld;

        public IO()
        {
           mNew = new ButtonState[3];
           mOld = new ButtonState[3];
        }
               
        public char pollKeys()
        {
            char keyPressed = 'x';
            newKState = Keyboard.GetState();
            if (newKState.GetPressedKeys().Length > 0)
            {
                if (newKState.IsKeyDown(newKState.GetPressedKeys()[0]))
                {
                    if (!oldKState.IsKeyDown(newKState.GetPressedKeys()[0]))
                    {
                        keyPressed = (char)newKState.GetPressedKeys()[0];
                    }
                }
            }
            oldKState = newKState;
            return keyPressed;
        }

        public bool mouseButtonsStateChange()
        {
            bool changed = false;
            mNew[0] = Mouse.GetState().LeftButton;
            mNew[1] = Mouse.GetState().MiddleButton;
            mNew[2] = Mouse.GetState().RightButton;
            for (int btn = 0; btn < 3; btn++ )
            {
                if (mNew[btn] != mOld[btn])
                {
                    mOld[btn] = mNew[btn];
                    changed = true;
                }
            }      
            return changed;
        }

        public MouseState pollMouse()
        {            
            return Mouse.GetState();
        }

    }
}
