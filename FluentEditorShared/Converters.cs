﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace FluentEditorShared
{
    public class ColorToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            if(value is Color c)
            {
                return c.ToString();
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            try
            {
                if (Color.TryParse(value as string, out var c))
                {
                    return c;
                }
                else
                {
                    return Colors.White;
                }
            }
            catch
            {
                return Colors.White;
            }
        }
    }

    public class NullableColorToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            if (value is Nullable<Color>)
            {
                var c = (Color?)value;
                if(c.HasValue)
                {
                    return c.Value.ToString();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            try
            {
                if (Color.TryParse(value as string, out var c))
                {
                    return c;
                }
                else
                {
                    return Colors.White;
                }
            }
            catch
            {
                return Colors.White;
            }
        }
    }
}
