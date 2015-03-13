/**
 * Copyright (C) 2012 Bush Life Pty Limited
 * 
 * All rights reserved.  No unauthorised copying or redistribution without the prior written 
 * consent of the management of Bush Life Pty Limited.
 * 
 * www.bushlife.com.au
 * sales@bushlife.com.au
 * 
 * PO Box 865, Redcliffe, QLD, 4020, Australia
 * 
 * 
 * @(#) EnumUtils.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace AU.Com.BushLife.Utils
{
	/// <summary>
	/// A class of helper functions for working with enumerations
	/// </summary
	public static class EnumUtils
	{
		/// <summary>
		/// Get the real underlying value of the enum
		/// </summary>
		/// <param name="e">The enumeration to extract the value for</param>
		/// <returns>The integer representation of the enumerated value</returns>
		/// <exception cref="ArgumentNullException">The passed in value is null</exception>
		public static Int32 GetEnumValue(this Enum e)
		{
			if (e == null)
				throw new ArgumentNullException();
			object value = e.GetType().GetField("value__").GetValue(e);
			return (Int32)value;
		}

		/// <summary>
		/// Create an enum with the supplied value.
		/// <para>Attempts to parse the input value into a string using the ToString method, and then if possible creates an enum with the equivalent value</para>
		/// </summary>
		/// <typeparam name="Tout">The enumeration type</typeparam>
		/// <typeparam name="Tin">The input value</typeparam>
		/// <param name="value">The value to parse into a string</param>
		/// <returns>An enumerated type</returns>
		/// <exception cref="ArgumentNullException">The passed in value is null</exception>
		public static Tout SetEnumValue<Tout, Tin>(Tin value)
		{
			if (value == null)
				throw new ArgumentNullException();
			return (Tout) Enum.Parse(typeof(Tout), value.ToString());
		}

		/// <summary>
		/// Return a list of string values representing all of the items in the Enum
		/// </summary>
		/// <typeparam name="T">The type of the enum to extract the values for</typeparam>
		/// <returns>A list of strings representing each value in the Enum</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when type of T is not a System.Enum</exception>
        public static IEnumerable<string> GetEnumValues<T>()
            where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new InvalidOperationException("Generic type argument is not a System.Enum");
            
            foreach (T e in Enum.GetValues(typeof(T)))
			{
				yield return e.ToString();
			}
		}

		/// <summary>
		/// Return a list of values of all of the items in the Enum
		/// </summary>
		/// <typeparam name="T">The type of the enum to extract the values for</typeparam>
		/// <returns>A list of enums representing each value in the Enum</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when type of T is not a System.Enum</exception>
        public static IEnumerable<T> GetValues<T>() 
            where T : struct
        {
            if (!typeof(T).IsEnum) 
                throw new InvalidOperationException("Generic type argument is not a System.Enum");

            return Enum.GetValues(typeof(T)).OfType<T>();
        } 

		/// <summary>
		/// Get the name representation of the label for the enum
		/// </summary>
		/// <typeparam name="T">The type of the enum</typeparam>
		/// <param name="value">The enum to get the label for</param>
		/// <returns>The string representation of the enum label</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when type of T is not a System.Enum</exception>
        public static string GetName<T>(T value)
            where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new InvalidOperationException("Generic type argument is not a System.Enum");

            return Enum.GetName(typeof(T), value);
		}

		/// <summary>
		/// Get the name representation of the label for the enum
		/// </summary>
		/// <param name="type">The type of the enum</param>
		/// <param name="value">The enum to get the label for</param>
		/// <returns>The string representation of the enum label</returns>
		public static string GetName(Type type, Enum value)
		{
			return Enum.GetName(type, value);
		}

		/// <summary>
		/// Get the description label for the enum value.
		/// <para>[Description("A")] custom attribute on the enum value will return the value A</para>
        /// <para>Defaults to the name of the enum label if no description attribute available</para>
		/// </summary>
		/// <typeparam name="T">The type of the enum</typeparam>
		/// <param name="value">The value of the enum the description is required for</param>
		/// <returns>The custom attribute Description value</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when type of T is not a System.Enum</exception>
        public static string GetDescription<T>(this T value)
            where T : struct
		{
            if (!typeof(T).IsEnum)
                throw new InvalidOperationException("Generic type argument is not a System.Enum");

            try
            {
                return ((DescriptionAttribute)Attribute.GetCustomAttribute(
                    typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static)
                            .Single(x => ((T)x.GetValue(null)).Equals(value)),
                    typeof(DescriptionAttribute))).Description;
			}
			catch (NullReferenceException)
			{
				// This exception received when no Description attribute
				// associated with Enum members
				return GetName<T>(value);
			}
		}

		/// <summary>
		/// Get the enum value that matches the description text
		/// <para>Must match the text in a Description attribute associated with the enum value</para>
		/// </summary>
		/// <typeparam name="T">The type of the enum</typeparam>
		/// <param name="enumDescription">The enum description string</param>
		/// <param name="stringComparison">The string comparison mechanism to use - defaults to AsIs</param>
		/// <returns>The enum for the string</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">The enumDescription
		/// did not match any Description attributes on the enum</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when type of T is not a System.Enum</exception>
        public static T GetValueForDescription<T>(string enumDescription, StringComparison stringComparison = StringComparison.CurrentCulture)
            where T : struct
		{
            if (!typeof(T).IsEnum)
                throw new InvalidOperationException("Generic type argument is not a System.Enum");

            foreach (T t in GetValues<T>())
			{
				if (GetDescription<T>(t).Equals(enumDescription, stringComparison))
					return t;
			}
			throw new ArgumentOutOfRangeException(string.Format("{0} is invalid for {1}", enumDescription, typeof(T).Name));
		}

        /// <summary>
        /// Provide an iterator for iterating over all of the set
        /// bits in the enum value
        /// </summary>
        /// <param name="input">The enum value</param>
        /// <returns>The list of set flag bits</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when type of T is not a System.Enum</exception>
        public static IEnumerable<T> GetFlags<T>(this T input)
            where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new InvalidOperationException("Generic type argument is not a System.Enum");

            var inputEnum = input as Enum;
            foreach (var value in EnumUtils.GetValues<T>())
            {
                var valueEnum = value as Enum;
                if (inputEnum.HasFlag(valueEnum))
                    yield return value;
            }
        }
        
        /// <summary>
		/// Return the contents of the enumeration as formatted for a combo box
		/// relying on the Description attribute containing the display value
		/// within the enum definition
		/// </summary>
		/// <typeparam name="T">The type of the enum being retrieved</typeparam>
		/// <returns>The collection of enum values and description fields</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when type of T is not a System.Enum</exception>
        public static ICollection<ComboBoxLoader<T>> GetEnumComboBox<T>()
            where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new InvalidOperationException("Generic type argument is not a System.Enum");

            ICollection<ComboBoxLoader<T>> result = new List<ComboBoxLoader<T>>();
			foreach (T e in Enum.GetValues(typeof(T)))
			{
				ComboBoxLoader<T> value = new ComboBoxLoader<T>();
				value.Display = GetDescription(e);
				value.Value = e;
				result.Add(value);
			}
			return result;
		}

		/// <summary>
		/// Convert a list of enums that are represented in another form
		/// into the list of correct enum types
		/// </summary>
		/// <typeparam name="Tout">The output type (ie. IList&lt;YourEnumType>)</typeparam>
		/// <typeparam name="Tin">The type of the list element for the list to be converted</typeparam>
		/// <param name="reply">The list of input values to be converted to enums</param>
		/// <returns>The list of converted enums</returns>
		public static IList<Tout> Convert<Tout, Tin>(IList<Tin> reply)
		{
			MethodInfo setEnumValueMethod = typeof(EnumUtils).GetGenericMethod("SetEnumValue", new Type[] { typeof(Tin) });
			MethodInfo setEnumValueGenericMethod = setEnumValueMethod.MakeGenericMethod(typeof(Tout), typeof(Tin));
			IList<Tout> result = (dynamic)reply.Select(v => (Tout) (dynamic) setEnumValueGenericMethod.Invoke(null, new object[] { v })).ToList();
			return (dynamic) result;
		}



	}
}
