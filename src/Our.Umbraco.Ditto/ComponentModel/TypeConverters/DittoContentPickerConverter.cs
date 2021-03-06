﻿namespace Our.Umbraco.Ditto
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;

    using global::Umbraco.Core.Models;

    /// <summary>
    /// Provides a unified way of converting content picker properties to strong typed model.
    /// </summary>
    public class DittoContentPickerConverter : DittoConverter
    {
        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            // We can pass null here.
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (sourceType == null
                || sourceType == typeof(string)
                || sourceType == typeof(int)
                || typeof(IPublishedContent).IsAssignableFrom(sourceType))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that represents the converted value.
        /// </returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (value.IsNullOrEmptyString() || context == null || context.PropertyDescriptor == null)
            {
                return null;
            }

            var propertyType = context.PropertyDescriptor.PropertyType;
            var isGenericType = propertyType.IsGenericType;
            var targetType = isGenericType
                                ? propertyType.GenericTypeArguments.First()
                                : propertyType;

            // DictionaryPublishedContent 
            IPublishedContent content = value as IPublishedContent;
            if (content != null)
            {
                return content.As(targetType, culture);
            }

            if (value is int)
            {
                return this.ConvertContentFromInt((int)value, targetType, culture);
            }

            int id;
            var s = value as string;
            if (s != null && int.TryParse(s, NumberStyles.Any, culture, out id))
            {
                return this.ConvertContentFromInt(id, targetType, culture);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}