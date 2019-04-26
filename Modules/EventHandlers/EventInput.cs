using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RpgMapEditor.Modules.EventHandlers
{

    public class KeyboardLayout
    {
        const uint KlfActivate = 1; //activate the layout
        const int KlNamelength = 9; // length of the keyboard buffer
        const string LangEnUs = "00000409";
        const string LangHeIl = "0001101A";

        [DllImport("user32.dll")]
        private static extern long LoadKeyboardLayout(
              string pwszKlid,  // input locale identifier
              uint flags       // input locale identifier options
              );

        [DllImport("user32.dll")]
        private static extern long GetKeyboardLayoutName(
              System.Text.StringBuilder pwszKlid  //[out] string that receives the name of the locale identifier
              );

        public static string GetName()
        {
            System.Text.StringBuilder name = new System.Text.StringBuilder(KlNamelength);
            GetKeyboardLayoutName(name);
            return name.ToString();
        }
    }

    public class CharacterEventArgs : EventArgs
    {
        private readonly char _character;
        private readonly int _lParam;

        public CharacterEventArgs(char character, int lParam)
        {
            this._character = character;
            this._lParam = lParam;
        }

        public char Character
        {
            get { return _character; }
        }

        public int Param
        {
            get { return _lParam; }
        }

        public int RepeatCount
        {
            get { return _lParam & 0xffff; }
        }

        public bool ExtendedKey
        {
            get { return (_lParam & (1 << 24)) > 0; }
        }

        public bool AltPressed
        {
            get { return (_lParam & (1 << 29)) > 0; }
        }

        public bool PreviousState
        {
            get { return (_lParam & (1 << 30)) > 0; }
        }

        public bool TransitionState
        {
            get { return (_lParam & (1 << 31)) > 0; }
        }
    }

    public class KeyEventArgs : EventArgs
    {
        private Keys _keyCode;

        public KeyEventArgs(Keys keyCode)
        {
            this._keyCode = keyCode;
        }

        public Keys KeyCode
        {
            get { return _keyCode; }
        }
    }

    public delegate void CharEnteredHandler(object sender, CharacterEventArgs e);
    public delegate void KeyEventHandler(object sender, KeyEventArgs e);

    public static class EventInput
    {
        /// <summary>
        /// Event raised when a character has been entered.
        /// </summary>
        public static event CharEnteredHandler CharEntered;

        /// <summary>
        /// Event raised when a key has been pressed down. May fire multiple times due to keyboard repeat.
        /// </summary>
        public static event KeyEventHandler KeyDown;

        /// <summary>
        /// Event raised when a key has been released.
        /// </summary>
        public static event KeyEventHandler KeyUp;

        delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        static bool _initialized;
        static IntPtr _prevWndProc;
        static WndProc _hookProcDelegate;
        static IntPtr _hImc;

        //various Win32 constants that we need
        const int GwlWndproc = -4;
        const int WmKeydown = 0x100;
        const int WmKeyup = 0x101;
        const int WmChar = 0x102;
        const int WmImeSetcontext = 0x0281;
        const int WmInputlangchange = 0x51;
        const int WmGetdlgcode = 0x87;
        const int WmImeComposition = 0x10f;
        const int DlgcWantallkeys = 4;

        //Win32 functions that we're using
        [DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hImc);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


        /// <summary>
        /// Initialize the TextInput with the given GameWindow.
        /// </summary>
        /// <param name="window">The XNA window to which text input should be linked.</param>
        public static void Initialize(GameWindow window)
        {
            if (_initialized)
                throw new InvalidOperationException("TextInput.Initialize can only be called once!");

            _hookProcDelegate = new WndProc(HookProc);
            _prevWndProc = (IntPtr)SetWindowLong(window.Handle, GwlWndproc,
                (int)Marshal.GetFunctionPointerForDelegate(_hookProcDelegate));

            _hImc = ImmGetContext(window.Handle);
            _initialized = true;
        }

        static IntPtr HookProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            IntPtr returnCode = CallWindowProc(_prevWndProc, hWnd, msg, wParam, lParam);

            switch (msg)
            {
                case WmGetdlgcode:
                    returnCode = (IntPtr)(returnCode.ToInt32() | DlgcWantallkeys);
                    break;

                case WmKeydown:
                    if (KeyDown != null)
                        KeyDown(null, new KeyEventArgs((Keys)wParam));
                    break;

                case WmKeyup:
                    if (KeyUp != null)
                        KeyUp(null, new KeyEventArgs((Keys)wParam));
                    break;

                case WmChar:
                    if (CharEntered != null)
                        CharEntered(null, new CharacterEventArgs((char)wParam, lParam.ToInt32()));
                    break;

                case WmImeSetcontext:
                    if (wParam.ToInt32() == 1)
                        ImmAssociateContext(hWnd, _hImc);
                    break;

                case WmInputlangchange:
                    ImmAssociateContext(hWnd, _hImc);
                    returnCode = (IntPtr)1;
                    break;
            }

            return returnCode;
        }
    }
}
