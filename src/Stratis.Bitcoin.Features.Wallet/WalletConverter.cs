using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBitcoin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Stratis.Bitcoin.Features.Wallet
{
    public class WalletConverter : JsonConverter<Wallet>
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, Wallet value, JsonSerializer serializer)
        {
           // serializer.Serialize(writer, value);
           // writer.WriteValue(value); // Newtonsoft.Json.JsonWriterException: 'Unsupported type: Stratis.Bitcoin.Features.Wallet.Wallet. Use the JsonSerializer class to get the object's JSON representation. Path ''.'
           serializer.
        }

        /// <inheritdoc />
        public override Wallet ReadJson(JsonReader reader, Type objectType, Wallet existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            string version = (string)jObject["version"];
            if (version == "1")
            {
                return jObject.ToObject<Wallet>();
            }

            // First, try to deserialize the wallet.
            Wallet wallet = jObject.ToObject<Wallet>();

            //JArray accountsRoot = (JArray)jObject["accountsRoot"];
            //JToken accountRoot = accountsRoot[0];
            AccountRoot accountRoot = wallet.AccountsRoot.First();

            wallet.CoinType = accountRoot.CoinType; //(CoinType)Enum.Parse(typeof(CoinType), (string)accountRoot["coinType"]);
            wallet.LastBlockSyncedHeight = accountRoot.LastBlockSyncedHeight; //(int)accountRoot["lastBlockSyncedHeight"];
            wallet.LastBlockSyncedHash = accountRoot.LastBlockSyncedHash; //uint256.Parse((string)accountRoot["lastBlockSyncedHash"]);
            wallet.Accounts = accountRoot.Accounts;
            return wallet;
        }
    }
}
