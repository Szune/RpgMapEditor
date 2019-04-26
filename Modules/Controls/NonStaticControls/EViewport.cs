using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RpgMapEditor.Modules.Utilities;

namespace RpgMapEditor.Modules.Controls.NonStaticControls
{
    public enum ClickedMouseButton
    {
        Left,
        Right,
        Both
    }
    public class EViewport
    {
        /* Extended Viewport */
        public Viewport Viewport;
        public Screen ParentScreen = new Screen();
        public int Id;
        public string Name;
        public bool AllowResize;
        public int MaxWidth;
        public int MaxHeight;
        public int MinHeight;
        private ClickedMouseButton _clickedButton;

        private bool _clicked = false;
        private Vector2 _lastMousePosition = Vector2.Zero;

        public delegate void ViewportEventHandler(object sender, ViewportEventArgs e);

        public event ViewportEventHandler Click;
        public event ViewportEventHandler MouseOver;
        public event ViewportEventHandler MouseDownMove;

        public float AspectRatio => (float)MaxWidth / (float)MaxHeight;

        public EViewport()
        {
            Id = -1;
        }

        public EViewport(int id, string name, int x, int y, int width, int height, bool allowResize = false, int minHeight = 0)
        {
            Id = id;
            Name = name;
            Viewport = new Viewport(x, y, width, height);
            AllowResize = allowResize;
            MinHeight = minHeight;
            MaxWidth = width;
            MaxHeight = height;
        }

        public bool HasParentScreen()
        {
            return (ParentScreen.Id != -1);
        }

        protected virtual void OnClick(ViewportEventArgs e)
        {
            var handler = Click;
            if (handler != null)
            {
                if (ParentScreen.Name == Storage.Instance.CurrentScreen.Name)
                {
                    handler(this, e);
                }
            }
        }

        protected virtual void OnMouseOver(ViewportEventArgs e)
        {
            var handler = MouseOver;
            handler?.Invoke(this, e);
        }

        protected virtual void OnMouseDownMove(ViewportEventArgs e)
        {
            var handler = MouseDownMove;
            handler?.Invoke(this, e);
        }

        public void IsClicked(MouseState state, Vector2 cursorPos)
        {
            if (ParentScreen.Visible)
            {
                if (!_clicked && state.LeftButton == ButtonState.Pressed || state.RightButton == ButtonState.Pressed)
                {
                    if (Viewport.Bounds.Contains(cursorPos.X, cursorPos.Y))
                    {
                        if (state.LeftButton == ButtonState.Pressed)
                        {
                            _clickedButton = ClickedMouseButton.Left;
                        }
                        else if (state.RightButton == ButtonState.Pressed)
                        {
                            _clickedButton = ClickedMouseButton.Right;
                        }
                        _clicked = true;
                    }
                }
                else if (_clicked)
                {
                    if (_clickedButton == ClickedMouseButton.Left && state.LeftButton == ButtonState.Released)
                    {
                        _clicked = false;
                    }
                    else if (_clickedButton == ClickedMouseButton.Right && state.RightButton == ButtonState.Released)
                    {
                        _clicked = false;
                    }
                    if (!_clicked)
                    {
                        bool inScreen = Viewport.Bounds.Contains(cursorPos.X, cursorPos.Y);
                        var args = new ViewportEventArgs(this, (int)((float)(cursorPos.X - this.Viewport.X) / ((float)this.Viewport.Width / (float)this.MaxWidth)), (int)(cursorPos.Y / ((float)this.Viewport.Height / (float)this.MaxHeight)), this._clickedButton, inScreen);
                        OnClick(args);
                    }
                }
                else if (!_clicked && state.LeftButton == ButtonState.Released && state.RightButton == ButtonState.Released)
                {
                    if (Viewport.Bounds.Contains(cursorPos.X, cursorPos.Y))
                    {
                        var args = new ViewportEventArgs(this, (int)((float)(cursorPos.X - this.Viewport.X) / ((float)this.Viewport.Width / (float)this.MaxWidth)), (int)(cursorPos.Y / ((float)this.Viewport.Height / (float)this.MaxHeight)), this._clickedButton, true);
                        OnMouseOver(args);
                    }
                }

                if (!cursorPos.Equals(_lastMousePosition) &&
                    Viewport.Bounds.Contains(cursorPos.X, cursorPos.Y) &&
                    state.LeftButton == ButtonState.Pressed)
                {
                    _lastMousePosition = new Vector2(cursorPos.X, cursorPos.Y);
                    var args = new ViewportEventArgs(this, (int)((float)(cursorPos.X - this.Viewport.X) / ((float)this.Viewport.Width / (float)this.MaxWidth)), (int)(cursorPos.Y / ((float)this.Viewport.Height / (float)this.MaxHeight)), this._clickedButton, true);
                    OnMouseDownMove(args);
                }
            }
        }
    }
    public class ViewportEventArgs : EventArgs
    {
        public ViewportEventArgs(EViewport eventViewport, int x, int y, ClickedMouseButton clickedButton, bool clickedInScreen)
        {
            EventViewport = eventViewport;
            ClickedX = x;
            ClickedY = y;
            ClickedButton = clickedButton;
            ClickedInScreen = clickedInScreen;
        }

        public EViewport EventViewport { get; }

        public int ClickedX { get; }

        public int ClickedY { get; }
        public bool ClickedInScreen { get; }

        public ClickedMouseButton ClickedButton { get; }
    }
}
