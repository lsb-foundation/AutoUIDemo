using System.Collections.Generic;
using System.Windows;

namespace AutoUIDemo.UIAuto
{
    public interface IBuildControl
    {
        IEnumerable<DependencyObject> Build();
    }
}
