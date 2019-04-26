using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgMapEditor.Modules.Objects;

namespace RpgMapEditor.Modules.Controls.NonStaticControls
{
    public class Scrollbar : Control
    {
        public Rectangle UpArrow;
        public Rectangle DownArrow;
        public Rectangle ScrolledPosition;
        public Rectangle ScrollPlace;


        public SpriteObject ScrollbarBar;
        public SpriteObject ScrollbarUp;
        public SpriteObject ScrollbarDown;

        public int Height;
        public int MaxHeight;
        public int Start;
        public int End;
        public int CurrentStep;
        public dynamic ToScroll;
        public ListBox ListScroll;
        public Panel PanelToScroll;
        public MouseState OldCursorPos;
        private ScrollDirection _scrolled = ScrollDirection.None;
        public int MaxSteps;

        private enum ScrollDirection
        {
            None,
            Up,
            Down,
            Drag
        }

        public Scrollbar()
        {
            OffsetPosition = new Vector2();
            UpArrow = new Rectangle();
            DownArrow = new Rectangle();
            ScrolledPosition = new Rectangle();
            Start = 0;
            End = 0;
            CurrentStep = 0;
            Height = 0;
            MaxHeight = 0;
            Id = -1;
        }

        /// <summary>
        /// Creates a new scrollbar
        /// </summary>
        /// <param name="id">The ID of the scrollbar</param>
        /// <param name="name">The name of the scrollbar</param>
        /// <param name="height">The height of the scrollbar when it is drawn</param>
        /// <param name="maxHeight">The total height that the scrollbar can scroll up or down</param>
        /// <param name="position">The position of the scrollbar</param>
        /// <param name="toScroll">The control that will be scrolled</param>
        public Scrollbar(int id, string name, int height, int maxHeight, Vector2 position, dynamic toScroll, EViewport parentViewport, SpriteObject scrollbarBar, SpriteObject scrollbarUp, SpriteObject scrollbarDown, Panel panelToScroll = null)
        {
            Id = id;
            Name = name;
            MaxHeight = maxHeight;
            Height = height;
            OffsetPosition = position;
            ToScroll = toScroll;
            ParentViewport = parentViewport;
            if (panelToScroll == null)
            {
                UpArrow = new Rectangle((int)OffsetPosition.X, (int)OffsetPosition.Y, 10, 10);
            }
            else
            {
                UpArrow = new Rectangle((int)OffsetPosition.X, (int)OffsetPosition.Y, 10, 10);
            }
            int plusHeight = 0;
            if (ToScroll == null && ListScroll == null)
            {
                plusHeight = (int)OffsetPosition.Y - panelToScroll.Stroke;
                //CurrentStep = (MaxHeight / 10) - (_PanelToScroll.Height / 10);
            }
            DownArrow = new Rectangle((int)OffsetPosition.X, Height + plusHeight - 9, 10, 10);
            int minusHeight = 0;
            if (ToScroll != null)
            {
                minusHeight = 40;
                MaxHeight = ToScroll.Height;
            }
            else if (ListScroll != null)
            {
                minusHeight = 40;
                MaxHeight = ListScroll.Height;
            }
            ScrolledPosition = new Rectangle((int)OffsetPosition.X, (int)OffsetPosition.Y, 10, Height - minusHeight);
            ScrollbarBar = scrollbarBar;
            ScrollbarUp = scrollbarUp;
            ScrollbarDown = scrollbarDown;
            CountClick = ClickState.Try;
            PanelToScroll = panelToScroll;
        }


        public void UpdateWidth(int width)
        {
            Width = width;
        }

        public void UpdateHeight(int height, int maxHeight)
        {
            Height = height;
            MaxHeight = maxHeight;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MaxSteps = Math.Abs(-((MaxHeight / 10) - (Height / 10) + 1));
            Vector2 position = GetCorrectPosition();


            UpArrow = new Rectangle((int)position.X - Width, (int)position.Y + 7, 8, 7);

            DownArrow = new Rectangle((int)position.X - Width, (int)position.Y + Height - 47, 8, 7);

            ScrolledPosition = new Rectangle((int)position.X - Width, (int)position.Y + 14, 8, Height - 61);
            // - ((Height - 61) / MaxSteps))
            //ScrollPlace = new Rectangle((int)Position.X - Width, (int)Position.Y + 14 + (Math.Abs(CurrentStep) * ((Height - 61) / MaxSteps)) - ((Height - 61) / MaxSteps), 8, ((Height - 61) / MaxSteps));

            Sprite.Draw(spriteBatch, ScrollbarBar, ScrolledPosition, Color.White);
            //Sprite.Draw(_spriteBatch, Scrollbar_Bar, ScrollPlace, Color.White);
            Sprite.Draw(spriteBatch, ScrollbarUp, UpArrow, Color.White);
            Sprite.Draw(spriteBatch, ScrollbarDown, DownArrow, Color.White);
            //_spriteBatch.Draw(Scrollbar_Down, DownArrow, Color.Red);
            // Draw scrollbar sprite first
            // then draw up and down arrow
            // taking the current scrolled amount into consideration (currentStep)
        }

        public bool IsClicked(MouseState state, Vector2 cursorPos, bool visible)
        {
            bool isClicked = false;
            if (ToScroll.Scrollable)
            {
                int minusX = this.ToScroll.ParentViewport.Viewport.X;
                int minusY = this.ToScroll.ParentViewport.Viewport.Y;

                if((UpArrow.Contains(cursorPos.X - minusX, cursorPos.Y - minusY)) || (DownArrow.Contains(cursorPos.X - minusX, cursorPos.Y - minusY)) || (ScrolledPosition.Contains(cursorPos.X - minusX, cursorPos.Y - minusY)) || _scrolled == ScrollDirection.Drag)
                {
                    isClicked = true;
                }

                if (_scrolled != ScrollDirection.Drag)
                {
                    if (CountClick == ClickState.Try && _scrolled == ScrollDirection.None && state.LeftButton == ButtonState.Pressed)
                    {
                        if (UpArrow.Contains(cursorPos.X - minusX, cursorPos.Y - minusY))
                        {
                            _scrolled = ScrollDirection.Up;
                        }
                        else if (DownArrow.Contains(cursorPos.X - minusX, cursorPos.Y - minusY))
                        {
                            _scrolled = ScrollDirection.Down;
                        }
                        else if (ScrolledPosition.Contains(cursorPos.X - minusX, cursorPos.Y - minusY))
                        {
                            _scrolled = ScrollDirection.Drag;
                            OldCursorPos = state;
                        }
                        if (_scrolled == ScrollDirection.None)
                        {
                            CountClick = ClickState.Dismiss;
                        }
                        else
                        {
                            CountClick = ClickState.Count;
                        }
                    }
                    else if (CountClick == ClickState.Count && _scrolled != ScrollDirection.None && state.LeftButton == ButtonState.Released)
                    {
                        if (_scrolled == ScrollDirection.Up)
                        {
                            _scrolled = ScrollDirection.None;
                            CountClick = ClickState.Try;
                            ScrollUp();
                        }
                        else if (_scrolled == ScrollDirection.Down)
                        {
                            _scrolled = ScrollDirection.None;
                            CountClick = ClickState.Try;
                            ScrollDown();
                        }
                    }
                    else if (CountClick == ClickState.Dismiss && state.LeftButton == ButtonState.Released)
                    {
                        CountClick = ClickState.Try;
                    }
                }
                else
                {
                    if (state.LeftButton != ButtonState.Released)
                    {
                        ScrollDrag(state);
                    }
                    else
                    {
                        _scrolled = ScrollDirection.None;
                        CountClick = ClickState.Try;
                    }
                }
                DoScrollWheelEvents(state, cursorPos, visible);
            }
            return isClicked;
        }

        private void DoScrollWheelEvents(MouseState state, Vector2 cursorPos, bool visible)
        {
            if (visible)
            {
                Viewport parentViewport;
                if (ToScroll != null)
                {
                    parentViewport = ToScroll.ParentViewport.Viewport;
                }
                else if(ListScroll != null)
                {
                    parentViewport = ListScroll.ParentViewport.Viewport;
                }
                else
                {
                    parentViewport = PanelToScroll.ParentViewport.Viewport;
                }

                if (parentViewport.Bounds.Contains(cursorPos.X, cursorPos.Y))
                {
                    if (state.ScrollWheelValue > OldCursorPos.ScrollWheelValue)
                    {
                        // Scroll up
                        int wheel = (state.ScrollWheelValue - OldCursorPos.ScrollWheelValue) / 120;
                        if (wheel == 1)
                        {
                            ScrollUp();
                            ScrollUp();
                        }
                        OldCursorPos = state;
                    }
                    else if (state.ScrollWheelValue < OldCursorPos.ScrollWheelValue)
                    {
                        // Scroll down
                        int wheel = (OldCursorPos.ScrollWheelValue - state.ScrollWheelValue) / 120;
                        if (wheel == 1)
                        {
                            ScrollDown();
                            ScrollDown();
                        }
                        OldCursorPos = state;
                    }
                }
            }
        }

        public void ScrollDrag(MouseState state)
        {
            if (MaxHeight > Height - 20)
            {
                int y = state.Y;

                if (y > OldCursorPos.Y)
                {
                    ScrollDown();
                    ScrollDown();
                    if (y != OldCursorPos.Y)
                        OldCursorPos = Mouse.GetState();
                }
                else if (y < OldCursorPos.Y)
                {
                    ScrollUp();
                    ScrollUp();
                    if (y != OldCursorPos.Y)
                        OldCursorPos = Mouse.GetState();
                }
            }
        }

        public void ScrollUp()
        {

            if (MaxHeight > Height - 20)
            {
                // Scroll up
                if (CurrentStep > -1)
                {
                    CurrentStep = 0;
                }
                else
                {
                    CurrentStep += 1;
                }
            }
        }

        public void ScrollDown()
        {
            if (MaxHeight > Height - 20)
            {
                // Scroll down

                //if (CurrentStep > -((MaxHeight / 10) - (ScrollHeight / 10)))
                if (CurrentStep <= -((MaxHeight / 10) - (Height / 10) + 1))
                {
                    CurrentStep = -((MaxHeight / 10) - (Height / 10) + 1);
                }
                else
                {
                    CurrentStep -= 1;
                }
            }
        }
    }
}
