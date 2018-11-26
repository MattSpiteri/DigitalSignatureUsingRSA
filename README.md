# DigitalSignatureUsingRSA
A console application which uses Microsoft RSACryptoserviceProvider and openSSL to create and verify a digital signature

This console application is signing simple text (in this case a name and surname) but can widely accept any data type or object.

The certificates were generated using openSSL through gitBash using the below commands:

Private:

``` openssl genrsa -out private.pem 2048``` 

Public:

```openssl rsa -pubout -in private.pem -out public.pem -outform PEM```
