using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace Example2
{
  public class Program
  {
    public static void Main (string [] args)
    {
      var wssv = new WebSocketServer (4649);
      //var wssv = new WebSocketServer (4649, true); // Secure
      //var wssv = new WebSocketServer ("ws://localhost:4649");
      //var wssv = new WebSocketServer ("wss://localhost:4649"); // Secure

#if DEBUG
      wssv.Log.Level = LogLevel.TRACE;
#endif

      /* Secure Connection
      var cert = ConfigurationManager.AppSettings ["ServerCertFile"];
      var password = ConfigurationManager.AppSettings ["CertFilePassword"];
      wssv.Certificate = new X509Certificate2 (cert, password);
       */

      /* HTTP Authentication (Basic/Digest)
      wssv.AuthenticationSchemes = AuthenticationSchemes.Basic;
      wssv.Realm = "WebSocket Test";
      wssv.UserCredentialsFinder = identity => {
        var name = identity.Name;
        return name == "nobita"
               ? new NetworkCredential (name, "password")
               : null;
      };
       */

      //wssv.KeepClean = false;

      wssv.AddWebSocketService<Echo> ("/Echo");
      wssv.AddWebSocketService<Chat> ("/Chat");
      //wssv.AddWebSocketService<Chat> ("/Chat", () => new Chat ("Anon#"));

      wssv.Start ();
      if (wssv.IsListening) {
        Console.WriteLine (
          "A WebSocket server listening on port: {0} service paths:", wssv.Port);

        foreach (var path in wssv.WebSocketServices.ServicePaths)
          Console.WriteLine ("  {0}", path);
      }

      Console.WriteLine ("\nPress Enter key to stop server...");
      Console.ReadLine ();

      wssv.Stop ();
    }
  }
}
