using System;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using FluentAssertions;
using MessagePack;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Security.Cryptography;

namespace LSL.Signing.Tests
{
    public class ObjectSignerTests
    {
        [TestCaseSource(nameof(Tests))]
        public void RunTests(Action<ObjectSignerBuilder> binarySerialiserBuilder, Action<ObjectSignerBuilder> signerBuilder, Pair validAndInvalidPair, int expectedSignaureLength)
        {
            MessagePackSerializer.SetDefaultResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);

            var factory = new ObjectSignerFactory();
            var objectSigner = factory.Build(cfg => {
                signerBuilder(cfg);
                binarySerialiserBuilder(cfg);
            });

            var signature = objectSigner.GenerateSignature(validAndInvalidPair.Valid());

            signature.Length.Should().Be(expectedSignaureLength, "the signer shoulld prduce consistently lengthed signaures");
            objectSigner.Verify(
                validAndInvalidPair.Valid(),
                signature
            ).Should().BeTrue("the same object should produce the same signature");

            objectSigner.Verify(
                validAndInvalidPair.Invalid(),
                signature
            ).Should().BeFalse("a different object should produce a different signature");
        }

        [Serializable]
        public class Test {
            public int Id {get;set;}
            public string Name {get;set;}
        }

        [Serializable]
        public class OuterClass {
            public int Id {get;set;}
            public Test Inner {get;set;}
        }

        public static IEnumerable Tests {
            get {
                var secret = Encoding.UTF8.GetBytes("als-test");

                return from binarySerialiser in new[] {
                    Serialiser("Json",  cfg => cfg.WithStringBasedSerialiser(JsonConvert.SerializeObject)),
                    Serialiser("MsgPack", cfg => cfg.WithBinarySerialiser(MessagePackSerializer.Serialize)),
                    Serialiser("Default", cfg => {})
                } 
                from signer in new[] {
                    Signer("HMAC256", 32, cfg => cfg.WithHmac256SignatureProvider(secret)),
                    Signer("HMAC384", 48, cfg => cfg.WithHmac384SignatureProvider(secret)),
                    Signer("HMAC512", 64, cfg => cfg.WithHmac512SignatureProvider(secret)),
                    Signer("Default", 32, cfg => {})
                }
                from validAndInvalidPair in new Pair[] {
                    new Pair(() => new Test { Id = 12, Name = "Als" }, () => new Test { Id = 13, Name = "Als"}),
                    new Pair(() => "qwe", () => "qwe2"),
                    new Pair(() => new OuterClass { Id = 12, Inner = new Test { Id = 13, Name = "Als" }}, () => new OuterClass { Id = 12, Inner = new Test { Id = 14, Name = "Als" }})
                }
                let valid = JsonConvert.SerializeObject(validAndInvalidPair.Valid())
                let invalid = JsonConvert.SerializeObject(validAndInvalidPair.Invalid())
                select new TestCaseData(binarySerialiser.Serialiser, signer.SignerBuilder, validAndInvalidPair, signer.ExpectedSignatureLength)
                    .SetName($"Given a {binarySerialiser.Name} serialiser and a {signer.Name} signer for a valid value of {valid} and invalid value of {invalid}, it should validate both correctly");
            }
        }

        public static SignerInfo Signer(string name, int expectedSignatureLength, Action<ObjectSignerBuilder> signerBuilder) => new SignerInfo
        {
            Name = name,
            SignerBuilder = signerBuilder,
            ExpectedSignatureLength = expectedSignatureLength
        };

        public static SerialiserInfo Serialiser(string name, Action<ObjectSignerBuilder> serialiserBuilder) => new SerialiserInfo
        {
            Name = name,
            Serialiser = serialiserBuilder
        };

        public class SignerInfo {
            public string Name {get;set;}
            public Action<ObjectSignerBuilder> SignerBuilder {get;set;}
            public int ExpectedSignatureLength { get; internal set; }
        }

        public class SerialiserInfo {            
            public string Name {get;set;}
            public Action<ObjectSignerBuilder> Serialiser {get;set;}
        }

        public class Pair {
            public Pair(Func<object> valid, Func<object> invalid)
            {
                Valid = valid;
                Invalid = invalid;
            }

            public Func<object> Valid { get; }
            public Func<object> Invalid { get; }
        }
    }
}
