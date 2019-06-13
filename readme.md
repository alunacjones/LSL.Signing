[![Build status](https://ci.appveyor.com/api/projects/status/9i2as11ge6ecash8?svg=true)](https://ci.appveyor.com/project/alunacjones/lsl-signing)

# LSL.Signing

This library provides a simple wrapper to quickly generate a signature for an object.

## Quick start

### Signing objects

Using the default options a signature can be obtained with the following:

```csharp
var signature = new ObjectSignerFactory()
    .Build()
    .GenerateSignature("test-string-object");
```

The previous example uses a primitive type but complex objects can also be signed:

```csharp
/*
    This example relies on the definition of two classes:

    public class Test {
        public int Id { get; set; }
        public Inner InnerInstance { get; set; }
    }

    public class Inner {
        public int OtherId { get; set; }
        public string Name { get; set; }
    }
*/
var signature = new ObjectSignerFactory()
    .Build()
    .GenerateSignature(new Test {
        Id = 12,
        InnerInstance = new Inner {
            OtherId = 13,
            Name = "Als"
        }
    });
```

### Verifying objects

Once we have a signature we can then verify an object using the `Verify` extension method:

```csharp
bool Verify(
    this IObjectSigner source, 
    object signaturee, 
    byte[] expectedSignature)
```

Example:

```csharp
var signer = new ObjectSignerFactory()
    .Build();

var signature = signer
    .GenerateSignature("test-string-object");

// Will be true
var verifyResullt1 = signer.Verify("test-string-object", signature);

// Will be false
var verifyResullt2 = signer.Verify("different-test-string-object", signature);
```

## Configuration

### Default settings

When the `ObjectSignerFactory`'s `Build` method is called without passing in a delegate to configure the signer then it uses a `BinaryFormatter` to serialise the object then uses an instance of  `HMACSHA256` to calculate a hash as the signature:

```csharp
var signer = new ObjectSignerFactory().Build();
```

### Using a custom serialiser

Passing a delegate into the `Build` method allows for customisation of the signer's behaviour. The configuration object exposes the `WithBinarySerialiser` method to customise serlisation. It is defined as:

```csharp
ObjectSignerBuilder WithBinarySerialiser(
    Func<object, byte[]> serialiser)
```

Example:

```csharp
// The following code relies on the installation of the MessagePack nuget library 
//(https://www.nuget.org/packages/MessagePack/)
var signer = new ObjectSignerFactory()
    .Build(cfg => cfg.WithBinarySerialiser(MessagePackSerializer.Serialize));
```

### Using a custom string seriliaser

The configuration object has an extension that allows us to customise serialisation to a string instead of a byte array. The method is defined as:

```csharp
ObjectSignerBuilder WithStringBasedSerialiser(
    this ObjectSignerBuilder source, 
    Func<object, string> objectToStringSerialiser)
```

Example:

```csharp
// This code snippet depends on the NetwonSoft.JSON nuget package for serialisation to a string 
//(https://www.nuget.org/packages/Newtonsoft.Json/)
var signer = new ObjectSignerFactory()
    .Build(cfg => cfg.WithStringBasedSerialiser(JsonConvert.SerializeObject));
```

### Customisation of object signing

As with serialisation, we can provide a custom signer via the signer configuration method `WithSignatureProvider`:

```csharp
ObjectSignerBuilder WithSignatureProvider(
    Func<byte[], byte[]> signatureProvider)
```

The following example uses a `HMACSHA512` instance with a secret of `[1,2,3]` to for signature generation.

```csharp
var signer = new ObjectSignerFactory()
    .Build(cfg => cfg.WithSignatureProvider(new HMACSHA512(new[] {1,2,3}).ComputeHash));
```

### Signer extensions

Shortcut extensions are provided to quickly configure different sized HMAC instances that a signer should use.

#### WithHmac256SignatureProvider

Definition:

```csharp
ObjectSignerBuilder WithHmac256SignatureProvider(
    this ObjectSignerBuilder source, 
    byte[] secret)
```

Example:

```csharp
var signer = new ObjectSignerFactory()
    .Build(cfg => cfg.WithHmac256SignatureProvider(new[] {1,2,3}));
```

#### WithHmac384SignatureProvider

Definition:

```csharp
ObjectSignerBuilder WithHmac384SignatureProvider(
    this ObjectSignerBuilder source, 
    byte[] secret)
```

Example:

```csharp
var signer = new ObjectSignerFactory()
    .Build(cfg => cfg.WithHmac384SignatureProvider(new[] {1,2,3}));
```

#### WithHmac512SignatureProvider

Definition:

```csharp
ObjectSignerBuilder WithHmac512SignatureProvider(
    this ObjectSignerBuilder source, 
    byte[] secret)
```

Example:

```csharp
var signer = new ObjectSignerFactory()
    .Build(cfg => cfg.WithHmac512SignatureProvider(new[] {1,2,3}));
```