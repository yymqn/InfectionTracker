#pragma warning disable SYSLIB0014 // Type or member is obsolete - right now WebClient still works and will likely continue to work and I'm not rewriting this mess to use HttpClient
using System.IO;
using System.Net;
using System.Text;

namespace Hypixel;

// some random code from github I stole that actually works
class HypixelAPI
{

    private static readonly string BASE_URL = "https://api.hypixel.net/";
    private static readonly string API_KEY;
    public const string API_FILE = "api.key";

    static HypixelAPI()
    {
        API_KEY = File.ReadAllText(API_FILE);
    }

    public static string GetPlayerByUuid(string uuid)
    {
        string? reply = null;
        // https://api.hypixel.net/player?key=<api_key>&uuid=<uuid>
        string url = BASE_URL + "player?key=" + API_KEY + "&uuid=" + uuid;
        WebResponse? response = null;
        StreamReader? reader = null;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            reply = reader.ReadToEnd();
        }
        finally
        {
            reader?.Close();
            response?.Close();
        }

        return reply;
    }

    public static string GetPlayerByPlayer(string player)
    {
        string? reply = null;
        // https://api.hypixel.net/player?key=<api_key>&player=<player>
        string url = BASE_URL + "player?key=" + API_KEY + "&player=" + player;
        WebResponse? response = null;
        StreamReader? reader = null;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            reply = reader.ReadToEnd();
        }
        finally
        {
            reader?.Close();
            response?.Close();
        }

        return reply;
    }

    public static string GetPlayerByName(string name)
    {
        string? reply = null;
        // https://api.hypixel.net/player?key=<api_key>&name=<name>
        string url = BASE_URL + "player?key=" + API_KEY + "&name=" + name;
        WebResponse? response = null;
        StreamReader? reader = null;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            reply = reader.ReadToEnd();
        }
        finally
        {
            reader?.Close();
            response?.Close();
        }

        return reply;
    }

    public static string GetFriendsByUuid(string uuid)
    {
        string? reply = null;
        // https://api.hypixel.net/friends?key=<api_key>&uuid=<uuid>
        string url = BASE_URL + "friends?key=" + API_KEY + "&uuid=" + uuid;
        WebResponse? response = null;
        StreamReader? reader = null;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            reply = reader.ReadToEnd();
        }
        finally
        {
            reader?.Close();
            response?.Close();
        }

        return reply;
    }

    public static string GetFriendsByPlayer(string player)
    {
        string? reply = null;
        // https://api.hypixel.net/friends?key=<api_key>&player=<player>
        string url = BASE_URL + "friends?key=" + API_KEY + "&player=" + player;
        WebResponse? response = null;
        StreamReader? reader = null;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            reply = reader.ReadToEnd();
        }
        finally
        {
            reader?.Close();
            response?.Close();
        }

        return reply;
    }

    public static string GetFriendsByName(string name)
    {
        string? reply = null;
        // https://api.hypixel.net/friends?key=<api_key>&name=<name>
        string url = BASE_URL + "friends?key=" + API_KEY + "&name=" + name;
        WebResponse? response = null;
        StreamReader? reader = null;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            reply = reader.ReadToEnd();
        }
        finally
        {
            reader?.Close();
            response?.Close();
        }

        return reply;
    }

    public static string GetGuildInfo(string id)
    {
        string? reply = null;
        // https://api.hypixel.net/guild?key=<api_key>&id=<id>
        string url = BASE_URL + "guild?key=" + API_KEY + "&id=" + id;
        WebResponse? response = null;
        StreamReader? reader = null;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            reply = reader.ReadToEnd();
        }
        finally
        {
            reader?.Close();
            response?.Close();
        }

        return reply;
    }

    public static string FindGuildByUuid(string uuid)
    {
        string? reply = null;
        // https://api.hypixel.net/findGuild?key=<api_key>&byUuid=<uuid>
        string url = BASE_URL + "findGuild?key=" + API_KEY + "byUuid" + uuid;
        WebResponse? response = null;
        StreamReader? reader = null;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            reply = reader.ReadToEnd();
        }
        finally
        {
            reader?.Close();
            response?.Close();
        }

        return reply;
    }

    public static string FindGuildByPlayer(string player)
    {
        string? reply = null;
        // https://api.hypixel.net/findGuild?key=<api_key>&byPlayer=<player>
        string url = BASE_URL + "findGuild?key=" + API_KEY + "&byPlayer=" + player;
        WebResponse? response = null;
        StreamReader? reader = null;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            reply = reader.ReadToEnd();
        }
        finally
        {
            reader?.Close();
            response?.Close();
        }

        return reply;
    }

    public static string FindGuildByName(string name)
    {
        string? reply = null;
        // https://api.hypixel.net/findGuild?key=<api_key>&byName=<name>
        string url = BASE_URL + "findGuild?key=" + API_KEY + "&byName=" + name;
        WebResponse? response = null;
        StreamReader? reader = null;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            reply = reader.ReadToEnd();
        }
        finally
        {
            reader?.Close();
            response?.Close();
        }

        return reply;
    }

    public static string GetSessionByUuid(string uuid)
    {
        string? reply = null;
        // https://api.hypixel.net/session?key=<api_key>&uuid=<uuid>
        string url = BASE_URL + "session?key=" + API_KEY + "&uuid=" + uuid;
        WebResponse? response = null;
        StreamReader? reader = null;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            reply = reader.ReadToEnd();
        }
        finally
        {
            reader?.Close();
            response?.Close();
        }

        return reply;
    }

    public static string GetSessionByPlayer(string player)
    {
        string? reply = null;
        // https://api.hypixel.net/session?key=<api_key>&player=<player>
        string url = BASE_URL + "session?key=" + API_KEY + "&player=" + player;
        WebResponse? response = null;
        StreamReader? reader = null;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            reply = reader.ReadToEnd();
        }
        finally
        {
            reader?.Close();
            response?.Close();
        }

        return reply;
    }

    public static string GetSessionByName(string name)
    {
        string? reply = null;
        // https://api.hypixel.net/session?key=<api_key>&name=<name>
        string url = BASE_URL + "session?key=" + API_KEY + "&name=" + name;
        WebResponse? response = null;
        StreamReader? reader = null;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            reply = reader.ReadToEnd();
        }
        finally
        {
            reader?.Close();
            response?.Close();
        }

        return reply;
    }

    public static string GetBoosters()
    {
        string? reply = null;
        // https://api.hypixel.net/boosters?key=<api_key>
        string url = BASE_URL + "boosters?key=" + API_KEY;
        WebResponse? response = null;
        StreamReader? reader = null;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            reply = reader.ReadToEnd();
        }
        finally
        {
            reader?.Close();
            response?.Close();
        }

        return reply;
    }

    public static string GetKeyInfo()
    {
        string? reply = null;
        // https://api.hypixel.net/key?key=<api_key>
        string url = BASE_URL + "key?key=" + API_KEY;
        WebResponse? response = null;
        StreamReader? reader = null;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            reply = reader.ReadToEnd();
        }
        finally
        {
            reader?.Close();
            response?.Close();
        }

        return reply;
    }

    public static string GetKeyInfo(string key)
    {
        string? reply = null;
        // https://api.hypixel.net/key?key=<api_key>
        string url = BASE_URL + "key?key=" + key;
        WebResponse? response = null;
        StreamReader? reader = null;

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            reply = reader.ReadToEnd();
        }
        finally
        {
            reader?.Close();
            response?.Close();
        }

        return reply;
    }

}
#pragma warning restore SYSLIB0014 // Type or member is obsolete