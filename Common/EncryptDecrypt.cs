using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace HMS.Common
{
    public class EncryptDecrypt
    {
        private const string EncryptDecryptValue = "c_|4M9^t+ga1u=-JAhSddfeg$DQ1^|MoriHNmuhjIkjhmbnkjyug$#4%";
        private readonly IHttpContextAccessor context;
        private readonly IDataProtectionProvider dataProtectionProvider;
        public EncryptDecrypt()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddDataProtection();
            ServiceProvider services = serviceCollection.BuildServiceProvider();

            context = new HttpContextAccessor();
            dataProtectionProvider = services.GetDataProtector("HMS.APP");
        }
        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="InputString">The input string.</param> 
        /// <returns></returns>
        public string EncryptString(string InputString)
        {
            return dataProtectionProvider.CreateProtector(EncryptDecryptValue).Protect(InputString);
        }

        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="InputString">The input string.</param>
        /// <returns></returns>
        public string DecryptString(string InputString)
        {
            if (InputString.Contains("%"))
            {
                InputString = System.Net.WebUtility.HtmlDecode(InputString);
            }
            return dataProtectionProvider.CreateProtector(EncryptDecryptValue).Unprotect(InputString);
        }
    }
}
