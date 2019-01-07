using System;
using System.Collections.Generic;
using NBitcoin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stratis.Bitcoin.Features.Wallet
{
    public class WalletConverter : JsonConverter<Wallet>
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, Wallet value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool CanWrite => false;

        /// <inheritdoc />
        public override Wallet ReadJson(JsonReader reader, Type objectType, Wallet existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            // Check the version number and deserialize accordingly.
            // An object with a missing version is considered version 1.
            int? version = (int?)jObject["version"];

            // If the Json is the latest version, we deserialize with the default contract and return.
            if (version == Wallet.VersionNumber)
            {
                return jObject.ToObject<Wallet>();
            }

            // Transformation from the first version to the latest version.
            if (version == null)
            {
                // The wallet JSON is out of date and needs updating, which should only happen on the first run after the wallet update.
                // First, we deserialize the wallet as is in order to assign as many unchanged properties as possible.
                Wallet wallet = jObject.ToObject<Wallet>();

                JArray accountsRoot = (JArray)jObject["accountsRoot"];
                JToken accountRoot = accountsRoot[0];
                JArray accounts = (JArray)accountRoot["accounts"];

                wallet.CoinType = (CoinType)Enum.Parse(typeof(CoinType), (string)accountRoot["coinType"]);
                wallet.LastBlockSyncedHeight = (int)accountRoot["lastBlockSyncedHeight"];
                wallet.LastBlockSyncedHash = uint256.Parse((string)accountRoot["lastBlockSyncedHash"]);
                wallet.Accounts = accounts.ToObject<ICollection<HdAccount>>();
                return wallet;
            }

            throw new WalletException("Couldn't deserialize a wallet. Please restore it using your mnemonic.");
        }
    }
}
