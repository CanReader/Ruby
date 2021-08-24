using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruby.Resources
{
    public class RegistryManager
    {
        protected static readonly string RegPath = @"Software\RubySoft\RubyCafe\";

        public RegistryManager()
        {
            
        }

        public bool SaveRegistry(string Name,string Key,string Value)
        {
            try
            {
                Microsoft.Win32.RegistryKey _key;
                _key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RegPath + Name);

                _key.SetValue(Key, Value);

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
