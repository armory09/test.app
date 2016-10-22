using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Me.DataLayer.Util
{
    public static class ClearCheckNull
    {
        public static void TextBox(DependencyObject obj)
        {
            TextBox tb = obj as TextBox;
            if(tb!= null)
            {
                tb.Text = "";
            }
            for(int i = 0; i<VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                TextBox(VisualTreeHelper.GetChild(obj, i));
            }
        }
        /// <summary>
        /// Check if T has data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }
    }
}
