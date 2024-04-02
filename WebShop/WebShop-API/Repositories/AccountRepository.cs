namespace WebShop_API.Repositories
{
    /// <summary>
    /// Interface with methods/referance: GetById, Create, Update, Authenticate, RefreshToken and RevokeToken.
    /// </summary>
    public interface IAccountRepository
    {
        Task<Account> GetById( int accountId );
        Task<Account> Create( Account request );
        Task<Account> Update( int accountId, Account request );
        Task<AuthenticationResponse> Authenticate( string username_email, string password, string ipAddress );
        Task<AuthenticationResponse> RefreshToken( string token, string ipAddress );
        Task<bool> RevokeToken( string token, string ipAddress );
    }

    /// <summary>
    /// Using IAccountRepository interface.
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        private readonly DatabaseContext m_context;
        private readonly AppSettings m_appSettings;

        /// <summary>
        /// Constructor for AccountRepository.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="appSettings"></param>
        public AccountRepository( DatabaseContext context, IOptions<AppSettings> appSettings )
        {
            m_appSettings = appSettings.Value;
            m_context = context;
        }

        /// <summary>
        /// Checking for the clients username, email, password and if you have a JWTToken and a RefreshToken.
        /// </summary>
        /// <param name="username_email"></param>
        /// <param name="password"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public async Task<AuthenticationResponse> Authenticate( string username_email, string password, string ipAddress )
        {
            Account account = await m_context.Account.Include(e => e.Customer).FirstOrDefaultAsync( x => x.Username == username_email || x.Email == username_email);
            
            if(account == null)
            {
                return null;
            }

            if(!BC.Verify( password, account.Password ))
            {
                return null;
            }

            string accessToken = JWTHandler.GenerateJWTToken( account, m_appSettings );
            RefreshToken refreshToken = JWTHandler.GenerateRefreshToken( ipAddress );

            account.RefreshTokens.Add( refreshToken );

            m_context.Update( account );
            await m_context.SaveChangesAsync();

            return new AuthenticationResponse( refreshToken.Token, accessToken );
        }

        /// <summary>
        /// Creates an Account in the database.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>DirectResponse</returns>
        public async Task<Account> Create( Account request )
        {
            request.Password = BC.HashPassword( request.Password );
            m_context.Account.Add(request);
            await m_context.SaveChangesAsync();
            return await GetById(request.AccountID);
        }

        /// <summary>
        /// Crates a RefreshToken
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ipAddress"></param>
        /// <returns>AuthenticationResponse (newRefreshToken.Token, accessToken)</returns>
        public async Task<AuthenticationResponse> RefreshToken( string token, string ipAddress )
        {
            Account account = await m_context.Account.Include(e => e.RefreshTokens).Include(e => e.Customer).FirstOrDefaultAsync( c => c.RefreshTokens.Any( t => t.Token == token ) );

            if(account == null)
            {
                return null;
            }

            RefreshToken refreshToken = account.RefreshTokens.Single( x => x.Token == token );

            if (!refreshToken.IsActive)
            {
                return null;
            }

            RefreshToken newRefreshToken = JWTHandler.GenerateRefreshToken( ipAddress );

            refreshToken.Revoked_At = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            account.RefreshTokens.Add( newRefreshToken );

            m_context.Update( account );
            m_context.Update( refreshToken );
            await m_context.SaveChangesAsync();

            string accessToken = JWTHandler.GenerateJWTToken( account, m_appSettings );

            return new AuthenticationResponse( newRefreshToken.Token, accessToken );
        }

        /// <summary>
        /// Gets an account by its accountid
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>account</returns>
        public async Task<Account> GetById( int accountId )
        {
            return await m_context.Account.Include(x => x.Customer).FirstOrDefaultAsync(x => x.AccountID == accountId);
        }

        public async Task<Account> Update( int accountId, Account request )
        {
            Account account = await GetById(accountId);
            if(account != null)
            {
                account.Username = request.Username;
                account.Email = request.Email;
                account.Role = request.Role;
                account.Modified_At = DateTime.UtcNow;

                await m_context.SaveChangesAsync();
            }
            return account;
        }

        /// <summary>
        /// Updates the refreshToken so you can stay logged in, if the account exists and if refreshtoken is not active.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ipAddress"></param>
        /// <returns>true</returns>
        public async Task<bool> RevokeToken( string token, string ipAddress )
        {
            Account account = await m_context.Account.Include(e => e.RefreshTokens).SingleOrDefaultAsync( u => u.RefreshTokens.Any( t => t.Token == token ) );

            if(account == null)
            {
                return false;
            }

            RefreshToken refreshToken = account.RefreshTokens.Single( x => x.Token == token );

            if (!refreshToken.IsActive)
            {
                return false;
            }

            refreshToken.Revoked_At = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            m_context.Update( account );

            await m_context.SaveChangesAsync();

            return true;
        }
    }
}
