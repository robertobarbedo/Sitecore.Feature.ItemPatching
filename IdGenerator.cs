using Sitecore.Data;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Sitecore.Feature.ItemPatching
{
    public static class IdGenerator
    {
        public static ID GetPredictableId(string value)
        {
            var md5Hasher = MD5.Create();
            //do not change the salt hard coded - a change in these values will cause ids to change - this may break several dependencies
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes("7K3Bzgbc3PjeSkXUhY6Bl" + value + "DSHJKF2983hjUIg98"));
            return new ID(new Guid(data));
        }
    }
}
