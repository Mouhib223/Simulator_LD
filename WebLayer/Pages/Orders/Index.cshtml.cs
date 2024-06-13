using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimulatorLD.DBLayer.Repository;
using SimulatorLD.DBLayer.DAOs;

public class OrdersModel : PageModel
{
    private readonly RulesManagementDbContext _context;

    public OrdersModel(RulesManagementDbContext context)
    {
        _context = context;
    }

    public IList<Order> Orders { get; set; }

    public async Task OnGetAsync()
    {
        Orders = await _context.Orders.ToListAsync();
    }
}
