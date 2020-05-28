using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Shell
{
    public class DragBehavior : Behavior<UIElement>
    {
        protected override void OnAttached()
        {
            Window parent = Application.Current.MainWindow;
            

            AssociatedObject.MouseLeftButtonDown += (sender, e) =>
            {
                parent?.DragMove();
            };
        }
    }
}
