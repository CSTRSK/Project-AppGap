using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Project_AppGap.Helper
{
    public class AppSettings
    {
        public static ApplicationDataContainer Setting = ApplicationData.Current.LocalSettings;
    }
}
