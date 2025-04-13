using AutoMapper;
using backend.Context;
using backend.Core.Dto.UserDto;
using backend.Core.Models;
using backend.TokenGeneration;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class RepositoryUser
    {
        private ApplicationDbContext _context { get; }        
        private RepositoryCare _care {  get; }
        private IMapper _mapper { get; }
        private CreateTokin _createTokin { get; }
        public RepositoryUser(ApplicationDbContext context , IMapper mapper, CreateTokin createTokin)
        {
            _context = context;
            _mapper = mapper;
            _care = new RepositoryCare(_context,_mapper);
            _createTokin = createTokin;
        }

        public async Task<string> Add(UserRegistries user)
        {
            if (user.Login.Length > 20 || user.Login.Length < 5)
            {
                return "Длина логина не может быть более 20 символов или меньше 5 символов ";
            }
            if (user.Password.Length > 20 || user.Password.Length < 5)
            {
                return "Длина пароля не может быть более 20 символов или меньше 5 символов ";
            }
            if (_context.Users.Any(x => x.Login == user.Login))
            {
                return "Пользователь с данным логином зарегистрирован";
            }

            var newUser = new User()
            {
                Login = user.Login,
                Password = user.Password,                
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return string.Empty;
        }
        public async Task<string> Update(UserUpdate user)
        {
            if (!_context.Users.Any(x => x.Login == user.Login))
            {
                return "Пользователь с данным логином не зарегистрирован";
            }
            if (!_context.Users.Any(x=> x.Password == user.Password))
            {
                return "Не верный пароль";
            }

            if (user.NewPassword.Length>20 || user.NewPassword.Length < 5)
            {
                return "Длина Нового пароля не может быть более 20 символов или меньше 5 символов ";
            }

            await _context.Users
                .Where(x => x.Login == user.Login)
                .ExecuteUpdateAsync(y => y
                .SetProperty(z => z.Password, user.NewPassword));                

            await _context.SaveChangesAsync();

            return string.Empty;
        }
        
        public async Task<string> Delete (string login)
        {
            if (!_context.Users.Any(y=>y.Login == login))
            {
                return "неверный логин";
            };

            var userDelete = await _context.Users.FirstOrDefaultAsync(x=>x.Login== login);

            var deleteCares = await _context.Cares.Where(x => x.UserID == userDelete.Id).Select(x=>x.Id).ToListAsync();

            foreach (var care in deleteCares) 
            {
                await _care.Delete(care);
            }

            await _context.Users.Where(x=>x.Login==login).ExecuteDeleteAsync();          

            return string.Empty;
        }

        public async Task<string> GenerateToken(string login, string password)
        {
            if (_context.Users.Any(x => x.Login == login && x.Password == password))
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.Login == login && x.Password == password);

                var result = _createTokin.GenerationToken(user);

                return result;
            }

            return string.Empty;
        }
    }
}
