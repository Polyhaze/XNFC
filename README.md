# XNFC
Xamarin/Cross-Platform Near Field Communication (NFC) library.

## Getting Started
Currently, XNFC is only supported on .NET Standard and Android platforms.

### Installing
- Install the Ultz.XNFC package to your .NET Standard project
- Install the Ultz.XNFC.Android package to your Android project

### Android
- Add the following line anywhere in your `OnCreate` override:
```cs
XNFCPlatform.Init(this);
```

- Next, we need to inform XNFC whenever we actually get a tag. To do so, we need to override `OnNewIntent` like so:
```cs
protected override void OnNewIntent(Intent intent)
{
    XNFC.GlobalCallback(intent);
}
```

### Using XNFC
The most basic use:
```cs
// get an API instance
var xnfc = XNFC.GetApi();
xnfc.TagDetected += (sender,e) => { Console.WriteLine("Got NFC Tag!"); foreach (var record in e.Records)
{Console.WriteLine("Data: "+record.Payload);} XNFC.GloballyStopListening();};
// you can use xnfc.StartListeningAsync(), but we added a global feature so that you don't have to
destroy your head trying to get a callback.
// note that only one API instance can be globally listening; and if you call
this method again while another listener is active, the currently listening listener never receives anything.
XNFC.GloballyStartListening(xnfc);
```
