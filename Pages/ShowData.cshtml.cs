using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using FabricWebApp.Services;

public class ShowDataModel : PageModel
{
    private readonly FabricDbService _dbService;

    public ShowDataModel(FabricDbService dbService)
    {
        _dbService = dbService;
    }

    [BindProperty]
    public string SelectedTable { get; set; }

    public List<string> Tables { get; } = new() { "Customer", "Product", "SalesOrderHeader", "SalesOrderDetail" };

    public DataTable TableData { get; set; }

    public async Task OnPostAsync()
    {
        if (!string.IsNullOrEmpty(SelectedTable))
        {
            TableData = await _dbService.GetTableDataAsync(SelectedTable);
        }
    }
}