using QuickFix;
using SimulatorLD.DBLayer.DAOs;
using SimulatorLD.DBLayer.Repository;
using SimulatorLD.WebLayer;

namespace SimulatorLD.BuisnessLayer.BOs
{
    public class OrderBo
    {
        private OrderRepo _order;
        public SimpleAcceptorApp test;

        
        
        


        public OrderBo()
        {
            _order = new DBLayer.Repository.OrderRepo();
        }
        public List<Order> GetAllOrders()
        {

            return _order.GetAllOrders();
        }

        public void AddOrder(Order order)
        {
            _order.AddOrder(order);
           


        }
    }
}
