using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DissAndAss.Assembly;
using DissAndAss.Assembly.Compiler;
using DissAndAss.Tests.TestWrappers;
using Xunit;

namespace DissAndAss.Tests
{
    public class TokenToBinaryConverterTests
    {
        private readonly TokenToBinaryConverter _converter;

        public TokenToBinaryConverterTests()
        {
            _converter = new TokenToBinaryConverter();
        }


        [Fact]
        //[Trait("Category", "SkipWhenLiveUnitTesting")]
        public void Compile_ShouldThrowExceptionWhenOperationWasNotMatched()
        {
            // Arrange
            var lines = new List<Token> { new Token() { Type = TokenType.Comma, Value = "," }, new Token() { Type = TokenType.GenericRegister, Value = "C" } };
            List<List<Token>> subject = new List<List<Token>> { lines };

            // Act & Assert
            Assert.Throws<Exception>(() => _converter.Compile(subject));
        }

        [Fact]
        //[Trait("Category", "SkipWhenLiveUnitTesting")]
        public void Compile_ShouldThrowExceptionWhenSyntaxErrorWasDetected()
        {
            // Arrange
            var lines = new List<Token> { new Token() { Type = TokenType.Comma, Value = "," }, new Token() { Type = TokenType.GenericRegister, Value = "C" }, new Token() { Type = TokenType.Invalid, Value = "-ADDx" },  };
            List<List<Token>> subject = new List<List<Token>> { lines };

            // Act & Assert
            Assert.Throws<Exception>(() => _converter.Compile(subject));
        }

        [Fact]
        public void Compile_ShouldThrowExceptionWhenJumpingFromWrongRegister()
        {
            // Arrange
            var lines = new List<Token>
            {
                new Token() { Type = TokenType.Mnemonic, Value = "JP" },
                new Token() { Type = TokenType.GenericRegister, Value = "V1" },
                new Token() { Type = TokenType.Comma, Value = "," },
                new Token() { Type = TokenType.HeximalData, Value = "123" },
                new Token() { Type = TokenType.SequenceEnd, Value = "" }
            };

            List<List<Token>> subject = new List<List<Token>> { lines };

            // Act & Assert
            Assert.Throws<Exception>(() => _converter.Compile(subject));
        }

        [Fact]
        [Trait("TestCategory", "")]
        [Trait("Category", "")]
        public void Compile_ShouldThrowExceptionWhenExceedingFreeDataMaxLength()
        {
            // Arrange
            var lines = new List<Token>
            {
                new Token() { Type = TokenType.Mnemonic, Value = "JP" },
                new Token() { Type = TokenType.GenericRegister, Value = "V0" },
                new Token() { Type = TokenType.Comma, Value = "," },
                new Token() { Type = TokenType.HeximalData, Value = "FFFFFFF" },
                new Token() { Type = TokenType.SequenceEnd, Value = "" }
            };

            List<List<Token>> subject = new List<List<Token>> { lines };

            // Act & Assert
            Assert.Throws<Exception>(() => _converter.Compile(subject));
        }


        [Fact]
        public void Compile_ShouldReturnUShortArrayWithProperInput()
        {
            // Arrange
            var lines = new List<Token> { new Token() { Type = TokenType.Mnemonic, Value = "JP" }, new Token() { Type = TokenType.HeximalData, Value = "123" }, new Token() { Type = TokenType.SequenceEnd, Value = "" } };

            List<List<Token>> subject = new List<List<Token>> { lines };

            // Act 
            var output = _converter.Compile(subject);

            // Assert
            Assert.IsType<ushort[]>(output);       
        }

        [Fact]
        public void Compile_ShouldOmitComments()
        {
            // Arrange
            var lines = new List<Token> 
            {
                new Token() { Type = TokenType.Comment, Value = "!@#!#asdadQ@eawewaeqweqeq" },
                new Token() { Type = TokenType.Mnemonic, Value = "JP" },
                new Token() { Type = TokenType.Comment, Value = "!@#!#Q@eawewaeqweqeq" },
                new Token() { Type = TokenType.HeximalData, Value = "123" },
                new Token() { Type = TokenType.Comment, Value = "Welcome to my comment" },
                new Token() { Type = TokenType.SequenceEnd, Value = "" } 
            };

            List<List<Token>> subject = new List<List<Token>> { lines };

            // Act 
            var output = _converter.Compile(subject);

            // Act & Assert
            Assert.IsType<ushort[]>(output);
            Assert.True(output.Any());
            Assert.True(output[0] == 0x1123 );
        }

        [Fact]
        public void Compile_ShouldOmitLinesContainingOnlyComments()
        {
            // Arrange
            var lines = new List<Token>
            {
                new Token() { Type = TokenType.Comment, Value = "!@#!#asdadQ@eawewaeqweqeq" },
                new Token() { Type = TokenType.Comment, Value = "!@#!#Q@eawewaeqweqeq" },
                new Token() { Type = TokenType.SequenceEnd, Value = "" }
            };

            List<List<Token>> subject = new List<List<Token>> { lines };

            // Act 
            var output = _converter.Compile(subject);

            // Assert
            Assert.True(!output.Any());
        }

        [Fact]
        public void Compile_GivenMultipleOperationsShouldAssembleCorrectly()
        {
            // Arrange 
            var l0 = new List<Token>
            {
                new Token() { Type = TokenType.Comment, Value = "!@#!#asdadQ@eawewaeqweqeq" },
                new Token() { Type = TokenType.Mnemonic, Value = "JP" },
                new Token() { Type = TokenType.Comment, Value = "!@#!#Q@eawewaeqweqeq" },
                new Token() { Type = TokenType.HeximalData, Value = "123" },
                new Token() { Type = TokenType.Comment, Value = "Welcome to my comment" },
                new Token() { Type = TokenType.SequenceEnd, Value = "" }
            };

            var l1 = new List<Token>
            {
                new Token() { Type = TokenType.Mnemonic, Value = "SYS" },
                new Token() { Type = TokenType.HeximalData, Value = "F1D" },
                new Token() { Type = TokenType.SequenceEnd, Value = "" },
            };

            var l2 = new List<Token>
            {
                new Token() { Type = TokenType.Mnemonic, Value = "LD" },
                new Token() { Type = TokenType.GenericRegister, Value = "V3" },
                new Token() { Type = TokenType.Comma, Value = "," },
                new Token() { Type = TokenType.HeximalData, Value = "44" },
                new Token() { Type = TokenType.SequenceEnd, Value = "" }
            };

            List<List<Token>> subject = new List<List<Token>> { l0,l1,l2 };

            ushort expected0 = 0x1123;
            ushort expected1 = 0x0F1D;
            ushort expected2 = 0x6344;

            // Act 
            var output = _converter.Compile(subject);

            // Assert
            Assert.True(output.Count() == 3);
            Assert.True(output[0] == expected0 && output[1] == expected1 && output[2] == expected2);
        }

        [Theory]
        [MemberData (nameof(TestData))]
        public void Compile_ShouldAssembleAllOperationsCorrectly(ushort expected, TestLineBuilder line)
        {
            // Arrange

            List<List<Token>> subject = new List<List<Token>> { line.Build().ToList() };

            // Act 
            var output = _converter.Compile(subject);

            // Assert
            Assert.True(output.Count() == 1);
            Assert.True(output[0] == expected);
        }

        public static IEnumerable<Object[]> TestData()
        {
            // for testing purposes (serialization) - not to clutter actual classes
            // every line is wrapped with TestLineBuilder which allows to retrive List<Token> 
            // and every Token is wrapped with TestTokenBuilder which allows to retrive Token

            yield return new object[] { (ushort) 0x0F1D,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder(new Token() { Type = TokenType.Mnemonic, Value = "SYS" }),
                new TestTokenBuilder(new Token() { Type = TokenType.HeximalData, Value = "F1D" }),
                new TestTokenBuilder(new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x00E0,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "CLS" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x00EE,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder(  new Token() { Type = TokenType.Mnemonic, Value = "RET" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x1FFE,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "JP" }),
                new TestTokenBuilder( new Token() { Type = TokenType.HeximalData, Value = "FFE" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x2111,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "CALL" }),
                new TestTokenBuilder( new Token() { Type = TokenType.HeximalData, Value = "111" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x3E1F,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "SE" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "VE" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.HeximalData, Value = "1F" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x4E1F,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "SNE" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "VE" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.HeximalData, Value = "1F" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x5E10,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "SE" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "VE" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V1" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x6344,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "LD" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V3" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.HeximalData, Value = "44" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x754F,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "ADD" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V5" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.HeximalData, Value = "4F" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x85E0,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "LD" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V5" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "VE" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x8131,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "OR" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V1" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V3" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x8792,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "AND" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V7" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V9" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x8253,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "XOR" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V2" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V5" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x8154,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "ADD" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V1" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V5" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x8EE5,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "SUB" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "VE" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "VE" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};


            yield return new object[] { (ushort) 0x8706,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "SHR" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V7" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x8357,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "SUBN" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V3" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V5" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x870E,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "SHL" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V7" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0x91F0,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "SNE" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V1" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "VF" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0xAE1F,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "LD" }),
                new TestTokenBuilder( new Token() { Type = TokenType.IRgeister, Value = "I" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.HeximalData, Value = "E1F" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0xBFFF,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "JP" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V0" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.HeximalData, Value = "FFF" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0xC4FF,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "RND" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V4" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.HeximalData, Value = "FF" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0xD4E6,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "DRW" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V4" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "VE" }),
                new TestTokenBuilder( new Token() { Type = TokenType.Comma, Value = "," }),
                new TestTokenBuilder( new Token() { Type = TokenType.HeximalData, Value = "6" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0xE79E,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "SKP" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V7" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};

            yield return new object[] { (ushort) 0xE7A1,new TestLineBuilder(new List<TestTokenBuilder>()
            {
                new TestTokenBuilder( new Token() { Type = TokenType.Mnemonic, Value = "SKNP" }),
                new TestTokenBuilder( new Token() { Type = TokenType.GenericRegister, Value = "V7" }),
                new TestTokenBuilder( new Token() { Type = TokenType.SequenceEnd, Value = "" }),
            })};




            //AssocietedTokenSet = new List<TokenType>() { TokenType.Mnemonic, TokenType.GenericRegister, TokenType.Comma, TokenType.HeximalData, TokenType.SequenceEnd } });




        }



    }
}
