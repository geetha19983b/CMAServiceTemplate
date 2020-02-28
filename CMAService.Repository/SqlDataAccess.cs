//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Linq;
//using Microsoft.EntityFrameworkCore;
//using System.Threading.Tasks;

//namespace CMAService.Repository
//{
//    public class SqlDataAccess : IDataAccess, IDisposable
//    {
//        private readonly AuthorContext _context;

//        public SqlDataAccess(AuthorContext context)
//        {
//            _context = context ?? throw new ArgumentNullException(nameof(context));
//        }
//        public async Task<IEnumerable<Author>> GetAuthors()
//        {
//            return await _context.Authors.ToListAsync<Author>();
//        }
//        public async Task<Author> GetAuthor(Guid authorId)
//        {
//            if (authorId == Guid.Empty)
//            {
//                throw new ArgumentNullException(nameof(authorId));
//            }

//            return await _context.Authors.FirstOrDefaultAsync(a => a.Id == authorId);
//        }

//        public async Task AddAuthor(Author author)
//        {
//            if (author == null)
//            {
//                throw new ArgumentNullException(nameof(author));
//            }

//            // the repository fills the id (instead of using identity columns)
//            author.Id = Guid.NewGuid();

//            await _context.Authors.AddAsync(author);
//            await _context.SaveChangesAsync();
//        }
//        public async void UpdateAuthor(Guid authorId, Author author)
//        {
//            var authorfromRepo = _context.Authors.Where(x => x.Id == authorId).FirstOrDefault();

//            if (authorfromRepo != null)
//            {
//                authorfromRepo.FirstName = author.FirstName;
//                authorfromRepo.LastName = author.LastName;
//                //_context.Entry(author).State = EntityState.Modified;

//                try
//                {
//                    await _context.SaveChangesAsync();
//                }
//                catch (Exception ex)
//                {
//                    throw;
//                }
//            }
//            else
//            {
//                throw new Exception("Author id does not exists in db");
//            }
//        }
//        private bool AuthorExists(Guid id) =>
//         _context.Authors.Any(e => e.Id == id);



//        public async void DeleteAuthor(Author author)
//        {
//            if (author == null)
//            {
//                throw new ArgumentNullException(nameof(author));
//            }

//            _context.Authors.Remove(author);
//            await _context.SaveChangesAsync();
//        }

//        public bool Save()
//        {
//            return (_context.SaveChanges() >= 0);
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                // dispose resources when needed
//            }
//        }
//    }
//}
