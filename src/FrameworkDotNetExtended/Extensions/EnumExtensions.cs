﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FrameworkDotNetExtended.Extensions
{

    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the textual description of the enum if it has one. e.g.
        /// 
        /// <code>
        /// enum UserColors
        /// {
        ///     [Description("Bright Red")]
        ///     BrightRed
        /// }
        /// UserColors.BrightRed.ToDescription();
        /// </code>
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string ToDescription(this Enum @enum)
        {
            Type type = @enum.GetType();

            MemberInfo[] memInfo = type.GetMember(@enum.ToString());

            string description = null;
            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
                else
                {
                    attrs = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                    if (attrs.Length > 0)
                    {
                        description = ((DisplayAttribute)attrs[0]).Name;
                    }
                }

            }
            return description ?? @enum.ToString();
        }
        public static String ToChar(this Enum @enum)
        {
            return System.Convert.ToChar(@enum).ToString();
        }

        public static List<string> ToList(this Enum @enum)
        {
            return new List<string>(Enum.GetNames(@enum.GetType()));
        }

        public static TypeCode GetTypeCode(this Enum @enum)
        {
            return Type.GetTypeCode(Enum.GetUnderlyingType(@enum.GetType()));
        }

        public static bool Has<T>(this Enum @enum, T value)
        {
            TypeCode typeCode = @enum.GetTypeCode();
            switch (typeCode)
            {
                case TypeCode.Byte:
                    return (((byte)(object)@enum & (byte)(object)value) == (byte)(object)value);
                case TypeCode.Int16:
                    return (((short)(object)@enum & (short)(object)value) == (short)(object)value);
                case TypeCode.Int32:
                    return (((int)(object)@enum & (int)(object)value) == (int)(object)value);
                case TypeCode.Int64:
                    return (((long)(object)@enum & (long)(object)value) == (long)(object)value);
                default:
                    throw new NotSupportedException(String.Format("Enums of type {0}", @enum.GetType().Name));
            }
        }

        public static bool Is<T>(this Enum @enum, T value)
        {
            TypeCode typeCode = @enum.GetTypeCode();
            switch (typeCode)
            {
                case TypeCode.Byte:
                    return (byte)(object)@enum == (byte)(object)value;
                case TypeCode.Int16:
                    return (short)(object)@enum == (short)(object)value;
                case TypeCode.Int32:
                    return (int)(object)@enum == (int)(object)value;
                case TypeCode.Int64:
                    return (long)(object)@enum == (long)(object)value;
                default:
                    throw new NotSupportedException(String.Format("Enums of type {0}", @enum.GetType().Name));
            }
        }

        public static T Add<T>(this Enum @enum, T value)
        {
            TypeCode typeCode = @enum.GetTypeCode();
            switch (typeCode)
            {
                case TypeCode.Byte:
                    return (T)(object)(((byte)(object)@enum | (byte)(object)value));
                case TypeCode.Int16:
                    return (T)(object)(((short)(object)@enum | (short)(object)value));
                case TypeCode.Int32:
                    return (T)(object)(((int)(object)@enum | (int)(object)value));
                case TypeCode.Int64:
                    return (T)(object)(((long)(object)@enum | (long)(object)value));
                default:
                    throw new NotSupportedException(String.Format("Enums of type {0}", @enum.GetType().Name));
            }
        }

        public static T Remove<T>(this Enum @enum, T value)
        {
            TypeCode typeCode = @enum.GetTypeCode();
            switch (typeCode)
            {
                case TypeCode.Byte:
                    return (T)(object)(((byte)(object)@enum & ~(byte)(object)value));
                case TypeCode.Int16:
                    return (T)(object)(((short)(object)@enum & ~(short)(object)value));
                case TypeCode.Int32:
                    return (T)(object)(((int)(object)@enum & ~(int)(object)value));
                case TypeCode.Int64:
                    return (T)(object)(((long)(object)@enum & ~(long)(object)value));
                default:
                    throw new NotSupportedException(String.Format("Enums of type {0}", @enum.GetType().Name));
            }
        }
    }
}
