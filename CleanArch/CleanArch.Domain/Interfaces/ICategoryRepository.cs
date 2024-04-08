using CleanArch.Domain.Entities;

namespace CleanArch.Domain.Validation
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCatories();
        Task<Category> GetById(int? id);

        Task<Category> Create(Category category);
        Task<Category> Update(Category category);
        Task<Category> Remove(Category category);
    }
}
