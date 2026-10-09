// Session shim preserving ASP.NET (System.Web) semantics the legacy code relies on:
//  - keys are case-insensitive ("LoginUserId" and "loginuserid" are the same slot),
//  - the indexer returns null for absent keys, a string for string values,
//  - non-string values (menu tree, report parameters) are stored as JSON and read back with Get<T>(),
//  - Clear/Abandon/RemoveAll all empty the session (legacy called all three in a row).
using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace New_Accounts.Compat
{
    public class LegacySession
    {
        private readonly ISession _session;
        public LegacySession(ISession session) { _session = session; }

        private static string Key(string key) => (key ?? "").ToLowerInvariant();

        public object this[string key]
        {
            get => _session.GetString(Key(key));
            set
            {
                if (value == null) { _session.Remove(Key(key)); return; }
                _session.SetString(Key(key), value is string s ? s : JsonConvert.SerializeObject(value));
            }
        }

        public T Get<T>(string key) where T : class
        {
            string json = _session.GetString(Key(key));
            return json == null ? null : JsonConvert.DeserializeObject<T>(json);
        }

        public void Clear() => _session.Clear();
        public void Abandon() => _session.Clear();
        public void RemoveAll() => _session.Clear();
        public void Remove(string key) => _session.Remove(Key(key));
    }
}
