using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgMapEditor.Modules.Controls;
using RpgMapEditor.Modules.Controls.NonStaticControls;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Objects
{
    public class ChatChannel : Control
    {
        public int ChannelId;
        public List<ChatMessage> Messages = new List<ChatMessage>();
        public Tab ChatTab;
        public ChatPanel ParentChatPanel;
        public Scrollbar ChatScrollbar;
        public bool CanClose;
        public SpriteFont Font;

        public ChatChannel()
        {
            ChannelId = -1;
        }

        public ChatChannel(int channelId, string name, int width, int height, Texture2D backgroundSprite, ChatPanel parentPanel, bool canClose, SpriteFont font)
        {
            Font = font;
            ChannelId = channelId;
            ChatTab = new Tab(name, width, height, this, backgroundSprite);
            ParentChatPanel = parentPanel;
            CanClose = canClose;
            ParentViewport = ParentChatPanel.ParentViewport;
            int offsetY = ChatTab.Height + Tab.Stroke + 1;
            Height = ParentChatPanel.Height - offsetY;
            OffsetPosition = new Vector2(ParentChatPanel.ParentViewport.Viewport.X + ParentChatPanel.Stroke + 1, ParentChatPanel.ParentViewport.Viewport.Y + offsetY);
            //ChatScrollbar = new Scrollbar(1, "ChatScrollbar", ParentChatPanel.Height - ParentChatPanel.Stroke, ParentChatPanel.Height - ParentChatPanel.Stroke, new Vector2(ParentChatPanel.Position.X + ParentChatPanel.Width - 10 - ParentChatPanel.Stroke, ParentChatPanel.Position.Y + ParentChatPanel.Stroke + _Height + Tab.Stroke + 1), this, ParentChatPanel.ParentViewport, Storage.Instance.GetSpriteByName("UI_Scrollbar"), Storage.Instance.GetSpriteByName("UI_Arrow_Up"), Storage.Instance.GetSpriteByName("UI_Arrow_Down"));
        }

        public void Draw(SpriteBatch spriteBatch, int index, bool focused)
        {
            ChatTab.Draw(spriteBatch, Font, index, focused); // Draw ChatTab first
            if (focused)
            {
                int y = 0; // This variable will keep track of which line we are on
                int offsetY = (ChatScrollbar.CurrentStep * 10); // We want to draw the text beneath the tabs
                int offsetYClip = ChatTab.Height + Tab.Stroke + 1; // We want to draw the text beneath the tabs
                //int OffsetY = (y * (OpenBackpacks[i].Height + Coordinates.Step) - OpenBackpacks[i].Scroll.CurrentStep * 10);

                RasterizerState r = new RasterizerState();
                r.ScissorTestEnable = true;

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, r);

                Rectangle scrollbarRectangle = new Rectangle() { X = (int)ParentChatPanel.ParentViewport.Viewport.X, Y = ParentChatPanel.ParentViewport.Viewport.Y + offsetYClip, Height = ParentChatPanel.Height - offsetYClip, Width = ParentChatPanel.Width - 10 };

                spriteBatch.GraphicsDevice.ScissorRectangle = scrollbarRectangle; // Make sure we're not drawing over the tabs

                int maxC = Messages.Count - 1;
                for (int i = maxC; i > -1; i--)
                {
                    ChatMessage cm = Messages[i];
                    string text = WordWrap.WrapText(Font, cm.Text, ParentChatPanel.Width - 40);
                    if (i != maxC)
                    {
                        if (text.IndexOf("\n") > -1)
                        {
                            y += cm.LineCount + 1;
                            DrawOutlinedString(spriteBatch, cm.ChatPlayer.Name + ": " + text, new Vector2(ParentChatPanel.OffsetPosition.X + ParentChatPanel.Stroke + 1, ParentChatPanel.Height - 5 - (y * 16) + offsetY), Color.Yellow);
                        }
                        else
                        {
                            y++;
                            DrawOutlinedString(spriteBatch, cm.ChatPlayer.Name + ": " + text, new Vector2(ParentChatPanel.OffsetPosition.X + ParentChatPanel.Stroke + 1, ParentChatPanel.Height - 5 - (y * 16) + offsetY), Color.Yellow);
                        }
                    }
                    else
                    {
                        if (text.IndexOf("\n") > -1)
                        {
                            y += cm.LineCount + 1;
                            DrawOutlinedString(spriteBatch, cm.ChatPlayer.Name + ": " + text, new Vector2(ParentChatPanel.OffsetPosition.X + ParentChatPanel.Stroke + 1, ParentChatPanel.Height - 5 - (y * 16) + offsetY), Color.Yellow);
                        }
                        else
                        {
                            y++;
                            DrawOutlinedString(spriteBatch, cm.ChatPlayer.Name + ": " + text, new Vector2(ParentChatPanel.OffsetPosition.X + ParentChatPanel.Stroke + 1, ParentChatPanel.Height - 5 - (y * 16) + offsetY), Color.Yellow);
                        }
                    }
                }
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

                //ChatScrollbar.Draw(_spriteBatch, (y * 16) + 8);
            }
        }

        public void DrawOutlinedString(SpriteBatch spriteBatch, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(Font, text, new Vector2(position.X - 1, position.Y), Color.Black);
            spriteBatch.DrawString(Font, text, new Vector2(position.X + 1, position.Y), Color.Black);
            spriteBatch.DrawString(Font, text, new Vector2(position.X, position.Y + 1), Color.Black);
            spriteBatch.DrawString(Font, text, new Vector2(position.X, position.Y - 1), Color.Black);
            spriteBatch.DrawString(Font, text, position, color);
        }

        internal void AddMessage(ChatMessage chatMessage)
        {
            ChatMessage cm = new ChatMessage(chatMessage.Id, chatMessage.ChatPlayer, chatMessage.Text, Font, ParentChatPanel.Width - 10);
            Messages.Add(cm);
            if (Messages.Count > 250)
            {
                Messages.RemoveAt(0);
            }
        }

        internal void ClearMessages()
        {
            Messages.Clear();
        }
    }
}
