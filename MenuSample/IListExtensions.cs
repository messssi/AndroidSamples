using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MenuSample
{

    public static class IListExtensions
    {
        /// <summary>
        /// 指定されたインデックスに要素が存在する場合に true を返します
        /// https://baba-s.hatenablog.com/entry/2015/04/01/104854
        /// </summary>
        public static bool IsDefinedAt<T>(this IList<T> self, int index)
        {
            return index < self.Count;
        }
    }
}