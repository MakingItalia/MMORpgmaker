using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using MMORpgmaker_Client.Controls;
using MMORpgmaker_Client;
using UIXControls;
using MMORpgmaker.Controls;
using Packet;
using MMORpgmaker.Helper;

namespace MMORpgmaker_Client.Controls 
{

   
    public class Msgbox : IControls
    {

        private Game1 Game;

        #region Private Field

        SkinSystem Skin;
        const int sz_w = 280;
        const int sz_h = 180;
        Vector2 pos;
        Vector2 old;
        Texture2D WinText;
        Rectangle dragArea = new Rectangle(4, 6, 272, 18);
        bool mdown = false;
        bool focus = false;
        bool drag = false;
        Vector2 downTo;
        Vector2 LabelUsername;
        Vector2 LabelPassword;
        SpriteFont Font;

        #endregion

        #region Other Controls

        Utils ut = new Utils();
        Button ok;

        TextBox tx;
        TextBox psw;

        /// <summary>
        /// Accetta un controllo di tipo Message per
        /// visualizzare un determinato messaggio nel caso servisse
        /// </summary>
        public Message refereced;

        #endregion

        #region Delegates

        public delegate void MouseMove(MouseMoveArgs e);
        public delegate void MouseDown(MouseMoveArgs e);
        public delegate void MouseUp(MouseMoveArgs e);

        #endregion

        #region Events

        public event MouseMove OnMouseMove;
        public event MouseDown OnMouseDown;
        public event MouseUp OnMouseUp;

        #endregion

        #region Public Field

        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }

        #endregion

        public bool OkButtonPressed = false;

        public PacketData paket = new PacketData();

        GameClient Client;

        public Msgbox(Vector2 position,SkinSystem skin,SpriteFont font,GameClient c,Game1 game)
        {
            Position = position;
            Skin = skin;
            WinText = ut.LoadFromFileStream(Skin.SkinBase + "\\win_msgbox.png", Skin.graphicsDevice);

            OnMouseDown += new MouseDown(Msgbox_OnMouseDown);
            OnMouseUp += new MouseUp(Msgbox_OnMouseUp);
            OnMouseMove += new MouseMove(Msgbox_OnMouseMove);

            dragArea = new Rectangle((int)Position.X + dragArea.X, (int)Position.Y + dragArea.Y, 272,18);
            
            //Creazione pulsante
            ok = new Button(Button.Button_Type.ok, new Vector2(Position.X + (sz_w - 50), Position.Y + (sz_h - 85)), skin);
            ok.ButtonClick += new Button.OnClick(ok_ButtonClick);
            Font = font;



            tx = new TextBox(new Vector2(Position.X + 10, Position.Y + 38), "server", font, skin);
            LabelUsername = new Vector2(Position.X + 14, Position.Y + 22);
            LabelPassword = new Vector2(Position.X + 14, Position.Y + 58);
            psw = new TextBox(new Vector2(Position.X + 10, Position.Y + 73 ), "server", font, skin);
            psw.passwords = "server";
            psw.IsPassword = true;

            Client = c;

            Game = game;
        }

        bool down = false;
        public int account_id = 0;
        void ok_ButtonClick()
        {
            refereced.ShowMessage();

            
            // Try to login          
            if (Client != null && down  == false)
            {
                //Connect to server
                Client.Connect(); 

                //Send Login data Packet
                bool check = Client.Login(tx.Text, psw.passwords);
                
                //Checking
                if (check)
                {
                    refereced.MessageText = "Login Succesfull";
                    refereced.ShowMessage();
                    OkButtonPressed = true;

                    
                    //Send packet request account id
                    PacketData p = new PacketData();
                    p.Command = (uint)PacketHeader.HeaderCommand.ACT_GET_ACC_ID;
                    p.Argument1 = tx.Text;

                    //Send Packet and Get packet from server
                    object t = Client.SendGetPacket(p);

                    //Convert object to PacketData
                    paket = (PacketData)t;        
                    
                    //set account id
                    account_id = Convert.ToInt16(paket.Argument1);

                }
                else
                {
                    refereced.MessageText = "Wrong username or password.";
                    refereced.ShowMessage();
                }
                down = true;
            }

            //------

            
        }


        void Msgbox_OnMouseUp(MouseMoveArgs e)
        {
            if (mdown)
            {
                
                mdown = false;
                drag = false;
                dragArea = new Rectangle((int)Position.X + 4, (int)Position.Y + 5, 272, 18);
                down=false;
            }

        }

        void Msgbox_OnMouseDown(MouseMoveArgs e)
        {
               
            if (dragArea.Contains((int)e.mouseState.X, (int)e.mouseState.Y) && mdown)
            {
                drag = true;              
            }

            downTo = new Vector2(e.mouseState.X, e.mouseState.Y);
            mdown = true;
        }


        void Msgbox_OnMouseMove(MouseMoveArgs e)
        {
          
            if (drag && mdown)
            {
                Position = new Vector2(e.mouseState.X, e.mouseState.Y);
                ok.Position = new Vector2(e.mouseState.X + (sz_w - 50), e.mouseState.Y + (sz_h - 85));
                tx.Position = new Vector2(e.mouseState.X + 10, e.mouseState.Y + 38);
                LabelUsername = new Vector2(e.mouseState.X +14,e.mouseState.Y+22);
                LabelPassword = new Vector2(e.mouseState.X + 14, e.mouseState.Y + 58);
                psw.Position = new Vector2(e.mouseState.X + 10, e.mouseState.Y + 73);
                
            }

        }




        public void Initialize()
        {
           
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sprite)
        {
            if (!drag)
            {
                sprite.Draw(WinText, Position, Color.White);
            }
            else
            {
                sprite.Draw(WinText, Position, Color.FromNonPremultiplied(255,255,255,150));
            }

            ok.Draw(sprite);

            sprite.DrawString(Font, "Username", LabelUsername, Color.Black);

            tx.Draw(sprite);

            sprite.DrawString(Font, "Password", LabelPassword, Color.Black);

            psw.Draw(sprite);

        }

        public void Update(GameTime gameTime, Microsoft.Xna.Framework.Input.KeyboardState kb, Microsoft.Xna.Framework.Input.MouseState ms)
        {
            MouseMoveArgs e = new MouseMoveArgs(ms);

            if (ms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                OnMouseDown(e);
            }

            if (ms.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
            {
                OnMouseUp(e);
            }

            OnMouseMove(e);

            ok.Update(gameTime, kb, ms); //Update Button
            tx.Update(gameTime, kb, ms); //Update TextBox
            psw.Update(gameTime, kb, ms); //Update TextBox Password
        }
    }
}
