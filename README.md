# ISO8583

Uma biblioteca para auxiliar na construção e leitura mensagens ISO8583.

Como montar uma mensagem:

```csharp
var message = new ISO8583.Message.SendMessage("0800") //Message Type
    .Append(3, "008000")
    .Append(7, "1234567890")
    .Append(11, "000001")
    .Append(12, "123456")
    .Encode(Encoding.ASCII)
    .Build()
    .Pack();
```