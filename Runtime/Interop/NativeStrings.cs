using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PlaytoliaSDK.Runtime
{
    /// <summary>
    /// String marshalling for the desktop native library (playtolia.dll), used by the
    /// <c>UNITY_STANDALONE_WIN</c> externs in the PlatformFunc classes.
    /// <para>
    /// On Windows, default <c>string</c> marshalling is the ANSI code page and
    /// <c>Marshal.FreeHGlobal</c> is <c>LocalFree</c>, neither of which matches the native side.
    /// So strings go in as NUL-terminated UTF-8 <c>byte[]</c> (supported by both Mono and IL2CPP),
    /// and strings returned by the library are decoded as UTF-8 and released with
    /// <c>playtolia_free</c>, which frees on the heap that allocated them.
    /// </para>
    /// </summary>
    internal static class NativeStrings
    {
        /// <summary>Base name of the desktop native library (playtolia.dll / libplaytolia.dylib).</summary>
        internal const string Library = "playtolia";

        /// <summary>NUL-terminated UTF-8 bytes of <paramref name="value"/>; null stays null (passed as NULL).</summary>
        internal static byte[] ToUtf8(string value)
        {
            if (value == null) return null;
            var bytes = new byte[Encoding.UTF8.GetByteCount(value) + 1];
            Encoding.UTF8.GetBytes(value, 0, value.Length, bytes, 0);
            return bytes; // last byte is already 0
        }

        /// <summary>
        /// Decodes a UTF-8 string returned by the desktop library and frees it with playtolia_free.
        /// Returns null for <see cref="IntPtr.Zero"/>. The pointer must not be used afterwards.
        /// </summary>
        internal static string TakeUtf8(IntPtr pointer)
        {
            if (pointer == IntPtr.Zero) return null;
            try
            {
                var length = 0;
                while (Marshal.ReadByte(pointer, length) != 0) length++;
                if (length == 0) return string.Empty;
                var bytes = new byte[length];
                Marshal.Copy(pointer, bytes, 0, length);
                return Encoding.UTF8.GetString(bytes);
            }
            finally
            {
                Free(pointer);
            }
        }

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        [DllImport(Library, CallingConvention = CallingConvention.Cdecl)] private static extern void playtolia_free(IntPtr pointer);

        private static void Free(IntPtr pointer) { playtolia_free(pointer); }
#else
        // Only the WindowsPlayer code paths hand pointers to TakeUtf8, and those only run in a
        // Windows player build, where the branch above is compiled instead.
        private static void Free(IntPtr pointer) { }
#endif
    }
}
