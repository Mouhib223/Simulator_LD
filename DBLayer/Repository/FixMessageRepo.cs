using SimulatorLD.DBLayer.DAOs;

namespace SimulatorLD.DBLayer.Repository
{
    public class FixMessageRepo 
    {

        RulesManagementDbContext db = new RulesManagementDbContext();
        public List<Fixmessage> GetAllMessages()
        {
            //var db = new RulesManagementDbContext();
            return db.Fixmessages.ToList();
        }

        public void AddMessage(Fixmessage msg)
        {
            
            db.Fixmessages.Add(msg);
            db.SaveChanges();
        }



        public void RemoveMessage(Fixmessage msg) 
        {  
                //var db = new RulesManagementDbContext();
                db.Fixmessages.Remove(msg);
         }

    }
}
