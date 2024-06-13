using Microsoft.AspNetCore.Mvc;
using SimulatorLD.BuisnessLayer.BOs;
using SimulatorLD.DBLayer.DAOs;
using SimulatorLD.DBLayer.Repository;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimulatorLD.WebLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController: ControllerBase
    {

        private OrderBo _order;
        //private OrderRepo _order;


        public OrdersController(OrderBo order)
        {
            _order = order;
        }


        [HttpGet]
        [Route("RecievedOrders")]
        public List<Order> GetAllOrders()
        {
            return _order.GetAllOrders();   
        }

        [HttpPost]
        [Route("AddOrder")]
        public void AddOrder(Order order)
        {
            _order.AddOrder(order);

        }

    }
}
