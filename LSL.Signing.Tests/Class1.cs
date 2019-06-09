using System;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace LSL.Signing.Tests
{
    public class Class1
    {
        //[TestCaseSource(nameof(Tests))]
        [Test]
        public void Blah2()
        {            
            var factory = new ObjectSignerFactory();
            var signer = factory.Build(cfg => cfg.WithHmac512SignatureProvider(Encoding.UTF8.GetBytes("als-shite")));

            var signature = signer.GenerateSignature("qwe");

            signer.Verify("qwe", signature).Should().BeTrue();
            signer.Verify("qwe2", signature).Should().BeFalse();
        }

        [Test]
        public void Blah3()
        {
            var factory = new ObjectSignerFactory();
            var signer = factory.Build(cfg => cfg.WithStringBasedSerialiser(JsonConvert.SerializeObject));

            var signature = signer.GenerateSignature(new Test {
                Id = 12,
                Name = "als"
            });

            signer.Verify(
                new Test {
                    Id = 12,
                    Name = "als"
                },
                signature
            ).Should().BeTrue();

            signer.Verify(
                new Test {
                    Id = 12,
                    Name = "bls"
                },
                signature
            ).Should().BeFalse();
        }

        [Serializable]
        private class Test {
            public int Id {get;set;}
            public string Name {get;set;}
        }

        public IEnumerable Tests {
            get {
                yield return new TestCaseData();
            }
        }
    }
}
