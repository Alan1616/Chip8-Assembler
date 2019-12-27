using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DissAndAss.Assembly
{
    public class Operation
    {
        private ushort _freeDataMaxLength = 0;

        public Opcode Opcode { get; set; }
        public string Representation { get; set; }
        public byte OperandsCount 
        {
            get
            {
                byte count = 0;
                var properties = typeof(Operation).GetProperties();

                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(bool))
                    {
                        if ((bool) property.GetValue(this))
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
        }
        public bool HasSource { get; set; }
        public bool HasTarget { get; set; }
        public bool HasFreeData { get; set; }
        public ushort FreeDataMaxLength
        {
            get => HasFreeData? _freeDataMaxLength :  (byte)0; 
            set { _freeDataMaxLength = value; }
        }
        //public bool IsSpecialMnemonic { get => Mnemonic.Contains(" "); }
        public string Mnemonic { get; set; }

        public ushort BaseValue { get => ushort.Parse(Representation.Replace("x","0").Replace("y","0").Replace("kk","00").Replace("nnn","000").Replace("n","0"), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture); }
    }
}
