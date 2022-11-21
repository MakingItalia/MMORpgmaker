using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


/* UIX SDK BY Thejuster
 * see my other project on pierotofy.it and Github
 * */


namespace UIXControls
{
    public class UIXProgressBar : Controls
    {
        int minval = 0, maxval = 100, val = 0;
        float step;
        Primitive p;
        SpriteFont f;
        Color highlightText = Color.White;

        public delegate void ValueChanged();

        public event ValueChanged OnValueChanged;

        public Color HighlightText
        {
            get { return highlightText; }
            set { highlightText = value; }
        }

        public int MinimumValue
        {
            get { return minval; }
            set { minval = value; }
        }


        public int MaxmumValue
        {
            get { return maxval; }
            set { maxval = value; }
        }

        public int Value
        {
            get { return val; }
            set { val = value; }
        }




        public UIXProgressBar(GraphicsDevice dev, SpriteFont font, Vector2 Position, int Width, int Height = 30)
            : base(dev)
        {
            base.Position = Position;
            base.Width = Width;
            base.Height = Height;
            p = new Primitive(dev);
            f = font;
            OnValueChanged += new ValueChanged(UIXProgressBar_OnValueChanged);
    
        }


     


        void UIXProgressBar_OnValueChanged()
        {
            
        }


        public override void Draw(SpriteBatch sprite)
        {
            base.Draw(sprite);
            p.Begin();

            //Background
            p.FillRectangle(new Rectangle((int)Position.X, (int)Position.Y, base.Width, base.Height), Color.Azure);
                       
            //Progress
            p.FillRectangle(new Rectangle((int)Position.X, (int)Position.Y, GetWidth(), base.Height), Color.DarkBlue);
            p.FillRectangle(new Rectangle((int)base.Position.X, (int)base.Position.Y, GetWidth(), (int)base.Height /2), Color.Blue);

            
            //Text
            Vector2 Size = f.MeasureString(Value + " %");

            //Border
            p.DrawRectangle(new Rectangle((int)Position.X, (int)Position.Y, base.Width, base.Height), Color.Black);


            if (Value < (maxval / 2))
            {
                p.DrawString(f, Value + " %", new Vector2(((base.Width - Size.X) / 2) + Position.X + 1, ((base.Height - Size.Y) / 2) + Position.Y + 1), Color.DarkGray);
                p.DrawString(f, Value + " %", new Vector2(((base.Width - Size.X) / 2) + Position.X, ((base.Height - Size.Y) / 2) + Position.Y), Color.Black);
            }
            else
            {
                p.DrawString(f, Value + " %", new Vector2(((base.Width - Size.X) / 2) + Position.X + 1, ((base.Height - Size.Y) / 2) + Position.Y + 1), Color.Black);
                p.DrawString(f, Value + " %", new Vector2(((base.Width - Size.X) / 2) + Position.X, ((base.Height - Size.Y) / 2) + Position.Y), highlightText);
            }
            
            p.End();
        }


        public void Increment()
        {
            if (Value < MaxmumValue)
            {
                Value++;
                OnValueChanged();
            }
        }

        public void Increment(int value)
        {
            if (Value < MaxmumValue)
            {
                Value += value;
                OnValueChanged();
            }
        }


        public void Decrement()
        {
            if (Value > 0)
            {
                Value--;
                OnValueChanged();
            }
        }

        public void Decrement(int value)
        {
            if (Value > 0)
            {
                Value -= value;
                OnValueChanged();
            }
        }


        int GetWidth()
        {
            float max = float.Parse(MaxmumValue.ToString());
            float val = float.Parse(Value.ToString());

            float BarPixelAmount = Width;

            float Rate = val / max;

            float output = BarPixelAmount * Rate;

            return Convert.ToInt16(output);
        }


        public override void Update(GameTime gameTime, MouseState ms, KeyboardState kb)
        {
            base.Update(gameTime, ms, kb);
        }

    }
}
