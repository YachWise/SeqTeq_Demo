using Microsoft.EntityFrameworkCore;

public class UserAccountBLL
{
    private readonly ApplicationDbContext _context;

    public UserAccountBLL(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserAccountModel> CreateAccount(UserAccountModel userAccount)
    {
        UserAccountModel newModel = new UserAccountModel
        {
            Id = Guid.NewGuid(),
            UserName = "RandomUserName",
            Email = "RandomEmail"
        };
        _context.UserAccountModels.Add(userAccount);
        Console.WriteLine("Create account called: " + userAccount);
        await _context.SaveChangesAsync();
        return userAccount;
    }

    public async Task<UserAccountModel?> GetAccountById(Guid id)
    {
        return await _context.UserAccountModels.FindAsync(id);
    }

    public async Task<UserAccountModel?> UpdateAccountAsync(UserAccountModel updatedAccount)
    {
        var existingAccount = await _context.UserAccountModels.FindAsync(updatedAccount.Id);
        if (existingAccount == null)
        {
            return null;
        }

        existingAccount.UserName = updatedAccount.UserName;
        existingAccount.Email = updatedAccount.Email;

        _context.UserAccountModels.Update(existingAccount);
        await _context.SaveChangesAsync();
        return existingAccount;
    }

    public async Task<bool> DeleteAccountAsync(Guid id)
    {
        var account = await _context.UserAccountModels.FindAsync(id);
        if (account == null)
        {
            return false;
        }

        _context.UserAccountModels.Remove(account);
        await _context.SaveChangesAsync();
        return true;
    }
}
