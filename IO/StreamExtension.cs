using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace FalcomPACTool
{
    public static partial class StreamExtension
    {
        public static void FillAlignedSpace(this Stream stream, int align)
        {
            long pos = stream.Position;
            if (pos % align != 0)
                stream.Write(new byte[align - pos % align]);
        }
        public static void AlignPosition(this Stream stream, int align)
        {
            long pos = stream.Position;
            if (pos % align != 0)
                stream.Position = (align - pos % align) + pos;
        }
        public static void WriteMonoString(this Stream stream, string str, bool align=true)
        {
            byte[] strBytes = Encoding.UTF8.GetBytes(str);
            stream.WriteS32(strBytes.Length);
            stream.Write(strBytes);
            if (align)
                stream.AlignPosition(4);
        }
        public static string ReadMonoString(this Stream stream)
        {
            uint len = stream.ReadU32();
            string result = Encoding.UTF8.GetString(stream.ReadBytes(len));
            stream.AlignPosition(4);
            return result;
        }

        public static string ReadCString(this Stream stream, Encoding encoding, ulong position, char end = '\0')
        {
            var save = stream.Position;
            stream.Position = (long)position;
            string result = stream.ReadCString(encoding, end);
            stream.Position = save;
            return result;
        }
        public static string ReadCString(this Stream stream, Encoding encoding, char end = '\0')
        {
            int characterSize = encoding.GetByteCount("e");
            Debug.Assert(characterSize == 1 || characterSize == 2 || characterSize == 4);
            string characterEnd = end.ToString(CultureInfo.InvariantCulture);

            int i = 0;
            var data = new byte[128 * characterSize];

            while (true)
            {
                if (i + characterSize > data.Length)
                {
                    Array.Resize(ref data, data.Length + (128 * characterSize));
                }

                int read = stream.Read(data, i, characterSize);
                Debug.Assert(read == characterSize);

                if (encoding.GetString(data, i, characterSize) == characterEnd)
                {
                    break;
                }

                i += characterSize;
            }

            if (i == 0)
            {
                return "";
            }

            return encoding.GetString(data, 0, i);
        }

        public static byte[] ReadBytes(this Stream stream, uint size)
        {
            byte[] data = new byte[size];
            if (stream.Read(data, 0, (int)size) != size)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return data;
        }

        public static byte[] ReadBytes(this Stream stream, uint offset, uint count, bool returnToInitialPosition = true)
        {
            long save = stream.Position;
            stream.Position = offset;
            byte[] data = new byte[count];
            if(stream.Read(data, 0, (int)count) != count)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if(returnToInitialPosition)
            {
                stream.Position = save;
            }
            return data;
        }
        public static byte ReadU8(this Stream stream)
        {
            byte[] buffer = new byte[1];
            if(stream.Read(buffer, 0, 1) != 1)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return buffer[0];
        }

        public static ushort ReadU16(this Stream stream)
        {
            byte[] buffer = new byte[2];
            if (stream.Read(buffer, 0, 2) != 2)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return BitConverter.ToUInt16(buffer, 0);
        }

        public static int ReadS32(this Stream stream)
        {
            return (int)stream.ReadU32();
        }

        public static uint ReadU32(this Stream stream)
        {
            byte[] buffer = new byte[4];
            if (stream.Read(buffer, 0, 4) != 4)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return BitConverter.ToUInt32(buffer, 0);
        }

        public static long ReadS64(this Stream stream)
        {
            return (long)stream.ReadU64();
        }

        public static ulong ReadU64(this Stream stream)
        {
            byte[] buffer = new byte[8];
            if (stream.Read(buffer, 0, 8) != 8)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return BitConverter.ToUInt64(buffer, 0);
        }
        public static bool ReadBoolean(this Stream stream, bool align = true)
        {
            bool result = stream.ReadByte() == 1 ? true : false;
            if (align) stream.AlignPosition(4);
            return result;
        }
        public static float ReadSingle(this Stream stream)
        {
            byte[] buffer = new byte[4];
            if (stream.Read(buffer, 0, 4) != 4)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return BitConverter.ToSingle(buffer, 0);
        }
        public static double ReadDouble(this Stream stream)
        {
            byte[] buffer = new byte[8];
            if (stream.Read(buffer, 0, 8) != 8)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return BitConverter.ToDouble(buffer, 0);
        }
        public static void WriteU16(this Stream stream, ushort var)
        {
            stream.Write(BitConverter.GetBytes(var));
        }
        public static void WriteS16(this Stream stream, short var)
        {
            stream.Write(BitConverter.GetBytes(var));
        }
        public static void WriteU32(this Stream stream, uint var)
        {
            stream.Write(BitConverter.GetBytes(var));
        }
        public static void WriteS32(this Stream stream, int var)
        {
            stream.Write(BitConverter.GetBytes(var));
        }
        public static void WriteU64(this Stream stream, ulong var)
        {
            stream.Write(BitConverter.GetBytes(var));
        }
        public static void WriteS64(this Stream stream, long var)
        {
            stream.Write(BitConverter.GetBytes(var));
        }
        public static void WriteBoolean(this Stream stream, bool var, bool align = true)
        {
            stream.Write(BitConverter.GetBytes(var));
            if (align) stream.AlignPosition(4);
        }
        public static void WriteSingle(this Stream stream, float var)
        {
            stream.Write(BitConverter.GetBytes(var));
        }
        public static void WriteDouble(this Stream stream, double var)
        {
            stream.Write(BitConverter.GetBytes(var));
        }
    }
}
