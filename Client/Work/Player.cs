using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MMORpgmaker.Controls;
using MMORpgmaker.Helper;
using MMORpgmaker_Client;
using Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMORpgmaker.Work
{

    //By Thejuster


    public enum PlayerDirection
    {
        Dowm,
        Left,
        Right,
        Up
    }


    public class Player
    {

        int pos_X = 0, pos_y = 0;
        int move_x = 0, move_y = 0;
        int frame = 0, pause = 0;
        private Texture2D m_imageSprites, m_head;
        public SpriteBatch sprites;
        public SpriteFont font;
        public PlayerDirection Direction;
        //public character CharInfo;
        public int X, Y;
        public Dialog talk;
        GameClient client;
        Camera2d cams;

        public Player (GraphicsDevice dev,Texture2D body,Texture2D head,CharPaket c, SpriteFont font,Vector2 Position,GameClient cli,Camera2d cam)
        {
            pos_X = (int)Position.X;
            pos_y = (int)Position.Y;
            m_imageSprites = body;
            m_head = head;
            talk = new Dialog(dev, font);
            client = cli;
            cams = cam;
        }

  

        /// <summary>
        /// Move Player to Right
        /// </summary>
        public void MoveRight()
        {
            Direction = PlayerDirection.Right;
            move_x = 32;
        }

        /// <summary>
        /// Remote Player Move to Rightr
        /// </summary>
        public void StepRight()
        {
            Direction = PlayerDirection.Right;
            move_x = 32;
        }

        /// <summary>
        /// Move Player to Left
        /// </summary>
        public void MoveLeft()
        {
            Direction = PlayerDirection.Left;
            move_x = -32;
        }

        /// <summary>
        /// Remote player Move to Left
        /// </summary>
        public void StepLeft()
        {
            Direction = PlayerDirection.Left;
            move_x = -32;
        }

        /// <summary>
        /// Move Player To Down
        /// </summary>
        public void MoveDown()
        {
            Direction = PlayerDirection.Dowm;
            move_y = 32;
        }

        /// <summary>
        /// Remote Player Move to Down
        /// </summary>
        public void StepDown()
        {
            Direction = PlayerDirection.Dowm;
            move_y = 32;
        }

        /// <summary>
        /// Move Player to Up
        /// </summary>
        public void MoveUp()
        {
            Direction = PlayerDirection.Up;
            move_y = -32;
        }

        /// <summary>
        /// Remote Player move to Up
        /// </summary>
        public void StepUp()
        {
            Direction = PlayerDirection.Up;
            move_y = -32;
        }


        /// <summary>
        /// Move Player to Coordinates
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public void MoveTo(int x, int y)
        {
            move_x = x;
            move_y = y;
        }


        public void Update(GameTime gameTime,KeyboardState kb,MouseState ms)
        {
            talk.Update(gameTime, kb, ms, new Vector2(pos_X, pos_y));

            if (move_x != 0 || move_y != 0)
                frame = (frame + 1) % 16;
            else
                frame = 0;

            if (pause > 0)
                --pause;

            if (move_x > 0)
            {
                pos_X += 2;
                move_x -= 2;
            }

            if (move_x < 0)
            {
                pos_X -= 2;
                move_x += 2;
            }

            if (move_y > 0)
            {
                pos_y += 2;
                move_y -= 2;
            }

            if (move_y < 0)
            {
                pos_y -= 2;
                move_y += 2;
            }

            X = pos_X;
            Y = pos_y;

            cams.Pos = new Vector2(X, Y);
         
        }


        public void Draw(SpriteBatch sprite)
        {
            Rectangle rectSource = new Rectangle();
            //rectSource.X = (m_iFrame / 4) * 64;
            rectSource.X = (frame / 4) * 32;
            //rectSource.Y = ((int)m_npcDirection) * 96;
            rectSource.Y = ((int)Direction) * 48;
            //rectSource.Width = 64;
            rectSource.Width = 32;
            //rectSource.Height = 96;
            rectSource.Height = 48;

            Rectangle rectDest = new Rectangle();
            //rectDest.X = m_iPosX - 32;
            rectDest.X = pos_X - 32;
            //rectDest.Y = m_iPosY - 96;
            rectDest.Y = pos_y - 48;
            //rectDest.Width = 64;
            rectDest.Width = 32;
            //rectDest.Height = 96;
            rectDest.Height = 48;

            //sprite.Draw(m_imageSprites, rectDest, rectSource, Color.White);
            sprite.Draw(m_imageSprites, new Rectangle(rectDest.X + 10, rectDest.Y + 64, rectDest.Width, rectDest.Height), rectSource, Color.FromNonPremultiplied(0, 0, 0, 150), 26f, new Vector2(32, 64), SpriteEffects.None, 0);
            sprite.Draw(m_imageSprites, rectDest, rectSource, Color.White);
            sprite.Draw(m_head, rectDest, rectSource, Color.White);

            talk.Draw(sprite);
        }

        public void Talk(string message)
        {
            talk.Show(message, new Vector2(pos_X, pos_y));
        }
    }
}
