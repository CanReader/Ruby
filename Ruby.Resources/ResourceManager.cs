using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ruby.Resources
{
    public static class ResourceManager
    {
        public static System.Drawing.Text.PrivateFontCollection GetFontFromResource(string FontName)
        {
            FontName = FontName + ".TTF";

            System.IO.Stream FontStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(FontName);

            IntPtr data = Marshal.AllocCoTaskMem((int)FontStream.Length);

            Byte[] FontByte = new byte[FontStream.Length];

            FontStream.Read(FontByte,0,(int)FontStream.Length);

            Marshal.Copy(FontByte,0,data,(int)FontStream.Length);

            System.Drawing.Text.PrivateFontCollection font = new System.Drawing.Text.PrivateFontCollection();
            font.AddMemoryFont(data,(int)FontStream.Length);

            FontStream.Close();

            return font;
        }
    }
}
