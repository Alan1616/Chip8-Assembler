using System;
using System.Collections.Generic;
using System.Text;

namespace DissAndAss.Disassemble
{
    internal class Opcode
    {
        public ushort FullCode { get; set; }
        public ushort FirstNibble { get => (ushort)(FullCode & 0xF000); }
        public byte X { get => (byte)((FullCode & 0x0F00) >> 8); }
        public byte Y { get => (byte)((FullCode & 0x00F0) >> 4); }
        public byte KK { get => (byte)((FullCode & 0x00FF)); }
        public byte N { get => (byte)((FullCode & 0x000F)); }
        public ushort NNN { get => (ushort)((FullCode & 0x0FFF)); }
        public Opcode(ushort opcode)
        {
            FullCode = opcode;
        }
    }
}
