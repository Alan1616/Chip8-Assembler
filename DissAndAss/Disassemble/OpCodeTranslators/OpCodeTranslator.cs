using System;
using System.Collections.Generic;
using System.Text;

namespace DissAndAss.Disassemble.OpCodeTranslators
{
    internal class OpCodeTranslator : IOpCodeTranslator
    {
        public  Dictionary<ushort, Func<Opcode, string>> availableOpcodes { get; }

        public OpCodeTranslator()
        {
            availableOpcodes = new Dictionary<ushort, Func<Opcode, string>>
            {
                {0x0000, StartDecodes },
                {0x1000, DecodeJP },
                {0x2000, DecodeCALL },
                {0x3000, DecodeSE_Vx },
                {0x4000, DecodeSNE_Vx },
                {0x5000, DecodeSE_Vx_Vy },
                {0x6000, DecodeLD_Vx },
                {0x7000, DecodeADD_Vx },
                {0x8000, ArithmeticDecodes },
                {0x9000, DecodeSNE_Vx_Vy },
                {0xA000, DecodeLD_I },
                {0xB000, DecodeJP_V0 },
                {0xC000, DecodeRND_Vx },
                {0xD000, DecodeDRW_Vx_Vy},
                {0xE000, KeyboardDecodes },
                {0xF000, LoadsDecodes },
            };
        }

        public string TranslateOpcode(Opcode opcode)
        {
            if (availableOpcodes.ContainsKey(opcode.FirstNibble))
            {
                return availableOpcodes[opcode.FirstNibble](opcode);
            }

            return "";
        }

        private string StartDecodes(Opcode opcode)
        {
            if (opcode.FullCode == 0x00E0)
            {
                return "CLS";
            }
            else if (opcode.FullCode == 0x00EE)
            {
                return "RET";
            }
            else
            {
                return "";
            }

        }

        private string DecodeJP(Opcode opcode)
        {
            return $"JP {opcode.NNN:X3}";
        }

        private string DecodeCALL(Opcode opcode)
        {
            return $"CALL {opcode.NNN:X3}";
        }

        private string DecodeSE_Vx(Opcode opcode)
        {
            return $"SE V{opcode.X:X1}, {opcode.KK:X2}";
        }

        private string DecodeSNE_Vx(Opcode opcode)
        {
            return $"SNE V{opcode.X:X1}, {opcode.KK:X2}";
        }

        private string DecodeSE_Vx_Vy(Opcode opcode)
        {
            return $"SN V{opcode.X:X1}, V{opcode.Y:X1}";
        }

        private string DecodeLD_Vx(Opcode opcode)
        {
            return $"LD V{opcode.X:X1}, {opcode.KK:X2}";
        }

        private string DecodeADD_Vx(Opcode opcode)
        {
            return $"ADD V{opcode.X:X1}, {opcode.KK:X2}";
        }

        private string DecodeSNE_Vx_Vy(Opcode opcode)
        {
            return $"SNE V{opcode.X:X1}, V{opcode.Y:X1}";
        }

        private string DecodeLD_I(Opcode opcode)
        {
            return $"LD I, {opcode.NNN:X3}";
        }

        private string DecodeJP_V0(Opcode opcode)
        {
            return $"JP V0, {opcode.NNN:X3}";
        }

        private string DecodeRND_Vx(Opcode opcode)
        {
            return $"RND V{opcode.X:X1}, {opcode.KK:X2}";
        }

        private string DecodeDRW_Vx_Vy(Opcode opcode)
        {
            return $"DRW V{opcode.X:X1}, V{opcode.Y:X1}, {opcode.N:X1}";
        }

        private string ArithmeticDecodes(Opcode opcode)
        {
            switch (opcode.N)
            {
                case 0x1:
                    return $"OR V{opcode.X:X1}, V{opcode.Y:X1}";
                case 0x2:
                    return $"AND V{opcode.X:X1}, V{opcode.Y:X1}";
                case 0x3:
                    return $"XOR V{opcode.X:X1}, V{opcode.Y:X1}";
                case 0x4:
                    return $"ADD V{opcode.X:X1}, V{opcode.Y:X1}";
                case 0x5:
                    return $"SUB V{opcode.X:X1}, V{opcode.Y:X1}";
                case 0x6:
                    return $"SHR V{opcode.X:X1}";
                case 0x7:
                    return $"SUBN V{opcode.X:X1}, V{opcode.Y:X1}";
                case 0xE:
                    return $"SHL V{opcode.X:X1}";

                default:
                    return "";
            }
        }

        private string KeyboardDecodes(Opcode opcode)
        {
            if (opcode.KK == 0x9E)
            {
                return $"SKP V{opcode.X:X1}";
            }
            else if (opcode.KK == 0xA1)
            {
                return $"SKNP V{opcode.X:X1}";
            }
            else
            {
                return "";
            }
        }

        private string LoadsDecodes(Opcode opcode)
        {
            switch (opcode.KK)
            {
                case 0x07:
                    return $"LD V{opcode.X:X1}, DT";
                case 0x0A:
                    return $"LD V{opcode.X:X1}, K";
                case 0x15:
                    return $"LD DT, V{opcode.X:X1}";
                case 0x18:
                    return $"LD ST, V{opcode.X:X1}";
                case 0x1E:
                    return $"ADD I, V{opcode.X:X1}";
                case 0x29:
                    return $"LD (F), V{opcode.X:X1}";
                case 0x33:
                    return $"LD (B), V{opcode.Y:X1}";
                case 0x55:
                    return $"LD [I], V{opcode.X:X1}";
                case 0x65:
                    return $"LD V{opcode.X:X1}, [I]";

                default:
                    return "";
            }
        }


    }
}
