using System.Collections.Generic;
using Xunit;

namespace Stratis.Bitcoin.Features.Wallet.Tests
{
    public class AccountRootTest : WalletTestBase
    {
        [Fact]
        public void GetFirstUnusedAccountWithoutAccountsReturnsNull()
        {
            Wallet wallet = new Wallet {Accounts = new List<HdAccount>()};

            HdAccount result = wallet.GetFirstUnusedAccount();

            Assert.Null(result);
        }

        [Fact]
        public void GetFirstUnusedAccountReturnsAccountWithLowerIndexHavingNoAddresses()
        {
            Wallet wallet = new Wallet { Accounts = new List<HdAccount>() };

            HdAccount unused = CreateAccount("unused1");
            unused.Index = 2;
            wallet.Accounts.Add(unused);

            HdAccount unused2 = CreateAccount("unused2");
            unused2.Index = 1;
            wallet.Accounts.Add(unused2);

            HdAccount used = CreateAccount("used");
            used.ExternalAddresses.Add(CreateAddress());
            used.Index = 3;
            wallet.Accounts.Add(used);

            HdAccount used2 = CreateAccount("used2");
            used2.InternalAddresses.Add(CreateAddress());
            used2.Index = 4;
            wallet.Accounts.Add(used2);

            HdAccount result = wallet.GetFirstUnusedAccount();

            Assert.NotNull(result);
            Assert.Equal(1, result.Index);
            Assert.Equal("unused2", result.Name);
        }

        [Fact]
        public void GetAccountByNameWithMatchingNameReturnsAccount()
        {
            Wallet wallet = new Wallet();
            wallet.Accounts.Add(CreateHdAccountWithAddresses("Test"));

            HdAccount result = wallet.GetAccountByName("Test");

            Assert.NotNull(result);
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public void GetAccountByNameWithNonMatchingNameThrowsException()
        {
            Wallet wallet = new Wallet();
            wallet.Accounts.Add(CreateHdAccountWithAddresses("Test"));
            
            Assert.Throws<WalletException>(() => { wallet.GetAccountByName("test"); });
        }
    }
}
