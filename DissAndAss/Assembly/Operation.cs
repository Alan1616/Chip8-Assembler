using System;
using System.Collections.Generic;
using System.Text;

namespace DissAndAss.Assembly
{
    public class Operation
    {
        private byte _freeDataLength = 0;

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
        public byte FreeDataLength
        {
            get => HasFreeData?  _freeDataLength :  (byte)0; 
            set { _freeDataLength = value; }
        }
        //public bool IsSpecialMnemonic { get => Mnemonic.Contains(" "); }
        public string Mnemonic { get; set; }
    }
}
