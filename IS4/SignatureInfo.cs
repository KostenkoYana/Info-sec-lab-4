using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IS4
{
    [Serializable]
    public class SignatureInfo
    {
        public byte[] signature;
        public List<byte[]> parameters;
        public byte[] data;
    }
}
