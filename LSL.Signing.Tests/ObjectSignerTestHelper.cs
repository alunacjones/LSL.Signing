using System;

namespace LSL.Signing.Tests
{
    public static class ObjectSignerTestHelper 
    {
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

        public static PairInfo Pair(Func<object> valid, Func<object> invalid) => new PairInfo(valid, invalid);
        
        public class SignerInfo {
            public string Name {get;set;}
            public Action<ObjectSignerBuilder> SignerBuilder {get;set;}
            public int ExpectedSignatureLength { get; internal set; }
        }

        public class SerialiserInfo {            
            public string Name {get;set;}
            public Action<ObjectSignerBuilder> Serialiser {get;set;}
        }

        public class PairInfo {
            public PairInfo(Func<object> valid, Func<object> invalid)
            {
                Valid = valid;
                Invalid = invalid;
            }

            public Func<object> Valid { get; }
            public Func<object> Invalid { get; }
        }
    }
}
