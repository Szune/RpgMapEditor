using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgMapEditor.Modules.Objects;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Controls.NonStaticControls
{
    public class ChatPanel : Control
    {
        public List<ChatChannel> Channels = new List<ChatChannel>();
        public int FocusedChannelId;
        //private Network net = Network.Instance;
        public Texture2D BackgroundSprite;
        public int Stroke = 2;

        public ChatPanel()
        {
            FocusedChannelId = -1;
        }

        /// <summary>
        /// Creates a panel containing a list of all currently open chat channels
        /// </summary>
        /// <param name="_Channels">A list of all currently open chat channels</param>
        /// <param name="_c_ChannelID">The channel which is currently focused</param>
        public ChatPanel(int id, string name, Texture2D backgroundSprite, int width, int height, Vector2 position, EViewport parentViewport, bool visible = false)
        {
            Id = id;
            Name = name;
            BackgroundSprite = backgroundSprite;
            Width = width;
            Height = height;
            OffsetPosition = position;
            Visible = visible;
            ParentViewport = parentViewport;
        }

        /// <summary>
        /// Changes the currently focused channel.
        /// </summary>
        /// <param name="cChannelId">The channel which is currently focused</param>
        public void SetFocusedChannel(int cChannelId)
        {
            FocusedChannelId = cChannelId;
            GetChannelById(FocusedChannelId).ChatScrollbar.CurrentStep = 0;
        }

        public void OpenChannel(ChatChannel channel)
        {
            throw new Exception("Not implemented!");
            /*if (!Channels.Contains(_Channel))
            {
                Channels.Add(_Channel);
                SetFocusedChannel(_Channel.ChannelID);
                net.SendPacket(_Channel.ChannelID.ToString(), PacketType.OpenChannel, Lidgren.Network.NetDeliveryMethod.ReliableUnordered, Map.Instance.Player.ID);
                // Send OpenChannelPacket
            }*/
        }

        /// <summary>
        /// This method should only be called when initializing the ChatPanel!
        /// </summary>
        /// <param name="channels"></param>
        public void AddChannels(List<ChatChannel> channels)
        {
            if (Channels.Count < 1)
            {
                foreach (ChatChannel c in channels)
                {
                    c.ParentViewport = ParentViewport;
                    Channels.Add(c);
                }
                if (Channels.Count > 0)
                {
                    SetFocusedChannel(Channels[0].ChannelId);
                }
            }
        }

        public void FocusNextChannel()
        {
            if (Channels.Count > 1)
            {
                if (FocusedChannelId == Channels[Channels.Count - 1].ChannelId)
                {
                    SetFocusedChannel(Channels[0].ChannelId);
                }
                else
                {
                    SetFocusedChannel(Channels[GetChannelIndexById(FocusedChannelId) + 1].ChannelId);
                }
            }
        }

        public void FocusPreviousChannel()
        {
            if (Channels.Count > 1)
            {
                if (FocusedChannelId == Channels[0].ChannelId)
                {
                    SetFocusedChannel(Channels[Channels.Count - 1].ChannelId);
                }
                else
                {
                    SetFocusedChannel(Channels[GetChannelIndexById(FocusedChannelId) - 1].ChannelId);
                }
            }
        }

        public void CloseChannel(ChatChannel channel)
        {
            throw new Exception("Not implemented!");
            /*
            if (_Channel.CanClose)
            {
                if (FocusedChannelID == _Channel.ChannelID)
                {
                    Channels.Remove(_Channel);
                    SetFocusedChannel(Channels[Channels.Count].ChannelID);
                }
                else
                {
                    Channels.Remove(_Channel);
                }
                net.SendPacket(_Channel.ChannelID.ToString(), PacketType.CloseChannel, Lidgren.Network.NetDeliveryMethod.ReliableUnordered, Map.Instance.Player.ID);
                // Send CloseChannelPacket
            }*/
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            /*
                        int X = (int)ParentChannel.ParentPanel.Position.X + (Width * Index);
            int Y = (int)ParentChannel.ParentPanel.Position.Y + Stroke + 1;
            if (Stroke > 0)
            {
                _spriteBatch.Draw(Storage.Instance.GetSpriteByName("UI_Black_Stroke"), new Rectangle((int)X, (int)Y, Width + (Stroke * 2), Height + (Stroke * 2)), Color.White);
            }
            _spriteBatch.Draw(BackgroundSprite, new Rectangle((int)X + Stroke, (int)Y + Stroke, Width, Height), Color.White);
            DrawOutlinedString(_spriteBatch, font, Text, new Vector2(Stroke + X + (Width / 2) - (font.MeasureString(Text).X / 2), Stroke + Y + (Height / 2) - (font.MeasureString(Text).Y / 2)), FocusColor);
            */
            if (Stroke > 0)
            {
                spriteBatch.Draw(Storage.Instance.GetSpriteByName("UI_Black_Stroke").Sprite, new Rectangle((int)OffsetPosition.X, (int)OffsetPosition.Y, Width, Height), Color.White);
            }
            spriteBatch.Draw(BackgroundSprite, new Rectangle((int)OffsetPosition.X + Stroke, (int)OffsetPosition.Y + Stroke, Width - (Stroke * 2), Height - (Stroke * 2)), Color.White);
            int i = 0;
            foreach (ChatChannel c in Channels)
            {
                c.Draw(spriteBatch, i, c.ChannelId == FocusedChannelId);
                i++;
                // Draw all channel tabs - (maybe add a horizontal scrollbar?
                // either way, make horizontal scrollbars a possibility, may need it for
                // future features, always good to have)
                // then draw currently focused channel
                // then draw text - wrap text before it reaches scrollbar
                // taking into account the currently scrolled amount
                // draw scrollbar
            }
        }

        public ChatChannel GetChannelById(int channelId)
        {
            foreach (ChatChannel c in Channels)
            {
                if (c.ChannelId == channelId)
                {
                    return c;
                }
            }
            return new ChatChannel();
        }

        public int GetChannelIndexById(int channelId)
        {
            for (int i = 0; i < Channels.Count; i++)
            {
                if (Channels[i].ChannelId == channelId)
                {
                    return i;
                }
            }
            return -1;
        }

        internal void DoEvents(MouseState state, Vector2 cursorPos)
        {
            int i = 0;
            foreach (ChatChannel c in Channels)
            {
                c.ChatTab.IsClicked(state, cursorPos, i);
                c.ChatScrollbar.IsClicked(state, cursorPos, (c.ChannelId == FocusedChannelId));
                i++;
            }
        }
    }
}
