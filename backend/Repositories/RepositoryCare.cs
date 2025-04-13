using AutoMapper;
using backend.Context;
using backend.Core.Dto.CareDto;
using backend.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class RepositoryCare
    {
        private IMapper _mapper { get; }
        private ApplicationDbContext _context { get; }
        public RepositoryCare(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }      

        public async Task<List<User>> GetUs()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }


        public async Task<string> Create(CreateCare care)
        {            

            if (IsEmptyCreate(care))
            {
                return "Не правильные данные в заполнении анкеты";
            };

            if (IsEmptyUserId(care.UserID))
            {
                return "Нет пользователя с данным ID";
            };

            var newCare = _mapper.Map<Care>(care);
            await _context.Cares.AddAsync(newCare);
            await _context.SaveChangesAsync();
            return string.Empty;
        }

        public async Task<string> Update(Guid id, UpdateCare care)
        {
            if (IsEmptyUpdate(care) )
            {
                return "Не правильные данные в заполнении анкеты";
            };
            if (IsEmptyCare(id))
            {
                return "Нет пользователя с данным ID";
            };

            await _context.Cares
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(y => y.Title, care.Title)
                .SetProperty(y => y.Phone, care.Phone)
                .SetProperty(y => y.Price, care.Price));

            await _context.SaveChangesAsync();
            return string.Empty;
        }

        public async Task<List<GetCare>> Get ()
        {

            var getCares = await _context.Cares.ToListAsync();
            var convertCares=_mapper.Map<List<GetCare>>(getCares);
            
            return convertCares;            
        }

        public async Task<GetCare?> GetId(Guid id)
        {
            if (_context.Cares.Any(x => x.Id == id))
            {
                var getCare = await _context.Cares.FirstOrDefaultAsync(x => x.Id == id);
                
                return _mapper.Map<GetCare>(getCare);
                              
            }
            return null;
        }

        public async Task<bool> Delete (Guid id)
        {
            if (IsEmptyCare(id))
            {
                return false;
            }
            var images = await _context.Images.Where(x => x.ImageId == id).Select(x => x.ImageName).ToListAsync();

            foreach (var delete in images)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "imageFile", delete);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            await _context.Images.Where(x => x.ImageId == id).ExecuteDeleteAsync();

            await _context.Cares.Where(x => x.Id == id).ExecuteDeleteAsync();

            return true;
        }
       
        /// <summary>
        /// Проверка на наполненность данных при создании 
        /// </summary>      
        /// <returns>Возвращает true если есть не каретные данные </returns>
        private bool IsEmptyCreate(CreateCare care)
        {
            uint year = (uint)DateTime.Now.Year;
            return (care.Phone == string.Empty || care.Brand == string.Empty || care.YearRelease >year
                || care.Mileage <= 0 || care.Price <= 0 || care.YearRelease < 1900);
        }
        /// <summary>
        /// Проверка на наполненность данных при Обновлении 
        /// </summary>       
        /// <returns>Возвращает true если есть не каретные данные</returns>
        private bool IsEmptyUpdate(UpdateCare care)
        {
            return (care.Phone == string.Empty || care.Price <= 0);            
        }

        /// <summary>
        /// Проверка на наличие User 
        /// </summary>        
        /// <returns>Возвращает true если есть пользователь не найден </returns>                
        private bool IsEmptyUserId(Guid userId )
        {
            return !_context.Users.Any(x=>x.Id == userId);
        }

        /// <summary>
        /// Проверка на наличие анкеты машины с данным ID 
        /// </summary>       
        /// <returns>Возвращает true если есть пользователь не найден</returns>

        private bool IsEmptyCare(Guid Id)
        {
            return !_context.Cares.Any(x => x.Id == Id);
        }
    }
}
