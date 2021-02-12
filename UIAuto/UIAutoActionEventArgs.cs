using System;

namespace AutoUIDemo.UIAuto
{
    public sealed class UIAutoActionEventArgs : EventArgs
    {
        public ActionElement Action { get; }

        public UIAutoActionEventArgs(ActionElement action) : base()
        {
            Action = action;
        }
    }
}
