using System;
using System.Text;

namespace XSigUtilityLibrary.Intersystem.Tokens
{
    public sealed class XSigSerialToken : XSigToken
    {
        private readonly string _value;

        public XSigSerialToken(int index, string value)
            : base(index)
        {
            // 10-bits available for serial encoded data
            if (index >= 1024 || index < 0)
                throw new ArgumentOutOfRangeException("index");
            
            _value = value;
        }

        public string Value {
            get { return _value; }
        }

        public override XSigTokenType TokenType {
            get { return XSigTokenType.Serial; }
        }

        public override byte[] GetBytes()
        {
            var serialBytes = Encoding.GetEncoding(28591).GetBytes(Value);
            var xsig = new byte[serialBytes.Length + 3];
            xsig[0] = (byte)(0xC8 | (Index >> 7));
            xsig[1] = (byte)((Index - 1) & 0x7F);
            xsig[xsig.Length - 1] = 0xFF;

            Buffer.BlockCopy(serialBytes, 0, xsig, 2, serialBytes.Length);
            return xsig;
        }

        public override XSigToken GetTokenWithOffset(int offset)
        {
            if (offset == 0) return base.GetTokenWithOffset(offset);
            return new XSigSerialToken(Index + offset, Value);
        }

        public override string ToString()
        {
            return "\"" + Value + "\"";
        }
    }
}