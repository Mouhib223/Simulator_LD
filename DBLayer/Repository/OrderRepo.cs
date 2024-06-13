using SimulatorLD.DBLayer.DAOs;

namespace SimulatorLD.DBLayer.Repository
{
    public class OrderRepo
    {
        public List<Order> GetAllOrders()
        {
            var db = new RulesManagementDbContext();
            return db.Orders.ToList();
        }

        public Order GetOrderById(int id)
        {
            var db = new RulesManagementDbContext();
            Order order = new Order();
            order = db.Orders.FirstOrDefault(x => x.OrderId == id);
            if (order == null)
                throw new Exception("NotFound");
            return order;

        }




        public void AddOrder(Order order)
        {
            var db = new RulesManagementDbContext();
            db.Orders.Add(order);
            db.SaveChanges();
        }
        public void DeleteOrder(int id)
        {
            var db = new RulesManagementDbContext();

        }
    }
}
