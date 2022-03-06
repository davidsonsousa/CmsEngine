namespace CmsEngine.Ui.Areas.Cms.Controllers;

[Area("Cms")]
public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ILoggerFactory loggerFactory, IService service, ICategoryService categoryService)
                             : base(loggerFactory, service)
    {
        _categoryService = categoryService;
    }

    public IActionResult Index()
    {
        SetupMessages("Categories", PageType.List, panelTitle: "List of categories");
        return View("List");
    }

    public IActionResult Create()
    {
        SetupMessages("Category", PageType.Create, panelTitle: "Create a new category");
        var categoryEditModel = _categoryService.SetupEditModel();

        return View("CreateEdit", categoryEditModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(CategoryEditModel categoryEditModel)
    {
        if (!ModelState.IsValid)
        {
            SetupMessages("Categories", PageType.Create, panelTitle: "Create a new category");
            return View("CreateEdit", categoryEditModel);
        }

        return await SaveAsync(categoryEditModel, nameof(CategoryController.Create));
    }

    public async Task<IActionResult> EditAsync(Guid vanityId)
    {
        SetupMessages("Categories", PageType.Edit, panelTitle: "Edit an existing category");
        var categoryEditModel = await _categoryService.SetupEditModel(vanityId);

        return View("CreateEdit", categoryEditModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(CategoryEditModel categoryEditModel)
    {
        if (!ModelState.IsValid)
        {
            SetupMessages("Categories", PageType.Edit, panelTitle: "Edit an existing category");
            TempData[MessageConstants.WarningMessage] = "Please double check the information in the form and try again.";
            return View("CreateEdit", categoryEditModel);
        }

        var categoryToUpdate = await _categoryService.SetupEditModel(categoryEditModel.VanityId);

        if (await TryUpdateModelAsync(categoryToUpdate))
        {
            return await SaveAsync(categoryEditModel, nameof(CategoryController.EditAsync));
        }

        TempData[MessageConstants.WarningMessage] = "The model could not be updated.";
        return RedirectToAction(nameof(CategoryController.EditAsync), categoryEditModel);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteAsync(Guid vanityId)
    {
        return Ok(await _categoryService.Delete(vanityId));
    }

    [HttpPost("cms/category/bulk-delete")]
    public async Task<IActionResult> BulkDeleteAsync([FromForm] Guid[] vanityId)
    {
        return Ok(await _categoryService.DeleteRange(vanityId));
    }

    [HttpPost]
    public async Task<IActionResult> GetDataAsync([FromForm] DataParameters parameters)
    {
        Guard.Against.Null(parameters);

        var items = await _categoryService.GetForDataTable(parameters);
        var dataTable = DataTableHelper.BuildDataTable(items.Data, items.RecordsTotal, items.RecordsFiltered, parameters.Draw, parameters.Start, parameters.Length);

        return Ok(dataTable);
    }

    private async Task<IActionResult> SaveAsync(CategoryEditModel categoryEditModel, string sender)
    {
        var returnValue = await _categoryService.Save(categoryEditModel);

        if (!returnValue.IsError)
        {
            TempData[MessageConstants.SuccessMessage] = returnValue.Message;
            return RedirectToAction(nameof(CategoryController.Index));
        }
        else
        {
            TempData[MessageConstants.DangerMessage] = returnValue.Message;
            return RedirectToAction(sender);
        }
    }
}
