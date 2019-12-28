using System;
using System.Collections.Generic;
using System.Text;

namespace DissAndAss.Assembly
{
    public enum TokenType
    {
        Invalid,
        Space,
        Comma,
        HeximalData,
        Mnemonic,
        GenericRegister,
        IRgeister,
        ST,
        DT,
        K,
        F,
        B,
        IRange,
        Comment,
        SequenceEnd,
    }
}
