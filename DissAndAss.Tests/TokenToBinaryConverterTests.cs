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
            // Assert
            var lines = new List<Token> { new Token() { Type = TokenType.Comma, Value = "," }, new Token() { Type = TokenType.GenericRegister, Value = "C" } };
            List<List<Token>> subject = new List<List<Token>> { lines };

            // Act & Assert
            Assert.Throws<Exception>(() => _converter.Compile(subject));
        }

        [Fact]
        //[Trait("Category", "SkipWhenLiveUnitTesting")]
        public void Compile_ShouldThrowExceptionWhenSyntaxErrorWasDetected()
        {
            // Assert
            var lines = new List<Token> { new Token() { Type = TokenType.Comma, Value = "," }, new Token() { Type = TokenType.GenericRegister, Value = "C" }, new Token() { Type = TokenType.Invalid, Value = "-ADDx" },  };
            List<List<Token>> subject = new List<List<Token>> { lines };

            // Act & Assert
            Assert.Throws<Exception>(() => _converter.Compile(subject));
        }

        [Fact]
        public void Compile_ShouldThrowExceptionWhenJumpingFromWrongRegister()
        {
            // Assert
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
        public void Compile_ShouldThrowExceptionWhenExceedingFreeDataMaxLength()
        {
            // Assert
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
        [Trait("TestCategory", "")]
        [Trait("Category", "")]
        public void Compile_ShouldReturnUShortArrayWithProperInput()
        {
            // Assert
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
            // Assert
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
            // Assert
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
        [MemberData(nameof(TestData))]
        public void Compile_GivenMultipleOperationsShouldAssembleProperly()
        {
            // Assert
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
        public void Compile_ShouldAssembleAllOperationsProperly(ushort expected, TestLineBuilder line)
        {
            // Assert

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

        }



    }
}
