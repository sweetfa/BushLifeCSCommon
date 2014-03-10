using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

using AU.Com.BushLife.Utils;

namespace AU.Com.BushLife.Utils
{
	/// <summary>
	/// Unit Tests for the StreamUtils class
	/// </summary>
	[TestFixture]
	public class StreamUtilsTest
	{
		private IEnumerable<object[]> DataProviderByte
		{
			get
			{
				yield return new object[] { Encoding.ASCII, 0 };
				yield return new object[] { Encoding.ASCII, 'A' };
				yield return new object[] { Encoding.ASCII, 'a' };
				yield return new object[] { Encoding.ASCII, 0x21 };
				yield return new object[] { Encoding.ASCII, 0xC7 };
				yield return new object[] { Encoding.ASCII, 0xD8 };
				yield return new object[] { Encoding.ASCII, 0xFF };
				yield return new object[] { Encoding.ASCII, 0x7C };
				yield return new object[] { Encoding.UTF8, 0 };
				yield return new object[] { Encoding.UTF8, 'A' };
				yield return new object[] { Encoding.UTF8, 'a' };
				yield return new object[] { Encoding.UTF8, 0x21 };
				yield return new object[] { Encoding.UTF8, 0xC7 };
				yield return new object[] { Encoding.UTF8, 0xD8 };
				yield return new object[] { Encoding.UTF8, 0xFF };
				yield return new object[] { Encoding.UTF8, 0x7C };
				yield return new object[] { Encoding.GetEncoding(1252), 0 };
				yield return new object[] { Encoding.GetEncoding(1252), 'A' };
				yield return new object[] { Encoding.GetEncoding(1252), 'a' };
				yield return new object[] { Encoding.GetEncoding(1252), 0x21 };
				yield return new object[] { Encoding.GetEncoding(1252), 0xC7 };
				yield return new object[] { Encoding.GetEncoding(1252), 0xD8 };
				yield return new object[] { Encoding.GetEncoding(1252), 0xFF };
				yield return new object[] { Encoding.GetEncoding(1252), 0x7C };
			}
		}

		private IEnumerable<object[]> DataProviderShort
		{
			get
			{
				yield return new object[] { Encoding.ASCII, 0 };
				yield return new object[] { Encoding.ASCII, 'A' };
				yield return new object[] { Encoding.ASCII, 'a' };
				yield return new object[] { Encoding.ASCII, 0x21 };
				yield return new object[] { Encoding.ASCII, 0xC7 };
				yield return new object[] { Encoding.ASCII, 0xD8 };
				yield return new object[] { Encoding.ASCII, 0xFF };
				yield return new object[] { Encoding.ASCII, 0x7C };
				yield return new object[] { Encoding.ASCII, 0x2100 };
				yield return new object[] { Encoding.ASCII, 0xC7C7 };
				yield return new object[] { Encoding.ASCII, 0xD8D8 };
				yield return new object[] { Encoding.ASCII, 0xFFFF };
				yield return new object[] { Encoding.ASCII, 0x7CC7 };
				yield return new object[] { Encoding.UTF8, 0 };
				yield return new object[] { Encoding.UTF8, 'A' };
				yield return new object[] { Encoding.UTF8, 'a' };
				yield return new object[] { Encoding.UTF8, 0x21 };
				yield return new object[] { Encoding.UTF8, 0xC7 };
				yield return new object[] { Encoding.UTF8, 0xD8 };
				yield return new object[] { Encoding.UTF8, 0xFF };
				yield return new object[] { Encoding.UTF8, 0x7C };
				yield return new object[] { Encoding.UTF8, 0x2100 };
				yield return new object[] { Encoding.UTF8, 0xC7C7 };
				yield return new object[] { Encoding.UTF8, 0xD8D8 };
				yield return new object[] { Encoding.UTF8, 0xFFFF };
				yield return new object[] { Encoding.UTF8, 0x7CC7 };
				yield return new object[] { Encoding.GetEncoding(1252), 0 };
				yield return new object[] { Encoding.GetEncoding(1252), 'A' };
				yield return new object[] { Encoding.GetEncoding(1252), 'a' };
				yield return new object[] { Encoding.GetEncoding(1252), 0x21 };
				yield return new object[] { Encoding.GetEncoding(1252), 0xC7 };
				yield return new object[] { Encoding.GetEncoding(1252), 0xD8 };
				yield return new object[] { Encoding.GetEncoding(1252), 0xFF };
				yield return new object[] { Encoding.GetEncoding(1252), 0x7C };
				yield return new object[] { Encoding.GetEncoding(1252), 0x2100 };
				yield return new object[] { Encoding.GetEncoding(1252), 0xC7C7 };
				yield return new object[] { Encoding.GetEncoding(1252), 0xD8D8 };
				yield return new object[] { Encoding.GetEncoding(1252), 0xFFFF };
				yield return new object[] { Encoding.GetEncoding(1252), 0x7CC7 };
			}
		}

		[Test, Factory("DataProviderByte")]
		public void TestPutByteMemory(Encoding encoding, byte value)
		{
			Stream st = new MemoryStream();
			StreamUtils.PutByte(st, value);
			st.Flush();
			st.Seek(0, SeekOrigin.Begin);
			Assert.AreEqual(value, StreamUtils.GetByte(st));
		}

		[Test, Factory("DataProviderByte")]
		public void TestPutByteFile(Encoding encoding, byte value)
		{
			string filepath = Path.GetTempFileName();
			// Cleanup
			File.Delete(filepath);

			Stream st = new FileStream(filepath,FileMode.CreateNew);
			StreamUtils.PutByte(st, value);
			st.Flush();
			st.Close();
			st.Dispose();

			// Read it back to make sure it got rit (:)) correctly
			st = new FileStream(filepath, FileMode.Open);
			Assert.AreEqual(value, StreamUtils.GetByte(st));
			st.Close();
			st.Dispose();

			// Cleanup
			File.Delete(filepath);
		}

		[Test, Factory("DataProviderShort")]
		public void TestPutShortMemory(Encoding encoding, UInt16 value)
		{
			Stream st = new MemoryStream();
			StreamUtils.PutUInt16(st, value);
			st.Flush();
			st.Seek(0, SeekOrigin.Begin);
			Assert.AreEqual(value, (UInt16)StreamUtils.GetInt16(st));
		}

		[Test]
		public void TestFileIO()
		{
			bool directoryCreated = false;
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.FileName = @"C:\Temp\TestFile.hex";
			string directoryPath = Path.GetDirectoryName(dialog.FileName);
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
				directoryCreated = true;
			}
			using (Stream fs = dialog.OpenFile())
			{
				UInt16 value = 0xC7C7;
				fs.Write(BitConverter.GetBytes(value), 0, 2);
			}
			if (directoryCreated)
				Directory.Delete(directoryPath, true);
		}

	}
}
