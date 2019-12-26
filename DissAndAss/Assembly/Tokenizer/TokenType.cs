using System;
using System.Collections.Generic;
using System.Text;

namespace DissAndAss.Assembly
{
    public enum TokenType
    {
        NotDefined,
        Space,
        Comma,
        HeximalData,
        Mnemonic,
        GenericRegister,
        IRgeister,
        ST,
        DT,
        K,
        IRange,
        Comment,
        SequenceEnd,

    }
}
