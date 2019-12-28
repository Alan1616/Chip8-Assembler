using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DissAndAss.Assembly
{
    public class OperationDefinition
    {
        private ushort _freeDataMaxLength = 0;
        public string Representation { get; set; }
        public bool HasSource { get; set; }
        public bool HasTarget { get; set; }
        public bool HasFreeData { get; set; }
        public ushort FreeDataMaxLength
        {
            get => HasFreeData ? _freeDataMaxLength : (byte)0;
            set { _freeDataMaxLength = value; }
        }
        public string Mnemonic { get; set; }
        public List<TokenType> AssocietedTokenSet { get; set; }
        public ushort BaseValue { get => ushort.Parse(Representation.Replace("x","0").Replace("y","0").Replace("kk","00").Replace("nnn","000").Replace("n","0"), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture); }
    }
}
