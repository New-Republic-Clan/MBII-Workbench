using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MB2_Workbench.Classes.Helpers
{
    public static class WindowFinder
    {

        public static List<T> FindOpenWindowsByType<T>()
        {
            var windows = Application.Current.Windows.OfType<T>();

            if (windows == null || windows.Count() == 0)
                return null;

            return (List<T>)windows;
        }
        public static T FindOpenWindowByType<T>()
        {
            var windows = Application.Current.Windows.OfType<T>();

            if (windows == null || windows.Count() == 0)
                return default(T);

            return (T)windows.FirstOrDefault();
        }

    }
}
