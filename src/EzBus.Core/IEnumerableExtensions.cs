﻿using System;
using System.Collections.Generic;

namespace EzBus.Core
{
    public static class IEnumerableExtensions
    {
        public static void Apply<T>(this IEnumerable<T> source, Action<T> a)
        {
            foreach (var item in source)
            {
                a(item);
            }
        }
    }
}