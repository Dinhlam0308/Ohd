using Ohd.DTOs.Admin;
using Ohd.Entities;
using Ohd.Repositories.Interfaces;
using Ohd.Utils;
using OfficeOpenXml;             
using Ohd.DTOs.Common;
namespace Ohd.Services
{
    public class AdminUserService
    {
        private readonly IUserRepository _users;
        private readonly IRoleRepository _roles;
        private readonly IOutboxRepository _outbox;

        public AdminUserService(
            IUserRepository users,
            IRoleRepository roles,
            IOutboxRepository outbox)
        {
            _users = users;
            _roles = roles;
            _outbox = outbox;
        }

        // ==========================
        // GET ALL USERS
        // ==========================
        public async Task<IEnumerable<object>> GetAllUsersAsync()
        {
            var users = await _users.GetAllUsersAsync();
            var result = new List<object>();

            foreach (var u in users)
            {
                var roleId = await _users.GetUserRoleIdAsync(u.Id);

                result.Add(new
                {
                    id = u.Id,
                    email = u.Email,
                    username = u.Username,
                    is_Active = u.Is_Active,
                    roleId = roleId        // ko parse nữa!
                });
            }

            return result;
        }
// ==========================
// GET USERS WITH PAGING + SEARCH
// ==========================
        public async Task<(IEnumerable<object> items, int total)> GetUsersPagedAsync(
            string? search,
            int page,
            int pageSize)
        {
            // Lấy toàn bộ user từ repo (đã có sẵn)
            var users = await _users.GetAllUsersAsync();   // IEnumerable<User>

            // Chuyển sang LINQ queryable
            var query = users.AsQueryable();

            // 🔎 Search theo email / username
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(u =>
                    (u.Email != null && u.Email.Contains(search)) ||
                    (u.Username != null && u.Username.Contains(search))
                );
            }

            var total = query.Count();

            // Phân trang
            var pageUsers = query
                .OrderByDescending(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new List<object>();

            // Chỉ lấy roleId cho user nằm trong trang hiện tại
            foreach (var u in pageUsers)
            {
                var roleId = await _users.GetUserRoleIdAsync(u.Id);

                result.Add(new
                {
                    id = u.Id,
                    email = u.Email,
                    username = u.Username,
                    is_Active = u.Is_Active,
                    roleId = roleId
                });
            }

            return (result, total);
        }

        // ==========================
        // CREATE USER
        // ==========================
        public async Task<(bool ok, string? error, string? rawPassword)> CreateAsync(AdminCreateUserRequest req)
        {
            if (await _users.EmailExistsAsync(req.Email))
                return (false, "Email already exists", null);

            var tempPass = PasswordGenerator.Generate(10);
            var hash = PasswordHasher.Hash(tempPass);

            var user = new User
            {
                Email = req.Email,
                Username = string.IsNullOrWhiteSpace(req.Username) ? req.Email : req.Username,
                Password_Hash = hash,
                Is_First_Login = true,
                Is_Active = true,
                Created_At = DateTime.UtcNow
            };

            await _users.AddAsync(user);

            if (req.RoleId.HasValue)
                await _roles.AssignRoleToUser(user.Id, req.RoleId.Value);

            await _outbox.QueueEmailAsync(user.Email!,
                "Your New Account",
                $"Your temporary password is: {tempPass}");

            return (true, null, tempPass);
        }

        // ==========================
        // CHANGE ROLE
        // ==========================
        public async Task<(bool ok, string? error)> ChangeUserRoleAsync(long userId, int roleId)
        {
            var user = await _users.GetByIdAsync(userId);
            if (user == null)
                return (false, "User not found");

            await _roles.RemoveAllRolesFromUser(userId);
            await _roles.AssignRoleToUser(userId, roleId);
            await _users.SaveChangesAsync();

            return (true, null);
        }

        // ==========================
        // TOGGLE ACTIVE
        // ==========================
        public async Task<bool> ToggleActiveAsync(long userId, bool isActive)
        {
            var user = await _users.GetByIdAsync(userId);
            if (user == null) return false;

            user.Is_Active = isActive;
            await _users.SaveChangesAsync();
            return true;
        }

        // ==========================
        // DELETE USER
        // ==========================
        public async Task<bool> DeleteAsync(long userId)
        {
            return await _users.DeleteAsync(userId);
        }
        public async Task<byte[]> ExportUsersToExcelAsync()
        {
            var users = await _users.GetAllUsersAsync();

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Users");

            ws.Cells["A1"].Value = "Email";
            ws.Cells["B1"].Value = "Username";
            ws.Cells["C1"].Value = "RoleId";
            ws.Cells["D1"].Value = "Active";

            int row = 2;

            foreach (var u in users)
            {
                int? roleId = await _users.GetUserRoleIdAsync(u.Id);

                ws.Cells[row, 1].Value = u.Email;
                ws.Cells[row, 2].Value = u.Username;
                ws.Cells[row, 3].Value = roleId?.ToString() ?? "";
                ws.Cells[row, 4].Value = u.Is_Active ? "1" : "0";
                row++;
            }

            return package.GetAsByteArray();
        }
        public async Task<object> ImportUsersFromExcelUpdateAsync(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);

            var ws = package.Workbook.Worksheets[0];
            int rows = ws.Dimension.Rows;

            var updated = new List<string>();
            var failed = new List<string>();

            for (int r = 2; r <= rows; r++)
            {
                string email = ws.Cells[r, 1].Text;
                string username = ws.Cells[r, 2].Text;
                string roleStr = ws.Cells[r, 3].Text;
                string activeStr = ws.Cells[r, 4].Text;

                var user = await _users.GetByEmailAsync(email);
                if (user == null)
                {
                    failed.Add($"{email} not found");
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(username))
                    user.Username = username;

                if (activeStr == "1" || activeStr.ToLower() == "true")
                    user.Is_Active = true;
                else if (activeStr == "0" || activeStr.ToLower() == "false")
                    user.Is_Active = false;

                if (int.TryParse(roleStr, out int roleId))
                {
                    await _roles.RemoveAllRolesFromUser(user.Id);
                    await _roles.AssignRoleToUser(user.Id, roleId);
                }

                updated.Add(email);
            }

            await _users.SaveChangesAsync();

            return new
            {
                updated,
                failed,
                message = $"Updated {updated.Count}, Failed {failed.Count}"
            };
        }
        public async Task<IEnumerable<User>> GetUsersByRole(string roleName)
        {
            return await _users.GetUsersByRole(roleName);
        }
    }
}