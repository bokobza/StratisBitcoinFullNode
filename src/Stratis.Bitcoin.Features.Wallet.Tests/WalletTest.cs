using System.Collections.Generic;
using System.Linq;
using NBitcoin;
using Xunit;

namespace Stratis.Bitcoin.Features.Wallet.Tests
{
    public class WalletTest : WalletTestBase
    {
        [Fact]
        public void GetAccountsWithoutAccountsReturnsEmptyList()
        {
            var wallet = new Wallet();

            IEnumerable<HdAccount> result = wallet.GetAccounts();

            Assert.Empty(result);
        }

        [Fact]
        public void GetAllTransactionsReturnsTransactionsFromWallet()
        {
            TransactionData transaction1 = CreateTransaction(new uint256(1), new Money(15000), 1);
            TransactionData transaction2 = CreateTransaction(new uint256(2), new Money(91209), 1);
            TransactionData transaction3 = CreateTransaction(new uint256(5), new Money(52387), 1);
            TransactionData transaction4 = CreateTransaction(new uint256(6), new Money(879873), 1);

            HdAccount account1 = CreateHdAccountWithAddresses("StratisAccount");
            HdAccount account2 = CreateHdAccountWithAddresses("StratisAccount2");
            
            account1.InternalAddresses.ElementAt(0).Transactions.Add(transaction1);
            account1.ExternalAddresses.ElementAt(0).Transactions.Add(transaction2);
            account2.InternalAddresses.ElementAt(0).Transactions.Add(transaction3);
            account2.ExternalAddresses.ElementAt(0).Transactions.Add(transaction4);

            var wallet = new Wallet();
            wallet.Accounts.Add(account1);
            wallet.Accounts.Add(account2);

            List<TransactionData> result = wallet.GetAllTransactions().ToList();

            Assert.Equal(4, result.Count);
            Assert.Equal(transaction2, result[0]);
            Assert.Equal(transaction4, result[1]);
            Assert.Equal(transaction1, result[2]);
            Assert.Equal(transaction3, result[3]);
        }

        [Fact]
        public void GetAllTransactionsWithoutAccountRootReturnsEmptyList()
        {
            var wallet = new Wallet();

            List<TransactionData> result = wallet.GetAllTransactions().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void GetAllPubKeysReturnsPubkeysFromWallet()
        {
            HdAccount account1 = CreateHdAccountWithAddresses("StratisAccount");
            HdAccount account2 = CreateHdAccountWithAddresses("StratisAccount2");

            var wallet = new Wallet();
            wallet.Accounts.Add(account1);
            wallet.Accounts.Add(account2);

            List<Script> result = wallet.GetAllPubKeys().ToList();

            Assert.Equal(4, result.Count);
            Assert.Equal(account1.ExternalAddresses.ElementAt(0).ScriptPubKey, result[0]);
            Assert.Equal(account2.ExternalAddresses.ElementAt(0).ScriptPubKey, result[1]);
            Assert.Equal(account1.InternalAddresses.ElementAt(0).ScriptPubKey, result[2]);
            Assert.Equal(account2.InternalAddresses.ElementAt(0).ScriptPubKey, result[3]);
        }

        [Fact]
        public void GetAllPubKeysByCoinTypeWithoutAccountRootsReturnsEmptyList()
        {
            var wallet = new Wallet();

            List<Script> result = wallet.GetAllPubKeys().ToList();

            Assert.Empty(result);
        }
    }
}
