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
 * @(#) StreamUtils.cs
 */
using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Utils
{
    /// <summary>
    /// Class to provide a number of utilities for dealing with character conversion and retrieval from files encompassing support for multiple encoding types
    /// </summary>
    public class StreamUtils
    {
        public const Int16 SizeofChar = 1;
        public const Int16 SizeofShort = 2;
        public const Int16 SizeofLong = 4;

		// Remember the encoding type for this stream
        private Encoding encoding;

		/// <summary>
		/// Constructor that takes a particular character encoding to use for the string conversions
		/// </summary>
		/// <param name="encoding">The encoding type to use for string encoding</param>
        public StreamUtils(Encoding encoding)
        {
            this.encoding = encoding;
        }

        #region Conversion Functions

        /// <summary>
        /// Convert a byte array to an ascii string
		/// <para>Note that we deal with it as if it is a C style string with a null terminating character at the end.</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="maxStringLen"></param>
        /// <returns></returns>
        public string ConvertToString(byte[] source, int maxStringLen)
        {
            char[] chars = ConvertToCharArray(source);
            // Kill it at a null character - old C style strings
            int length = chars.Length;
            for (int i = 0; i < chars.Length; ++i)
            {
                if (chars[i] == '\0')
                {
                    length = i;
                    break;
                }
            }
            if (length > maxStringLen)
            {
                length = maxStringLen;
            }
            string result = new string(chars, 0, length);
            return result;
        }

		/// <summary>
		/// Convert a string to bytes
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
        public byte[] ConvertToBytes(string source)
        {
            return encoding.GetBytes(source);
        }


		/// <summary>
		/// Convert a byte array to a char array using the current encoding
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
        public char[] ConvertToCharArray(byte[] source)
        {
            return encoding.GetChars(source);
        }
        #endregion

        #region Input Functions

		#region Get STRING
		public string GetString(Stream fs, int len)
        {
            return GetString(fs, len, len);
        }

        public string GetString(Stream fs, int extractLength, int maxLength)
        {
            return ConvertToString(GetBytes(fs, extractLength), maxLength);
        }
		#endregion

		#region Get CHAR
		public char GetChar(Stream fs)
        {
            byte[] array = GetBytes(fs, SizeofChar);
            char[] buf = ConvertToCharArray(array);
            return buf[0];
        }
		#endregion

		#region Get SINGLE BYTE
		public static byte GetByte(Stream fs)
        {
            byte[] array = GetBytes(fs, SizeofChar);
            return array[0];
        }

        public static byte[] GetByteArray(Stream fs, int arraySize)
        {
            byte[] array = GetByteArray<byte>(fs, arraySize);
            return array;
        }

        /**
         * Summary
         *      Return an array of a particular type represented as byte elements
         * Description
         *      Select an array of a particular type from the stream
         *      where the array is stored in the stream in individual bytes
         *      Handles the use of Enums appropriately
         *      
         */
        public static T[] GetByteArray<T>(Stream fs, int arraySize) where T : new()
        {
            T[] result = new T[arraySize];
            byte[] array = GetBytes(fs, arraySize);
            for (int i = 0; i < array.Length; ++i)
            {
                if (result[i] is Enum)
                {
                    result[i] = (T)Enum.Parse(typeof(T), array[i].ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    result[i] = (T)(object)array[i];
                }
            }
            return result;
        }
		#endregion

		#region Get TWO BYTES
		public static Int16 GetInt16(Stream fs)
		{
			byte[] array = GetBytes(fs, SizeofShort);
			return BitConverter.ToInt16(array, 0);
		}

		/// <summary>
		/// <para>Retrieve an enum value of two bytes from the stream.  </para>
		/// </summary>
		/// <typeparam name="T">The type of the enum</typeparam>
		/// <param name="fs">The stream to retrieve the bytes from</param>
		/// <returns>The enumeration retrieved from the stream</returns>
		public static T GetInt16<T>(Stream fs)
		{
			Int32 value = GetInt16(fs);
			T result = (T)Enum.Parse(typeof(T), value.ToString());
			return result;
		}

		public static UInt16 GetUInt16(Stream fs)
		{
			byte[] array = GetBytes(fs, SizeofShort);
			return BitConverter.ToUInt16(array, 0);
		}

		public static Int16[] GetInt16Array(Stream fs, int arraySize)
		{
			return GetInt16Array<Int16>(fs, arraySize);
		}

		public static UInt16[] GetUInt16Array(Stream fs, int arraySize)
		{
			return GetUInt16Array<ushort>(fs, arraySize);
		}

		/// <summary>
		/// <para>Return an array of a particular type represented as Int16 elements</para>
		/// <para>Select an array of a particular type from the stream where the array is stored in the stream in individual Int16 integers</para>
		/// <para>Handles the use of Enums appropriately</para>
		/// </summary>
		/// <typeparam name="T">The type of the object (either a Int16 or an enum)</typeparam>
		/// <param name="fs">The stream handle</param>
		/// <param name="arraySize">The size of the array to fetch</param>
		/// <returns>The array of the specified type as retrieved from the supplied stream</returns>
		public static T[] GetInt16Array<T>(Stream fs, int arraySize) where T : new()
		{
			T[] result = new T[arraySize];
			byte[] array = GetBytes(fs, arraySize * SizeofShort);
			for (int i = 0; i < arraySize; ++i)
			{
				object val = BitConverter.ToInt16(array, i * SizeofShort);
				if (result[i] is Enum)
				{
					result[i] = (T)Enum.Parse(typeof(T), val.ToString());
				}
				else
				{
					result[i] = (T)val;
				}
			}
			return result;
		}

		/// <summary>
		/// <para>Return an array of a particular type represented as Int16 elements</para>
		/// <para>Select an array of a particular type from the stream where the array is stored in the stream in individual Int16 integers</para>
		/// <para>Handles the use of Enums appropriately</para>
		/// </summary>
		/// <typeparam name="T">The type of the object (either a Int16 or an enum)</typeparam>
		/// <param name="fs">The stream handle</param>
		/// <param name="arraySize">The size of the array to fetch</param>
		/// <returns>The array of the specified type as retrieved from the supplied stream</returns>
		public static T[] GetUInt16Array<T>(Stream fs, int arraySize) where T : new()
		{
			T[] result = new T[arraySize];
			byte[] array = GetBytes(fs, arraySize * SizeofShort);
			for (int i = 0; i < arraySize; ++i)
			{
				object val = BitConverter.ToUInt16(array, i * SizeofShort);
				if (result[i] is Enum)
				{
					result[i] = (T)Enum.Parse(typeof(T), val.ToString());
				}
				else
				{
					result[i] = (T)val;
				}
			}
			return result;
		}
		#endregion

		#region Get FOUR BYTES
		public static Int32 GetInt32(Stream fs)
		{
			byte[] array = GetBytes(fs, SizeofLong);
			return BitConverter.ToInt32(array, 0);
		}

		public static UInt32 GetUInt32(Stream fs)
		{
			byte[] array = GetBytes(fs, SizeofLong);
			return BitConverter.ToUInt32(array, 0);
		}

		/// <summary>
		/// <para>Retrieve an enum value of four bytes from the stream.  </para>
		/// <para>NOTE: the top bit can never be used as it is not 
		/// supported by the Enum system</para>
		/// </summary>
		/// <typeparam name="T">The type of the enum</typeparam>
		/// <param name="fs">The stream to retrieve the bytes from</param>
		/// <returns>The enumeration retrieved from the stream</returns>
		public static T GetInt32<T>(Stream fs)
		{
			Int32 value = GetInt32(fs);
			T result = (T)Enum.Parse(typeof(T), value.ToString());
			return result;
		}

		public static Int32[] GetInt32Array(Stream fs, int arraySize)
        {
            Int32[] result = new Int32[arraySize];
            byte[] array = GetBytes(fs, arraySize * SizeofLong);
            for (int i = 0; i < arraySize; ++i)
            {
                result[i] = BitConverter.ToInt32( array, i * SizeofLong);
            }
            return result;
        }
		#endregion

		/// <summary>
		/// <para>Fetch the specified number of bytes from the stream</para>
		/// <para>If there are less than the required number of bytes to retrieve
		/// from the stream then the remaining bytes will be retrieved</para>
		/// <para>If no bytes are available an EndOfStreamException is thrown</para>
		/// </summary>
		/// <param name="fs">The stream to retrieve the bytes from</param>
		/// <param name="len">The number of bytes to retrieve</param>
		/// <returns>An array of bytes retrieved from the stream</returns>
        public static byte[] GetBytes(Stream fs, int len)
        {
            byte[] array = new byte[len];
            int readlen = fs.Read(array, 0, len);
			if (readlen != len)
			{
				if (readlen > 0)
				{
					byte[] result = new byte[readlen];
					Array.Copy(array, result, readlen);
					return result;
				}
				else
				{
					throw new EndOfStreamException();
				}
			}
            return array;
        }
        #endregion

        #region Output Functions

		#region Put STRING
		/// <summary>
		/// <para>Place a string of the specified Length onto the output stream.</para>
		/// <para>The Length output is the total number of bytes specified in the len parameter regardless of the Length of the string.  The bytes are padded with zero value bytes where the string is less than the required Length</para>
		/// </summary>
		/// <param name="fs">The stream to place the string on</param>
		/// <param name="value">The string to output</param>
		/// <param name="len">The Length of the field to output, padded with zero bytes if Length of the value is less than the required Length</param>
        public void PutString(Stream fs, string value, int len)
        {
            if (value == null)
                value = "";
			int arraylen = Math.Max(value.Length + 1, len + 1);
            byte[] array = new byte[arraylen];
            Array.Clear(array, 0, array.Length);
            byte[] valueAsArray = ConvertToBytes(value);
            Array.Copy(valueAsArray, 0, array, 0, valueAsArray.Length);
            PutBytes(fs, array, len);
        }
		#endregion

		#region Put SINGLE BYTE
		public static void PutByte(Stream fs, byte value)
        {
            byte[] array = BitConverter.GetBytes((char) value);
            PutBytes(fs, array, SizeofChar);
        }


        public static void PutByte(Stream fs, Enum value)
        {
            byte enumValue = (byte) value.GetEnumValue();
            PutByte(fs, enumValue);
        }

        public static void PutByteArray<T>(Stream fs, T[] source, int len)
        {
            byte[] array = new byte[len];
            for (int i = 0; i < len; ++i)
            {
                byte value;
                if (source == null)
                {
                    value = 0xA8;
                }
                else if (source[i] is Enum)
                {
                    value = Convert.ToByte(((Enum)(object)source[i]).GetEnumValue());
                }
                else
                {
                    value = Convert.ToByte(source[i], CultureInfo.InvariantCulture);
                }
                array[i] = value;
            }
            PutBytes(fs, array, len);
        }
		#endregion

		#region Put TWO BYTES
		public static void PutInt16(Stream fs, Int16 value)
		{
			byte[] array = BitConverter.GetBytes(value);
			PutBytes(fs, array, SizeofShort);
		}

		public static void PutUInt16(Stream fs, UInt16 value)
		{
			byte[] array = BitConverter.GetBytes(value);
			PutBytes(fs, array, SizeofShort);
		}

		public static void PutInt16(Stream fs, Enum value)
        {
            ushort enumVal = (ushort) value.GetEnumValue();
            PutUInt16(fs, enumVal);
        }

		public static void PutInt16Array<T>(Stream fs, T[] source, int len) where T : struct, IComparable, IFormattable, IConvertible
		{
			if (source == null)
				throw new ArgumentNullException("source", "Source Array not defined");

			byte[] array = new byte[len * SizeofShort];
			for (int i = 0; i < len; ++i)
			{
				if (!(source[i] is Enum))
					throw new ArgumentException("Source is not enum type", "source");

				Int16 value;
				Int32 enumVal = ((Enum)(object)source[i]).GetEnumValue();
				value = (Int16) enumVal;
				Array.Copy(BitConverter.GetBytes(value), 0, array, i * SizeofShort, SizeofShort);
			}
			PutBytes(fs, array, len * SizeofShort);
		}

		public static void PutInt16Array(Stream fs, Int16[] source, int len)
		{
			if (source == null)
				throw new ArgumentNullException("source", "Source Array not defined");

			byte[] array = new byte[len * SizeofShort];
			for (int i = 0; i < len; ++i)
			{
				Array.Copy(BitConverter.GetBytes(source[i]), 0, array, i * SizeofShort, SizeofShort);
			}
			PutBytes(fs, array, len * SizeofShort);
		}

		public static void PutUInt16Array(Stream fs, UInt16[] source, int len)
		{
			if (source == null)
				throw new ArgumentNullException("source", "Source Array not defined");

			byte[] array = new byte[len * SizeofShort];
			for (int i = 0; i < len; ++i)
			{
				Array.Copy(BitConverter.GetBytes(source[i]), 0, array, i * SizeofShort, SizeofShort);
			}
			PutBytes(fs, array, len * SizeofShort);
		}
		#endregion

		#region Put Int32
		public static void PutInt32(Stream fs, Int32 value)
		{
			byte[] array = BitConverter.GetBytes(value);
			PutBytes(fs, array, SizeofLong);
		}

		public static void PutUInt32(Stream fs, UInt32 value)
		{
			byte[] array = BitConverter.GetBytes(value);
			PutBytes(fs, array, SizeofLong);
		}

		/// <summary>
		/// Put an enum value out to the stream as 4 bytes
		/// of data.  NOTE: the top bit can never be
		/// used because the enum system does not support it
		/// </summary>
		/// <param name="fs">The stream to output the four bytes to</param>
		/// <param name="value">the enum value to output</param>
		public static void PutInt32(Stream fs, Enum value)
		{
			Int32 enumVal = value.GetEnumValue();
			PutInt32(fs, enumVal);
		}

		public static void PutInt32Array(Stream fs, Int32[] source, int len)
        {
            byte[] array = new byte[len * SizeofLong];
            if (source != null)
            {
                for (int i = 0; i < len; ++i)
                {
                    Array.Copy(BitConverter.GetBytes(source[i]), 0, array, i * SizeofLong, SizeofLong);
                }
            }
            PutBytes(fs, array, len * SizeofLong);
        }
		#endregion

		/// <summary>
		/// <para>Output an array of bytes to the specified stream</para>
		/// <para>Regardless of the Length of the array, the len number of bytes will be written</para>
		/// <para>If the array parameter is null and empty array will be output</para>
		/// <para>This method is utilised by all other put methods in this class to ensure consistency of delivery</para>
		/// </summary>
		/// <param name="fs">The stream to output the bytes to</param>
		/// <param name="array">The array to output.</param>
		/// <param name="len">The number of bytes to write to the stream</param>
        public static void PutBytes(Stream fs, byte[] array, int len)
        {
            if (array == null)
                array = new byte[len];
            fs.Write(array, 0, len);
        }
        #endregion

    }
}
