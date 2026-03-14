using ECM.Application.Interfaces.Respository.Catalogo;
using ECM.Application.ServicesInterfaces.CategoryService;
using ECM.Domain.Common;
using ECM.Domain.Entities.Catalogo;


namespace ECM.Application.Services.Catalogo;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<IEnumerable<Category>>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();
        return OperationResult<IEnumerable<Category>>.Ok(categories);
    }

    public async Task<OperationResult<Category>> GetByIdAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null)
            return OperationResult<Category>.Fail("Categoría no encontrada.");

        return OperationResult<Category>.Ok(category);
    }

    public async Task<OperationResult<Category>> CreateAsync(Category category)
    {
        var result = await _repository.AddAsync(category);
        return OperationResult<Category>.Ok(result!, "Categoría creada.");
    }

    public async Task<OperationResult<Category>> UpdateAsync(Category category)
    {
        var result = await _repository.Update(category, category.Id);
        if (result == null)
            return OperationResult<Category>.Fail("Categoría no encontrada.");

        return OperationResult<Category>.Ok(result, "Categoría actualizada.");
    }

    public async Task<OperationResult<bool>> DeleteAsync(int id)
    {
        var result = await _repository.Disable(id);
        if (result == null)
            return OperationResult<bool>.Fail("Categoría no encontrada.");

        return OperationResult<bool>.Ok(true, "Categoría desactivada.");
    }
}